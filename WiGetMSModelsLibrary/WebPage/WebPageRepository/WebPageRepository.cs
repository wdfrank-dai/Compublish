using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WiGetMSModelsLibrary;
using System.Web.Mvc;

namespace WiGetMS.Models.Repository
{
    public class WebPageRepository
    {
        public IList<WebPageSetModel> WebPageSetAll()
        {
            try
            {
                WebpageDataContext db = new WebpageDataContext();
                IList<WebPageSetModel> rs =
                            (from m in db.WebPageSet
                             select new WebPageSetModel
            {
                pageno = m.pageno,
                pagename = m.pagename,
                descripte = m.descripte,
                iffrist = m.iffrist.Value,
                layoutstyle = m.layoutstyle,
                pageshowscape = m.pageshowscape
            }).ToList();
                return rs;
            }
            catch
            {
                return null;
            }
        }

        public void WebPageSetInsert(WebPageSetModel model)
        {
            try
            {
                WebpageDataContext db = new WebpageDataContext();
                var rs = (from m in db.WebPageSet where m.pagename == model.pagename select m).FirstOrDefault();
                if(rs == null)
                {
                    var re = new WebPageSet();
                    re.pagename = model.pagename;
                    re.descripte = model.descripte;
                    re.iffrist = model.iffrist;
                    re.layoutstyle = model.layoutstyle;
                    re.pageshowscape = model.pageshowscape;
                
                    db.WebPageSet.InsertOnSubmit(re);
                    db.SubmitChanges();
                }               
            }
            catch
            {

            }
        }


        public void WebPageSetUpdate(WebPageSetModel eventss)
        {
            try
            {
                WebpageDataContext db = new WebpageDataContext();
                WebPageSet db_d = db.WebPageSet.First(c => c.pageno == eventss.pageno);


                db_d.descripte = eventss.descripte;
                db_d.iffrist = eventss.iffrist;
                db_d.layoutstyle = eventss.layoutstyle;
                db_d.pagename = eventss.pagename;
                db_d.pageshowscape = eventss.pageshowscape;

                db.SubmitChanges();
            }
            catch { }
        }

        public void WebPageSetDelete(int id)
        {
            try
            {
                WebpageDataContext db = new WebpageDataContext();
                var db_d = db.WebPageSet.Single(c => c.pageno == id);
                if (db_d != null)
                {
                    db.WebPageSet.DeleteOnSubmit(db_d);
                    db.SubmitChanges();
                }
            }
            catch { }
        }


        //----------------------------------------------------

        public IList<WebPageUnitsModel> WebPageUnitsAll(int pid)
        {
            try
            {
                WebpageDataContext db = new WebpageDataContext();
                IList<WebPageUnitsModel> rs =
                            (from m in db.WebPageUnits
                             where m.pageid == pid
                             select new WebPageUnitsModel
                             {
                                 id = m.id == null ? 1 : m.id,
                                 ifloginshow = m.ifloginshow == null ? false : m.ifloginshow.Value,
                                 pageid = pid,
                                 pageunitid = GetAppUnitstByID(m.pageunitid).unitname,
                                 showcontainer = m.showcontainer,
                                 showorder = m.showorder == null ? 0 : m.showorder.Value,
                                 showtitle = m.showtitle,
                                 specialCssFile = m.specialCssFile,
                                 widgettype = m.widgettype
                             }).OrderBy(o=>o.showorder).ToList();
                return rs;
            }
            catch
            {
                return null;
            }
        }

        public void WebPageUnitsInsert(WebPageUnitsModel model,int pid)
        {
            try
            {
                WebpageDataContext db = new WebpageDataContext();
                
                var re = new WebPageUnits();
                re.ifloginshow = model.ifloginshow;
                re.pageid = pid;
                re.pageunitid = Convert.ToInt32(model.pageunitid);
                re.showcontainer = model.showcontainer;
                re.showorder = model.showorder;
                re.showtitle = model.showtitle;
                re.specialCssFile = model.specialCssFile;
                re.widgettype = model.widgettype;
                db.WebPageUnits.InsertOnSubmit(re);
                db.SubmitChanges();

            }
            catch
            {

            }
        }


        public void WebPageUnitsUpdate(WebPageUnitsModel eventss)
        {
            try
            {
                WebpageDataContext db = new WebpageDataContext();
                WebPageUnits db_d = db.WebPageUnits.First(c => c.id == eventss.id);

                db_d.ifloginshow = eventss.ifloginshow;
                db_d.pageunitid = Convert.ToInt32(eventss.pageunitid);
                db_d.showcontainer = eventss.showcontainer;
                db_d.showorder = eventss.showorder;
                db_d.showtitle = eventss.showtitle;
                db_d.specialCssFile = eventss.specialCssFile;
                db_d.widgettype = eventss.widgettype;

                db.SubmitChanges();
            }
            catch { }
        }

        public void WebPageUnitsDelete(int id)
        {
            try
            {
                WebpageDataContext db = new WebpageDataContext();
                var db_d = db.WebPageUnits.Single(c => c.id == id);
                if (db_d != null)
                {
                    db.WebPageUnits.DeleteOnSubmit(db_d);
                    db.SubmitChanges();
                }
            }
            catch { }
        }
        public IQueryable pageunitidDownList()
        {
            WebpageDataContext db = new WebpageDataContext();
            var rs = from m in db.AppUnits select new { m.id, m.unitname };
            return rs.OrderByDescending(o => o.id);
        
        }
        public AppUnits GetAppUnitstByID(int id)
        {
            WebpageDataContext db = new WebpageDataContext();
            var rs = (from m in db.AppUnits where m.id == id select m).FirstOrDefault();
            return rs;

        }
        public WebPageSet GetWebPageSetByStr(string str)
        {
            WebpageDataContext db = new WebpageDataContext();
            var rs = (from m in db.WebPageSet where m.pagename == str select m).FirstOrDefault();
            return rs;

        }

    }
}
