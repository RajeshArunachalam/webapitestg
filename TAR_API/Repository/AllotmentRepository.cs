using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TAR_API.App_Code;
using TAR_API.Interface;
using Dapper;

namespace TAR_API.Repository
{
    public class AllotmentRepository : BaseRepository,IAllotment
    {
       /// <summary>
       /// 
       /// </summary>
       /// <param name="PHMID"></param>
       /// <param name="UserID"></param>
       /// <returns></returns>
        public async Task<Tuple<IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>>> GetworkallotmentPreLoadData(int PHMID, int UserID)
        {
            //These are the declaration of return objects
            IEnumerable<dynamic> objSupplyFields = null;
            IEnumerable<dynamic> objUsers = null;
            IEnumerable<dynamic> objConfiguration = null;
            IEnumerable<dynamic> objAgingDetails = null;
            

            try
            {

                return await WithConnection(async c =>
                {
                    //This is to add parameters
                    var para = new DynamicParameters();
                    para.Add("@PHMID", PHMID);


                    //This is to excute the command and return the result
                    using (var multi = await c.QueryMultipleAsync(ClsProcedures.UspGetWorkAllotmentPreLoadData, para, commandType: CommandType.StoredProcedure, commandTimeout: 0))
                    {

                        if (!multi.IsConsumed)
                        {
                            //Assignning Configuration result
                            objSupplyFields = multi.Read().ToList();
                        }
                        if (!multi.IsConsumed)
                        {
                            //Assignning Additional Caputures result
                            objUsers = multi.Read().ToList();
                        }
                        if (!multi.IsConsumed)
                        {
                            //Assignning Additional Caputures result
                            objConfiguration = multi.Read().ToList();
                        }
                        if (!multi.IsConsumed)
                        {
                            //Assignning Additional Caputures result
                            objAgingDetails = multi.Read().ToList();
                        }

                    }

                    //Values are retured in the form of Tuple with mutiple objects
                    return Tuple.Create(objSupplyFields, objUsers, objConfiguration, objAgingDetails);
                });

            }
            catch (Exception ex)
            {

                return null;
            }
            finally
            {
                //These are the declaration of return objects
                objSupplyFields = null;
                objUsers = null;
                objConfiguration = null;
                objAgingDetails = null;
            }
        }

