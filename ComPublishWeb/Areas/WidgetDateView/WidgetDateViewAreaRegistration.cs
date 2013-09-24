using System.Web.Mvc;

namespace WidgetDateView
{
    public class WidgetDateViewAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "WidgetDateView";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "WidgetDateView_default",
                "WidgetDateView/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
