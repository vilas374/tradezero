using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Owin;

namespace TurnKeyBrokerSignUp.WebUI.Models
{
    public class AuthorizationServerProvider: OAuthAuthorizationServerProvider
    {
        #region
        RegistrationInfo regModel = new RegistrationInfo();
        bool output = false;
        TurnKeyBrokerSignUpDataContext dataContext = new TurnKeyBrokerSignUpDataContext();
        bool isDebug = false;
        string ob = string.Empty;
        string _cookieType = string.Empty;
        Helper objHelper = new Helper();
        SignupHelper objSignupHelper = new SignupHelper();
        string StoreURI = " ";
        string UserName = string.Empty;
        string Password = string.Empty;
        tb_account account = new tb_account();
        tb_address addrs = new tb_address();
        //Plaid.Net.HttpPlaidClient objplaid = new Plaid.Net.HttpPlaidClient(serviceUri,clientId,clientSecret);
        #endregion
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {

            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
         
                //using (Signup _repo = new Signup())
                //{
                //    IdentityUser user = await _repo.FindUser(context.UserName, context.Password);

                //    if (user == null)
                //    {
                //        context.SetError("invalid_grant", "The user name or password is incorrect.");
                //        return;
                //    }
                //}

                var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim("sub", context.UserName));
            identity.AddClaim(new Claim("role", "user"));

            context.Validated(identity);
            //var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            //context.Validated(identity);
        }
    }
}