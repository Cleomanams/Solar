using SolarManager.Models;
using Microsoft.Owin;
using Owin;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SolarManager
{
    public partial class Startup
    {
        public static List<TempModel> ModelList = TempModel.GetList();
        public static string RegistrationUri = Properties.Settings.Default.RegistrationUri;

        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}