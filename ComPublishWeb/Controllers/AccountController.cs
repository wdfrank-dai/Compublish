using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using ComPublishWeb.Models;
using CoreLibrary.Helper;
using CoreLibrary.Models.Account;
using CoreLibrary.Models.Repository;

namespace ComPublishWeb.Controllers
{
    public class AccountController : Controller
    {

        //
        // GET: /Account/LogOn

        public ActionResult LogOn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            //验证注册信息
            string localCode = System.Configuration.ConfigurationManager.AppSettings["LocalCode"];
            if (localCode == null || localCode != "98D4A31D9BC700F0B11F2679E9316814BA3DED4CF7C77EBA")//开发期间本地跳过注册程序
            {
                if (!Auth())
                {
                    ModelState.AddModelError("", "系统未注册，无法登录！");
                    return View(model);
                }
            }

            //AccountRepository accountRp = new AccountRepository();
            var userinfo = new NewUserRepository().GetUser(model.UserName, model.Password);
            if (userinfo != null)
            {
                string onlineName = userinfo.UserID + userinfo.UserName;
                string loginIp = HttpContext.Request.UserHostAddress;
                OnlineUser nowOnlineUser = new OnlineUser();
                try
                {
                    nowOnlineUser = UserOnlineModule.OnlineList.Find(e => e.UserName == onlineName);
                }
                catch
                { }
                if (nowOnlineUser != null)
                {
                    if (nowOnlineUser.LoginIp != null)
                    {
                        if (nowOnlineUser.LoginIp != loginIp)
                        {
                            ModelState.AddModelError("", "所登录帐号已在其他地址登录.");
                            return View(model);
                        }
                    }
                }
                else
                {
                    OnlineUser newOnlineUser = new OnlineUser();
                    newOnlineUser.UserName = onlineName;
                    newOnlineUser.LoginTime = DateTime.Now;
                    newOnlineUser.LastTime = DateTime.Now;
                    newOnlineUser.LoginIp = HttpContext.Request.UserHostAddress;
                    newOnlineUser.LastActionUrl = HttpContext.Request.Url.PathAndQuery;
                    newOnlineUser.SessionID = HttpContext.Session.SessionID.ToUpper();
                    newOnlineUser.IsGuest = false;
                    UserOnlineModule.OnlineList.Add(newOnlineUser);
                }

                string userData = userinfo.UserID + "," + userinfo.UserName + "," +  userinfo.DepNO +"," +userinfo.PID ;
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1,
                    userData,
                    DateTime.Now,
                    DateTime.Now.AddMinutes(30),
                    false,
                    userData,
                    FormsAuthentication.FormsCookiePath);

                // Encrypt the ticket.
                string encTicket = FormsAuthentication.Encrypt(ticket);

                var cookietemp = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                //cookietemp.Expires = DateTime.Now.AddMinutes(20);
                // Create the cookie.
                Response.Cookies.Add(cookietemp);
                FormsAuthentication.SetAuthCookie(userinfo.UserID + "," + userinfo.UserName + "," + userinfo.DepNO + "," + userinfo.PID, false);
                //在后续的函数中，通过例如UserID = HttpContext.Current.User.Identity.Name.Split(',')[0];的方式获得需要的用户信息元数据
                //还可以通过FormsAuthenticationTicket的方式，参见http://msdn.microsoft.com/en-us/library/system.web.security.formsauthenticationticket.aspx
                //可以实现Cookie的加密等等，以后要实现。
                if (!String.IsNullOrEmpty(returnUrl)) return Redirect(returnUrl);
                else return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "用户帐号信息有误，帐号或密码错误.");
            return View(model);
        }

        public ActionResult PortalPageSet()
        {
            List<ChannelModels> sre = new List<ChannelModels> { 
                    new ChannelModels {ChannelID = 1,ChannelName="部门通告"},
                    new ChannelModels {ChannelID = 2,ChannelName="教学信息"},
                    new ChannelModels {ChannelID = 3,ChannelName="事务信息"},
                    new ChannelModels {ChannelID = 4,ChannelName="科研信息"},
                    new ChannelModels {ChannelID = 4,ChannelName="图书资源信息"},
                    new ChannelModels {ChannelID = 5,ChannelName="书刊查询"},                    
            };

            List<SelectListItem> list = new List<SelectListItem>
                               {
                                    new SelectListItem {Text = "中国红", Value = "red"},
                                    new SelectListItem {Text = "清新蓝", Value = "blue"},
                                    new SelectListItem {Text = "浅绿", Value = "bec-green"},
                                    new SelectListItem {Text = "橄榄色", Value = "olive"},
                                    new SelectListItem {Text = "橘红色", Value = "orange"},
                                };

            ViewData["Channels"] = new SelectList(sre, "ChannelID", "ChannelName");
            ViewData["PageStyle"] = new SelectList(list, "Value", "Text");
            return View();
        }

        [HttpPost]
        public ActionResult PortalPageSet(PortalPageSetModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                //ViewBag.NowPageStyle = model.PageStyle;
                return RedirectToAction("Index", "Home", new { style = model.PageStyle });
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public ActionResult LogOff()
        {
            try
            {
                HttpCookie aCookie;
                string cookieName;
                int limit = Request.Cookies.Count;
                for (int i = 0; i < limit; i++)
                {
                    cookieName = Request.Cookies[i].Name;
                    aCookie = new HttpCookie(cookieName);
                    aCookie.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(aCookie);
                }
                string onlineName = HttpContext.User.Identity.Name.Split(',')[0] + HttpContext.User.Identity.Name.Split(',')[1];
                string loginIp = HttpContext.Request.UserHostAddress;

                OnlineUser nowOnlineUser = UserOnlineModule.OnlineList.Find(e => e.UserName == onlineName && e.LoginIp == loginIp);

                FormsAuthentication.SignOut();
                if (nowOnlineUser != null)
                {
                    UserOnlineModule.OnlineList.Remove(nowOnlineUser);
                }
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register

        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                MembershipCreateStatus createStatus;
                Membership.CreateUser(model.UserName, model.Password, model.Email, null, null, true, null, out createStatus);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, false /* createPersistentCookie */);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", ErrorCodeToString(createStatus));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePassword

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Account/ChangePassword

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {

                // ChangePassword will throw an exception rather
                // than return false in certain failure scenarios.
                bool changePasswordSucceeded;
                try
                {
                    MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true /* userIsOnline */);
                    changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePasswordSuccess

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        //配置文件内注册码RegCode与用机器码+授权码AuthCode算出的注册码相同返回true，不同返回false
        public bool Auth()
        {
            string machCode = "";
            string localRegCode = "";
            string regCode = "";
            string authCode = "";
            try
            {
                regCode = System.Configuration.ConfigurationManager.AppSettings["RegCode"];
                authCode = System.Configuration.ConfigurationManager.AppSettings["AuthCode"];
                if (regCode == null || regCode == "" || authCode == null || authCode == "")
                    return false;

                FileConverter.FileConverter ds = new FileConverter.FileConverter();
                regCode = ds.Decrypt(regCode);
                authCode = ds.Decrypt(authCode);

                machCode = UsrRegisUtils.CodeManageInfo.createMachCode();
                localRegCode = UsrRegisUtils.CodeManageInfo.createRegisCode(authCode, machCode, "", "");
            }
            catch (Exception ex)
            {
                return false;
            }

            if (!(localRegCode.Equals(regCode)))
            {
                return false;
            }
            return true;
        }
        #region Status Codes
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
