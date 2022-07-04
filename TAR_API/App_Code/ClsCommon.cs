using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Web;

namespace TAR_API.App_Code
{
    public static class ClsCommon
    {
        public static readonly string _ConnectionString = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("ConnectionStrings")["sConnectionString"].Replace("{IP}", Environment.GetEnvironmentVariable("DB_IP"));

        public static readonly string ApplicationName = "ARC";

        public static readonly string _DestinationPath = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AppSettings")["sDestinationPath"].Replace("{IP}", Environment.GetEnvironmentVariable("DB_IP"));
        public static readonly string _DestinationPathAccounts = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AppSettings")["sDestinationPathAccounts"].Replace("{IP}", Environment.GetEnvironmentVariable("DB_IP"));


        /// <summary>
        /// Get Local API address
        /// </summary>
        /// <returns></returns>
        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        public static DataTable ParseSearchData(String SearchData)
        {
            DataTable dtSearch = new DataTable();
            dtSearch.Columns.Add("field", typeof(string));
            dtSearch.Columns.Add("condition", typeof(string));
            dtSearch.Columns.Add("value", typeof(string));

            try
            {
                string Value = string.Empty;

                dynamic dynJson = JsonConvert.DeserializeObject(SearchData);
                foreach (var item in dynJson)
                {
                    DataRow drRow = dtSearch.NewRow();
                    drRow["field"] = item.field;
                    drRow["condition"] = item.condition;

                    Value = Convert.ToString(item.value);

                    if (Convert.ToString(item.condition) == "=" || Convert.ToString(item.condition) == "IN")
                    {
                        if (Value.Split(',').Length > 1)
                        {
                            drRow["condition"] = "IN";
                            Value = "(" + string.Join(",", Value.Split(',').Select(x => string.Format("'{0}'", x)).ToList()) + ")";

                        }
                        else
                        {
                            drRow["condition"] = "=";
                            Value = "'" + String.Join("','", Value) + "'";
                        }

                        drRow["value"] = Value;
                    }
                    else if (Convert.ToString(item.condition) == ">" || Convert.ToString(item.condition) == "<" || Convert.ToString(item.condition) == ">=" || Convert.ToString(item.condition) == "<=")
                    {
                        if (Convert.ToString(item.datatype) == "DATE")
                        {
                            Value = string.Format("CONVERT(date,'{0}')", String.Join("','", Convert.ToString(item.value)));
                            drRow["value"] = Value;
                        }
                        else
                        {
                            Value = String.Join("','", Convert.ToString(item.value));
                            drRow["value"] = Value;
                        }
                    }
                    else if (Convert.ToString(item.condition) == "LIKE")
                    {
                        Value = string.Format("'%{0}%'", Value);
                        drRow["value"] = Value;
                    }
                    else if (Convert.ToString(item.datatype) == "DATE")
                    {
                        Value = string.Format("CONVERT(date,'{0}')", Value);
                        drRow["value"] = Value;
                    }
                    else
                    {
                        drRow["value"] = Value;
                    }

                    dtSearch.Rows.Add(drRow);
                }
            }
            catch (Exception ex)
            {

            }

            return dtSearch;
        }
    }
}