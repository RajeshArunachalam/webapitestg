using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAR_API.Interface
{
   public interface IAllotment
    {

        /// <summary>
        /// GetworkallotmentPreLoadData
        /// </summary>
        /// <param name="PHMID"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        Task<Tuple<IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>>> GetworkallotmentPreLoadData(int PHMID, int UserID);

        /// <summary>
        /// GetAccounts
        /// </summary>
        /// <param name="PHMID"></param>
        /// <param name="UserID"></param>
        /// <param name="ImportFileID"></param>
        /// <param name="RoleCode"></param>
        /// <param name="StatusCode"></param>
        /// <returns></returns>
        Task<Tuple<IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>>> GetAccounts(int PHMID, int UserID, string ImportFileID, string RoleCode, string StatusCode, DataTable CustomSearch);

        /// <summary>
        /// AssignSupply
        /// </summary>
        /// <param name="SupplyData"></param>
        /// <param name="LoginUserID"></param>
        /// <param name="AllocationType"></param>
        /// <param name="NeedCallRemarks"></param>
        /// <returns></returns>
        Task<int> AssignSupply(String SupplyData, int LoginUserID, string AllocationType, string NeedCallRemarks,int PHMID);

        /// <summary>
        /// GetImportFileList
        /// </summary>
        /// <param name="PHMID"></param>
        /// <param name="activeMenu"></param>
        /// <returns></returns>
        Task<IEnumerable<dynamic>> GetImportFileList(int PHMID, string activeMenu);

        /// <summary>
        /// GetAccountStatusSummary
        /// </summary>
        /// <param name="PHMID"></param>
        /// <param name="UserID"></param>
        /// <param name="ImportFileID"></param>
        /// <param name="RoleCode"></param>
        /// <param name="StatusCode"></param>
        /// <returns></returns>
        Task<Tuple<IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>>> GetAccountStatusSummary(int PHMID, int UserID, int ImportFileID, string RoleCode, string StatusCode);

        /// <summary>
        /// GetFilterdataAccounts
        /// </summary>
        /// <param name="PHMID"></param>
        /// <param name="UserID"></param>
        /// <param name="ImportFileID"></param>
        /// <param name="RoleCode"></param>
        /// <param name="StatusCode"></param>
        /// <returns></returns>
        Task<Tuple<IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>>> GetFilterdataAccounts(int PHMID, int UserID, int ImportFileID, string RoleCode, string StatusCode);




    }
}
