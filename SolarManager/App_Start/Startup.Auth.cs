using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.Cookies;
using Owin;
using Microsoft.Owin.Security.OpenIdConnect;
using System.Web.Helpers;
using System.Security.Claims;
using SolarManager.Models;
using NLog;
using Microsoft.Owin.Security;
using Microsoft.IdentityModel.Protocols;
using System.Threading.Tasks;

namespace SolarManager
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            Logger _log = LogManager.GetCurrentClassLogger();
            _log.Info("ConfigureAuth");
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
            });


            app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
            {
                Authority = Properties.Settings.Default.Authority,
                ClientId = Properties.Settings.Default.ClientId,
                Scope = "openid profile roles",
                RedirectUri = Properties.Settings.Default.RedirectUri,
                ResponseType = "id_token",
                PostLogoutRedirectUri = Properties.Settings.Default.PostLogoutRedirectUri,

                SignInAsAuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,


                Notifications = new OpenIdConnectAuthenticationNotifications
                {
                    //This is to attach the id_token as a claim so that the logout can access the claim once the IdentityServer has logged the user out of the user service.
                    SecurityTokenValidated = n =>
                    {
                        var id = n.AuthenticationTicket.Identity;

                        id.AddClaim(new Claim("id_token", n.ProtocolMessage.IdToken));
                        n.AuthenticationTicket = new AuthenticationTicket(id, n.AuthenticationTicket.Properties);
                        return Task.FromResult(0);
                    },

                    //This uses the id_token claim set above to access the redirect uri
                    RedirectToIdentityProvider = n =>
                    {
                        if (n.ProtocolMessage.RequestType == OpenIdConnectRequestType.LogoutRequest)
                        {
                            var idTokenHint = n.OwinContext.Authentication.User.FindFirst("id_token");
                            if (idTokenHint != null)
                            {
                                n.ProtocolMessage.IdTokenHint = idTokenHint.Value;
                            }
                        }
                        return Task.FromResult(0);
                    },
                }
            });

            //This tells the app what class will be looking after Authorization (access to resources, NOT Authentication which is validating the user)
            app.UseResourceAuthorization(new AuthorisationManager());
            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;
        }
    }
}