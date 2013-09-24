using System.Web.Mvc;

//namespace ComPublishWeb.Areas.WidgetAdmin
namespace WidgetAdmin
{
    public class WidgetAdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "WidgetAdmin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "WidgetAdmin_default",
                "WidgetAdmin/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
