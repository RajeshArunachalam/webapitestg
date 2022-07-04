using TAR_API.App_Code;
using TAR_API.Interface;
using TAR_API.Models;
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
    public class AuditRepository:BaseRepository,IAudit
    {

        public async Task<Tuple<IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>>> GetAssignedAccounts(Audit obj)
        {
            //These are the declaration of return objects
            IEnumerable<dynamic> objAccount = null;
            IEnumerable<dynamic> AccountDetails = null;
            IEnumerable<dynamic> AccountUserDetails = null;
            IEnumerable<dynamic> AuditRoleBasedCount = null;
            try
            {
                return await WithConnection(async c =>
                {
                    //This is to add parameters
                    DynamicParameters para = new DynamicParameters();
                    para.Add("@PHMID", obj.PHMID);
                    para.Add("@ImportFileID", obj.ImportFileID);
                    para.Add("@UserID", obj.UserID);
                    para.Add("@StatusCode", obj.StatusCode);
                    para.Add("@DispositionID", obj.DispositionID);
                    para.Add("@SubDispositionID", obj.SubDispositionID);
                    para.Add("@ActionCodeID", obj.ActionCodeID);
                    para.Add("@CallTypeID", obj.CallTypeID);
                    para.Add("@Associate", obj.Associate);
                    para.Add("@Payer", obj.Payer);
                    para.Add("@FromDate", obj.FromDate);
                    para.Add("@ToDate", obj.ToDate);
                    para.Add("@AssociateKeyNoteSearch", obj.AssociateKeyNoteSearch);
                    
                    //This is to excute the command and return the result
                    using (var multi = await c.QueryMultipleAsync(ClsProcedures.UspGetAuditAccounts, para, commandType: CommandType.StoredProcedure, commandTimeout: 0))
                    {
                        if (!multi.IsConsumed)
                        {
                            //Assignning result to variable
                            objAccount = multi.Read().ToList();
                        }
                        if (!multi.IsConsumed)
                        {
                            //Assignning result to variable
                            AccountDetails = multi.Read().ToList();
                        }
                        if (!multi.IsConsumed)
                        {
                            //Assignning result to variable
                            AccountUserDetails = multi.Read().ToList();
                        }
                    }


                    //This is to add parameters
                    DynamicParameters paraNew = new DynamicParameters();
                    paraNew.Add("@PHMID", obj.PHMID);
                    paraNew.Add("@UserID", obj.UserID);
                    //This is to excute the command and return the result
                    using (var multi = await c.QueryMultipleAsync(ClsProcedures.UspDashboardrolewise, paraNew, commandType: CommandType.StoredProcedure, commandTimeout: 0))
                    {
                        if (!multi.IsConsumed)
                        {
                            //Assignning result to variable
                            AuditRoleBasedCount = multi.Read().ToList();
                        }
                    }
                    //Values are retured in the form of Tuple with mutiple objects
                    return Tuple.Create(objAccount, AccountDetails, AccountUserDetails, AuditRoleBasedCount);
                });
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                objAccount = null;
                AccountDetails = null;
                AccountUserDetails = null;
                AuditRoleBasedCount = null;
            }
        }
        public async Task<Tuple<IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>>> GetAuditPreLoadData(int PHMID, int UserID)
        {
            //These are the declaration of return objects
            IEnumerable<dynamic> objSupplyFields = null;
            IEnumerable<dynamic> objAdditionalCaptures = null;
            IEnumerable<dynamic> objScenario = null;
            IEnumerable<dynamic> objCallType = null;
            IEnumerable<dynamic> objAgingDetails = null;
            IEnumerable<dynamic> objConfiguration = null;
            try
            {

                return await WithConnection(async c =>
                {
                    //This is to add parameters
                    var para = new DynamicParameters();
                    para.Add("@PHMID", PHMID);
                    para.Add("@UserID", UserID);

                    //This is to excute the command and return the result
                    using (var multi = await c.QueryMultipleAsync(ClsProcedures.UspGetAuditInboxPreLoadData, para, commandType: CommandType.StoredProcedure, commandTimeout: 0))
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
                    }

                    //Values are retured in the form of Tuple with mutiple objects
                    return Tuple.Create(objSupplyFields, objAdditionalCaptures, objScenario, objCallType, objAgingDetails, objConfiguration);
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


        public IEnumerable<dynamic> SubmitClarificationResponse(int UserID, string ResponseComment, string ResponseDescription, int SubCategoryID, string AccountID)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(ClsCommon._ConnectionString))
                {
                    DynamicParameters para = new DynamicParameters();
                  
                    para.Add("@UserID", UserID);
                    para.Add("@ResponseComment", ResponseComment);
                    para.Add("@ResponseDescription", ResponseDescription);
                    para.Add("@SubCategoryID", SubCategoryID);
                    para.Add("@AccountIDs", AccountID, DbType.String);
                   

                    return db.Query<dynamic>(ClsProcedures.UspSubmitClarificationResponse, param: para, commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public async Task<IEnumerable<dynamic>> SubmitAuditTransaction(SubmitAuditTransaction obj)
        {
            try
            {
                return await WithConnection(async c =>
                {
                    //This is to add parameters
                    DynamicParameters para = new DynamicParameters();
                    para.Add("@PHMID", obj.PHMID);
                    para.Add("@SupplysJSONDATA", obj.AccountIDs, DbType.String);
                    para.Add("@ErrorMappingID", obj.ErrorMappingID);
                    para.Add("@CorrectiveNote", obj.CorrectiveNote);
                    para.Add("@Note", obj.Note);
                    para.Add("@UserID", obj.UserID);
                    para.Add("@IsReject", obj.IsReject);
                    para.Add("@IsBulk", obj.IsBulk);
                    para.Add("@ErrorList", obj.ErrorList);
                    para.Add("@AuditList", obj.AuditList);
                    para.Add("@TimeTakenJSON", obj.TimeTakenJSON);
                    var result = await c.QueryAsync<dynamic>(ClsProcedures.UspSubmitAuditTransaction, param: para, commandType: CommandType.StoredProcedure, commandTimeout: 0);

                    return result.ToList();
                });
            }
            catch (Exception ex)
            {

                return null;
            }
        }


        public async Task<IEnumerable<dynamic>> editAuditTransaction(EditAuditTransaction obj)
        {
            try
            {
                return await WithConnection(async c =>
                {
                    //This is to add parameters
                    DynamicParameters para = new DynamicParameters();
                    para.Add("@PHMID", obj.PHMID);
                    para.Add("@UserID", obj.UserID);
                    para.Add("@SupplysJSONDATA", obj.AccountIDs, DbType.String);
                    para.Add("@AssociateNotes", obj.AssociateNotes);
                    para.Add("@DispositionID", obj.DispositionID);
                    para.Add("@SubDispositionID", obj.SubDispositionID);
                    para.Add("@ActionCode", obj.ActionCode);
                    para.Add("@CallType", obj.CallType);
                    var result = await c.QueryAsync<dynamic>(ClsProcedures.uspEditAuditTransaction, param: para, commandType: CommandType.StoredProcedure, commandTimeout: 0);

                    return result.ToList();
                });
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public async Task<Tuple<IEnumerable<dynamic>>> GetAuditErrorPreLoadData(int PHMID)
        {
            //These are the declaration of return objects
            IEnumerable<dynamic> objAuditErrorDetails = null;
            try
            {
                return await WithConnection(async c =>
                {
                    //This is to add parameters
                    var para = new DynamicParameters();
                    para.Add("@PHMID", PHMID);


                    //This is to excute the command and return the result
                    using (var multi = await c.QueryMultipleAsync(ClsProcedures.UspGetErrorCategoryMappedDetails, para, commandType: CommandType.StoredProcedure, commandTimeout: 0))
                    {

                        if (!multi.IsConsumed)
                        {
                            //Assignning Configuration result
                            objAuditErrorDetails = multi.Read().ToList();
                        }

                    }

                    //Values are retured in the form of Tuple with mutiple objects
                    return Tuple.Create(objAuditErrorDetails);
                });

            }
            catch (Exception ex)
            {

                return null;
            }
            finally
            {
                objAuditErrorDetails = null;
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


    }
}