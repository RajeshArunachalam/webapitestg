using TAR_API.App_Code;
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
    public class SupplyRepository : BaseRepository
    {
        /// <summary>
        /// To get mapping details based on PHMID
        /// </summary>
        /// <param name="PHMID"></param>
        /// <returns></returns>
        public async Task<IEnumerable<dynamic>> GetFieldMappingDetails(int PHMID)
        {
            try
            {
                return await WithConnection(async c =>
                {
                    DynamicParameters para = new DynamicParameters();
                    para.Add("@PHMID", PHMID);
                    var result = await c.QueryAsync<dynamic>(ClsProcedures.UspGetFieldMappingDetails, param: para, commandType: CommandType.StoredProcedure, commandTimeout: 0);
                    return (result.ToList());

                });
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// To Save field mapping details
        /// </summary>
        /// <param name="PHMID"></param>
        /// <param name="LoginName"></param>
        /// <param name="FieldMapping"></param>
        /// <returns></returns>
        public async Task<IEnumerable<dynamic>> saveSupplyFieldMapping(int PHMID, string UserName, DataTable FieldMapping)
        {
            try
            {
                //These are the declaration of return objects
                //return null;
                return await WithConnection(async c => {
                    DynamicParameters para = new DynamicParameters();
                    para.Add("@PHMID", PHMID);
                    para.Add("@UserName", UserName);
                    para.Add("@SupFieldMap", FieldMapping, DbType.Object);
                    var result = await c.QueryAsync<dynamic>(ClsProcedures.UspSaveMappedFieldDetails, para, commandType: CommandType.StoredProcedure, commandTimeout: 0);
                    return result.ToList();
                });
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// This is to get the Template for column mappings
        /// </summary>
        /// <param name="InventoryTypeID"></param>
        /// <param name="CHMapID"></param>
        /// <returns></returns>
        public async Task<List<FieldMappings>> GetTemplate(int SupplyTypeID, int PHMID)
        {
            try
            {
                return await WithConnection(async c =>
                {
                    //This is to add parameters
                    DynamicParameters para = new DynamicParameters();
                    para.Add("@SupplyTypeID", SupplyTypeID);
                    para.Add("@PHMID", PHMID);
                    var result = await c.QueryAsync<FieldMappings>(ClsProcedures.UspGetFieldMappings, param: para, commandType: CommandType.StoredProcedure, commandTimeout: 0);

                    return result.ToList();

                });

            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public async Task<int> SSISPackageExecution(string ExcelFilePath, string Filename, string RenamedFilename, int PHMID, int SupplyTypeID, string UploadModeCode,  string UserName)
        {
            int returnval = 0;
            try
            {
                //These are the declaration of return objects
                string ExecutionStatus = string.Empty;

                return await WithConnection(async c => {

                    //This is to add parameters
                    DynamicParameters para = new DynamicParameters();
                    para.Add("@Sharedpath", ExcelFilePath);
                    para.Add("@Filename", Filename);
                    para.Add("@PHMID", PHMID);
                    para.Add("@SupplyTypeID", SupplyTypeID);
                    para.Add("@RenamedFilename", RenamedFilename);
                    para.Add("@UploadModeCode", UploadModeCode);
                    para.Add("@UserName", UserName);
                    IEnumerable<int> obj = await c.QueryAsync<int>(ClsProcedures.UspSSISPackageExecution, para, commandType: CommandType.StoredProcedure, commandTimeout: 0);

                    if (obj.Count() > 0)
                    {
                        foreach (int value in obj)
                        {
                            returnval = value;
                        }
                    }

                    return returnval;


                });

            }
            catch (Exception ex)
            {

                return returnval;
            }
        }

        /// <summary>
        ///  This method is used to get all the Supply column in an excel template
        /// </summary>
        /// <param name="PHMID"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetSupplyTemplate(int PHMID)
        {
            //IEnumerable<dynamic> objResult = null;
            try
            {
                using (IDbConnection db = new SqlConnection(ClsCommon._ConnectionString))
                {
                    DynamicParameters para = new DynamicParameters();
                    para.Add("@PHMID", PHMID);
                    return db.Query<dynamic>(ClsProcedures.UspGetSupplyTemplate, para, commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                //objResult = null;
            }
        }

        /// <summary>
        /// This is to get the Imported file details
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="clientID"></param>
        /// <param name="locationID"></param>
        /// <param name="practiceID"></param>
        /// <param name="recordType"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetImportFileDetails(int userID, string fromDate, string toDate, int clientID, int locationID, int projectID, string recordType)
        {
            using (IDbConnection db = new SqlConnection(ClsCommon._ConnectionString))
            {
                DynamicParameters para = new DynamicParameters();
                para.Add("@UserID", userID);
                para.Add("@FromDate", fromDate);
                para.Add("@ToDate", toDate);
                para.Add("@ClientID", clientID);
                para.Add("@LocationID", locationID);
                para.Add("@ProjectID", projectID);
                para.Add("@RecordType", recordType);
                return db.Query<dynamic>(ClsProcedures.UspGetImportFileDetails, para, commandType: CommandType.StoredProcedure).ToList();
            }
        }

        /// <summary>
        /// This is to get the Supply Error records for the uploaded Supply file
        /// </summary>
        /// <param name="ImportFileID"></param>
        /// <returns></returns>

        public IEnumerable<dynamic> GetSupplyErrorRecords(int ImportFileID)
        {
            using (IDbConnection db = new SqlConnection(ClsCommon._ConnectionString))
            {
                DynamicParameters para = new DynamicParameters();
                para.Add("@ImportFileID", ImportFileID);
                return db.Query<dynamic>(ClsProcedures.UspGetErrorRecords, para, commandType: CommandType.StoredProcedure);

            }
        }

    }
}