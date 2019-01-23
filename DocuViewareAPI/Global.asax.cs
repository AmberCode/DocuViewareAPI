using GdPicture14.WEB;
using System.Configuration;
using System.Web;
using System.Web.Http;

namespace DocuViewareAPI
{
    public class WebApiApplication : HttpApplication
    {
        public static readonly int SESSION_TIMEOUT = 20;
        private static readonly bool STICKY_SESSION = true;
        private const DocuViewareSessionStateMode DOCUVIEWARE_SESSION_STATE_MODE = DocuViewareSessionStateMode.InProc;

        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            DocuViewareManager.SetupConfiguration(STICKY_SESSION, DOCUVIEWARE_SESSION_STATE_MODE, GetCacheDirectory());
            DocuViewareLicensing.RegisterKEY(ConfigurationManager.AppSettings["DocuViewareLicensingKey"]);
            DocuViewareEventsHandler.NewDocumentLoaded += NewDocumentLoadedHandler;
        }

        private static string GetCacheDirectory()
        {
            return $"{HttpRuntime.AppDomainAppPath}\\Cache";
        }

        private static void NewDocumentLoadedHandler(object sender, NewDocumentLoadedEventArgs e)
        {
            e.docuVieware.PagePreload = e.docuVieware.PageCount <= 50 ? PagePreloadMode.AllPages : PagePreloadMode.AdjacentPages;
        }
    }
}
