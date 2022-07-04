using TAR_API.App_Code;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;


namespace TAR_API.Repository
{
    public class DashboardRepository : BaseRepository
    {
        public async Task<Tuple<IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>>> GetProductionHourChart(int UserID, string CompltedDate, int PHMID)
        {
            IEnumerable<dynamic> Target = null;
            IEnumerable<dynamic> HourChart = null;
            IEnumerable<dynamic> StatusTable = null;



            try
            {

                return await WithConnection(async c =>
                {
                    //This is to add parameters
                    DynamicParameters para = new DynamicParameters();
                    para.Add("@UserID", UserID);
                    para.Add("@PHMID", PHMID);

                    para.Add("@CompletedDate", CompltedDate);

                    var multi = await c.QueryMultipleAsync(ClsProcedures.UspGetProductionHourChart, param: para, commandType: CommandType.StoredProcedure);


                    if (!multi.IsConsumed)
                    {

                        HourChart = multi.Read().ToList();
                    }
                    if (!multi.IsConsumed)
                    {

                        StatusTable = multi.Read().ToList();
                    }
                    if (!multi.IsConsumed)
                    {

                        Target = multi.Read().ToList();
                    }

                    return Tuple.Create(HourChart, StatusTable, Target);
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
                HourChart = null;

            }


        }

        public async Task<Tuple<IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>>> GetQCAnalysis(int UserID, int PHMID)
        {
            // IEnumerable<dynamic> AgingDetails = null;
            IEnumerable<dynamic> MTD = null;
            IEnumerable<dynamic> AccuracyPer = null;
            IEnumerable<dynamic> ErrorPer = null;

            try
            {

                return await WithConnection(async c =>
                {
                    //This is to add parameters
                    DynamicParameters para = new DynamicParameters();
                    para.Add("@UserID", UserID);
                    para.Add("@PHMID", PHMID);


                    var multi = await c.QueryMultipleAsync(ClsProcedures.uspQCAnalysis, param: para, commandType: CommandType.StoredProcedure);


                    if (!multi.IsConsumed)
                    {

                        MTD = multi.Read().ToList();
                    }
                    if (!multi.IsConsumed)
                    {

                        AccuracyPer = multi.Read().ToList();
                    }
                    if (!multi.IsConsumed)
                    {

                        ErrorPer = multi.Read().ToList();
                    }

                    return Tuple.Create(MTD,AccuracyPer,ErrorPer);
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
                MTD = null;
                AccuracyPer = null;
                ErrorPer = null;

            }


        }

        public async Task<Tuple<IEnumerable<dynamic>, IEnumerable<dynamic>>> GetQualityAnalysis(int UserID, int PHMID)
        {
            // IEnumerable<dynamic> AgingDetails = null;
            IEnumerable<dynamic> Category = null;
            IEnumerable<dynamic> SubCategory = null;
            

            try
            {

                return await WithConnection(async c =>
                {
                    //This is to add parameters
                    DynamicParameters para = new DynamicParameters();
                    para.Add("@UserID", UserID);
                    para.Add("@PHMID", PHMID);


                    var multi = await c.QueryMultipleAsync(ClsProcedures.UspGetQualityAnalysis, param: para, commandType: CommandType.StoredProcedure);


                   
                    if (!multi.IsConsumed)
                    {

                        Category = multi.Read().ToList();
                    }
                    if (!multi.IsConsumed)
                    {

                        SubCategory = multi.Read().ToList();
                    }

                    return Tuple.Create(Category, SubCategory);
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
                
               
                Category = null;
                SubCategory = null;

            }


        }

        public async Task<Tuple<IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>>> GetAgentQualityAnalysis(int UserID, int PHMID)
        {
            // IEnumerable<dynamic> AgingDetails = null;
            IEnumerable<dynamic> Overall = null;
            IEnumerable<dynamic> DayWiseError = null;
            IEnumerable<dynamic> DailyIQ = null;
            IEnumerable<dynamic> WeeklyIQ = null;
            try
            {

                return await WithConnection(async c =>
                {
                    //This is to add parameters
                    DynamicParameters para = new DynamicParameters();
                    para.Add("@UserID", UserID);
                    para.Add("@PHMID", PHMID);

                    var multi = await c.QueryMultipleAsync(ClsProcedures.UspGetAgentAnalysis, param: para, commandType: CommandType.StoredProcedure);


                    if (!multi.IsConsumed)
                    {

                        Overall = multi.Read().ToList();
                    }
                    if (!multi.IsConsumed)
                    {

                        DayWiseError = multi.Read().ToList();
                    }
                    if (!multi.IsConsumed)
                    {

                        DailyIQ = multi.Read().ToList();
                    }
                    if (!multi.IsConsumed)
                    {

                        WeeklyIQ = multi.Read().ToList();
                    }

                    return Tuple.Create(Overall, DayWiseError, DailyIQ, WeeklyIQ);
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
                Overall = null; DayWiseError = null; DailyIQ = null; WeeklyIQ = null;

            }


        }

        public async Task<Tuple<IEnumerable<dynamic>>> GetProductionPerformanceChart(int UserID, int PHMID, string FromDate, string ToDate)
        {
            // IEnumerable<dynamic> AgingDetails = null;

            IEnumerable<dynamic> PerformanceChart = null;
            try
            {

                return await WithConnection(async c =>
                {
                    //This is to add parameters
                    DynamicParameters para = new DynamicParameters();
                    para.Add("@UserID", UserID);
                    para.Add("@PHMID", PHMID);

                    para.Add("@FromDate", FromDate);
                    para.Add("@ToDate", ToDate);

                    var multi = await c.QueryMultipleAsync(ClsProcedures.UspGetProductionPerformanceChart, param: para, commandType: CommandType.StoredProcedure);



                    if (!multi.IsConsumed)
                    {

                        PerformanceChart = multi.Read().ToList();
                    }
                    return Tuple.Create(PerformanceChart);
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

                PerformanceChart = null;
            }


        }


        public async Task<Tuple<IEnumerable<dynamic>>> GetProductivityAnalysis(string UserID,string analysis, int PHMID,string flag, string FromDate, string ToDate)
        {
            // IEnumerable<dynamic> AgingDetails = null;

            IEnumerable<dynamic> ProductivityChart = null;
            IEnumerable<dynamic> QualityAnalysisChart = null;
            IEnumerable<dynamic> PerformanceAnalysisChart = null;
            IEnumerable<dynamic> TimeAnalysisChart = null;
            IEnumerable<dynamic> AHTAnalysisChart = null;
            try
            {

                return await WithConnection(async c =>
                {
                    //This is to add parameters
                    DynamicParameters para = new DynamicParameters();
                    para.Add("@UserIDs", UserID);
                    para.Add("@PHMID", PHMID);
                    para.Add("@flag", flag);
                    para.Add("@fromdate", FromDate);
                    para.Add("@todate", ToDate);

                    if (analysis == "Q")
                    {
                        var multi = await c.QueryMultipleAsync(ClsProcedures.UspGetQualityAnalysisCommon, param: para, commandType: CommandType.StoredProcedure);

                        if (!multi.IsConsumed)
                        {

                            QualityAnalysisChart = multi.Read().ToList();
                        }
                        return Tuple.Create(QualityAnalysisChart);
                    }
                    else if (analysis == "T")
                    {
                        var multi = await c.QueryMultipleAsync(ClsProcedures.uspTimeAnalysisCommon, param: para, commandType: CommandType.StoredProcedure);

                        if (!multi.IsConsumed)
                        {
                            
                            TimeAnalysisChart = multi.Read().ToList();
                        }
                        return Tuple.Create(TimeAnalysisChart);
                    }
                    else if (analysis == "A")
                    {
                        var multi = await c.QueryMultipleAsync(ClsProcedures.UspAHTAnalysisCommon, param: para, commandType: CommandType.StoredProcedure);

                        if (!multi.IsConsumed)
                        {

                            AHTAnalysisChart = multi.Read().ToList();
                        }
                        return Tuple.Create(AHTAnalysisChart);
                    }
                    else if(analysis=="PR")
                    {
                        var multi = await c.QueryMultipleAsync(ClsProcedures.UspPerformanceAnalysisCommon, param: para, commandType: CommandType.StoredProcedure);

                        if (!multi.IsConsumed)
                        {

                            PerformanceAnalysisChart = multi.Read().ToList();
                        }
                        return Tuple.Create(PerformanceAnalysisChart);
                    }
                    else  //if (analysis == "P")
                    {
                        var multi = await c.QueryMultipleAsync(ClsProcedures.uspProductionHourChartCommon, param: para, commandType: CommandType.StoredProcedure);

                        if (!multi.IsConsumed)
                        {

                            ProductivityChart = multi.Read().ToList();
                        }
                        return Tuple.Create(ProductivityChart);
                    }
                    
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

                ProductivityChart = null;
                QualityAnalysisChart = null;
                TimeAnalysisChart = null;
                AHTAnalysisChart = null;
                PerformanceAnalysisChart = null;
            }


        }

        public async Task<Tuple<IEnumerable<dynamic>>> GetInventoryAnalysis(string FromDate, string ToDate, int PHMID, string ImportFileIDs, string UserID)
        {
            IEnumerable<dynamic> InventoryAnalysis = null;
            try
            {

                return await WithConnection(async c =>
                {
                    //This is to add parameters
                    DynamicParameters para = new DynamicParameters();
                    para.Add("@FROM_DATE", FromDate);
                    para.Add("@TO_DATE", ToDate);
                    para.Add("@PHMID", PHMID);
                    para.Add("@ImportFileIDs", ImportFileIDs);
                    para.Add("@UserIDs", UserID);

                    var multi = await c.QueryMultipleAsync(ClsProcedures.uspInventoryAnalysisCommon, param: para, commandType: CommandType.StoredProcedure);
                    if (!multi.IsConsumed)
                    {
                        InventoryAnalysis = multi.Read().ToList();
                    }
                    return Tuple.Create(InventoryAnalysis);

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
               InventoryAnalysis = null;
            }


        }
        public async Task<Tuple<IEnumerable<dynamic>, IEnumerable<dynamic>>> GetErrorAnalysis(string UserID, string analysis, int PHMID, string flag, string FromDate, string ToDate)
        {
            IEnumerable<dynamic> ErrorAnalysisChart = null;
            IEnumerable<dynamic> SubErrorAnalysisChart = null;
            try
            {

                return await WithConnection(async c =>
                {
                    //This is to add parameters
                    DynamicParameters para = new DynamicParameters();
                    para.Add("@UserIDs", UserID);
                    para.Add("@PHMID", PHMID);
                    para.Add("@flag", flag);
                    para.Add("@fromdate", FromDate);
                    para.Add("@todate", ToDate);

                      var multi = await c.QueryMultipleAsync(ClsProcedures.UspErrorAnalysisCommon, param: para, commandType: CommandType.StoredProcedure);
                    if (!multi.IsConsumed)
                        {
                            ErrorAnalysisChart = multi.Read().ToList();
                        }
                    var multiSub = await c.QueryMultipleAsync(ClsProcedures.UspSubErrorAnalysisCommon, param: para, commandType: CommandType.StoredProcedure);

                    if (!multiSub.IsConsumed)
                        {
                          SubErrorAnalysisChart = multiSub.Read().ToList();
                        }
                    return Tuple.Create(ErrorAnalysisChart, SubErrorAnalysisChart);
                    
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
                ErrorAnalysisChart = null;
                SubErrorAnalysisChart = null;
            }


        }

        public async Task<Tuple<IEnumerable<dynamic>>> GetAllUsers(int PHMID, int UserID)
        {

            IEnumerable<dynamic> objUsers = null;

            try
            {
                return await WithConnection(async c =>
                {
                    //This is to add parameters
                    var para = new DynamicParameters();
                    para.Add("@PHMID", PHMID);
                    para.Add("@UserID", UserID);


                    //This is to excute the command and return the result
                    using (var multi = await c.QueryMultipleAsync(ClsProcedures.uspGetAllusers, para, commandType: CommandType.StoredProcedure, commandTimeout: 0))
                    {




                        if (!multi.IsConsumed)
                        {
                            //Assignning Additional Caputures result
                            objUsers = multi.Read().ToList();
                        }



                    }



                    //Values are retured in the form of Tuple with mutiple objects
                    return Tuple.Create(objUsers);
                });



            }
            catch (Exception ex)
            {



                return null;
            }
            finally
            {
                //These are the declaration of return objects

                objUsers = null;

            }
        }


        public IEnumerable<dynamic> GetUserDetailsbyCondition(int UserID, string RoleCode, int PHMID)
        {

            try

            {
                using (IDbConnection db = new SqlConnection(ClsCommon._ConnectionString))
                {
                    DynamicParameters para = new DynamicParameters();
                    para.Add("@USERID", UserID);
                    para.Add("@ROLECODE", RoleCode);
                    para.Add("@PHMID", PHMID);


                    return db.Query<dynamic>(ClsProcedures.UspGetUserDetailsbyCondition, param: para, commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


    }
}
