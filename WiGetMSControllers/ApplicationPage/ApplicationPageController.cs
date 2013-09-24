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
    public class ApplicationPageController : Controller
    {
        static int APPID = 0;
        static int APPID2 = 0;
        public ActionResult Page()
        {
            //PageRepository re = new PageRepository();
            //ViewData["showtypeid"] = new SelectList(re.showtypelist(), "id", "name");
            //ViewData["pagelist"] = new SelectList(re.pagelist(), "nextpage", "pagename");
            //ViewData["Units"] = new SelectList(re.units(), "id", "unitname");
            return View();
        }
        
        public ActionResult AppPageEdit()//App-Page进入的主视图
        {
            PageRepository re = new PageRepository();
            ViewData["showtypeid"] = new SelectList(re.showtypelist(), "id", "name");
            //ViewData["pagelist"] = new SelectList(re.pagelist(), "nextpage", "pagename");
            ViewData["Units"] = new SelectList(re.units(), "id", "unitname");
            Session["nowappid"] = null;
            return View();
        }

        [GridAction]
        public ActionResult GetAllApps()
        {     
            ApplicationManageRepository rep = new ApplicationManageRepository();
            return View(new GridModel(rep.GetAllAppInfo()));
        }

        [GridAction]
        public ActionResult SelectAllPage()
        {
            PageRepository re = new PageRepository();
            return View(new GridModel(re.GetAllPage()));
        }

        [GridAction]
        public ActionResult _SelectPageAjaxEditing(int appid)
        {
            if (appid != 0)
                Session["nowappid"] = appid;
            else if (Session["nowappid"] != null)
                appid = int.Parse(Session["nowappid"].ToString());
            
            PageRepository re = new PageRepository();  
            return View(new GridModel(re.GetAllPageByAppid(appid)));
        }

        public ActionResult EditPage(int pid,int aid)//显示编辑Page弹窗
        {
            APPID2 = aid;
            PageRepository re = new PageRepository();
            var rep = new PageRepository().GetViewDatashowtype(pid);
            //var rep2 = new PageRepository().GetViewDatanextpage(pid);
            var rep3=new  PageRepository().GetViewDataunit(pid);
            ViewData["showtype"] = new SelectList(re.showtypelist(), "id", "name", rep.showtypeid);
            //ViewData["pagelist"] = new SelectList(re.pagelist(), "id", "pagename", rep2.nextpage);
            ViewData["Units"] = new MultiSelectList(re.units(), "id", "unitname",rep3);
            if (pid != 0)
                return PartialView("EditPage", new PageRepository().GetPageInfoByID(pid));
            else
                return null;
        }
        //C:\Users\DELL\Desktop\WiGetMS\WiGetMS\Views\  
        //C:\Users\DELL\Desktop\ComPublishWeb_主平台\ComPublishWeb_主平台\ComPublishWeb\Areas\WidgetAdmin\Views\ApplicationPage\EditPage.ascx
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditPageSaveByAppId(ApplicationPage page)   //点击保存按钮、重置调用。用来完成添加、修改的数据存储
        {

            string res="操作成功";
            PageRepository rep = new PageRepository();
            autoset(page);
            if (Request.Form["showtype"] != "")
            {
                page.showtypeid = int.Parse(Request.Form["showtype"]);
            }
            //if (Request.Form["pagelist"] != "")
            //{
            //    page.nextpage = int.Parse(Request.Form["pagelist"]);
            //}
            if (Request.Form["Units"] != "")
            {
                page.actionunits = Request.Form["Units"];
            }
            page.appid = APPID2;
            if (rep.UpdatePageByAdmin(page))
                ViewData["Operate"] = res;
            else
                ViewData["Operate"] = "操作失败";
            return PartialView("../Shared/ShowActionMessage");
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public string  EditPageSave(string EditModule, ApplicationPage page)   //点击保存按钮、重置调用。用来完成添加、修改的数据存储
        {         
            string res;
            PageRepository systemdb = new PageRepository();
            autoset(page);
            if (page.note == null)
                page.note = "";
            if (systemdb.GetPageByID(page.id) != null)   //修改
            {
                string uni = Request.Form["Units"].ToString();
                int Appid = int.Parse(Request.Form["Appname"].ToString());
               // string nex = Request.Form["pagelist"].ToString();
                string sho = Request.Form["showtype"].ToString();
                page.actionunits = uni;
                page.appid = Appid;
                //page.nextpage = int.Parse(nex);
                page.showtypeid = int.Parse(sho);

                try
                {
                    if (systemdb.UpdatePageByAdmin(page))
                    {
                        res = "操作成功";
                    }
                    else res = "操作失败";
                }
                catch
                {
                    res = "操作失败";
                }
            }

            else                 // 添加操作
            {
                string Uni = Request.Form["Units2"].ToString();
                int appid = int.Parse(Request.Form["Appname2"].ToString());
               // string Nex = Request.Form["pagelist2"].ToString();
                string Sho = Request.Form["showtypeid2"].ToString();
                page.actionunits = Uni;
                page.appid = appid;
                //page.nextpage = int.Parse(Nex);
                page.showtypeid = int.Parse(Sho);

                if (systemdb.InsertPageByAdmin(page))
                {
                    res = "操作成功";
                }
                else
                {
                    // ViewData["Adderror"] = true;

                    res = "添加失败";
                }

            }
            //ViewData["Operate"] = res;
            return res;
            //return PartialView("../Shared/ShowActionMessage");
        }

        [GridAction]
        public string  _DeletePageAjaxEditing(int id)
        {
            string res;
            PageRepository rep = new PageRepository();
            if (rep.DeletePageByAdmin(id))
            {
                res = "删除成功";
            }
            else
            {
                res = "删除失败";
            }
            return res;
           // return View(new GridModel(rep.GetAllPage()));
        }

        public string _DeletePage(int id)
        {
            string res;
            PageRepository rep = new PageRepository();
            if (rep.DeletePage(id))
            {
                res = "删除成功";
            }
            else
            {
                res = "删除失败";
            }
            return res;
        }

        public ActionResult PageAdd(int aid)//显示添加Page的弹窗
        {
            PageRepository rep = new PageRepository();
            ViewData["Appid"] = aid;
            APPID = aid;
            //ViewData["pagelist2"] = new SelectList(rep.pagelist(), "nextpage", "pagename");
            ViewData["showtypeid2"] = new SelectList(rep.showtypelist(), "id", "name");
            ViewData["Units2"] = new SelectList(rep.units(), "id", "unitname");
            return PartialView("AddPage");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult PageAddSave(ApplicationPage ad)//添加page之后点保存
        {
            string Uni = Request.Form["Units2"].ToString();
            //string Nex = Request.Form["pagelist2"].ToString();
            string Sho = Request.Form["showtypeid2"].ToString();
            ApplicationPage ap = new ApplicationPage();
            string res = "数据元添加成功";
            PageRepository rep = new PageRepository();
            ad.appid = APPID;
            ad.actionunits = Uni;
            //ad.nextpage = int.Parse(Nex);
            ad.showtypeid = int.Parse(Sho);
            ad.lastmodifytime = DateTime.Now;

            try
            {
                ap.actionunits = Uni;
                rep.Insert(ad);
            }
            catch
            {
                res = "数据元添加失败";
            }
            ViewData["ActionMessagesForAdd"] = res;
            return PartialView("../Shared/ShowActionMessage");
        }

        private void autoset(ApplicationPage page)  //获取操作人的信息，和操作的时间
        {
            page.lastmodifytime = DateTime.Now.Date;
            //var a = ViewBag.EmployeeID;
            //module.OperatorID = ViewBag.EmployeeID;
            //module.OperatorName = ViewBag.UserName;
        }

        public ActionResult PageEditWindow(int confid)
        {
            PageRepository re = new PageRepository();
            var rep = new PageRepository().GetViewDatashowtype(confid);
            //var rep2 = new PageRepository().GetViewDatanextpage(confid);
            var rep3 = new PageRepository().GetViewDataunit(confid);
            ViewData["Appname"] = new SelectList(re.Applist(), "id", "appname",rep.appname);
            ViewData["showtype"] = new SelectList(re.showtypelist(), "id", "name", rep.showtypeid);
            //ViewData["pagelist"] = new SelectList(re.pagelist(), "id", "pagename", rep2.nextpage);
            ViewData["Units"] = new MultiSelectList(re.units(), "id", "unitname", rep3);
            if (confid == 0)
                return null;
            return PartialView(re.GetPageInfo(confid));
        }

        public ActionResult PageInsertWindow()
        {
            PageRepository rep = new PageRepository();
            ViewData["Appname2"] = new SelectList(rep.Applist(), "id", "appname");
            //ViewData["pagelist2"] = new SelectList(rep.pagelist(), "nextpage", "pagename");
            ViewData["showtypeid2"] = new SelectList(rep.showtypelist(), "id", "name");
            ViewData["Units2"] = new SelectList(rep.units(), "id", "unitname");
            return PartialView();
        }

        
       //////////////////////////////以下ShowType
      
        [GridAction]
        public ActionResult GetAllShowType()
        {
            ShowTypeRepository rep = new ShowTypeRepository();
            return View(new GridModel(rep.GetAllShowType()));
        }

        public string _DeleteShowType(int id)
        {
            string res;
            ShowTypeRepository rep = new ShowTypeRepository();
            if (rep.DeleteShowTypeByAdmin(id))
            {
                res = "删除成功";
            }
            else
            {
                res = "删除失败";
            }
            return res;
        }

        public ActionResult AddShowType()
        {
            // PageRepository re = new PageRepository();
            //ViewData["Unitsid"] = new SelectList(re.units(), "id", "unitname");
            return PartialView("ShowTypeInsertWindow");/////
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public string AddShowtypeForSave(ApplicationShowType ad)
        {
            //ApplicationShowType ad = new ApplicationShowType();            
            string res = "数据元添加成功";
            ShowTypeRepository rep = new ShowTypeRepository();
            try
            {
                rep.InsertShowType(ad);
            }
            catch
            {
                res = "数据元添加失败";
            }
            ViewData["ActionMessagesForShowType"] = res;
            return res;
        }

        public ActionResult EditShowType(int sid)//显示编辑Page弹窗
        {
            if (sid != 0)
                return PartialView("ShowTypeEditWindow", new ShowTypeRepository().GetShowTypeInfoByID(sid));
            else
                return null;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditShowTypeSaveByAppId(ApplicationShowType page)   //点击保存按钮、重置调用。用来完成添加、修改的数据存储
        {
            string res = "操作成功";
            ShowTypeRepository rep = new ShowTypeRepository();

            if (rep.UpdateShowTypeByAdmin(page))
                ViewData["Operate"] = res;
            else
                ViewData["Operate"] = "操作失败";
            return PartialView("../Shared/ShowActionMessage");
        }
    }
}
