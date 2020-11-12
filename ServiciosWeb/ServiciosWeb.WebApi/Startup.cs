using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.Cookies;

[assembly: OwinStartup(typeof(ServiciosWeb.WebApi.Startup))]

namespace ServiciosWeb.WebApi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ConfigureAuth(app);
            //HttpConfiguration config = new HttpConfiguration();
            //WebApiConfig.Register(config);
            //ConfigurarOauth(app);
            //app.UseWebApi(config);

        }

        public void ConfigurarOauth(IAppBuilder app)
        {
            OAuthAuthorizationServerOptions opcionesautorizacion =
               new OAuthAuthorizationServerOptions()
               {
                   AllowInsecureHttp = true,
                   TokenEndpointPath = new PathString("/recuperartoken"),
                   AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(15),
                   Provider = new Credentials.AutorizacionCredencialesToken()
               };
            app.UseOAuthAuthorizationServer(opcionesautorizacion);
            OAuthBearerAuthenticationOptions opcionesoauth =
                new OAuthBearerAuthenticationOptions()
                {
                    AuthenticationMode = Microsoft.Owin.Security.AuthenticationMode.Active
                };

            app.UseOAuthBearerAuthentication(opcionesoauth);
        }

    }
}
