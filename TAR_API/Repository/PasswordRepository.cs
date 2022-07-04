using TAR_API.App_Code;
using TAR_API.Controllers;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace TAR_API.Repository
{
    public class PasswordRepository:BaseRepository
    {
        public async Task<int> ResetPassword(EmployeePasswordDetails obj)
        {
            try
            {
                return await WithConnection(async c =>
                {
                    //This is to add parameters
                    DynamicParameters para = new DynamicParameters();
                    para.Add("@UserID", obj.UserID);
                    para.Add("@password", obj.newpassword);

                    return c.ExecuteAsync(ClsProcedures.UspResetPassword, param: para, commandType: CommandType.StoredProcedure).Result;

                    // result;
                });
            }
            catch (Exception ex)
            {

                return 0;
            }
        }

    }
}