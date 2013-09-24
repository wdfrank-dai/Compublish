using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WiGetMSModelsLibrary;

namespace WiGetMS.Models.Repository
{
    public class PageRepository
    {
        public IEnumerable<ShowPage> GetAllPage()
        {
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            var res = db.ApplicationPage.ToList();
            //var cl = new addressbookDataContext().classinfo.ToList();
            var rs = from m in res
                     select new ShowPage
                     {
                         id = m.id,
                         pagename = m.pagename == null ? "" : m.pagename,
                         showtypeid = m.showtypeid == null ? 0 : m.showtypeid.Value,
                         showtypename = m.ApplicationShowType.name,//
                         //nextpage = m.nextpage == null ? 0 : m.nextpage.Value,
                         //nextpagename = db.ApplicationPage.Where(p => p.id == m.nextpage).First().pagename,
                         actionname = m.actionname == null ? "" : m.actionname,
                         note = m.note == null ? "" : m.note,
                         appid = m.appid == null ? 0 : m.appid.Value,
                         appname = m.Application.appname,
                         actionunits = m.actionunits == null ? "" : m.actionunits,
                         actionunitsname = GetUnitsName(m.id),
                         isStartS = GetisStartConvert(m.id),
                         isEffictiveS = GetisEffictiveConvert(m.id),
                         lastmodifytime = m.lastmodifytime.HasValue ? m.lastmodifytime.Value.ToString("yyyy-MM-dd HH:mm") : "",
                     };
            return rs;
        }

        public IEnumerable<ShowPage> GetAllPageByAppid(int apid)
        {
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            var res = db.ApplicationPage.Where(p=>p.appid==apid).ToList();

            var rs = from m in res
                     select new ShowPage
                     {
                         id = m.id,
                         pagename = m.pagename == null ? "" : m.pagename,
                         showtypeid = m.showtypeid == null ? 0 : m.showtypeid.Value,
                         showtypename = m.ApplicationShowType.name,//
                         //nextpage = m.nextpage== null ? 0 : m.nextpage.Value,
                         //nextpagename = db.ApplicationPage.Where(p => p.id == m.nextpage).First().pagename,
                         actionname = m.actionname == null ? "" : m.actionname,
                         note = m.note == null ? "" : m.note,
                         appid = m.appid == null ? 0 : m.appid.Value,
                         appname=m.Application.appname,
                         actionunits = m.actionunits == null ? "" : m.actionunits,
                         actionunitsname=GetUnitsName(m.id),
                         isStartS = GetisStartConvert(m.id),
                         isEffictiveS = GetisEffictiveConvert(m.id),
                         lastmodifytime = m.lastmodifytime.HasValue ? m.lastmodifytime.Value.ToString("yyyy-MM-dd HH:mm") : "",
                     };
            return rs;
        }

        public string GetUnitsName(int pid)
        {
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            var res=(from pa in  db.ApplicationPage where pa.id==pid select pa.actionunits).FirstOrDefault();          
            var array=res.Split(',').Where(t => t.Trim() != "").Select(t => Convert.ToInt16(t.Trim())).ToArray();
            string last = "";
            foreach (int i in array)
            {
                var a = i;
                var re = db.ApplicationUnits.Where(u => u.id == a).First();
                last +=re.unitname+"  ";              
            }
            return last;
        }

        public ApplicationPage GetPageByID(int id)     //通过传递过来的ID来查找模块的全部信息
        {
            using (WiGetMSLinqDataContext systemdc = new WiGetMSLinqDataContext())
            {
                try
                {
                    return systemdc.ApplicationPage.FirstOrDefault(o => o.id == id);
                }
                catch { return null; }

            }
        }

        public IEnumerable<ApplicationPage> Insert(ApplicationPage ad)
        {
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            db.ApplicationPage.InsertOnSubmit(ad);
            db.SubmitChanges();
            return null;
        }

