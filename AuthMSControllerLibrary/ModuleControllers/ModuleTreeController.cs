using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using AuthModelManage.Models.Repository;
using AuthModelManage.Models;
using AuthMSModelsLibrary;


namespace AuthorityMgt.AuthModelManage.Controllers
{
    public class ModuleTreeController : Controller
    {
        //
        // GET: /ModuleTree/


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Tree()   //Tree视图的Action
        {
            ViewData["Delete"] = "";
            return View();
        }


        public ActionResult ModuleManage()   //ModuleManage视图的Action
        {
            ViewData["Operate"] = "";
            return View();
        }

        public ActionResult EditModule()     //EditModule视图的Action
        {
            return PartialView();
        }

        public ActionResult ShowModule()      //ShowModule视图的Action
        {
            return View();
        }
        //-----------------------------------------------------------------------------------------------------
        [HttpPost]
        public JsonResult GetModuleTreeDataAll()//没用到地点值，显示全部人员，只是保留
        {
            AuthMSLinqDataContext db = new AuthMSLinqDataContext();
            string dbid = "0";
            var d = db.T_Module.Where(c => c.ModuleParentID.ToString() == dbid).ToList();
            List<ModuleTreeModel> nodes = new List<ModuleTreeModel>();
            foreach (var a in d)
            {
                ModuleTreeModel rootNode = new ModuleTreeModel();
                rootNode.attr = new JsTreeAttribute();
                rootNode.data = a.ModuleName;
                rootNode.attr.id = a.ModuleID;
                var leaf = db.T_Module.Where(c => c.ModuleParentID==a.ModuleID).ToList();
                if (leaf.Count == 0)
                {
                    rootNode.state = "close";
                } //根节点设定为初始打开状态
                else 
                {
                    rootNode.state = "open";
                }
                new ModuleTreeRepository().GetModuleNodeChildren(a.ModuleID.ToString(), rootNode);
                nodes.Add(rootNode);
            }
            return Json(nodes);
        }



        //[HttpPost]
        //public ActionResult GetModuleTreeDataAll(int did)//没用到地点值，显示全部人员，只是保留
        //{
        //    AuthMSLinqDataContext db = new AuthMSLinqDataContext();
        //    int num = 0;
        //    ModuleTreeModel node = new ModuleTreeModel();
        //    node.children = new List<ModuleTreeModel>();
        //    node.attr = new JsTreeAttribute();
        //    node.data = "";
        //    node.attr.id = 0;
        //    node.state = "open"; //根节点设定为初始打开状态
        //    //var Mo = db.T_Module.FirstOrDefault(c => c.ModuleParentID == num);
        //    var b = db.T_Module.Where(c => c.ModuleParentID == num).ToList().OrderBy(c => c.ModulePriority);
        //    if (b != null)
        //    {  
        //        ModuleTreeModel rootNode = new ModuleTreeModel();              
        //        foreach (var d in b)
        //        {                  
        //            rootNode.attr = new JsTreeAttribute();
        //            rootNode.data = d.ModuleName;
        //            rootNode.attr.id = d.ModuleID;
        //            var pid = d.ModuleID;
        //            rootNode.state = "open"; //根节点设定为初始打开状态
        //            new ModuleTreeRepository().GetModuleNodeChildren(pid, rootNode);
        //            node.children.Add(rootNode);
        //        }
        //        return Json(rootNode);
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

      
        //----------------------------------------------Module的编辑操作-------------------------------------------------


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DisplayModule(int moduleid)   //获取模块信息在ShowModule中显示
        {
            // Session["ModuleTreeIDCrew"] = moduleid;
            ModuleTreeRepository re = new ModuleTreeRepository();
            return PartialView("ShowModule", re.ModuleTreeAll(moduleid));

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditModule(int moduleid)   // 获取模块信息在EditModule中进行编辑
        {
            ViewData["Operate"] = "";
            //Session["ModuleTreeIDCrew"] = moduleid;
            ModuleTreeRepository re = new ModuleTreeRepository();
            return PartialView("EditModule", re.ModuleTreeAll(moduleid));

        }



        [AcceptVerbs(HttpVerbs.Post)]
        public string Modify(string EditModule, T_Module module)   //点击保存按钮、重置调用。用来完成添加、修改的数据存储
        {
            string res;
            if (EditModule == "重置")
            {
                ViewData["Module"] = null;
            }

            ModuleTreeRepository systemdb = new ModuleTreeRepository();
            autoset(module);
            if (module.ModuleNotes == null)
                module.ModuleNotes = "";
            if (module.ModuleHierarchy == null)
                module.ModuleHierarchy = "";


            if (systemdb.GetModuleByID(module.ModuleID) != null)   //修改
            {
                try
                {
                    if (systemdb.Update(module))
                    // TempData["promotemessage"] = "操作成功";
                    {

                        //ModuleTreeWriteToFile();////////////////
                        res = "操作成功";
                    }
                    else res = "操作失败";
                }
                catch
                {
                    // ViewData["Moderror"] = true;

                    res = "操作失败";
                    //return "操作失败";

                }
            }

            else                 // 添加操作
            {
                if (module.ModuleHierarchy == null || module.ModuleHierarchy == "")
                    module.ModuleHierarchy = "";
                if (module.ModuleNotes == null || module.ModuleNotes == "")
                    module.ModuleNotes = "";

                if (Add(module))
                {
                    //ModuleTreeWriteToFile();/////////////////////////
                    res = "操作成功";

                }
                else
                {
                    // ViewData["Adderror"] = true;

                    res = "添加失败";
                }

            }
            return res;
            //return View("ModuleManage");
        }



        private void autoset(T_Module module)  //获取操作人的信息，和操作的时间
        {
            module.OperateDate = DateTime.Now.Date;
            //var a = ViewBag.EmployeeID;
            //module.OperatorID = ViewBag.EmployeeID;
            //module.OperatorName = ViewBag.UserName;
            module.OperatorID = 1;
            module.OperatorName = "李伟";
        }



        //-----------------------------Module的添加-----------------------------------------------


        private bool Add(T_Module m)  //添加模块，调用Repository的AddModule方法插入模块。
        {
            ModuleTreeRepository systemdb = new ModuleTreeRepository();
            if (systemdb.AddModule(m))
                return true;
            else return false;


        }


        [HttpPost]
        public ActionResult AddModule(string id) //添加模块时获取ModuleParentID  传递到EditModule视图中
        {
            Module module = new Module();

            int result = -1;
            if (int.TryParse(id, out result))
            {
                module.ModuleParentID = result;
                return PartialView("EditModule", module);
            }
            else return PartialView("EditModule");

        }

        //-------------------------------Module的删除--------------------------------------------

        //[HttpPost]
        public string DeleteModule(string id)   //通过ID值删除模块
        {
            string msg;
            int result = -1;
            ModuleTreeRepository systemdb = new ModuleTreeRepository();
            if (int.TryParse(id, out result))
            {
                if (systemdb.Delete(result))
                {
                    //ModuleTreeWriteToFile();//////////////////////
                    msg = "删除成功";
                }
                else msg = "删除失败";
            }
            else msg = "删除失败";
            //ViewData["Module"] = null;
            //ViewData["Operate"] = res;
            return msg;
            //return View("ModuleManage");

        }
    }
}
