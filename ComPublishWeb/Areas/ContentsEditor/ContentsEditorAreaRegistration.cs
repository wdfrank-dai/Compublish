using System.Web.Mvc;

namespace ComPublishWeb.Areas.ContentsEditor
{
    public class ContentsEditorAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "ContentsEditor";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "ContentsEditor_default",
                "ContentsEditor/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
