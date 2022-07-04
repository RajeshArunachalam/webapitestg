using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAR_API.Interface
{
   public interface IRuleBuilder
    {
        /// <summary>
        /// getMasterRule
        /// </summary>
        /// <param name="PHMID"></param>
        /// <param name="StatusID"></param>
        /// <returns></returns>
        Task<Tuple<IEnumerable<dynamic>>> getMasterRule(int PHMID, int StatusID);

        /// <summary>
        /// GetUsers
        /// </summary>
        /// <param name="PHMID"></param>
        /// <param name="AssignStatusCode"></param>
        /// <returns></returns>
        Task<Tuple<IEnumerable<dynamic>>> GetUsers(int PHMID, string AssignStatusCode);

        /// <summary>
        /// GetUsersPerRule
        /// </summary>
        /// <param name="RuleID"></param>
        /// <returns></returns>
        IEnumerable<dynamic> GetUsersPerRule(int RuleID);

        /// <summary>
        /// GetRuleBuilderProject
        /// </summary>
        /// <param name="PHMID"></param>
        /// <returns></returns>
        IEnumerable<dynamic> GetRuleBuilderProject(int PHMID);

        /// <summary>
        /// GetImportFilesByPHMID
        /// </summary>
        /// <param name="PHMID"></param>
        /// <returns></returns>
        IEnumerable<dynamic> GetImportFilesByPHMID(int PHMID);

        /// <summary>
        /// GetAccountDetailsByImportFileID
        /// </summary>
        /// <param name="importFileID"></param>
        /// <param name="PHMID"></param>
        /// <returns></returns>
        IEnumerable<dynamic> GetAccountDetailsByImportFileID(int importFileID, int PHMID);

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
        /// <param name="RuleTypeID"></param>
        /// <param name="ConclusiveOutcome"></param>
        /// <param name="AdjustmentCode"></param>
        /// <param name="TransferCode"></param>
        /// <param name="ResubmissionCode"></param>
        /// <param name="BillPatientCode"></param>
        /// <param name="Defer_TicklerDays"></param>
        /// <param name="ActionInstructionToSME"></param>
        /// <param name="AutoNotes"></param>
        /// <param name="AssignStatusCode"></param>
        /// <param name="StatusCode"></param>
        /// <param name="ActionCode"></param>
        /// <returns></returns>
        int AddNewRule(int PHMID, string condition, string DisplayCondition, string ImportFileId, string RuleName, string RuleDesc, string CreatedBy, int RuleTypeID, string ConclusiveOutcome, string AdjustmentCode, string TransferCode, string ResubmissionCode, string BillPatientCode, string Defer_TicklerDays, string ActionInstructionToSME, string AutoNotes, string AssignStatusCode, string StatusCode, string ActionCode);

        /// <summary>
        /// getRuletype
        /// </summary>
        /// <returns></returns>
        Task<Tuple<IEnumerable<dynamic>, IEnumerable<dynamic>>> getRuletype();

        /// <summary>
        /// ChangeRuleStatus
        /// </summary>
        /// <param name="RuleID"></param>
        /// <param name="IsActive"></param>
        /// <param name="UserName"></param>
        /// <returns></returns>
        Task<int> ChangeRuleStatus(int RuleID, bool IsActive, string UserName);
        /// <summary>
        /// AssignRuletoUser
        /// </summary>
        /// <param name="Mappinglist"></param>
        /// <returns></returns>
        int AssignRuletoUser(DataTable Mappinglist);
    }
}
