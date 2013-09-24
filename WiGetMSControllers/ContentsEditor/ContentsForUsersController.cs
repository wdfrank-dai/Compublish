using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Web.Mvc;
using WiGetMSModelsLibrary;
using WiGetMS.Models;

namespace ComPublishWeb.Areas.ContentsEditor.Controllers
{
    public class ContentsForUsersController : Controller
    {
        //
        // GET: /ContentsEditor/ContentsForUsers/

        public ActionResult Index()
        {
            ViewBag.Operator = "杨佳";
            return View();
        }

        [GridAction]
        public ActionResult SelectForTelerik()
        {
            ContentsForUsersRepository rep = new ContentsForUsersRepository();
            string name;
            //name = ViewBag.UserName;
            name = "李伟";
            return View(new GridModel(rep.GetAllContentInfoUser(name)));
        }

        //添加内容
        public ActionResult ContentsInsert()
        {
            return View("../ContentsForUsers/ContentsInsert");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public string ContentsInsert(FormCollection collection, Content data)
        {
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            var res = "添加成功！";
            //Content ads = new Content();
            var contents = collection["editor1"];
            data.t_content = contents;
            data.priority = 5;
            data.passed = "0";
            OperatorInfo(data);
            bool rel = new ContentsForUsersRepository().Insertcontents(data);
            if (rel == false)
                res = "添加失败！";
            else
                res = "添加成功！";
            return res;
        }

        [GridAction]
        public ActionResult GetAllContent()
        {
            // var name = ViewBag.UserName;
            string name;
            name = "李伟";
            ContentsForUsersRepository rep = new ContentsForUsersRepository();
            return View(new GridModel(rep.GetAllContentInfoUser(name)));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public string Create(FormCollection collection, Content content) //添加Contenet
        {
            string res;
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            ContentsForUsersRepository re = new ContentsForUsersRepository();
            ContentsModel co = new ContentsModel();
            content.t_content = collection["CreateContent"];
            content.passed = "0";
            CreateInfo(content);
            if (content.stick == true)
            {
                content.priority = 0;
            }
            else
            {
                content.priority = 5;
            }
            if (re.ContentInsert(content) == true)
            {
                res = "添加成功";
            }
            else
            {
                res = "添加失败";
            }
            // ViewData["AddContent"] = res;
            return res;
        }


        public ActionResult ContentOfUserEditWindow(int confid)
        {
            ContentsForUsersRepository re = new ContentsForUsersRepository();
            if (confid == 0)
                return null;
            var pu = re.GetContentByID(confid);
            return PartialView(pu);
        }


        public ActionResult ContentOfUserShowWindow(int confid)
        {
            ContentsForUsersRepository re = new ContentsForUsersRepository();
            if (confid == 0)
                return null;
             var content = re.GetContentByID(confid);
             ViewData["t_content"] = content.T_content;
            return PartialView(content);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public string SaveContentEdit(FormCollection collection, Content a)
        {
            string res;
            ContentsForUsersRepository rep = new ContentsForUsersRepository();
            a.t_content = collection["ContentEditOfUser"];
            OperatorInfo(a);
            if (rep.UpdateContentByUser(a))
            {
                res = "修改成功";
            }
            else
            {
                res = "修改失败";
            }
            return res;
            //ViewData["ActionMessagesForEdit"] = "修改成功";
            //return View("AppOfUser");

        }


        private void CreateInfo(Content module)  //作者添加时的信息（作者，添加时间）
        {
            module.createtime = DateTime.Now.Date;
            //ar a = ViewBag.EmployeeID;
            //module.OperatorID = ViewBag.EmployeeID;
            //module.OperatorName = ViewBag.UserName;
            //module.creator = "李伟";
        }

        private void OperatorInfo(Content module)  //获取操作人的信息和操作的时间
        {
            module.lastmodifytime = DateTime.Now.Date;
            //ar a = ViewBag.EmployeeID;
            //module.OperatorID = ViewBag.EmployeeID;
            //module.OperatorName = ViewBag.UserName;
            module.@operator = "李伟";
        }

        private void VerifyInfo(Content module)  //获取审核人的信息和审核时间
        {
            module.verifytime = DateTime.Now.Date;
            module.verifier = "李伟";
        }
    }
}