        public ShowPage GetPageInfoByID(int confid)
        {
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            IList<ShowPage> result1;
            var temp = (from m in new WiGetMSLinqDataContext().ApplicationPage
                        where m.id == confid
                        select new
                        {
                            id = m.id,
                            pagename = m.pagename == null ? "" : m.pagename,
                            showtypeid = m.showtypeid == null ? 0 : m.showtypeid.Value,
                            showtypename = m.ApplicationShowType.name,
                            //nextpage = m.nextpage == null ? 0 : m.nextpage.Value,
                            //nextpagename = db.ApplicationPage.Where(p => p.id == m.nextpage).First().pagename,
                            note = m.note == null ? "" : m.note,
                            actionname = m.actionname == null ? "" : m.actionname,
                            appid = m.appid == null ? 0 : m.appid.Value,
                            appname = m.Application.appname,
                            actionunits = m.actionunits == null ? "" : m.actionunits,
                            isStart = m.isStart == null ? false : m.isStart.Value,
                            isEffictive = m.isEffictive == null ? false : m.isEffictive.Value,
                        }).ToList();
            result1 = (from m in temp
                       select new ShowPage
                       {
                           id = m.id,
                           pagename = m.pagename,
                           showtypeid = m.showtypeid,
                           showtypename = m.showtypename,
                           //nextpage = m.nextpage,
                           //nextpagename = m.nextpagename,
                           actionname = m.actionname,
                           note = m.note,
                           appid = m.appid,
                           appname=m.appname,
                           actionunits = m.actionunits,
                           
                           isStart = m.isStart,
                           isEffictive = m.isEffictive,
                       }
                     ).ToList();

            return result1.FirstOrDefault();
        }

        public bool UpdatePageByAdmin(ApplicationPage ap)
        {
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            var pa = db.ApplicationPage.First(e => e.id == ap.id);
            
            try
            {
                if (pa != null)
                {
                    pa.id = ap.id;
                    pa.pagename = ap.pagename;
                    pa.showtypeid = ap.showtypeid;
                    //pa.nextpage = ap.nextpage;
                    pa.actionname = ap.actionname;
                    pa.note = ap.note;
                    pa.appid = ap.appid;
                    pa.actionunits = ap.actionunits;
                    pa.isStart = ap.isStart;
                    pa.isEffictive = ap.isEffictive;
                    pa.lastmodifytime = ap.lastmodifytime;
                    db.SubmitChanges();                 
                }
                return true ;
            }
            catch { return false; }
        }

        public IQueryable showtypelist()
        {
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            var showstylename = from m in db.ApplicationShowType select m;
            return showstylename;
        }//showtypelist

        public IQueryable Applist()
        {
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            var appname = from m in db.Application select m;
            return appname;
        }//showtypelist

        public IQueryable pagelist( )
        {
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            var pagename = from m in db.ApplicationPage select m;
            return pagename; 
        }

        public IQueryable units()//获得units的下拉列表
        {
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            var unitname = from m in db.ApplicationUnits select m;
            return unitname;
        }

        public bool  InsertPageByAdmin(ApplicationPage ap)
        {
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            try
            {
                db.ApplicationPage.InsertOnSubmit(ap);
                db.SubmitChanges();
                return true ;
            }
            catch { return false; }

        }

        public IEnumerable<SelectListItem> GetShowtype()//获得Appshowstyle
        {
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            var a = (from t in db.ApplicationShowType
                     select new SelectListItem
                     {
                         Value = t.id.ToString(),
                         Text = t.name

                     }).ToList();
            IList<SelectListItem> result = new List<SelectListItem>();
            result.Add(new SelectListItem() { Value = null, Text = "" });
            foreach (var item in a)
            {
                result.Add(item);
            }
            return result;

        }

        public bool DeletePageByAdmin(int id)
        {
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            var Pa = db.ApplicationPage.Where(e => e.id == id).FirstOrDefault();
            try
            {
                if (Pa != null)
                {
                    db.ApplicationPage.DeleteOnSubmit(Pa);
                    db.SubmitChanges();
                }
                return true ;
            }
            catch 
            {
                return false;
            }
        }

