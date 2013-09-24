using System.Web.Mvc;

namespace ComPublishWeb.Areas.AffairsAdmin
{
    public class AffairsAdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "AffairsAdmin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "AffairsAdmin_default",
                "AffairsAdmin/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
