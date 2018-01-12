using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using TurnKeyBrokerSignUp.WebUI.Providers;
using Microsoft.Owin.Security.OAuth;
using TurnKeyBrokerSignUp.WebUI.Models;
using System.Data.Entity.Migrations;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

[assembly: OwinStartup(typeof(TurnKeyBrokerSignUp.WebUI.Startup))]

namespace TurnKeyBrokerSignUp.WebUI
{
    public partial class Startup
    {
        public void Configuration(Owin.IAppBuilder app)
        {
            ConfigureOAuth(app);
            //Rest of code is here;
            //app.CreatePerOwinContext<TurnKeyBrokerSignUpDataContext>(() => new TurnKeyBrokerSignUpDataContext());
            //app.CreatePerOwinContext<UserManager<Identity>>(CreateManager);
        }
        //private static UserManager<Identity> CreateManager(IdentityFactoryOptions<UserManager<IdentityUser>> options, IOwinContext context)
        //{
        //    var userStore = new UserStore<IdentityUser>(context.Get<System.Data.Linq.DataContext>());
        //    var owinManager = new UserManager<IdentityUser>(userStore);
        //    return owinManager;
        //}
        public void ConfigureOAuth(Owin.IAppBuilder app)
        {
            Microsoft.Owin.Security.OAuth.OAuthAuthorizationServerOptions OAuthOptions = new Microsoft.Owin.Security.OAuth.OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromHours(8),
                Provider = new ApplicationOAuthProvider()
            };

            app.UseOAuthAuthorizationServer(OAuthOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }

        //private class aspnet_User<T>
        //{
        //}
    }
}
