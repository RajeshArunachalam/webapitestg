using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TAR_API.Interface
{
   public interface IMaster
    {
        /// <summary>
        /// ClientProjectMapping
        /// </summary>
        /// <param name="Mappinglist"></param>
        /// <returns></returns>
        int ClientProjectMapping(DataTable Mappinglist);
        /// <summary>
        /// GetMasterPreLoadData
        /// </summary>
        /// <returns></returns>
        Task<Tuple<IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>>> GetMasterPreLoadData();
        
            /// <summary>
            /// ChangeStatusOfPHM
            /// </summary>
            /// <param name="PHMID"></param>
            /// <param name="IsActive"></param>
            /// <returns></returns>
            Task<int> ChangeStatusOfPHM(int PHMID, bool IsActive);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Company"></param>
        /// <param name="Client"></param>
        /// <param name="Vertical"></param>
        /// <param name="Location"></param>
        /// <param name="Project"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        Task<int> AddClientDetails(string Company, string Client, string Vertical, string Location, string Project, string mode);

        /// <summary>
        /// GetSupplyType
        /// </summary>
        /// <returns></returns>
        IEnumerable<dynamic> GetSupplyType();

        /// <summary>
        /// GetUsersMappingDetails
        /// </summary>
        /// <param name="PHMID"></param>
        /// <param name="RoleCode"></param>
        /// <param name="EmpID"></param>
        /// <returns></returns>
        Task<Tuple<IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>>> GetUsersMappingDetails(int PHMID, String RoleCode, int EmpID);

        /// <summary>
        /// GetUserMasterPreLoadData
        /// </summary>
        /// <returns></returns>
        Task<Tuple<IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>>> GetUserMasterPreLoadData(int PHMID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PHMID"></param>
        /// <param name="EmployeeTypeID"></param>
        /// <param name="EmployeeID"></param>
        /// <param name="EmpName"></param>
        /// <param name="EmailID"></param>
        /// <param name="Designation"></param>
        /// <param name="Tenure"></param>
        /// <param name="LeadUserID"></param>
        /// <param name="ManagerUserID"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        int AddEmployeeDetails(int PHMID, int EmployeeTypeID, string EmployeeID, string EmpName, string EmailID, string Designation, int Tenure, int LeadUserID, int ManagerUserID, int UserID, int ShiftID);
        
            /// <summary>
            /// GetClientAccessMappingByEmpID
            /// </summary>
            /// <param name="Emp_ID"></param>
            /// <returns></returns>
            Task<IEnumerable<dynamic>> GetClientAccessMappingByEmpID(string Emp_ID, string EmailID);

        /// <summary>
        /// SaveMappUserData
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="loginNTLG"></param>
        /// <returns></returns>
        Task<IEnumerable<dynamic>> SaveMappUserData(DataTable dt, string loginNTLG);

        /// <summary>
        /// GetAccountHistoryDetails
        /// </summary>
        /// <param name="PHMID"></param>
        /// <param name="AccountNo"></param>
        /// <returns></returns>
        IEnumerable<dynamic> GetAccountHistoryDetails(int PHMID, string AccountNo);

        /// <summary>
        /// GetManualCaptureDetails
        /// </summary>
        /// <param name="PHMID"></param>
        /// <returns></returns>
        IEnumerable<dynamic> GetManualCaptureDetails(int PHMID);

        /// <summary>
        /// GetManualCaptureDetailsbyRole
        /// </summary>
        /// <param name="PHMID"></param>
        /// <returns></returns>
        IEnumerable<dynamic> GetManualCaptureDetailsbyRole(int PHMID);


        /// <summary>
        /// SaveManualCaptureOrder
        /// </summary>
        /// <param name="PHMID"></param>
        /// <param name="FieldID"></param>
        /// <param name="ControlTypeID"></param>
        /// <param name="DataTypeID"></param>
        /// <param name="CommaValues"></param>
        /// <param name="Required"></param>
        /// <returns></returns>
        int SaveManualCaptureOrder(int PHMID, int FieldID, string ControlTypeID, string DataTypeID, string CommaValues, bool Required);

        /// <summary>
        /// EnableOrDisableManualEntry
        /// </summary>
        /// <param name="ManualCaptureID"></param>
        /// <returns></returns>
        int EnableOrDisableManualEntry(int ManualCaptureID);

        /// <summary>
        /// GetBotInputDetails
        /// </summary>
        /// <param name="PHMID"></param>
        /// <returns></returns>
        IEnumerable<dynamic> GetBotInputDetails(int PHMID);

        /// <summary>
        /// SaveBotInputOrder
        /// </summary>
        /// <param name="FieldID"></param>
        /// <param name="InventroryPayerName"></param>
        /// <param name="BotInputName"></param>
        /// <param name="CreatedBy"></param>
        /// <param name="PHMID"></param>
        /// <returns></returns>
        int SaveBotInputOrder(int PHMID, int FieldID, string InventroryPayerName, string BotInputName, string CreatedBy);

        /// <summary>
        /// BotInputEnableOrDisable
        /// </summary>
        /// <param name="BotInputID"></param>
        /// <returns></returns>
        int BotInputEnableOrDisable(int BotInputID);

        /// <summary>
        /// GetBotInputPreLoadData
        /// </summary>
        /// <param name="PHMID"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        Task<Tuple<IEnumerable<dynamic>>> GetBotInputPreLoadData(int PHMID, int UserID);
    }

}




