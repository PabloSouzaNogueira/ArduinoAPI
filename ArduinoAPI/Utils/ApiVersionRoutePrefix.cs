using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace ArduinoAPI.Utils
{
    public class ApiVersionRoutePrefix: RoutePrefixAttribute
    {
        public ApiVersionRoutePrefix(string prefix, int version)
            : base(string.Format("api/v{0}/{1}", version, prefix))
        {
        }
    }
}