        public bool DeletePage(int id)
        {
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            var Pa = db.ApplicationPage.Where(e => e.id == id).FirstOrDefault();
            try
            {
                if (Pa != null)
                {
                    db.ApplicationPage.DeleteOnSubmit(Pa);
                    db.SubmitChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        static int asd = 0;
        //删除Units，对应的Page中的actionunits清空
        public void DeletePageByUnits(int id)
        {
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            foreach (var i in db.ApplicationPage.Where(e => e.actionunits.Contains(id.ToString())))
            {
                ApplicationPage res = db.ApplicationPage.Where(e => e.actionunits.Contains(id.ToString())).FirstOrDefault();
                ShowPage units = new ShowPage();
                res.actionunits = units.actionunits;
                db.SubmitChanges();
            }
            return ;
        }

        public ShowPage GetViewDatashowtype(int pid)
        {
            IList<ShowPage> result1;
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            var res = (from page in db.ApplicationPage
                       where page.id == pid
                       select new
                       {
                           //nextpagename=page.pagename,
                           //nextpage=page.id,
                           showtypeid = page.showtypeid.Value,
                           //showtypename = page.ApplicationShowType == null ? "" : page.ApplicationShowType.name,
                       }).ToList();
            result1 = (from u in res
                       select new ShowPage
                       {
                           //nextpagename=u.nextpagename,
                           //nextpage=u.nextpage,
                           showtypeid = u.showtypeid,
                           //showtypename = u.showtypename,
                       }).ToList();
            return result1.FirstOrDefault();
        }

        //public ShowPage GetViewDatanextpage(int pid)
        //{
        //    IList<ShowPage> result1;
        //    WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
        //    var res = (from page in db.ApplicationPage
        //               where page.id == pid
        //               select new
        //               {  
        //                   nextpage = page.nextpage.Value,                 
        //               }).ToList();
        //    result1 = (from u in res
        //               select new ShowPage
        //               {                                     
        //                   nextpage = u.nextpage,
        //               }).ToList();
        //    return result1.FirstOrDefault();
        //}

        public string[]  GetViewDataunit(int pid)
        {
      
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            var resu = db.ApplicationPage.Where(p => p.id == pid).First();
            string[] myunit;
            myunit = resu.actionunits.Split(',');
            return myunit;
        
        }

        public ShowPage GetPageInfo(int confid)
        {
            IList<ShowPage> result1;
            //if (confid == null)
            //    return null;
            var temp = (from m in new WiGetMSLinqDataContext().ApplicationPage
                        where m.id == confid
                        select new
                        {
                            id = m.id,
                            pagename = m.pagename == null ? "" : m.pagename,
                            showtypeid = m.showtypeid == null ? 0 : m.showtypeid.Value,
                            //nextpage = m.nextpage == null ? 0 : m.nextpage.Value,
                            note = m.note == null ? "" : m.note,
                            actionname = m.actionname == null ? "" : m.actionname,
                            appid = m.appid == null ? 0 : m.appid.Value,
                            actionunits = m.actionunits == null ? "" : m.actionunits,
                            isStart = m.isStart == null ? false : m.isStart.Value,
                            isEffictive = m.isEffictive == null ? false : m.isEffictive.Value,
                        }).ToList();
            result1 = (from m in temp
                       select new ShowPage
                       {
                           id = m.id,
                           pagename = m.pagename,
                           showtypeid = m.showtypeid,
                           //nextpage = m.nextpage,
                           actionname = m.actionname,
                           note = m.note,
                           appid = m.appid,
                           actionunits = m.actionunits,
                           isStart = m.isStart,
                           isEffictive = m.isEffictive,
                       }
                     ).ToList();

            return result1.FirstOrDefault();
        }

        public string GetisStartConvert(int pid)  //将Ispublish（是否发布）属性值准换为汉字
        {
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            var bo = (from rp in db.ApplicationPage where rp.id == pid select rp.isStart).FirstOrDefault();
            bool res = bo == null ? false : bo.Value;
            string pa;
            if (res)
            {
                pa = "是";
            }
            else
            {
                pa = "否";
            }
            return pa;
        }

        public string GetisEffictiveConvert(int pid)  //将Ispublish（是否发布）属性值准换为汉字
        {
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            var bo = (from rp in db.ApplicationPage where rp.id == pid select rp.isEffictive).FirstOrDefault();
            bool res = bo == null ? false : bo.Value;
            string pa;
            if (res)
            {
                pa = "是";
            }
            else
            {
                pa = "否";
            }
            return pa;
        }
    }
}