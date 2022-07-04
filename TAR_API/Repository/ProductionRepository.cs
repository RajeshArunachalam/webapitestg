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
using static TAR_API.Controllers.ProductionController;

namespace TAR_API.Repository
{
    public class ProductionRepository:BaseRepository,IProduction
    {

      /// <summary>
      /// 
      /// </summary>
      /// <param name="PHMID"></param>
      /// <param name="UserID"></param>
      /// <returns></returns>
        public async Task<Tuple<IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>,  IEnumerable<dynamic>, IEnumerable<dynamic>, Tuple<IEnumerable<dynamic>>>> GetProductionPreLoadData(int PHMID, int UserID)
        {
            //These are the declaration of return objects

            IEnumerable<dynamic> objSupplyFields = null;
            IEnumerable<dynamic> objAdditionalCaptures = null;
            IEnumerable<dynamic> objScenario = null;
            IEnumerable<dynamic> objCallType = null;
            IEnumerable<dynamic> objAgingDetails = null;
            IEnumerable<dynamic> objConfiguration = null;
            IEnumerable<dynamic> objUsers = null;
            IEnumerable<dynamic> objBotServiceType = null;

            try
            {

                return await WithConnection(async c =>
                {
                    //This is to add parameters
                    var para = new DynamicParameters();
                    para.Add("@PHMID", PHMID);
                    para.Add("@UserID", UserID);

                    //This is to excute the command and return the result
                    using (var multi = await c.QueryMultipleAsync(ClsProcedures.UspGetProductionInboxPreLoadData, para, commandType: CommandType.StoredProcedure, commandTimeout: 0))
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
                        if (!multi.IsConsumed)
                        {
                            //Assignning Additional Caputures result
                            objBotServiceType = multi.Read().ToList();
                        }

                    }

                    //Values are retured in the form of Tuple with mutiple objects
                    return Tuple.Create(objSupplyFields, objAdditionalCaptures, objScenario, objCallType, objAgingDetails, objConfiguration,objUsers,objBotServiceType);
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
                objBotServiceType = null;
            }
        }
       /// <summary>
       /// 
       /// </summary>
       /// <param name="PHMID"></param>
       /// <param name="UserID"></param>
       /// <param name="ImportFileID"></param>
       /// <param name="RoleCode"></param>
       /// <param name="StatusCode"></param>
       /// <returns></returns>
        public async Task<Tuple<IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>>> GetAccounts(int PHMID, int UserID, string ImportFileID, string RoleCode, string StatusCode, DataTable CustomSearch)
        {
           // IEnumerable<dynamic> AgingDetails = null;
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

                    using (var multi = await c.QueryMultipleAsync(ClsProcedures.UspProductionAllocation, param: para, commandType: CommandType.StoredProcedure))
                    {                        //if (!multi.IsConsumed)
                        //{
                        //    //Assignning result to variable
                        //    AgingDetails = multi.Read().ToList();
                        //}

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
                    return Tuple.Create( StatusCount, AccountDetails, RoleBasedCount);
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
        public IEnumerable<dynamic> GetAccountRuleInfo(int PHMID,String AccountID)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(ClsCommon._ConnectionString))
                {
                    //This is to add parameters
                    DynamicParameters para = new DynamicParameters();
                    para.Add("@PHMID", PHMID);
                    para.Add("@AccountID", AccountID, DbType.String);
                    return db.Query<dynamic>(ClsProcedures.UspGetAccountRuleDetails, param: para, commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// This is to update Claim Started Time when agent start working.
        /// </summary>
        /// <param name="AccountID"></param>
        /// <returns></returns>
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

        public async Task<IEnumerable<dynamic>> PreAuditAcknowledgement(PreAudit obj)
        {
            try
            {
                return await WithConnection(async c =>
                {
                    DynamicParameters para = new DynamicParameters();
                    para.Add("@SupplyIDsJSONDATA", obj.SupplyIDsJSONDATA,DbType.String);
                    para.Add("@PHMID", obj.PHMID);
                    para.Add("@UserID", obj.UserID);
                    para.Add("@PreAuditAcknowledgement", obj.PreAuditAcknowledgement);
                    var result = await c.QueryAsync<dynamic>(ClsProcedures.uspSubmitPreAuditAcknowledgement, param: para, commandType: CommandType.StoredProcedure, commandTimeout: 0);

                    return result.ToList();
                    //var result= c.ExecuteAsync(ClsProcedures.uspSubmitPreAuditAcknowledgement, param: para, commandType: CommandType.StoredProcedure, commandTimeout: 0);

                });

            }
            catch (Exception ex)
            {

                return null;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<IEnumerable<dynamic>> SubmitAssociateTransaction(SubmitProductionTransaction obj)
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
                    var result = await c.QueryAsync<dynamic>(ClsProcedures.UspSubmitProductionTransaction, param: para, commandType: CommandType.StoredProcedure, commandTimeout: 0);

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
        /// <param name="InventoryID"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> SubmitClarification(int PHMID, int UserID, string Query, string SubCategoryID, string AccountID,string clarificationrole,string ClarificationTo)
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
                    para.Add("@AccountIDs", AccountID ,DbType.String);
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
        public IEnumerable<dynamic> SubmitRebuttal(int PHMID, int UserID, String AccountIDs, string Notes)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(ClsCommon._ConnectionString))
                {
                    DynamicParameters para = new DynamicParameters();
                    para.Add("@PHMID", PHMID);
                    para.Add("@UserID", UserID);
                    para.Add("@AccountIDs", AccountIDs, DbType.String);
                    para.Add("@RebuttalComments", Notes);
                    return db.Query<dynamic>(ClsProcedures.Uspupdaterebuttalaccount, param: para, commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public IEnumerable<dynamic> DocumentUploadSave(int PHMID, int UserID, string AccountID, string filename, string description, string filepath,string filetype)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(ClsCommon._ConnectionString))
                {
                    DynamicParameters para = new DynamicParameters();
                    para.Add("@AccountID", AccountID);
                    para.Add("@PHMID", PHMID);
                    para.Add("@FileName", filename);
                    para.Add("@FileDescription", description);
                    para.Add("@FilePath", filepath);
                    para.Add("@UploadedBy", UserID);
                    para.Add("@FileType", filetype);
                    return db.Query<dynamic>(ClsProcedures.UspInsertdocuments, param: para, commandType: CommandType.StoredProcedure).ToList();
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


        public IEnumerable<dynamic> GetClientDocuments(int PHMID, string AccountID)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(ClsCommon._ConnectionString))
                {
                    DynamicParameters para = new DynamicParameters();
                    para.Add("@PHMID", PHMID);
                   
                    para.Add("@AccountID", AccountID, DbType.String);

                    return db.Query<dynamic>(ClsProcedures.GetClientDocuments, param: para, commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public IEnumerable<dynamic> DocumentDelete(int FileID)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(ClsCommon._ConnectionString))
                {
                    DynamicParameters para = new DynamicParameters();
                    para.Add("@fileid", FileID);
                   
                    return db.Query<dynamic>(ClsProcedures.UspDeleteClientdocument, param: para, commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }



    }
}