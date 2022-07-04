using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAR_API.Interface
{
   public interface ILogin
    {
        /// <summary>
        /// UpdateLogoutTime
        /// </summary>
        /// <param name="HistoryID"></param>
        /// <returns></returns>
        Task<int> UpdateLogoutTime(int HistoryID);
    }
}
