using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Web.Mvc;
using WiGetMSModelsLibrary;
using WiGetMS.Models;

namespace ComPublishWeb.Areas.ContentsEditor.Contents.Controllers
{
    public class ContentsController : Controller
    {
        //
        // GET: /ContentsEditor/Contents/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ContentsEditor(int id)
        {
            return View("../Contents/PassedContents");
        }

        [GridAction]
        public ActionResult SelectForTelerik()
        {
            ContentsRepository rep = new ContentsRepository();
            return View(new GridModel(rep.GetAllContents()));            
        }

        public ActionResult PassedContents(int confid)
        {
            Content edit = new Content();
            var rep = new ContentsRepository().PassedForContents(confid);
            return PartialView(rep);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public string ContentsForSave(FormCollection collection, ContentsModel data)
        {
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            string rep;
            var passed = Request.Form["passed"];
            Content ads = new Content();
            var contents = collection["PassedContent"];
            data.T_content = contents;
            data.Passed = passed;
            data.Verifytime = DateTime.Now.Date.ToString();
            data.Verifier = "杨佳"; //审核人
            new ContentsRepository().GetEditContents(data);
            ViewData["ActionMessagesForEdit"] = "操作成功";
            rep = "操作成功";
            return rep;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public string Passed(int contentsid, FormCollection collection, ContentsModel data)
        {
            ContentsRepository rep = new ContentsRepository();
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            Content ads = new Content();
            var contents = collection["editor1"];
            data.T_content = contents;
            data.Verifytime = DateTime.Now.Date.ToString();
            data.Verifier = "杨佳";
            string res = "1";
            //if(rep.ExaminePassed(contentsid) == )
            return res;
        }

    }
}
