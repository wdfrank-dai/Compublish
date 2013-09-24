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

            //��֤ע����Ϣ
            string localCode = System.Configuration.ConfigurationManager.AppSettings["LocalCode"];
            if (localCode == null || localCode != "98D4A31D9BC700F0B11F2679E9316814BA3DED4CF7C77EBA")//�����ڼ䱾������ע�����
            {
                if (!Auth())
                {
                    ModelState.AddModelError("", "ϵͳδע�ᣬ�޷���¼��");
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
                            ModelState.AddModelError("", "����¼�ʺ�����������ַ��¼.");
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
                //�ں����ĺ����У�ͨ������UserID = HttpContext.Current.User.Identity.Name.Split(',')[0];�ķ�ʽ�����Ҫ���û���ϢԪ����
                //������ͨ��FormsAuthenticationTicket�ķ�ʽ���μ�http://msdn.microsoft.com/en-us/library/system.web.security.formsauthenticationticket.aspx
                //����ʵ��Cookie�ļ��ܵȵȣ��Ժ�Ҫʵ�֡�
                if (!String.IsNullOrEmpty(returnUrl)) return Redirect(returnUrl);
                else return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "�û��ʺ���Ϣ�����ʺŻ��������.");
            return View(model);
        }

        public ActionResult PortalPageSet()
        {
            List<ChannelModels> sre = new List<ChannelModels> { 
                    new ChannelModels {ChannelID = 1,ChannelName="����ͨ��"},
                    new ChannelModels {ChannelID = 2,ChannelName="��ѧ��Ϣ"},
                    new ChannelModels {ChannelID = 3,ChannelName="������Ϣ"},
                    new ChannelModels {ChannelID = 4,ChannelName="������Ϣ"},
                    new ChannelModels {ChannelID = 4,ChannelName="ͼ����Դ��Ϣ"},
                    new ChannelModels {ChannelID = 5,ChannelName="�鿯��ѯ"},                    
            };

            List<SelectListItem> list = new List<SelectListItem>
                               {
                                    new SelectListItem {Text = "�й���", Value = "red"},
                                    new SelectListItem {Text = "������", Value = "blue"},
                                    new SelectListItem {Text = "ǳ��", Value = "bec-green"},
                                    new SelectListItem {Text = "���ɫ", Value = "olive"},
                                    new SelectListItem {Text = "�ٺ�ɫ", Value = "orange"},
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

        //�����ļ���ע����RegCode���û�����+��Ȩ��AuthCode�����ע������ͬ����true����ͬ����false
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
