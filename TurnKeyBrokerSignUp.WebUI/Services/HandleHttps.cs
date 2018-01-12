using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TurnKeyBrokerSignUp.WebUI.Services
{
    public class HandleHttps : RequireHttpsAttribute
    {
        protected override void HandleNonHttpsRequest(AuthorizationContext filterContext)
        {
            if (!filterContext.HttpContext.Request.Url.Host.Contains("localhost"))
            {
                base.HandleNonHttpsRequest(filterContext);
            }
        } 
    }
}