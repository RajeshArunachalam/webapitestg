using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TAR_API.App_Code;
using TAR_API.Interface;
using Dapper;

namespace TAR_API.Repository
{
    public class LoginRepository:BaseRepository,ILogin
    {
        /// <summary>
        /// This is to update logout time
        /// </summary>
        /// <param name="HistoryID"></param>
        /// <returns></returns>
        public async Task<int> UpdateLogoutTime(int HistoryID)
        {
            try
            {
                return await WithConnection(async c =>
                {
                    //This is to add parameters
                    DynamicParameters para = new DynamicParameters();
                    para.Add("@HistoryID", HistoryID);
                    return c.ExecuteAsync(ClsProcedures.UspUpdateLogoutTime, param: para, commandType: CommandType.StoredProcedure).Result;

                });

            }
            catch (Exception ex)
            {

                return 0;
            }
        }
    }
}