using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Web.Mvc;
using WiGetMSModelsLibrary;
using WiGetMS.Models.Repository;
using WiGetMS.Models;

namespace WidgetAdmin.WiGetMS.Controllers
{
    public class ApplicationManageController : Controller
    {
        //public ActionResult Index()
        //{
        //    return View();
        //}

        public class Publishto
        {
            public string Code { get; set; }
            public string Name { get; set; }

            public static List<Publishto> GetPublishtoList()
            {
                return new List<Publishto>
            {
                new Publishto { Code = "W", Name = "Web平台" },
                new Publishto { Code = "M", Name = "移动平台" },
                new Publishto { Code = "T", Name = "客户端平台" }
            };
            }
        }

        public class ShowType
        {
            public int Code { get; set; }
            public string Name { get; set; }

            public static List<ShowType> GetShowType()
            {
                return new List<ShowType>
                {
                    new ShowType{Code=0,Name="半行显示"},
                    new ShowType{Code=1,Name="全行显示"},
                    new ShowType{Code=2,Name="侧边显示"}
                };
            }
        
        }


        [GridAction]
        public ActionResult GetAllApp()
        {
            //ViewData["Publishto"] = new SelectList(Publishto.GetPublishtoList(), "Code", "Name");
            ApplicationManageRepository rep = new ApplicationManageRepository();
            return View(new GridModel(rep.GetAllAppInfo()));
        }

         [GridAction]
        public ActionResult DeleteApps(int id)
        {
     
            string res = "信息删除成功";
            ApplicationManageRepository rep = new ApplicationManageRepository();
            try
            {
                rep.DeleteAppsById(id);
                new ApplicationManageRepository().GetAllAppInfo(id);
            }
            catch
            {
                res = "信息删除失败";
            }
            ViewData["ActionMessagesForDel"] = res;
            return PartialView("../Shared/ShowActionMessage");
        }

         //public ActionResult EditApps(int cid)
         //{
         //    var rep = new ApplicationManageRepository().GetViewData(cid);
         //    var ad = new ApplicationManageRepository();
         //    ViewData["Publishto"] = new SelectList(Publishto.GetPublishtoList(), "Code", "Name", rep.Publishto);
         //    return PartialView("EditApps",new ApplicationManageRepository().GetAppInfoByID(cid));
         //}

        [HttpPost]
        [GridAction]
        public string  SaveAppEdit(showApplication a)
        {
            string Pub = Request.Form["Publishto"].ToString();
            a.Publishto = Pub;
            new ApplicationManageRepository().UpdateAppInfo(a);
            var res = "修改成功";
            return res;

        }

