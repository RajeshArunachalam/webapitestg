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
    public class MasterRepository:BaseRepository,IMaster
    {

        #region User Master
        /// <summary>
        /// This method is used to get all the user details
        /// </summary>
        /// <param name="Ntlg"></param>
        /// <param name="MachineName"></param>
        /// <param name="Application"></param>
        /// <param name="SuperUser"></param>
        /// <param name="ApplicationVerion"></param>
        /// <returns></returns>

        public async Task<IEnumerable<dynamic>> GetUserDetails(string UserName, string MachineName, string Application, bool SuperUser, string ApplicationVerion)
        {
            try
            {
                return await WithConnection(async c =>
                {
                    var para = new DynamicParameters();
                    para.Add("@UserName", UserName);
                    para.Add("@MachineName", MachineName);
                    para.Add("@Application", Application);
                    para.Add("@IsSuperUser", SuperUser);
                    para.Add("@ApplicationVersion", ApplicationVerion);
                    var people = await c.QueryAsync<dynamic>(ClsProcedures.UspGetUserDetails, param: para, commandType: CommandType.StoredProcedure);
                    return people.ToList();
                });
            }
            catch (Exception ex)
            {
                // 
                return null;
            }
        }

        #endregion

        #region "CLIENT MAPPING"

        /// <summary>
        /// This method is used to get the client project mapping details
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="LocalIPAddress"></param>
        /// <param name="ApplicationVersion"></param>
        /// <returns></returns>
        public async Task<IEnumerable<dynamic>> GetClientProjectMapping(int UserID, string LocalIPAddress, string ApplicationVersion)
        {
            try
            {
                return await WithConnection(async c =>
                {
                    //This is to add parameters
                    DynamicParameters para = new DynamicParameters();
                    para.Add("@UserID", UserID);
                    para.Add("@LocalIPAddress", LocalIPAddress);
                    para.Add("@ApplicationVersion", ApplicationVersion);
                    var result = await c.QueryAsync<dynamic>(ClsProcedures.UspGetClientProjectMapping, param: para, commandType: CommandType.StoredProcedure);

                    return (result.ToList());
                });
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion

        /// <summary>
        /// This method is to client hierarchy mapping
        /// </summary>
        /// <param name="Mappinglist"></param>
        /// <returns></returns>
        public int ClientProjectMapping(DataTable Mappinglist)
        {
            try
            {
                
                using (IDbConnection db = new SqlConnection(ClsCommon._ConnectionString))
                {
                    //This is to add parameters
                    DynamicParameters para = new DynamicParameters();
                    para.Add("@dt", Mappinglist, DbType.Object);
                    return db.Query<int>(ClsProcedures.UspClientProjectMapping, param: para, commandType: CommandType.StoredProcedure).FirstOrDefault();
                }


            }
            catch (Exception ex)
            {
                //  
                return 0;
            }

        }

        /// <summary>
        /// To get  Master Preload Data 
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public async Task<Tuple<IEnumerable<dynamic>,  IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>>> GetMasterPreLoadData()
        {
            
            IEnumerable<dynamic> ClientDetails = null;
            IEnumerable<dynamic> LocationDetails = null;
            IEnumerable<dynamic> ProjectDetails = null;
            IEnumerable<dynamic> AllMappingDetails = null;
            IEnumerable<dynamic> CompanyDetails = null;
            IEnumerable<dynamic> VerticalDetails = null;
            try
            {
                return await WithConnection(async c =>
                {

                    var multi = await c.QueryMultipleAsync(ClsProcedures.UspGetMasterPreLoadData, commandType: CommandType.StoredProcedure);
                    if (!multi.IsConsumed)
                    {

                        //Assignning result to variable
                        
                        ClientDetails = multi.Read().ToList();
                        //Assignning result to variable
                        LocationDetails = multi.Read().ToList();
                        //Assignning result to variable
                        ProjectDetails = multi.Read().ToList();
                        //Assignning result to variable
                        AllMappingDetails = multi.Read().ToList();
                        CompanyDetails = multi.Read().ToList();
                        VerticalDetails = multi.Read().ToList();
                    }
                    return Tuple.Create(ClientDetails, LocationDetails, ProjectDetails, AllMappingDetails, CompanyDetails, VerticalDetails);
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
                
                ClientDetails = null;
                LocationDetails = null;
                ProjectDetails = null;
                AllMappingDetails = null;
                CompanyDetails = null;
                VerticalDetails = null;
            }
        }

        /// <summary>
        /// This is to save token details.
        /// </summary>
        /// <param name="Username"></param>
        /// <param name="access_token"></param>
        /// <returns></returns>
        public async Task<int> ChangeStatusOfPHM(int PHMID, bool IsActive)
        {
            try
            {
                return await WithConnection(async c =>
                {
                    //This is to add parameters
                    DynamicParameters para = new DynamicParameters();
                    para.Add("@PHMID", PHMID);
                    para.Add("@IsActive", IsActive);
                    return c.ExecuteAsync(ClsProcedures.UspChangeStatusOfPHM, param: para, commandType: CommandType.StoredProcedure).Result;

                });

            }
            catch (Exception ex)
            {

                return 0;
            }
        }

        public async Task<int> changeStatusOfCHM(int PHMID, bool IsActive,int Userid)
        {
            try
            {
                return await WithConnection(async c =>
                {
                    //This is to add parameters
                    DynamicParameters para = new DynamicParameters();
                    para.Add("@PHMID", PHMID);
                    para.Add("@IsActive", IsActive);
                    para.Add("@Userid", Userid);
                    return c.ExecuteAsync(ClsProcedures.uspChangeStatusOfCHM, param: para, commandType: CommandType.StoredProcedure).Result;

                });

            }
            catch (Exception ex)
            {

                return 0;
            }
        }


        public async Task<int> AddClientDetails(string Company, string Client,string Vertical, string Location, string Project, string mode)
        {
            try
            {
                return await WithConnection(async c =>
                {
                    //This is to add parameters
                    DynamicParameters para = new DynamicParameters();
                    para.Add("@CompanyName", Company);
                    para.Add("@ClientName", Client);
                    para.Add("@VerticalName", Vertical);
                    para.Add("@LocationName", Location);
                    para.Add("@ProjectName", Project);
                    para.Add("@mode", mode);
                    return c.ExecuteAsync(ClsProcedures.UspAddClientDetails, param: para, commandType: CommandType.StoredProcedure).Result;

                });

            }
            catch (Exception ex)
            {

                return 0;
            }
        }
        /// <summary>
        /// This is to get Supply Type Details
        /// </summary>
        /// <returns></returns>
        public IEnumerable<dynamic> GetSupplyType()
        {
            try
            {
                using (IDbConnection db = new SqlConnection(ClsCommon._ConnectionString))
                {
                    //This is to add parameters
                    DynamicParameters para = new DynamicParameters();

                    return db.Query<dynamic>(ClsProcedures.UspGetSupplyTypes, param: para, commandType: CommandType.StoredProcedure, commandTimeout: 0).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #region "ADDITIONAL CAPTURES"

        /// <summary>
        /// GET ADDITIONAL CAPTURE DETAILS BASED ON PRACTICE
        /// </summary>
        /// <param name="PHMID"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetAdditionalCaptures(int PHMID)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(ClsCommon._ConnectionString))
                {
                    //This is to add parameters
                    DynamicParameters para = new DynamicParameters();
                    para.Add("@PHMID", PHMID);
                    return db.Query<dynamic>(ClsProcedures.UspGetAdditionalCaptures, param: para, commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// Get Additional Capture details based on practice and role wise
        /// </summary>
        /// <param name="PHMID"></param>
        /// <param name="RoleID"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetAdditionalCapturesByRole(int PHMID, int RoleID)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(ClsCommon._ConnectionString))
                {
                    DynamicParameters para = new DynamicParameters();
                    para.Add("@PHMID", PHMID);
                    para.Add("@RoleID", RoleID);
                    return db.Query<dynamic>(ClsProcedures.UspGetAdditionalCaptures, param: para, commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        ///    This is to get Roles for Additional Captures
        /// </summary>
        /// <returns></returns>

        public IEnumerable<dynamic> GetRoles()
        {
            try
            {
                using (IDbConnection db = new SqlConnection(ClsCommon._ConnectionString))
                {
                    return db.Query<dynamic>(ClsProcedures.UspGetRoles, commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }



        
        /// <summary>
        /// SAVE ADDITIONAL CAPTURE DETAILS TO DATABASE
        /// </summary>
        /// <param name="PHMID"></param>
        /// <param name="FieldName"></param>
        /// <param name="ControlTypeID"></param>
        /// <param name="DataTypeID"></param>
        /// <returns></returns>
        public int SaveAdditionalCaptures(int RoleID, int PHMID, string FieldName, string ControlTypeID, string DataTypeID, string CommaValues, bool Required, bool IsQCEdit, int AdditionalCaptureOrder)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(ClsCommon._ConnectionString))
                {
                    //This is to add parameters
                    DynamicParameters para = new DynamicParameters();
                    para.Add("@RoleID", RoleID);
                    para.Add("@PHMID", PHMID);
                    para.Add("@FieldName", FieldName);
                    para.Add("@ControlType", ControlTypeID);
                    para.Add("@DataType", DataTypeID);
                    para.Add("@list", CommaValues);
                    para.Add("@Mandet", Required);
                    para.Add("@IsQCEdit", IsQCEdit);
                    para.Add("@AdditionalCaptureOrder", AdditionalCaptureOrder);
                    return db.Query<int>(ClsProcedures.UspAddAdditionalCapture, param: para, commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        /// <summary>
        /// ENABLE OR DISABLE PROCSES FLOW
        /// </summary>
        /// <param name="AdditionalCaptureID"></param>
        /// <returns></returns>
        public int EnableOrDisableAdditionalCapture(int AdditionalCaptureID)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(ClsCommon._ConnectionString))
                {
                    //This is to add parameters
                    DynamicParameters para = new DynamicParameters();
                    para.Add("@AdditionalCaptureID", AdditionalCaptureID);
                    return db.Query<int>(ClsProcedures.UspEnableDisableAddCapture, param: para, commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        /// <summary>
        /// To save additional capture order
        /// </summary>
        /// <param name="Dt"></param>
        /// <returns></returns>
        public int SaveAdditionalCaptureOrder(DataTable Dt)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(ClsCommon._ConnectionString))
                {
                    //This is to add parameters
                    DynamicParameters para = new DynamicParameters();
                    para.Add("@AdditionalCaptureOrder", Dt, DbType.Object);
                    return db.Query<int>(ClsProcedures.UspReOrderAdditionalCapture, param: para, commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        #endregion "ADDITIONAL CAPTURES"




        #region "MANUAL CAPTURES"

        /// <summary>
        /// GET MANUAL CAPTURE DETAILS BASED ON PRACTICE
        /// </summary>
        /// <param name="PHMID"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetManualCaptureDetails(int PHMID)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(ClsCommon._ConnectionString))
                {
                    //This is to add parameters
                    DynamicParameters para = new DynamicParameters();
                    para.Add("@PHMID", PHMID);
                    return db.Query<dynamic>(ClsProcedures.UspGetManualCaptures, param: para, commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// Get Manual Entry Capture details based on practice and role wise
        /// </summary>
        /// <param name="PHMID"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetManualCaptureDetailsbyRole(int PHMID)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(ClsCommon._ConnectionString))
                {
                    DynamicParameters para = new DynamicParameters();
                    para.Add("@PHMID", PHMID);
                    return db.Query<dynamic>(ClsProcedures.UspGetManualCaptures, param: para, commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        /// <summary>
        /// SAVE Manual CAPTURE DETAILS TO DATABASE
        /// </summary>
        /// <param name="PHMID"></param>
        /// <param name="FieldID"></param>
        /// <param name="ControlTypeID"></param>
        /// <param name="DataTypeID"></param>
        /// <returns></returns>
        public int SaveManualCaptureOrder( int PHMID, int FieldID, string ControlTypeID, string DataTypeID, string CommaValues, bool Required)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(ClsCommon._ConnectionString))
                {
                    //This is to add parameters
                    DynamicParameters para = new DynamicParameters();
                    para.Add("@FieldID", FieldID);
                    para.Add("@ControlTypeID", ControlTypeID);
                    para.Add("@DataTypeID", DataTypeID);
                    para.Add("@PHMID", PHMID);
                    para.Add("@list", CommaValues);
                    para.Add("@Mandet", Required);
                   

                    return db.Query<int>(ClsProcedures.UspSumbitManualCaptures, param: para, commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        /// <summary>
        /// ENABLE OR DISABLE FLOW
        /// </summary>
        /// <param name="ManualCaptureID"></param>
        /// <returns></returns>
        public int EnableOrDisableManualEntry(int ManualCaptureID)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(ClsCommon._ConnectionString))
                {
                    //This is to add parameters
                    DynamicParameters para = new DynamicParameters();
                    para.Add("@ManualCaptureID", ManualCaptureID);
                    return db.Query<int>(ClsProcedures.UspEnableDisableManualCapture, param: para, commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        #endregion "Manual Entry"




        #region "BOT INPUT"

        /// <summary>
        /// GET BOT INPUT DETAILS BASED ON PRACTICE
        /// </summary>
        /// <param name="PHMID"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetBotInputDetails(int PHMID)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(ClsCommon._ConnectionString))
                {
                    //This is to add parameters
                    DynamicParameters para = new DynamicParameters();
                    para.Add("@PHMID", PHMID);
                    return db.Query<dynamic>(ClsProcedures.UspGetBotInputDetails, param: para, commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PHMID"></param>
        /// <param name="FieldID"></param>
        /// <param name="InventroryPayerName"></param>
        /// <param name="BotInputName"></param>
        /// <param name="CreatedBy"></param>
        /// <returns></returns>
        public int SaveBotInputOrder(int PHMID, int FieldID, string InventroryPayerName, string BotInputName,string CreatedBy)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(ClsCommon._ConnectionString))
                {
                    //This is to add parameters
                    DynamicParameters para = new DynamicParameters();
                    para.Add("@FieldID", FieldID);
                    para.Add("@InventroryPayerName", InventroryPayerName);
                    para.Add("@BotInputName", BotInputName);
                    para.Add("@CreatedBy", CreatedBy);
                    para.Add("@PHMID", PHMID);
                    para.Add("@BotInputID", "");
                    para.Add("@ProcessFlag", "I");




                    return db.Query<int>(ClsProcedures.UspSumbitBotInputs, param: para, commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public int UpdateBotInputOrder(int PHMID, int FieldID, string InventroryPayerName, string BotInputName, string CreatedBy,int BotInputID)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(ClsCommon._ConnectionString))
                {
                    //This is to add parameters
                    DynamicParameters para = new DynamicParameters();
                    para.Add("@FieldID", FieldID);
                    para.Add("@InventroryPayerName", InventroryPayerName);
                    para.Add("@BotInputName", BotInputName);
                    para.Add("@CreatedBy", CreatedBy);
                    para.Add("@PHMID", PHMID);
                    para.Add("@BotInputID", BotInputID);
                    para.Add("@ProcessFlag", "U");




                    return db.Query<int>(ClsProcedures.UspSumbitBotInputs, param: para, commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        /// <summary>
        /// ENABLE OR DISABLE FLOW
        /// </summary>
        /// <param name="BotInputID"></param>
        /// <returns></returns>
        public int BotInputEnableOrDisable(int BotInputID)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(ClsCommon._ConnectionString))
                {
                    //This is to add parameters
                    DynamicParameters para = new DynamicParameters();
                    para.Add("@BotInputID", BotInputID);
                    return db.Query<int>(ClsProcedures.UspBotInputEnableOrDisable, param: para, commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public async Task<Tuple<IEnumerable<dynamic>>> GetBotInputPreLoadData(int PHMID, int UserID)
        {
            //These are the declaration of return objects
            IEnumerable<dynamic> objSupplyFields = null;



            try
            {

                return await WithConnection(async c =>
                {
                    //This is to add parameters
                    var para = new DynamicParameters();
                    para.Add("@PHMID", PHMID);


                    //This is to excute the command and return the result
                    using (var multi = await c.QueryMultipleAsync(ClsProcedures.UspGetBotInputPreLoadData, para, commandType: CommandType.StoredProcedure, commandTimeout: 0))
                    {

                        if (!multi.IsConsumed)
                        {
                            //Assignning Configuration result
                            objSupplyFields = multi.Read().ToList();
                        }


                    }

                    //Values are retured in the form of Tuple with mutiple objects
                    return Tuple.Create(objSupplyFields);
                });

            }
            catch (Exception ex)
            {

                return null;
            }
            finally
            {
                //These are the declaration of return objects
                objSupplyFields = null;

            }
        }

        #endregion "Bot Input"



        #region "Process flow"
        /// <summary>
        /// To get  Master Preload Data 
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public async Task<Tuple<IEnumerable<dynamic>,  IEnumerable<dynamic>>> GetMasterProcessflowPreLoadData(int PHMID)
        {


            IEnumerable<dynamic> QAUserDetails = null;
            IEnumerable<dynamic> AllScenarioMappingDetails = null;
            try
            {
                return await WithConnection(async c =>
                {
                    DynamicParameters para = new DynamicParameters();
                    para.Add("@PHMID", PHMID);
                    var multi = await c.QueryMultipleAsync(ClsProcedures.UspGetMasterProcessflowPreLoadData,para, commandType: CommandType.StoredProcedure);
                    if (!multi.IsConsumed)
                    {

                        //Assignning result to variable
                        QAUserDetails = multi.Read().ToList();

                        //Assignning result to variable
                        AllScenarioMappingDetails = multi.Read().ToList();
                      
                    }
                    return Tuple.Create(QAUserDetails, AllScenarioMappingDetails);
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

                QAUserDetails = null;
                AllScenarioMappingDetails = null;

            }
        }


        /// <summary>
        /// To get  Process Flow Deails
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public async Task<Tuple<IEnumerable<dynamic>>> GetProcessFlowDeails(int PHMID)
        {
            IEnumerable<dynamic> ProcessflowDetails = null;
           
            try
            {
                return await WithConnection(async c =>
                {
                    DynamicParameters para = new DynamicParameters();
                    para.Add("@PHMID", PHMID);
                    var multi = await c.QueryMultipleAsync(ClsProcedures.UspGetProcessFlowDeails, para, commandType: CommandType.StoredProcedure);
                    if (!multi.IsConsumed)
                    {

                        //Assignning result to variable
                        ProcessflowDetails = multi.Read().ToList();

                    }
                    return Tuple.Create(ProcessflowDetails);
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

                ProcessflowDetails = null;
               

            }
        }

        /// <summary>
        /// ENABLE OR DISABLE PROCSES FLOW
        /// </summary>
        /// <param name="ProcessFlowID"></param>
        /// <returns></returns>
        public int EnableOrDisableProcessflow(int ProcessFlowID)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(ClsCommon._ConnectionString))
                {
                    //This is to add parameters
                    DynamicParameters para = new DynamicParameters();
                    para.Add("@ProcessFlowID", ProcessFlowID);
                    return db.Query<int>(ClsProcedures.UspEnableDisableAddprocess, param: para, commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        /// <summary>
        /// SAVE Process Flow DETAILS TO DATABASE
        /// </summary>
        /// <param name="PHMID"></param>
        /// <param name="FieldName"></param>
        /// <param name="ControlTypeID"></param>
        /// <param name="DataTypeID"></param>
        /// <returns></returns>
        public int SaveProcessflow( int PHMID, int ScenarioMappingID,int QAUserID)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(ClsCommon._ConnectionString))
                {
                    //This is to add parameters
                    DynamicParameters para = new DynamicParameters();
                   
                    para.Add("@PHMID", PHMID);
                    para.Add("@ScenarioMappingID", ScenarioMappingID);
                    para.Add("@QAUserID", QAUserID);
                    


                    return db.Query<int>(ClsProcedures.UspAddProcessFlow, param: para, commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }


        #endregion "Process flow"

        /// <summary>
        /// To get user mapping detail
        /// </summary>
        /// <param name="CHMapID"></param>
        /// <param name="RoleCode"></param>
        /// <param name="EmpID"></param>
        /// <returns></returns>
        public async Task<Tuple<IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>>> GetUsersMappingDetails(int PHMID, String RoleCode, int EmpID)
        {
            IEnumerable<dynamic> MappedUser = null;
            IEnumerable<dynamic> UserDetails = null;
            IEnumerable<dynamic> ClientUserDetails = null;
            try
            {
                return await WithConnection(async c =>
                {
                    //This is to add parameters
                    DynamicParameters para = new DynamicParameters();
                    para.Add("@PHMID", PHMID);
                    para.Add("@RoleCode", RoleCode);
                    para.Add("@EmpID", EmpID);
                    var multi = await c.QueryMultipleAsync(ClsProcedures.UspUsersMappingDetails, param: para, commandType: CommandType.StoredProcedure);
                    if (!multi.IsConsumed)
                    {
                        //Assignning result to variable
                        MappedUser = multi.Read().ToList();

                        //Assignning result to variable
                        UserDetails = multi.Read().ToList();

                        ////Assignning result to variable
                        ClientUserDetails = multi.Read().ToList();
                    }
                    return Tuple.Create(MappedUser, UserDetails, ClientUserDetails);
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
                MappedUser = null;
                UserDetails = null;
                ClientUserDetails = null;
            }
        }


        /// <summary>
        /// To get User Master Preload Data 
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public async Task<Tuple< IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>>> GetUserMasterPreLoadData(int PHMID)
        {
            try
            {
                IEnumerable<dynamic> EmployeeTypes = null;
                IEnumerable<dynamic> LeadUsers = null;
                IEnumerable<dynamic> ManagerUsers = null;
                IEnumerable<dynamic> Roles = null;
                IEnumerable<dynamic> QCADetails = null;
                IEnumerable<dynamic> UserTypes = null;
                IEnumerable<dynamic> ShiftTiming = null;

                return await WithConnection(async c =>
                {
                    DynamicParameters para = new DynamicParameters();
                    para.Add("@PHMID", PHMID);
                    
                 
                    var multi = await c.QueryMultipleAsync(ClsProcedures.UspGetUserMasterPreLoadData,param: para, commandType: CommandType.StoredProcedure);
                    if (!multi.IsConsumed)
                    {
                        //Assignning result to variable
                        EmployeeTypes = multi.Read().ToList();

                        //Assignning result to variable
                        LeadUsers = multi.Read().ToList();


                        //Assignning result to variable
                        ManagerUsers = multi.Read().ToList();

                  

                        //Assignning result to variable
                        Roles = multi.Read().ToList();

                   

                        QCADetails = multi.Read().ToList();


                        UserTypes = multi.Read().ToList();
                        ShiftTiming = multi.Read().ToList();
                       
                    }
                    return Tuple.Create(EmployeeTypes, LeadUsers, ManagerUsers,  Roles,  QCADetails, UserTypes, ShiftTiming);
                    //Values are retured in the form of Tuple with mutiple objects

                });
            }
            catch (Exception ex)
            {
                // ExceptionLogging.SendErrorToText(ex);
                return null;
            }
        }

        /// <summary>
        /// SAVE ADDITIONAL CAPTURE DETAILS TO DATABASE
        /// </summary>
        /// <param name="PHMID"></param>
        /// <param name="FieldName"></param>
        /// <param name="ControlTypeID"></param>
        /// <param name="DataTypeID"></param>
        /// <returns></returns>
        public int AddEmployeeDetails(int PHMID, int EmployeeTypeID, string EmployeeID, string EmpName, string EmailID, string Designation,int Tenure, int LeadUserID, int ManagerUserID,int UserID,int ShiftID)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(ClsCommon._ConnectionString))
                {
                    //This is to add parameters
                    DynamicParameters para = new DynamicParameters();
                   
                    para.Add("@PHMID", PHMID);
                    para.Add("@EmployeeTypeID", EmployeeTypeID);
                    para.Add("@EmployeeID", EmployeeID);
                    para.Add("@EmpName", EmpName);
                    para.Add("@EmailID", EmailID);
                    para.Add("@Designation", Designation);
                    para.Add("@Tenure", Tenure);
                    para.Add("@LeadUserID", LeadUserID);
                    para.Add("@ManagerUserID", ManagerUserID);
                    para.Add("@UserID", UserID);
                    para.Add("@ShiftID", ShiftID);
                    
                    return db.Query<int>(ClsProcedures.UspAddEmployeeDetails, param: para, commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        /// <summary>
        /// To Get Client Access Mapping By EmpID
        /// </summary>
        /// <param name="Emp_ID"></param>
        /// <param name="CHMapID"></param>
        /// <returns></returns>
        public async Task<IEnumerable<dynamic>> GetClientAccessMappingByEmpID(string Emp_ID,string EmailID)
        {
            try
            {
                return await WithConnection(async c =>
                {
                    //This is to add parameters
                    DynamicParameters para = new DynamicParameters();
                    para.Add("@EmpID", Emp_ID);
                    para.Add("@EmailID", EmailID);
                    var result = await c.QueryAsync<dynamic>(ClsProcedures.UspClientAccessMappingByEmpID, param: para, commandType: CommandType.StoredProcedure);

                    return (result.ToList());
                });
            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        /// To mapp user to Client Access mapping
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="loginNTLG"></param>
        /// <returns></returns>
        public async Task<IEnumerable<dynamic>> SaveMappUserData(DataTable dt, string loginNTLG)
        {
            try
            {
                return await WithConnection(async c =>
                {
                    //This is to add parameters
                    DynamicParameters para = new DynamicParameters();
                    para.Add("@User", dt, DbType.Object);
                    para.Add("@LoginNTLG", loginNTLG);
                    var result = await c.QueryAsync<dynamic>(ClsProcedures.UspSubmitMappUserDetails, param: para, commandType: CommandType.StoredProcedure);

                    return (result.ToList());
                });
            }

            catch (Exception ex)
            {
                //ExceptionLogging.SendErrorToText(ex);
                return null;
            }
        }

        /// <summary>
        /// This is to get the case history details
        /// </summary>
        /// <param name="CHMapID"></param>
        /// <param name="CaseID"></param>
        /// <returns></returns>

        public IEnumerable<dynamic> GetAccountHistoryDetails(int PHMID, string AccountNo)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(ClsCommon._ConnectionString))
                {
                    DynamicParameters para = new DynamicParameters();
                    para.Add("@PHMID", PHMID);
                    para.Add("@AccountNo", AccountNo);
                    return db.Query<dynamic>(ClsProcedures.UspGetAccountHistoryDetails, param: para, commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public IEnumerable<dynamic> GetAccountBotReponseDetails(string AccountNo)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(ClsCommon._ConnectionString))
                {
                    DynamicParameters para = new DynamicParameters();
                    para.Add("@AccountID", AccountNo);
                    return db.Query<dynamic>(ClsProcedures.UspGetAccountBotReponseDetails, param: para, commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<Tuple<IEnumerable<dynamic>>> GetManualEntryPreLoadData(int PHMID, int UserID)
        {
            //These are the declaration of return objects
            IEnumerable<dynamic> objSupplyFields = null;
            


            try
            {

                return await WithConnection(async c =>
                {
                    //This is to add parameters
                    var para = new DynamicParameters();
                    para.Add("@PHMID", PHMID);


                    //This is to excute the command and return the result
                    using (var multi = await c.QueryMultipleAsync(ClsProcedures.UspGetManualEntryPreLoadData, para, commandType: CommandType.StoredProcedure, commandTimeout: 0))
                    {

                        if (!multi.IsConsumed)
                        {
                            //Assignning Configuration result
                            objSupplyFields = multi.Read().ToList();
                        }
                       

                    }

                    //Values are retured in the form of Tuple with mutiple objects
                    return Tuple.Create(objSupplyFields);
                });

            }
            catch (Exception ex)
            {

                return null;
            }
            finally
            {
                //These are the declaration of return objects
                objSupplyFields = null;
               
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="PHMID"></param>
        /// <param name="FieldID"></param>
        /// <param name="ControlTypeID"></param>
        /// <param name="CommaValues"></param>
        /// <param name="Required"></param>
        /// <param name="loginID"></param>
        /// <returns></returns>
        public int SaveManaulCapture(int PHMID, int FieldID, string ControlTypeID, string CommaValues, bool Required, int loginID)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(ClsCommon._ConnectionString))
                {
                    //This is to add parameters
                    DynamicParameters para = new DynamicParameters();

                    para.Add("@FieldID", FieldID);
                    para.Add("@ControlType", ControlTypeID);
                    para.Add("@PHMID", PHMID);
                    para.Add("@list", CommaValues);
                    para.Add("@Mandet", Required);
                    para.Add("@UserID", loginID);
                    return db.Query<int>(ClsProcedures.UspManualCapture, param: para, commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

       /// <summary>
       /// 
       /// </summary>
       /// <param name="PHMID"></param>
       /// <returns></returns>
        public IEnumerable<dynamic> GetManualCaptures(int PHMID)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(ClsCommon._ConnectionString))
                {
                    //This is to add parameters
                    DynamicParameters para = new DynamicParameters();
                    para.Add("@PHMID", PHMID);
                    return db.Query<dynamic>(ClsProcedures.UspGetManualCaptures, param: para, commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        /// <summary>
        /// ENABLE OR DISABLE PROCSES FLOW
        /// </summary>
        /// <param name="AdditionalCaptureID"></param>
        /// <returns></returns>
        public int DeleteManualCapture(int ManualCaptureID)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(ClsCommon._ConnectionString))
                {
                    //This is to add parameters
                    DynamicParameters para = new DynamicParameters();
                    para.Add("@ManualCaptureID", ManualCaptureID);
                    return db.Query<int>(ClsProcedures.UspDeleteManualCapture, param: para, commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="PHMID"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> ResetData(int PHMID)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(ClsCommon._ConnectionString))
                {
                    //This is to add parameters
                    DynamicParameters para = new DynamicParameters();
                    para.Add("@PHMID", PHMID);
                    return db.Query<dynamic>(ClsProcedures.UspResetAccounts, param: para, commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public IEnumerable<dynamic> SearchAccountInformation(int PHMID, string stringToFind)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(ClsCommon._ConnectionString))
                {
                    //This is to add parameters
                    DynamicParameters para = new DynamicParameters();
                    para.Add("@PHMID", PHMID);
                    para.Add("@stringToFind", stringToFind);
                    return db.Query<dynamic>(ClsProcedures.UspFindAccuntInfo, param: para, commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }

        }

    }
}