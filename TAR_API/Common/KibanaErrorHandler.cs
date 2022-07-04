using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;


namespace TAR_API.Common
{
    public class KibanaErrorHandler
    {
        public string ElasticSearchURL = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AppSettings")["sDestinationPath"];
        public string IndexName = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AppSettings")["sDestinationPath"];

        public void Success()
        {
            var data = @"{
                          ""Type"": ""Success"",
                          ""Date"": ""John Smith"",
                          ""Result"" : {
                                ""ControllerName"":"""", 
                                ""MethodName"":"""",
                                ""RequestObject"":""""
                                }
                       }";
            ElasticSearch(data, ElasticSearchURL + "/" + IndexName + "/typename", "POST");
        }
        public void Error()
        {
            var data = @"{
                          ""Type"": ""Error"",
                          ""Date"": ""John Smith"",
                          ""Result"" : {
                                ""Controller Name"":"""", 
                                ""MethodNumber"":"""",
                                ""LineNumber"":"""",
                                ""ErrorMessage"":"""",
                                ""InnerException"":"""",
                                ""Request Object"":""""
                                }
                       }";

            ElasticSearch(data, ElasticSearchURL + "/" + IndexName + "/typename", "POST");
        }

        public void CheckIndex()
        {
            string res = ElasticSearch("", ElasticSearchURL+"/"+IndexName+ "?pretty", "PUT");
        }
        private string ElasticSearch(object input, string apiUrl,string head)
        {
            string res = "";
            
            HttpClient client = new HttpClient();
            try
            {
                    var httpRequest = (HttpWebRequest)WebRequest.Create(apiUrl);
                    httpRequest.Method = head;
                    httpRequest.ContentType = "application/json";
                    var data = input;
                    using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
                    {
                        streamWriter.Write(data);
                    }

                    var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        res = streamReader.ReadToEnd();
                    }

            }
            catch  (Exception ex)
            {
                res = "Index Already Created/Check the Service"+ ex.Message;
            }
            return res;

        }
    }
}