using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace TAR_API
{
    public class Authorize : TypeFilterAttribute
    {
        public Authorize(string actionName, string roleType): base(typeof(AuthorizeAction)) {
        Arguments = new object[] {
            actionName,
            roleType
    };
}
    }
}
