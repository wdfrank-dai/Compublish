//using System.Web.Mvc;

//namespace ComPublishWeb.Areas.AuthorityMgt
//{
//    public class AuthorityMgtAreaRegistration : AreaRegistration
//    {
//        public override string AreaName
//        {
//            get
//            {
//                return "AuthorityMgt";
//            }
//        }

//        public override void RegisterArea(AreaRegistrationContext context)
//        {
//            context.MapRoute(
//                "AuthorityMgt_default",
//                "AuthorityMgt/{controller}/{action}/{id}",
//                new { action = "Index", id = UrlParameter.Optional }
//            );
//        }
//    }
//}

using System.Web.Mvc;

namespace AuthorityMgt
{
    public class AuthorityMgtAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "AuthorityMgt";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "AuthorityMgt_default",
                "AuthorityMgt/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}