        public ActionResult AddApps()
        {
            var rep = new ApplicationManageRepository().GetViewData(1);
            var ad = new ApplicationManageRepository();
            ViewData["Publishto"] = new SelectList(Publishto.GetPublishtoList(), "Code", "Name", rep.Publishto);
            return PartialView("../ApplicationManage/AddApps");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddApps(string Dataid)
        {
            Application ap = new Application();
            string res = "数据元添加成功";
            ApplicationManageRepository rep = new ApplicationManageRepository();
            try
            {
                if (TryUpdateModel(ap))
                {
                    rep.Insert(ap);
                }
            }
            catch
            {
                res = "数据元添加失败";
            }
            ViewData["ActionMessagesForAdd"] = res;
            return PartialView("../Shared/ShowActionMessage");
        }

        public ActionResult EditApps(int cid)//显示App编辑的窗口
        {
            if (cid != 0)
                return PartialView(new ApplicationManageRepository().GetAppInfoByID(cid));
            else
                return null;
        }

        public ActionResult ApplyForApp()  //APP申请页面
        {
            ViewData["ActionMessagesForApply"] = "";
            ViewData["Publishto"] = new SelectList(Publishto.GetPublishtoList(), "Code", "Name");
            ViewData["ShowType"] = new SelectList(ShowType.GetShowType(), "Code", "Name");
            return View();
        }

        public ActionResult AppExamine()   //管理员审核页面Action
        {
            return View();
        }

        public ActionResult AppOfUser()   //用户申请的所有APP
        {
            ViewData["ActionMessagesForEdit"] = "";
            return View();
        }

        public string Passed(int appid)  //用户申请的App审核通过
        {
            ApplicationManageRepository rep = new ApplicationManageRepository();
            bool pa = true;
            string res;
            if (rep.ExaminePassed(appid, pa) == 2)
            {
                res = "审核通过操作成功";
            }
            else
            {
                res = "审核通过操作失败";
            }
            //ViewData["Passed"] = res;
            return res;
        }



        public string NoPassed(int appid)  //用户申请的APP审核不通过
        {
            ApplicationManageRepository rep = new ApplicationManageRepository();
            bool pa = false;
            string res;
            if (rep.ExaminePassed(appid, pa) == 1)
            {
                res = "审核不通过操作成功";
            }
            else
            {
                res = "审核不通过操作失败";
            }
            //ViewData["Passed"] = res;
            return res;
        }

        [GridAction]
        public ActionResult GetAllAppOfExamine() //获取未审核和审核未通过的APP
        {
            ApplicationManageRepository rep = new ApplicationManageRepository();
            return View(new GridModel(rep.GetAllAppInfoExamine()));
        }

        [GridAction]
        public ActionResult GetAllAppOfUser()
        {
             //var name = ViewBag.UserName;
            string name;
            name = "杨蕴坚";
            ApplicationManageRepository rep = new ApplicationManageRepository();
            return View(new GridModel(rep.GetAllAppInfoUser(name)));
        }

        public ActionResult AppExaminceWindow(int confid) //获得一个APP元组的信息
        {
            ApplicationManageRepository re = new ApplicationManageRepository();
            if (confid == 0)
                return null;
            return PartialView(re.GetAppByID(confid));
        }


        public ActionResult AppOfUserEditWindow(int confid)
        {
            ApplicationManageRepository re = new ApplicationManageRepository();
            string[] myselected;
            var app = re.GetAppByID(confid);
            myselected = app.publishtoMR.Split(',');
            var rep = re.GetViewDataShowType(confid).ShowType;
            ViewData["Publishto"] = new MultiSelectList(Publishto.GetPublishtoList(), "Code", "Name",myselected);
            ViewData["ShowType"] = new SelectList(ShowType.GetShowType(), "Code", "Name",rep);
            if (confid == 0)
                return null;
            return PartialView(app);
        }

        public ActionResult AppOfUserShowWindow(int confid)
        {
            ApplicationManageRepository re = new ApplicationManageRepository();
            if (confid == 0)
                return null;
            return PartialView(re.GetAppByID(confid));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public string ApplyForApp(Application ad,showApplication data)
        {
            //Application ap = new Application();
            //if (data.Appname == null)
            //{
            //    ModelState.AddModelError("Appname", "App名称不能为空");
            //}
            //if (!ModelState.IsValid)
            //{
            //    return PartialView("ApplyForApp");
            //}

            string res;
            string message = "";
            ApplicationManageRepository rep = new ApplicationManageRepository();
            string Pub = Request.Form["Publishto"].ToString();
            string Shty = Request.Form["ShowType"].ToString();

            if (Request.Files["FileUpload"] != null)
            {
                if (Request.Files["FileUpload"].ContentLength != 0)
                {

                    string upmess = rep.FileUpLoad(Request.Files["FileUpload"].FileName, Server.MapPath("~/Content/LogoImages/"));
                    if (upmess != "1")
                    {
                        Request.Files["FileUpload"].SaveAs(upmess);

                    }
                    else
                    {
                        message = message + "要上传的会议支撑文件已经存在！";
                    }
                    string filepath = Request.Files["FileUpload"].FileName;
                    string filename = filepath.Substring(filepath.LastIndexOf("\\") + 1);

                    ad.logourl = "~/Content/LogoImages/" + filename;
                }
            }
            else
                ad.logourl = null;
            autoset(ad);
            try
            {
                ad.passed = 0;
                ad.publishto = Pub;
                ad.ShowType = int.Parse(Shty);
                if (rep.AppApplyForInsert(ad))
                {
                    res = "第三方应用申请成功";
                }
                else
                {
                    res = "添加的第三方应用名称已经存在";
                }
            }
            catch
            {
                res = "第三方应用申请失败";
            }
            ViewData["Publishto"] = new SelectList(Publishto.GetPublishtoList(), "Code", "Name");
            //ViewData["ActionMessagesForApply"] = res;
            return res;
        }


        private void autoset(Application module)  //获取操作人的信息，和操作的时间
        {
            module.lastmodifytime = DateTime.Now.Date;
            //var a = ViewBag.EmployeeID;
            //module.OperatorID = ViewBag.EmployeeID;
            //module.OperatorName = ViewBag.UserName;
            module.@operator = "杨蕴坚";
        }

    }
}
