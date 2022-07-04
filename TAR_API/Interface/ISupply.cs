using TAR_API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAR_API.Interface
{
    interface ISupply
    {
        /// <summary>
        /// GetFieldMappingDetails
        /// </summary>
        /// <param name="PHMID"></param>
        /// <returns></returns>
        Task<IEnumerable<dynamic>> GetFieldMappingDetails(int PHMID);

        /// <summary>
        /// saveSupplyFieldMapping
        /// </summary>
        /// <param name="PHMID"></param>
        /// <param name="UserName"></param>
        /// <param name="FieldMapping"></param>
        /// <returns></returns>
        Task<IEnumerable<dynamic>> saveSupplyFieldMapping(int PHMID, string UserName, DataTable FieldMapping);

        /// <summary>
        /// GetTemplate
        /// </summary>
        /// <param name="SupplyTypeID"></param>
        /// <param name="PHMID"></param>
        /// <returns></returns>
        Task<List<FieldMappings>> GetTemplate(int SupplyTypeID, int PHMID);

        /// <summary>
        /// SSISPackageExecution
        /// </summary>
        /// <param name="ExcelFilePath"></param>
        /// <param name="Filename"></param>
        /// <param name="RenamedFilename"></param>
        /// <param name="PHMID"></param>
        /// <param name="SupplyTypeID"></param>
        /// <param name="UploadModeCode"></param>
        /// <param name="UserName"></param>
        /// <returns></returns>
        Task<int> SSISPackageExecution(string ExcelFilePath, string Filename, string RenamedFilename, int PHMID, int SupplyTypeID, string UploadModeCode, string UserName);

        /// <summary>
        /// GetSupplyTemplate
        /// </summary>
        /// <param name="PHMID"></param>
        /// <returns></returns>
        IEnumerable<dynamic> GetSupplyTemplate(int PHMID);

        /// <summary>
        /// GetImportFileDetails
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="clientID"></param>
        /// <param name="locationID"></param>
        /// <param name="projectID"></param>
        /// <param name="recordType"></param>
        /// <returns></returns>
        IEnumerable<dynamic> GetImportFileDetails(int userID, string fromDate, string toDate, int clientID, int locationID, int projectID, string recordType);

        /// <summary>
        /// GetSupplyErrorRecords
        /// </summary>
        /// <param name="ImportFileID"></param>
        /// <returns></returns>
        IEnumerable<dynamic> GetSupplyErrorRecords(int ImportFileID);
    }
}
