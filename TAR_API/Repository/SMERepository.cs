using TAR_API.App_Code;
using TAR_API.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TAR_API.Interface;

namespace TAR_API.Repository
{
    public class SMERepository:BaseRepository,ISME
    {
        public async Task<Tuple<IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>>> GetSMEPreLoadData(int PHMID, int UserID)
        {
            //These are the declaration of return objects

            IEnumerable<dynamic> objSupplyFields = null;
            IEnumerable<dynamic> objAdditionalCaptures = null;
            IEnumerable<dynamic> objScenario = null;
            IEnumerable<dynamic> objCallType = null;
            IEnumerable<dynamic> objAgingDetails = null;
            IEnumerable<dynamic> objConfiguration = null;
            IEnumerable<dynamic> objUsers = null;
            try
            {

                return await WithConnection(async c =>
                {
                    //This is to add parameters
                    var para = new DynamicParameters();
                    para.Add("@PHMID", PHMID);
                    para.Add("@UserID", UserID);

                    //This is to excute the command and return the result
                    using (var multi = await c.QueryMultipleAsync(ClsProcedures.UspGetSMEInboxPreLoadData, para, commandType: CommandType.StoredProcedure, commandTimeout: 0))
                    {

                        if (!multi.IsConsumed)
                        {
                            //Assignning Configuration result
                            objSupplyFields = multi.Read().ToList();
                        }
                        if (!multi.IsConsumed)
                        {
                            //Assignning Additional Caputures result
                            objAdditionalCaptures = multi.Read().ToList();
                        }
                        if (!multi.IsConsumed)
                        {
                            //Assignning Additional Caputures result
                            objScenario = multi.Read().ToList();
                        }
                        if (!multi.IsConsumed)
                        {
                            //Assignning Additional Caputures result
                            objCallType = multi.Read().ToList();
                        }
                        if (!multi.IsConsumed)
                        {
                            //Assignning Additional Caputures result
                            objAgingDetails = multi.Read().ToList();
                        }
                        if (!multi.IsConsumed)
                        {
                            //Assignning Additional Caputures result
                            objConfiguration = multi.Read().ToList();
                        }
                        if (!multi.IsConsumed)
                        {
                            //Assignning Additional Caputures result
                            objUsers = multi.Read().ToList();
                        }
                    }

                    //Values are retured in the form of Tuple with mutiple objects
                    return Tuple.Create(objSupplyFields, objAdditionalCaptures, objScenario, objCallType, objAgingDetails, objConfiguration, objUsers);
                });

            }
            catch (Exception ex)
            {

                return null;
            }
            finally
            {
                objSupplyFields = null;
                objAdditionalCaptures = null;
                objScenario = null;
                objCallType = null;
                objAgingDetails = null;
                objConfiguration = null;
            }
        }

