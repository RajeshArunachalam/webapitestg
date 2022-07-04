using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAR_API.Repository;

namespace TAR_API
{
    public class AuthorizeAction : IAuthorizationFilter
    {
        TokenRepository repToken = new TokenRepository();
        private readonly string _actionName;
        private readonly string _roleType;
        public AuthorizeAction(string actionName, string roleType)
        {
            _actionName = actionName;
            _roleType = roleType;
        }
        public void OnAuthorization(AuthorizationFilterContext actionContext)
        {
            string UserName = string.Empty;

            UserName = (actionContext.HttpContext.Request.Headers.Any(x => x.Key == "Username") ? actionContext.HttpContext.Request.Headers.Where(x => x.Key == "Username").FirstOrDefault().Value.SingleOrDefault().ToString() : string.Empty);

            /* Check whether HttpActionContext contains Bearer */
            if (actionContext.HttpContext.Request.Headers["Authorization"].ToString().StartsWith("Bearer"))
            {
                /* Parse & get the access_token */
                var access_token = actionContext.HttpContext.Request.Headers["Authorization"].ToString().Substring("Bearer ".Length);

                /* Check in DB whether token is valid & exist */
                IEnumerable<dynamic> token = Task.Run(async () => await repToken.CheckTokenDetails(UserName, access_token)).Result;

                /* If token count equl to Zero  return true else false */
                if (token != null && token.Count() > 0)
                {

                }
                else
                {
                    actionContext.Result = new JsonResult("Permission denined!");
                }

            }
            else
            {
                actionContext.Result = new JsonResult("Permission denined!");
            }
          
        }
    }
}
