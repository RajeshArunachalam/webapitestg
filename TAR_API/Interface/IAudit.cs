using TAR_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAR_API.Interface
{
   public interface IAudit
    {
        /// <summary>
        /// GetAssignedAccounts
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        Task<Tuple<IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>>> GetAssignedAccounts(Audit obj);

        /// <summary>
        /// GetAuditPreLoadData
        /// </summary>
        /// <param name="PHMID"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        Task<Tuple<IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>>> GetAuditPreLoadData(int PHMID, int UserID);

        /// <summary>
        /// SubmitAuditTransaction
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        Task<IEnumerable<dynamic>> SubmitAuditTransaction(SubmitAuditTransaction obj);

        /// <summary>
        /// GetAuditErrorPreLoadData
        /// </summary>
        /// <param name="PHMID"></param>
        /// <returns></returns>
        Task<Tuple<IEnumerable<dynamic>>> GetAuditErrorPreLoadData(int PHMID);

    }
}
