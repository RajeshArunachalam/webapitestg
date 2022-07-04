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
    public class RuleBuilderRepository : BaseRepository, IRuleBuilder
    {
        /// <summary>
        /// To get  Master Preload Data 
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public async Task<Tuple< IEnumerable<dynamic>>> getMasterRule(int PHMID,int StatusID)
        {


            IEnumerable<dynamic> AllRules = null;
           
            try
            {
                return await WithConnection(async c =>
                {
                    DynamicParameters para = new DynamicParameters();
                    para.Add("@PHMID", PHMID);
                    para.Add("@StatusID", StatusID);
                    var multi = await c.QueryMultipleAsync(ClsProcedures.UspGetMasterRule, para, commandType: CommandType.StoredProcedure);
                    if (!multi.IsConsumed)
                    {
                        //Assignning result to variable
                        AllRules = multi.Read().ToList();
                    }
                    return Tuple.Create(AllRules);
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

                AllRules = null;
              

            }
        }

        public async Task<Tuple<IEnumerable<dynamic>>> GetUsers(int PHMID,string AssignStatusCode)
        {


            IEnumerable<dynamic> AllRules = null;

            try
            {
                return await WithConnection(async c =>
                {
                    DynamicParameters para = new DynamicParameters();
                    para.Add("@PHMID", PHMID);
                    para.Add("@AssignStatusCode", AssignStatusCode);
                    
                    var multi = await c.QueryMultipleAsync(ClsProcedures.UspGetUserDetailsByPHMID, para, commandType: CommandType.StoredProcedure);
                    if (!multi.IsConsumed)
                    {
                        //Assignning result to variable
                        AllRules = multi.Read().ToList();
                    }
                    return Tuple.Create(AllRules);
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

                AllRules = null;


            }
        }


        public async Task<Tuple<IEnumerable<dynamic>>> ManualRuleExecution(int PHMID, int RuleID,int ImportFileID)
        {


            IEnumerable<dynamic> AllRules = null;

            try
            {
                return await WithConnection(async c =>
                {
                    DynamicParameters para = new DynamicParameters();
                    para.Add("@PHMID", PHMID);
                    para.Add("@ImportFileID", ImportFileID);
                    para.Add("@ManualRuleID", RuleID);

                    var multi = await c.QueryMultipleAsync(ClsProcedures.uspRuleAutoAllocation, para, commandType: CommandType.StoredProcedure);
                    if (!multi.IsConsumed)
                    {
                        //Assignning result to variable
                        AllRules = multi.Read().ToList();
                    }
                    return Tuple.Create(AllRules);
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

                AllRules = null;


            }
        }

        public IEnumerable<dynamic> GetUsersPerRule(int RuleID)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(ClsCommon._ConnectionString))
                {
                    //This is to add parameters
                    DynamicParameters para = new DynamicParameters();
                    para.Add("@RuleID", RuleID);
                    return db.Query<dynamic>(ClsProcedures.UspGetMappedUsersForRule, param: para, commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        /// <summary>
        /// GET Rule Builder Project BASED ON 
        /// </summary>
        /// <param name="PHMID"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetRuleBuilderProject(int PHMID)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(ClsCommon._ConnectionString))
                {
                    //This is to add parameters
                    DynamicParameters para = new DynamicParameters();
                    para.Add("@PHMID", PHMID);
                    return db.Query<dynamic>(ClsProcedures.UspGetRuleBuilderProject, param: para, commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// GET Rule Builder Project BASED ON 
        /// </summary>
        /// <param name="PHMID"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetImportFilesByPHMID(int PHMID)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(ClsCommon._ConnectionString))
                {
                    //This is to add parameters
                    DynamicParameters para = new DynamicParameters();
                    para.Add("@PHMID", PHMID);
                    return db.Query<dynamic>(ClsProcedures.UspGetImportFilesByPHMID, param: para, commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// GetAccountDetailsByImportFileID
        /// </summary>
        /// <param name="importFileID"></param>
        /// <param name="PHMID"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetAccountDetailsByImportFileID(int importFileID,int PHMID)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(ClsCommon._ConnectionString))
                {
                    //This is to add parameters
                    DynamicParameters para = new DynamicParameters();
                    para.Add("@ImportFileID", importFileID);
                    para.Add("@PHMID", PHMID);
                    return db.Query<dynamic>(ClsProcedures.UspGetAccountDetailsByImportFileID, param: para, commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        

        /// <summary>
        /// AddNewRule
        /// </summary>
        /// <param name="PHMID"></param>
        /// <param name="condition"></param>
        /// <param name="DisplayCondition"></param>
        /// <param name="ImportFileId"></param>
        /// <param name="RuleName"></param>
        /// <param name="RuleDesc"></param>
        /// <param name="CreatedBy"></param>
        /// <returns></returns>
        public int AddNewRule( int PHMID, string condition, string DisplayCondition, string ImportFileId, string RuleName, string RuleDesc, string CreatedBy,int RuleTypeID,string ConclusiveOutcome,string AdjustmentCode,string TransferCode,string ResubmissionCode,string BillPatientCode,string Defer_TicklerDays,string ActionInstructionToSME,string AutoNotes,string AssignStatusCode, string StatusCode,string ActionCode)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(ClsCommon._ConnectionString))
                {
                    //This is to add parameters
                    DynamicParameters para = new DynamicParameters();
                   
                    para.Add("@PHMID", PHMID);
                    para.Add("@Condition", condition);
                    para.Add("@DisplayCondition", DisplayCondition);
                    para.Add("@ImportFileId", ImportFileId);
                    para.Add("@RuleName", RuleName);
                    para.Add("@RuleDesc", RuleDesc);
                    para.Add("@CreatedBy", CreatedBy);
                    para.Add("@RuleTypeID", RuleTypeID);
                    para.Add("@ConclusiveOutcome", ConclusiveOutcome);
                    para.Add("@AdjustmentCode", AdjustmentCode);
                    para.Add("@TransferCode", TransferCode);
                    para.Add("@ResubmissionCode", ResubmissionCode);
                    para.Add("@BillPatientCode", BillPatientCode);
                    para.Add("@Defer_TicklerDays", Defer_TicklerDays);
                    para.Add("@ActionInstructionToSME", ActionInstructionToSME);
                    para.Add("@AutoNotes", AutoNotes);
                    para.Add("@AssignStatusCode", AssignStatusCode);
                    para.Add("@StatusCode", StatusCode);
                    para.Add("@ActionCode", ActionCode);
                 
                    return db.Query<int>(ClsProcedures.UspAddNewRule, param: para, commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public async Task<Tuple<IEnumerable<dynamic>, IEnumerable<dynamic>>> getRuletype()
        {

            IEnumerable<dynamic> RuletypeDetails = null;
            IEnumerable<dynamic> StatusDetails = null;

            try
            {
                return await WithConnection(async c =>
                {

                    var multi = await c.QueryMultipleAsync(ClsProcedures.UspGetRuletypeDetails, commandType: CommandType.StoredProcedure);
                    if (!multi.IsConsumed)
                    {

                        //Assignning result to variable
                        RuletypeDetails = multi.Read().ToList();

                        //Assignning result to variable
                        StatusDetails = multi.Read().ToList();


                    }
                    return Tuple.Create(RuletypeDetails, StatusDetails);
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

                RuletypeDetails = null;
             
            }
        }

        public async Task<int> ChangeRuleStatus(int RuleID, bool IsActive,string UserName)
        {
            try
            {
                return await WithConnection(async c =>
                {
                    //This is to add parameters
                    DynamicParameters para = new DynamicParameters();
                    para.Add("@RuleID", RuleID);
                    para.Add("@IsActive", IsActive);
                    para.Add("@UserName", UserName);
                    return c.ExecuteAsync(ClsProcedures.UspChangeRuleStatus, param: para, commandType: CommandType.StoredProcedure).Result;

                });

            }
            catch (Exception ex)
            {

                return 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Mappinglist"></param>
        /// <returns></returns>
        public int AssignRuletoUser(DataTable Mappinglist)
        {
            try
            {

                using (IDbConnection db = new SqlConnection(ClsCommon._ConnectionString))
                {
                    //This is to add parameters
                    DynamicParameters para = new DynamicParameters();
                    para.Add("@dt", Mappinglist, DbType.Object);
                    return db.Query<int>(ClsProcedures.UspAssignUserToRule, param: para, commandType: CommandType.StoredProcedure).FirstOrDefault();
                }


            }
            catch (Exception ex)
            {
                //  
                return 0;
            }

        }

    }
}