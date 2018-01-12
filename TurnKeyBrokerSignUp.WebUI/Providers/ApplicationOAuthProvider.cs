using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using TurnKeyBrokerSignUp.WebUI.Models;

namespace TurnKeyBrokerSignUp.WebUI.Providers
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        TurnKeyBrokerSignUpDataContext dataContext = new TurnKeyBrokerSignUpDataContext();
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
                context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            string message = "";

         
            var _user = dataContext.aspnet_Users.Where(au => au.UserName == context.UserName.ToLower()).FirstOrDefault();
           
            var _membership = dataContext.aspnet_Memberships.Where(am =>am.UserId == _user.UserId && am.Password == context.Password && am.IsApproved == true && am.IsLockedOut == false).FirstOrDefault();
            try
            {
                var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                if (context.UserName == _user.UserName && context.Password == _membership.Password)
                {
                    //identity.AddClaim(new Claim(ClaimTypes.Role, "admin"));
                    //identity.AddClaim(new Claim("username", "admin"));
                    //identity.AddClaim(new Claim(ClaimTypes.Name, "Sourav Mondal"));
                    context.Validated(identity);
                    // message = "Signin Susscefully Completed.";
                }
                else
                {
                    context.SetError("invalid_grant", "Provided username and password is incorrect");
                    //  message = "Provided username and password is incorrect.";
                    return;
                }
            }
            catch
            {
                context.SetError("invalid_grant", "Provided username and password is incorrect");
                //  message = "Provided username and password is incorrect.";
                return;
            }
               
                    //else if (context.UserName == "user" && context.Password == "user")
                    //{
                    //    identity.AddClaim(new Claim(ClaimTypes.Role, "user"));
                    //    identity.AddClaim(new Claim("username", "user"));
                    //    identity.AddClaim(new Claim(ClaimTypes.Name, "Suresh Sha"));
                    //    context.Validated(identity);
                    //}
                   
           }
   
    }
}