        public async Task<Tuple<IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>>> GetAccounts(int PHMID, int UserID, string ImportFileID, string RoleCode, string StatusCode, DataTable CustomSearch)
        {
            IEnumerable<dynamic> AccountDetails = null;
            IEnumerable<dynamic> AccountSummary = null;
            IEnumerable<dynamic> UserSummary = null;
            IEnumerable<dynamic> RoleBasedCount = null;
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

                    using (var multi = await c.QueryMultipleAsync(ClsProcedures.UspGetWorkAllotmentAccounts, param: para, commandType: CommandType.StoredProcedure))
                    {
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
                        {
                            //Assignning result to variable
                            UserSummary = multi.Read().ToList();
                        }
                    }

                    //This is to add parameters
                    DynamicParameters paraNew = new DynamicParameters();
                    paraNew.Add("@PHMID", PHMID);
                    paraNew.Add("@UserID", UserID);
                    //This is to excute the command and return the result
                    using (var multi = await c.QueryMultipleAsync(ClsProcedures.UspDashboardrolewise, paraNew, commandType: CommandType.StoredProcedure, commandTimeout: 0))
                    {
                        if (!multi.IsConsumed)
                        {
                            //Assignning result to variable
                            RoleBasedCount = multi.Read().ToList();
                        }
                    }
                    return Tuple.Create(AccountDetails, AccountSummary, UserSummary, RoleBasedCount);
                    //Values are retured in the form of Tuple with mutiple objects

                }
                
                
                );
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
                UserSummary = null;
            }


        }

        /// <summary>
        /// This method is to assign inventory
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="LoginUserID"></param>
        /// <param name="AllocationType"></param>
        /// <returns></returns>
        public async Task<int> AssignSupply(String SupplyData, int LoginUserID, string AllocationType,string NeedCallRemarks,int PHMID)
        {
            try
            {
                return await WithConnection(async c =>
                {
                    //This is to add parameters
                    DynamicParameters para = new DynamicParameters();
                    para.Add("@SupplyJsonData", SupplyData, DbType.String);
                    para.Add("@AllocationType", AllocationType, DbType.String);
                    para.Add("@LoginUserID", LoginUserID, DbType.Int32);
                    para.Add("@NeedCallRemarks", NeedCallRemarks, DbType.String);
                    para.Add("@PHMID", PHMID, DbType.Int32);
                    return await c.ExecuteAsync(ClsProcedures.UspAllocationProduction, param: para, commandType: CommandType.StoredProcedure, commandTimeout: 0);

                });
            }
            catch (Exception ex)
            {
                //ExceptionLogging.SendErrorToText(ex);
                return 0;
            }

        }

       /// <summary>
       /// 
       /// </summary>
       /// <param name="PHMID"></param>
       /// <param name="activeMenu"></param>
       /// <returns></returns>
        public async Task<IEnumerable<dynamic>> GetImportFileList(int PHMID, string activeMenu)
        {
            try
            {
                return await WithConnection(async c =>
                {
                    //This is to add parameters
                    DynamicParameters para = new DynamicParameters();
                    para.Add("@PHMID", PHMID);
                    para.Add("@ActiveMenu", activeMenu);
                    var result = await c.QueryAsync<dynamic>(ClsProcedures.UspGetImportFileList, param: para, commandType: CommandType.StoredProcedure, commandTimeout: 0);
                    return (result.ToList());
                });
            }
            catch (Exception ex)
            {

                return null;
            }

        }
        public async Task<Tuple<IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>>> GetAccountStatusSummary(int PHMID, int UserID, int ImportFileID, string RoleCode, string StatusCode)
        {
            IEnumerable<dynamic> AgingDetails = null;
            IEnumerable<dynamic> AccountSummary = null;
            IEnumerable<dynamic> UserSummary = null;
            try
            {
                return await WithConnection(async c =>
                {
                    //This is to add parameters
                    DynamicParameters para = new DynamicParameters();
                    para.Add("@PHMID", PHMID);
                    para.Add("@ImportFileID", ImportFileID);
                    para.Add("@StatusCode", StatusCode);
                    para.Add("@UserID", UserID);
                    var multi = await c.QueryMultipleAsync(ClsProcedures.uspGetAccountStatusSummary, param: para, commandType: CommandType.StoredProcedure);
                    if (!multi.IsConsumed)
                    {
                        //Assignning result to variable
                        AgingDetails = multi.Read().ToList();
                    }
                    if (!multi.IsConsumed)
                    {
                        //Assignning result to variable
                        AccountSummary = multi.Read().ToList();
                    }
                    if (!multi.IsConsumed)
                    {
                        //Assignning result to variable
                        UserSummary = multi.Read().ToList();
                    }
                    return Tuple.Create(AgingDetails, AccountSummary, UserSummary);
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
                AgingDetails = null;
                AccountSummary = null;
                UserSummary = null;
            }


        }

        public async Task<Tuple<IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>>> GetFilterdataAccounts(int PHMID, int UserID, int ImportFileID, string RoleCode, string StatusCode)
        {
            IEnumerable<dynamic> AccountStatusDetails = null;
            IEnumerable<dynamic> AgingStatuDetails = null;
            IEnumerable<dynamic> AccountDetails = null;
            IEnumerable<dynamic> AccountUserSummary = null;
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
                    var multi = await c.QueryMultipleAsync(ClsProcedures.UspGetWorkAllocationFilterData, param: para, commandType: CommandType.StoredProcedure);
                    if (!multi.IsConsumed)
                    {
                        //Assignning result to variable
                        AccountStatusDetails = multi.Read().ToList();
                    }
                    if (!multi.IsConsumed)
                    {
                        //Assignning result to variable
                        AgingStatuDetails = multi.Read().ToList();
                    }
                    if (!multi.IsConsumed)
                    {
                        //Assignning result to variable
                        AccountDetails = multi.Read().ToList();
                    }
                    if (!multi.IsConsumed)
                    {
                        //Assignning result to variable
                        AccountUserSummary = multi.Read().ToList();
                    }
                    return Tuple.Create(AccountStatusDetails, AgingStatuDetails,AccountDetails, AccountUserSummary);
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
                AgingStatuDetails = null;
                AccountDetails = null;
                AccountUserSummary = null;
            }


        }

        /// <summary>
        /// This is to submit clarificatin response
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public int SubmiClarificationResponse(string data)
        {
            try
            {
                int iAffectedRows = 0;

                using (IDbConnection db = new SqlConnection(ClsCommon._ConnectionString))
                {
                    //This is to add parameters
                    DynamicParameters para = new DynamicParameters();
                    para.Add("@jsonData", data);
                    iAffectedRows = db.Execute(ClsProcedures.UspSubmitTLClarificationResponse, para, commandType: CommandType.StoredProcedure);

                    return iAffectedRows;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public IEnumerable<dynamic> SubmitRebuttal(String Request)
        {
            try
            {
                
                using (IDbConnection db = new SqlConnection(ClsCommon._ConnectionString))
                {
                    DynamicParameters para = new DynamicParameters();
                   
                    para.Add("@RequestJson", Request, DbType.String);
                   
                    return db.Query<dynamic>(ClsProcedures.UspSubmitRebuttalData, param: para, commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}