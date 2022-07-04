using TAR_API.App_Code;
using TAR_API.Interface;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace TAR_API.Repository
{
    public class MenuRepository: BaseRepository, IMenu
    {
        #region Menus
        /// <summary>
        /// This method is to get all menu
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<dynamic>> GetAllMenu(int UserID, string RoleCode, int PHMID)
        {
            try
            {
                return await WithConnection(async c => {
                    DynamicParameters para = new DynamicParameters();
                    para.Add("@UserID", UserID);
                    para.Add("@RoleCode", RoleCode);
                    para.Add("@PHMID", PHMID);
                    var result = await c.QueryAsync<dynamic>(ClsProcedures.UspGetMenuDetails, param: para, commandType: CommandType.StoredProcedure);
                    return (result.ToList());
                });
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion
    }
}