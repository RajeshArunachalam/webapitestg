using TAR_API.App_Code;
using TAR_API.Interface;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;


namespace TAR_API.Repository
{
    public class ReportRepository:BaseRepository,IReport
    {
        public async Task<IEnumerable<dynamic>> GetReportRDLFileDetails(string ReportURL,  int PHMID)
        {
            try
            {
                return await WithConnection(async c => {
                    DynamicParameters para = new DynamicParameters();
                    para.Add("@ReportURL", ReportURL);
                    para.Add("@PHMID", PHMID);
                    var result = await c.QueryAsync<dynamic>(ClsProcedures.UspGetReportRDLFileDetails, param: para, commandType: CommandType.StoredProcedure);
                    return (result.ToList());
                });
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public IEnumerable<dynamic> GetFileDetails(int PHMID)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(ClsCommon._ConnectionString))
                {
                    //This is to add parameters
                    DynamicParameters para = new DynamicParameters();
                    para.Add("@PHMID", PHMID);
                    return db.Query<dynamic>(ClsProcedures.UspGetFileDetails, param: para, commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception ex)
            {
                //
                return null;
            }
        }
    }
}