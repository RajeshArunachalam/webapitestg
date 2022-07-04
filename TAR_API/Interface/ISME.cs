using TAR_API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAR_API.Interface
{
   public interface ISME
    {

        /// <summary>
        /// GetSMEPreLoadData
        /// </summary>
        /// <param name="PHMID"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        Task<Tuple<IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>>> GetSMEPreLoadData(int PHMID, int UserID);


        /// <summary>
        /// GetSMEAccounts
        /// </summary>
        /// <param name="PHMID"></param>
        /// <param name="UserID"></param>
        /// <param name="ImportFileID"></param>
        /// <param name="RoleCode"></param>
        /// <param name="StatusCode"></param>
        /// <returns></returns>
        Task<Tuple<IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>>> GetSMEAccounts(int PHMID, int UserID, string ImportFileID, string RoleCode, string StatusCode, DataTable CustomSearch);

        /// <summary>
        /// GetAccountRuleInfo
        /// </summary>
        /// <param name="PHMID"></param>
        /// <param name="AccountID"></param>
        /// <returns></returns>
        IEnumerable<dynamic> GetAccountRuleInfo(int PHMID, String AccountID);

        /// <summary>
        /// updateAccountStartedTime
        /// </summary>
        /// <param name="AccountID"></param>
        /// <returns></returns>
        Task<int> updateAccountStartedTime(int AccountID);

        /// <summary>
        /// SubmitSMETransaction
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        Task<IEnumerable<dynamic>> SubmitSMETransaction(SubmitSMETransaction obj);


    }
}
