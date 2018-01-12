using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace TurnKeyBrokerSignUp.WebUI.Models
{
    public class TenantRouteConstraint : IRouteConstraint
    {
        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            var fullAddress = httpContext.Request.Headers["Host"];
            var tenantSubdomain = fullAddress[0];
            if (!values.ContainsKey("tenant"))
            {
                values.Add("tenant", fullAddress);
            }
            return true;
        }
    }
}