using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TAR_API.App_Code;
using Dapper;

namespace TAR_API.Repository
{
    public class TokenRepository:BaseRepository
    {
        /// <summary>
        /// This is to save token details.
        /// </summary>
        /// <param name="Username"></param>
        /// <param name="access_token"></param>
        /// <returns></returns>
        public async Task<int> SaveTokenDetails(string Username, string access_token, string refresh_token, string UserIP)
        {
            try
            {
                return await WithConnection(async c =>
                {
                    //This is to add parameters
                    DynamicParameters para = new DynamicParameters();
                    para.Add("@Username", Username);
                    para.Add("@access_token", access_token.ToString());
                    para.Add("@refresh_token", refresh_token.ToString());
                    para.Add("@IPAddress", UserIP);
                    return c.ExecuteAsync(ClsProcedures.UspSaveTokenDetails, param: para, commandType: CommandType.StoredProcedure).Result;

                });

            }
            catch (Exception ex)
            {

                return 0;
            }
        }

        /// <summary>
        /// This is to check token details
        /// </summary>
        /// <param name="access_token"></param>
        /// <returns></returns>
        public async Task<IEnumerable<dynamic>> CheckTokenDetails(string Username, string access_token)
        {
            try
            {
                return await WithConnection(async c =>
                {
                    //This is to add parameters
                    DynamicParameters para = new DynamicParameters();
                    para.Add("@Username", Username);
                    para.Add("@access_token", access_token);
                    return await c.QueryAsync(ClsProcedures.UspCheckTokenDetails, param: para, commandType: CommandType.StoredProcedure);

                });

            }
            catch (Exception ex)
            {

                return null;
            }
        }
    }
}