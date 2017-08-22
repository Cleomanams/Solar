using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Thinktecture.IdentityModel.Owin.ResourceAuthorization;
using System.Diagnostics;

namespace SolarManager.Models
{
    public class AuthorisationManager : ResourceAuthorizationManager
    {
        public override Task<bool> CheckAccessAsync(ResourceAuthorizationContext context)
        {
            //The IdentityServer admin must add the relevant application claim to the user allow the user to access the application
            if (!context.Principal.HasClaim("application", Properties.Settings.Default.ClientId))
            {
                return Nok();
            }
            switch (context.Resource.First().Value)
            {
                #region A resouce roughly translates to a table in your database
                //This determines if the user has access to the resource.
                //If the user has access, the action parameter will determine
                //whether the action being performed is authorised or not.
                //Again, it's up to you to decided what acions are defined
                //for which resource.
                #endregion
                case "SubUser":
                    return DefaultActionAuthorize(context);
                case "SuperUserEndOfShift":
                    return DefaultActionAuthorize(context);
                case "TempModel":
                    return DefaultActionAuthorize(context);
                default:
                    return Nok();
            }
        }

        private Task<bool> DefaultActionAuthorize(ResourceAuthorizationContext context)
        {
            #region Below is a sample of what actions are. 
            //You can create actions to suit the requirements of your app
            //such as "delete", "update", "create" etc
            #endregion
            switch (context.Action.First().Value)
            {
                case "read":
                    return Eval(context.Principal.HasClaim(ClaimTypes.Role, "reader"));
                case "edit":
                    return Eval(context.Principal.HasClaim(ClaimTypes.Role, "editor"));
                case "create":
                    return Eval(context.Principal.HasClaim(ClaimTypes.Role, "creator"));
                case "delete":
                    return Eval(context.Principal.HasClaim(ClaimTypes.Role, "remover"));

                case "Transactions":
                    return Eval(context.Principal.HasClaim(ClaimTypes.Role, "transactions"));//not added yet
                case "PrintOut":
                    return Eval(context.Principal.HasClaim(ClaimTypes.Role, "printout"));//not added yet
                default:
                    return Nok();
            }
        }
    }
}