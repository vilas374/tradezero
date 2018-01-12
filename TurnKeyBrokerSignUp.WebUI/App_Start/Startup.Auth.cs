using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System.Security.Claims;
using System.Threading.Tasks;
using TurnKeyBrokerSignUp.WebUI.Providers;
using TurnKeyBrokerSignUp.WebUI.Models;
using System.Web.Http;
using Microsoft.Owin.Infrastructure;

namespace TurnKeyBrokerSignUp.WebUI
{
    public partial class Startup
    {
        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        public static string PublicClientId { get; private set; }

        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            var oauthProvider = new OAuthAuthorizationServerProvider
            {
                OnGrantResourceOwnerCredentials = async context =>
                {
                    if (context.UserName == "xyz" && context.Password == "xyz@123")
                    {
                        var claimsIdentity = new ClaimsIdentity(context.Options.AuthenticationType);
                        claimsIdentity.AddClaim(new Claim("user", context.UserName));
                        context.Validated(claimsIdentity);
                        return;
                    }
                    context.Rejected();
                },
                OnValidateClientAuthentication = async context =>
                {
                    string clientId;
                    string clientSecret;
                    if (context.TryGetBasicCredentials(out clientId, out clientSecret))
                    {
                        if (clientId == "xyz" && clientSecret == "secretKey")
                        {
                            context.Validated();
                        }
                    }
                }
            };
            var oauthOptions = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/accesstoken"),
                Provider = oauthProvider,
                AuthorizationCodeExpireTimeSpan = TimeSpan.FromMinutes(1),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(3),
                SystemClock = new SystemClock()

            };
            app.UseOAuthAuthorizationServer(oauthOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

            var config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();
            app.UseWebApi(config);
            //  // Configure the db context and user manager to use a single instance per request
            //  app.CreatePerOwinContext(ApplicationDbContext.Create);
            //  app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);

            //  // Enable the application to use a cookie to store information for the signed in user
            //  // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            //  app.UseCookieAuthentication(new CookieAuthenticationOptions());
            //  app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            //  // Configure the application for OAuth based flow
            ////  PublicClientId = "self";
            //  OAuthOptions = new OAuthAuthorizationServerOptions
            //  {
            //      TokenEndpointPath = new PathString("/Token"),
            //      Provider = new ApplicationOAuthProvider(),
            //      AuthorizeEndpointPath = new PathString("/api/SignProcess/Validateuser"),
            //      AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
            //      // In production mode set AllowInsecureHttp = false
            //      AllowInsecureHttp = true
            //  };

            //  // Enable the application to use bearer tokens to authenticate users
            //  app.UseOAuthBearerTokens(OAuthOptions);

            //  // Uncomment the following lines to enable logging in with third party login providers
            //  //app.UseMicrosoftAccountAuthentication(
            //  //    clientId: "",
            //  //    clientSecret: "");

            //  //app.UseTwitterAuthentication(
            //  //    consumerKey: "",
            //  //    consumerSecret: "");

            //  //app.UseFacebookAuthentication(
            //  //    appId: "",
            //  //    appSecret: "");

            //  //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            //  //{
            //  //    ClientId = "",
            //  //    ClientSecret = ""
            //  //});
        }



    }
}
