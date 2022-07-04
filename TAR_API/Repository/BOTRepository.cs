using TAR_API.App_Code;
using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using static TAR_API.Controllers.BOTController;

namespace TAR_API.Repository
{
    public class BOTRepository:BaseRepository
    {
       // public static readonly string _ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        public async Task<Tuple<IEnumerable<dynamic>, IEnumerable<dynamic>>> getImportBotServiceList(int PHMID, int UserID)
        {
            //These are the declaration of return objects
            IEnumerable<dynamic> objUsers = null;
            IEnumerable<dynamic> objServices = null;
            try
            {
                return await WithConnection(async db =>
                {
                    //This is to add parameters
                    var para = new DynamicParameters();
                    para.Add("@PHMID", PHMID);
                    //This is to excute the command and return the result
                    using (var multi = await db.QueryMultipleAsync(ClsProcedures.UspBotProcess, para, commandType: CommandType.StoredProcedure, commandTimeout: 0))
                    {



                        if (!multi.IsConsumed)
                        {
                            //Assignning Additional Caputures result
                            objServices = multi.Read().ToList();
                        }
                        if (!multi.IsConsumed)
                        {
                            //Assignning Additional Caputures result
                            objUsers = multi.Read().ToList();
                        }



                    }
                    //Values are retured in the form of Tuple with mutiple objects
                    return Tuple.Create(objServices,objUsers);
                });
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                //These are the declaration of return objects
                objUsers = null;
                objServices = null;
            }
        }



        public async Task<int> UpdateAvailityRequest(string AccID, string ResponseData, string BOTRequestID,string RequestData)
        {
            try
            {


                return await WithConnection(async db =>
                {
                    //This is to add parameters
                    DynamicParameters para = new DynamicParameters();
                    para.Add("@AccountID", AccID);
                    para.Add("@BOTResponseData", ResponseData);
                    para.Add("@BOTRequestData", RequestData);
                    para.Add("@BOTRequestID", BOTRequestID);
                    return db.Query<int>(ClsProcedures.UspupdateManualAvailityAccountData, param: para, commandType: CommandType.StoredProcedure).FirstOrDefault();
                });
               
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public async Task<Tuple<IEnumerable<dynamic>>> GetBotResponseView(int AccountID)
        {
            //These are the declaration of return objects
            IEnumerable<dynamic> objResponse = null;
          
            try
            {
                return await WithConnection(async db =>
                {
                    //This is to add parameters
                    var para = new DynamicParameters();
                    para.Add("@AccountID", AccountID);
                    //This is to excute the command and return the result
                    using (var multi = await db.QueryMultipleAsync(ClsProcedures.uspGetBotResponseView, para, commandType: CommandType.StoredProcedure, commandTimeout: 0))
                    {



                        if (!multi.IsConsumed)
                        {
                            //Assignning Additional Caputures result
                            objResponse = multi.Read().ToList();
                        }
                        



                    }
                    //Values are retured in the form of Tuple with mutiple objects
                    return Tuple.Create(objResponse);
                });
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                //These are the declaration of return objects
                objResponse = null;
                
            }
        }

        public async Task<int> InsertAvailityRequestAsync(RequestBOT obj)
        {
            try
            {
                return await WithConnection(async db =>
                {
                    //This is to add parameters
                    DynamicParameters para = new DynamicParameters();
                    para.Add("@AccountIDs", obj.AccountIDs, DbType.String);
                    para.Add("@ServiceType", obj.ServiceType);
                    para.Add("@UserID", obj.UserID);
                    para.Add("@PHMID", obj.PHMID);
                    return  db.Query<int>(ClsProcedures.UspInsertAvailityAccountData, param: para, commandType: CommandType.StoredProcedure).FirstOrDefault();
                });
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<Tuple<IEnumerable<dynamic>>> GetAvailityConfigDetails()
        {
            IEnumerable<dynamic> objResponse = null;
            try
            {

                return await WithConnection(async db =>
                {
                    DynamicParameters para = new DynamicParameters();
                    //return db.Query<dynamic>("uspGetAvailityConfig", param: para, commandType: CommandType.StoredProcedure).ToList();
                    using (var multi = await db.QueryMultipleAsync(ClsProcedures.UspGetAvailityConfig, para, commandType: CommandType.StoredProcedure, commandTimeout: 0))
                    {

                        if (!multi.IsConsumed)
                        {
                            //Assignning Additional Caputures result
                            objResponse = multi.Read().ToList();
                        }

                    }
                    //Values are retured in the form of Tuple with mutiple objects
                    return Tuple.Create(objResponse);

                });
               
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Tuple<IEnumerable<dynamic>, IEnumerable<dynamic>>> GetAccounts(int PHMID, int UserID, int ImportFileID, string RoleCode, string StatusCode, DataTable CustomSearch)
        {
            IEnumerable<dynamic> AccountDetails = null;
            IEnumerable<dynamic> AccountSummary = null;
            
            try
            {
                return await WithConnection(async c =>
                {
                    //This is to add parameters
                    DynamicParameters para = new DynamicParameters();
                    para.Add("@ImportFileID", ImportFileID);
                    para.Add("@PHMID", PHMID);
                    para.Add("@UserID", UserID);
                    para.Add("@StatusCode", StatusCode);
                    para.Add("@RoleCode", RoleCode);
                    para.Add("@CustomSearch", CustomSearch, DbType.Object);

                    var multi = await c.QueryMultipleAsync(ClsProcedures.uspGetBotAccounts, param: para, commandType: CommandType.StoredProcedure);
                    if (!multi.IsConsumed)
                    {
                        //Assignning result to variable
                        AccountDetails = multi.Read().ToList();
                    }
                    if (!multi.IsConsumed)
                    {
                        //Assignning result to variable
                        AccountSummary = multi.Read().ToList();
                    }
                   
                    return Tuple.Create(AccountDetails, AccountSummary);
                    //Values are retured in the form of Tuple with mutiple objects

                });
            }
            catch (Exception ex)
            {
                //
                return null;
            }
            finally
            {
                AccountDetails = null;
                AccountSummary = null;
              
            }


        }



    }
}