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
using TAR_API.Interface;

namespace TAR_API.Repository
{
    public class ManualEntryRepository : BaseRepository, IManualEntry
    {
        public Tuple<IEnumerable<dynamic>, IEnumerable<dynamic>, IEnumerable<dynamic>,  IEnumerable<dynamic>> ManualAccountLoadData(int PHMID)
        {
            try
            {
                //These are the declaration of return objects
                IEnumerable<dynamic> objSupplyFields = null;
                IEnumerable<dynamic> objEPICCategoryDetails = null;
                IEnumerable<dynamic> objEPICConfiguration = null;
              
                IEnumerable<dynamic> objSupplyFieldsItems = null;
                using (IDbConnection db = new SqlConnection(ClsCommon._ConnectionString))
                {
                    //This is to add parameters
                    DynamicParameters para = new DynamicParameters();
                    para.Add("@PHMID", PHMID);

                    //This is to excute the command and return the result
                    using (var multi = db.QueryMultiple(ClsProcedures.UspGetManualAccountLoadData, para, commandType: CommandType.StoredProcedure))
                    {
                        if (!multi.IsConsumed)
                        {
                            //Assignning result to variable
                            objSupplyFields = multi.Read().ToList();
                        }
                        if (!multi.IsConsumed)
                        {
                            //Assignning result to variable
                            objEPICCategoryDetails = multi.Read().ToList();
                        }
                        if (!multi.IsConsumed)
                        {
                            //Assignning result to variable
                            objEPICConfiguration = multi.Read().ToList();
                        }
                        if (!multi.IsConsumed)
                        {
                            //Assignning result to variable
                            objSupplyFieldsItems = multi.Read().ToList();
                        }

                    }

                    //Values are retured in the form of Tuple with mutiple objects
                    return Tuple.Create(objSupplyFields, objEPICCategoryDetails, objEPICConfiguration, objSupplyFieldsItems);
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
        /// <param name="UserID"></param>
        /// <param name="DistinctID"></param>
        /// <param name="Note"></param>
        /// <param name="dtManualAccountDetailsValues"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> SubmitMaualAgentTransaction(int PHMID , int UserID , string DistinctID, string Note, string UserName, DataTable dtManualAccountDetailsValues)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(ClsCommon._ConnectionString))
                {
                    //This is to add parameters
                    DynamicParameters para = new DynamicParameters();
                    para.Add("@PHMID", PHMID);
                    para.Add("@ManualAccountTrans", dtManualAccountDetailsValues, DbType.Object);
                    para.Add("@Note", Note);
                    para.Add("@UserID", UserID);
                    para.Add("@DistinctID", DistinctID);
                    para.Add("@UserLoginName", UserName);
                    



                    return db.Query<dynamic>(ClsProcedures.UspSubmitManualEntryTransaction, param: para, commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}