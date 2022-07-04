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
    public class SupplyEntryRepository:BaseRepository,ISupplyEntry
    {
        public Tuple<IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>> SupplyEntryAccountLoadData(int PHMID)
        {
            try
            {
                //These are the declaration of return objects
                IEnumerable<dynamic> objSupplyFields = null;
                IEnumerable<dynamic> objScenarioDetails = null;
                IEnumerable<dynamic> objCalltype = null;
                IEnumerable<dynamic> objAdditinalCaptures = null;
                IEnumerable<dynamic> objConfiguration = null;
                IEnumerable<dynamic> objSupplyFieldsItems = null;
                using (IDbConnection db = new SqlConnection(ClsCommon._ConnectionString))
                {
                    //This is to add parameters
                    DynamicParameters para = new DynamicParameters();
                    para.Add("@PHMID", PHMID);

                    //This is to excute the command and return the result
                    using (var multi = db.QueryMultiple(ClsProcedures.UspGetSupplyEntryAccountLoadData, para, commandType: CommandType.StoredProcedure))
                    {
                        if (!multi.IsConsumed)
                        {
                            //Assignning result to variable
                            objSupplyFields = multi.Read().ToList();
                        }
                        if (!multi.IsConsumed)
                        {
                            //Assignning result to variable
                            objScenarioDetails = multi.Read().ToList();
                        }
                        if (!multi.IsConsumed)
                        {
                            //Assignning result to variable
                            objCalltype = multi.Read().ToList();
                        }
                        if (!multi.IsConsumed)
                        {
                            //Assignning result to variable
                            objAdditinalCaptures = multi.Read().ToList();
                        }
                        if (!multi.IsConsumed)
                        {
                            //Assignning result to variable
                            objConfiguration = multi.Read().ToList();
                        }
                        if (!multi.IsConsumed)
                        {
                            //Assignning result to variable
                            objSupplyFieldsItems = multi.Read().ToList();
                        }

                    }

                    //Values are retured in the form of Tuple with mutiple objects
                    return Tuple.Create(objSupplyFields, objScenarioDetails, objCalltype, objAdditinalCaptures, objConfiguration, objSupplyFieldsItems);
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
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<IEnumerable<dynamic>> SubmitSupplyEntryTransaction(SubmitSUpplyEntryTransaction obj)
        {
            try
            {
                return await WithConnection(async c =>
                {
                    //This is to add parameters
                    DynamicParameters para = new DynamicParameters();
                    para.Add("@PHMID", obj.PHMID);
                    para.Add("@UserID", obj.UserID);
                    para.Add("@SupplyIDsJSONDATA", obj.AccountIDs, DbType.String);
                    para.Add("@DistinctID", obj.DistinctID);
                    para.Add("@Note", obj.Note);
                    para.Add("@UserName", obj.UserName);
                    para.Add("@RoleCode", obj.RoleCode);
                    para.Add("@AdditionalCaptureJSONDATA", obj.AdditionalCapture, DbType.String);
                    para.Add("@SoftwareNotes", obj.SoftwareNotes);
                    para.Add("@ScenarioMappingID", obj.ScenarioMappingID);
                    para.Add("@CallTypeID", obj.CallTypeID);
                    para.Add("@IsTempSave", obj.IsTempSave);
                    para.Add("@DeferDate", obj.DeferDate);
                    para.Add("@RuleID", obj.RuleID);
                    // para.Add("@TimeTakenJSON", obj.TimeTakenJSON);
                    var result = await c.QueryAsync<dynamic>(ClsProcedures.UspSubmitSupplyEntryTransaction, param: para, commandType: CommandType.StoredProcedure, commandTimeout: 0);

                    return result.ToList();
                });
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public Tuple<IEnumerable<dynamic>, IEnumerable<dynamic>> GetManualAccountRuleOutcomeInfo(int PHMID, string SupplyIDsJSONDATA , int UserID )
        {
            try
            {
                //These are the declaration of return objects
                IEnumerable<dynamic> objRuleID = null;
                IEnumerable<dynamic> objRuleOutComeInfo = null;
               
                using (IDbConnection db = new SqlConnection(ClsCommon._ConnectionString))
                {
                    //This is to add parameters
                    DynamicParameters para = new DynamicParameters();
                    para.Add("@PHMID", PHMID);
                    para.Add("@SupplyIDsJSONDATA", SupplyIDsJSONDATA);
                    para.Add("@UserID", UserID);

                    //This is to excute the command and return the result
                    using (var multi = db.QueryMultiple(ClsProcedures.UspGetManualAccountRuleOutcomeInfo, para, commandType: CommandType.StoredProcedure))
                    {
                        if (!multi.IsConsumed)
                        {
                            //Assignning result to variable
                            objRuleID = multi.Read().ToList();
                        }
                        if (!multi.IsConsumed)
                        {
                            //Assignning result to variable
                            objRuleOutComeInfo = multi.Read().ToList();
                        }
                        
                    }

                    //Values are retured in the form of Tuple with mutiple objects
                    return Tuple.Create(objRuleID, objRuleOutComeInfo);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }



    }
}