        public IEnumerable<dynamic> SubmitClarification(int PHMID, int UserID, string Query, string SubCategoryID, string AccountID, string clarificationrole, string ClarificationTo)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(ClsCommon._ConnectionString))
                {
                    DynamicParameters para = new DynamicParameters();
                    para.Add("@PHMID", PHMID);
                    para.Add("@UserID", UserID);
                    para.Add("@Query", Query);
                    para.Add("@SubCategoryID", SubCategoryID);
                    para.Add("@AccountIDs", AccountID, DbType.String);
                    para.Add("@ClarificationRoleCode", "QCA");
                    para.Add("@ClarificationType", "QCA");
                    para.Add("@ClarificationTo", ClarificationTo);

                    return db.Query<dynamic>(ClsProcedures.UspSubmitClarification, param: para, commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Tuple<IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>>> GetSMEAccounts(int PHMID, int UserID, string ImportFileID, string RoleCode, string StatusCode, DataTable CustomSearch)
        {
            IEnumerable<dynamic> AccountDetails = null;
            IEnumerable<dynamic> StatusCount = null;
            IEnumerable<dynamic> RoleBasedCount = null;
            try
            {

                return await WithConnection(async c =>
                {
                    //This is to add parameters
                    DynamicParameters para = new DynamicParameters();
                    para.Add("@PHMID", PHMID);
                    para.Add("@UserID", UserID);
                    para.Add("@ImportFileID", ImportFileID);
                    para.Add("@RoleCode", RoleCode);
                    para.Add("@StatusCode", StatusCode);
                    para.Add("@CustomSearch", CustomSearch, DbType.Object);
                    using (var multi = await c.QueryMultipleAsync(ClsProcedures.UspSMEAllocation, param: para, commandType: CommandType.StoredProcedure))
                    {
                        if (!multi.IsConsumed)
                        {
                            //Assignning result to variable
                            StatusCount = multi.Read().ToList();
                        }
                        if (!multi.IsConsumed)
                        {
                            //Assignning result to variable
                            AccountDetails = multi.Read().ToList();
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
                    return Tuple.Create(StatusCount, AccountDetails , RoleBasedCount);
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
                StatusCount = null;
                RoleBasedCount = null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="PHMID"></param>
        /// <param name="AccountID"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetAccountRuleInfo(int PHMID, String AccountID)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(ClsCommon._ConnectionString))
                {
                    //This is to add parameters
                    DynamicParameters para = new DynamicParameters();
                    para.Add("@PHMID", PHMID);
                    para.Add("@AccountID", AccountID);
                    return db.Query<dynamic>(ClsProcedures.UspGetAccountRuleDetails, param: para, commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<int> updateAccountStartedTime(int AccountID)
        {
            try
            {
                return await WithConnection(async c =>
                {
                    DynamicParameters para = new DynamicParameters();
                    para.Add("@AccountID", AccountID);

                    return c.ExecuteAsync(ClsProcedures.UspUpdateAccountStartedTime, param: para, commandType: CommandType.StoredProcedure, commandTimeout: 0).Result;

                });

            }
            catch (Exception ex)
            {

                return 0;
            }

        }
        public async Task<IEnumerable<dynamic>> SubmitSMETransaction(SubmitSMETransaction obj)
        {
            try
            {
                return await WithConnection(async c =>
                {
                    //This is to add parameters
                    DynamicParameters para = new DynamicParameters();
                    para.Add("@PHMID", obj.PHMID);
                    para.Add("@SupplyIDsJSONDATA", obj.AccountIDs, DbType.String);
                    para.Add("@AdditionalCaptureJSONDATA", obj.AdditionalCapture, DbType.String);
                    para.Add("@ScenarioMappingID", obj.ScenarioMappingID);
                    para.Add("@CallTypeID", obj.CallTypeID);
                    para.Add("@Note", obj.Note);
                    para.Add("@SoftwareNotes", obj.SoftwareNotes);
                    para.Add("@UserID", obj.UserID);
                    para.Add("@IsTempSave", obj.IsTempSave);
                    para.Add("@DeferDate", obj.DeferDate);
                    para.Add("@UserID", obj.UserID);
                    para.Add("@IsTempSave", obj.IsTempSave);
                    para.Add("@RoleCode", obj.RoleCode);
                    para.Add("@TimeTakenJSON", obj.TimeTakenJSON);
                    var result = await c.QueryAsync<dynamic>(ClsProcedures.UspSubmitSMETransaction, param: para, commandType: CommandType.StoredProcedure, commandTimeout: 0);

                    return result.ToList();
                });
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        /// <summary>
        /// This is to submit clarification
        /// </summary>
        /// <param name="AccountID"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> SubmitClarification(int PHMID, int UserID, string Query, string SubCategoryID, DataTable AccountID)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(ClsCommon._ConnectionString))
                {
                    DynamicParameters para = new DynamicParameters();
                    para.Add("@CHMapID", PHMID);
                    para.Add("@UserID", UserID);
                    para.Add("@Query", Query);
                    para.Add("@SubCategoryID", SubCategoryID);
                    para.Add("@AccountIDs", AccountID, DbType.Object);
                    return db.Query<dynamic>(ClsProcedures.UspSubmitSMEClarification, param: para, commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PHMID"></param>
        /// <param name="UserID"></param>
        /// <param name="AccountIDs"></param>
        /// <param name="Notes"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> SubmitNonWorkable(int PHMID, int UserID, String AccountIDs, string Notes)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(ClsCommon._ConnectionString))
                {
                    DynamicParameters para = new DynamicParameters();
                    para.Add("@PHMID", PHMID);
                    para.Add("@UserID", UserID);
                    para.Add("@AccountIDs", AccountIDs, DbType.String);
                    para.Add("@Notes", Notes);
                    return db.Query<dynamic>(ClsProcedures.UspSubmitNonWorkable, param: para, commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public IEnumerable<dynamic> GetClarification(int PHMID, int UserID, String AccountIDs)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(ClsCommon._ConnectionString))
                {
                    DynamicParameters para = new DynamicParameters();
                    para.Add("@PHMID", PHMID);
                    para.Add("@UserID", UserID);
                    para.Add("@AccountIDs", AccountIDs, DbType.String);

                    return db.Query<dynamic>(ClsProcedures.GetClarification, param: para, commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}