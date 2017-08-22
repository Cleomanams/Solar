using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OpenIdConnect;
using System.Web;
using System.Web.Mvc;

namespace SolarManager
{
    public class AccountController : Controller
    {
        [HttpGet]
        public void Login()
        {
            if (!Request.IsAuthenticated)
            {
                Request.GetOwinContext().Authentication.Challenge(
                    new AuthenticationProperties
                    {
                        RedirectUri = Properties.Settings.Default.RedirectUri
                    },
                    OpenIdConnectAuthenticationDefaults.AuthenticationType);
            }
        }

        [HttpPost]
        public ActionResult LogOff()
        {
            if (Request.GetOwinContext().Authentication.User.Identity.IsAuthenticated)
            {
                Request.GetOwinContext().Authentication.SignOut();
            }
            return Redirect(Properties.Settings.Default.PostLogoutRedirectUri);
        }        
    }
}