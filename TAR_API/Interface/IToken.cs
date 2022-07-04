using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAR_API.Interface
{
   public interface IToken
    {
        /// <summary>
        /// This is to save token details
        /// </summary>
        /// <param name="Username"></param>
        /// <param name="access_token"></param>
        /// <returns></returns>
        Task<int> SaveTokenDetails(string Username, string access_token, string refresh_token, string UserIP);
        /// <summary>
        /// This is to check token details
        /// </summary>
        /// <param name="access_token"></param>
        /// <returns></returns>
        Task<IEnumerable<dynamic>> CheckTokenDetails(string Username, string access_token); 
    }
}
