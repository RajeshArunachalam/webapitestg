using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAR_API.Interface
{
   public interface IReport
    {
        /// <summary>
        /// GetReportRDLFileDetails
        /// </summary>
        /// <param name="ReportURL"></param>
        /// <param name="PHMID"></param>
        /// <returns></returns>
        Task<IEnumerable<dynamic>> GetReportRDLFileDetails(string ReportURL, int PHMID);
        /// <summary>
        /// GetFileDetails
        /// </summary>
        /// <param name="PHMID"></param>
        /// <returns></returns>
        IEnumerable<dynamic> GetFileDetails(int PHMID);
    }
}
