using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WiGetMSModelsLibrary;
using System.Web.Mvc;

namespace WiGetMS.Models.Repository
{

    public class ApplicationManageRepository
    {
        public string GetPublishtoList(int pid)
        {
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            string res = (from rp in db.Application where rp.id == pid select rp.publishto).FirstOrDefault();
            var array = res.Split(',').Where(t => t.Trim() != "").Select(t => Convert.ToString(t.Trim())).ToArray();
            string last = "";
            foreach (string i in array)
            {
                var a = i;
                if (a == "W")
                    last += "Web ";
                if (a == "M")
                    last += "移动 ";
                if (a == "T")
                    last += "客户端 ";
            }
            return last;
        }

        public string GetShowTypeList(int pid)
        {
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            int res = (from rp in db.Application where rp.id == pid select rp.ShowType.Value).FirstOrDefault();
            string result=null;
            if (res == 0)
                result = "半行显示";
            if (res == 1)
                result = "全行显示";
            if (res == 2)
                result = "侧边显示";
            return result;
        }

        public string GetPassedList(int pid)
        {
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            int res = (from rp in db.Application where rp.id == pid select rp.passed).FirstOrDefault().Value;
            string last = null;
            if (res == 0)
                last = "未审核";
            if (res == 1)
                last = "审核未通过";
            if (res == 2)
                last = "审核通过";
            return last;
        }

        public string GetIfLoginShowList(int pid)
        {
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            bool res = (from rp in db.Application where rp.id == pid select rp.IfLoginShow).FirstOrDefault().Value;
            string last = null;
            if (res == true)
                last = "是";
            if (res == false)
                last = "否";
            
            return last;
        }

        public IEnumerable<showApplication> GetAllAppInfo()//显示所有APP信息
        {
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            var res = new WiGetMSLinqDataContext().Application.ToList();
            var re = new Application().publishto;
            var apps = from user in res
                       select new showApplication
                       {
                           id = user.id,
                           Appname = user.appname,
                           Note = user.note,
                           Publishto = GetPublishtoList(user.id),
                           Lastmodifytime = user.lastmodifytime.HasValue ? user.lastmodifytime.Value.ToString("yyyy-MM-dd HH:mm") : "",
                           Tags = user.tags,
                           Passed = GetPassedList(user.id),
                       };
            return apps;
        }

        public IEnumerable<showApplication> GetAllAppInfo(int id)
        {
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            var result = (from user in db.Application
                          select new showApplication
                          {
                              id = user.id,
                              Appname = user.appname,
                              Logourl = user.logourl,
                              Note = user.note,
                              Publishto = user.publishto,
                              Tags = user.tags,
                          });
            return result;
        }

        public IEnumerable<Application> Insert(Application ad)
        {
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            db.Application.InsertOnSubmit(ad);
            db.SubmitChanges();
            return null;
        }

        //public showApplication GetAppInfoByID(int id)
        //{
        //    IList<showApplication> result;
        //    WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
        //    var res = (from Apps in db.Application
        //               where Apps.id == id
        //               select new
        //               {
        //                  id=Apps.id,
        //                  appname = Apps.appname,
        //                  note = Apps.note,
        //                  logourl = Apps.logourl,
        //                  publishto = Apps.publishto,
        //                  tags = Apps.tags,
        //               }).ToList();
        //    result = (from a in res
        //              select new showApplication
        //               {
        //                   id=a.id,
        //                   Appname = a.appname,
        //                   Note = a.note,
        //                   Logourl = a.logourl,
        //                   Publishto = a.publishto,
        //                   Tags = a.publishto,
        //               }).ToList();
        //    return result.FirstOrDefault(); 
        //}

        //public void UpdateAppInfo(showApplication ae)
        //{
        //    WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
        //    var apps = db.Application.FirstOrDefault(a => a.id == ae.id);
        //    apps.appname = ae.Appname==null?"":ae.Appname;
        //    apps.logourl = ae.Logourl == null ? "" : ae.Logourl;
        //    apps.note = ae.Note == null ? "" : ae.Note;
        //    apps.publishto = Request.Form["Publishto"].ToString();
        //    apps.tags = ae.Tags == null ? "" : ae.Tags;
        //    db.SubmitChanges();
        //    return;

        //}

        public void DeleteAppsById(int id)
        {
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            var cl = db.Application.Where(e => e.id == id).FirstOrDefault();
            if (cl != null)
            {
                db.Application.DeleteOnSubmit(cl);
                db.SubmitChanges();
                return;
            }
            return;
        }

        public showApplication GetViewData(int config)
        {
            IList<showApplication> result1;
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            var res = (from user in db.Application
                       where user.id == config
                       select new
                       {
                           Publishto = user.publishto == null ? "" : user.publishto,
                       }).ToList();
            result1 = (from u in res
                       select new showApplication
                       {
                           Publishto = GetPublishtoList(config),
                       }).ToList();
            return result1.FirstOrDefault();
        }

        public showApplication GetViewDataShowType(int config)
        {
            IList<showApplication> result1;
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            var res = (from user in db.Application
                       where user.id == config
                       select new
                       {
                           Showtype = user.ShowType,
                       }).ToList();
            result1 = (from u in res
                       select new showApplication
                       {
                           ShowType = u.Showtype.Value,
                       }).ToList();
            return result1.FirstOrDefault();
        }


        public IQueryable pubishto(int cid)
        {
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            var pubishto = from m in db.Application where m.id == cid select m.publishto;
            return pubishto;
        }

        public IEnumerable<showApplication> GetAllAppInfoExamine()//管理员审核界面显示未审核和审核未通过APP信息
        {
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            var res = new WiGetMSLinqDataContext().Application.Where(c => c.passed == 0 || c.passed == 1).ToList();
            var apps = from m in res
                       select new showApplication
                       {
                           id = m.id,
                           Appname = m.appname,
                           Note = m.note,
                           Publishto = GetPublishtoList(m.id),
                           Lastmodifytime = m.lastmodifytime.HasValue ? m.lastmodifytime.Value.ToString("yyyy-MM-dd HH:mm") : "",//?    modifydate = ad.modifydate.HasValue ? ad.modifydate.Value.ToString("yyyy-MM-dd HH:mm") : "",
                           Tags = m.tags,
                           Ispublishstring = GetIspublishListExamine(m.id),
                           IfLoginShows = GetIfLoginShowList(m.id),
                           Logourl = m.logourl,
                           Passed = GetPassedListExamine(m.id),
                           ShowTypes = GetShowTypeList(m.id)
                       };

            return apps;
        }

        public IEnumerable<showApplication> GetAllAppInfoUser(string name)
        {
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            var res = new WiGetMSLinqDataContext().Application.Where(c => c.@operator == name).ToList();
            var apps = from m in res
                       select new showApplication
                       {
                           id = m.id,
                           Appname = m.appname,
                           Note = m.note,
                           Publishto = GetPublishtoList(m.id),
                           Lastmodifytime = m.lastmodifytime.HasValue ? m.lastmodifytime.Value.ToString("yyyy-MM-dd HH:mm") : "",//?    modifydate = ad.modifydate.HasValue ? ad.modifydate.Value.ToString("yyyy-MM-dd HH:mm") : "",
                           Tags = m.tags,
                           Ispublishstring = GetIspublishListExamine(m.id),
                           ShowTypes = GetShowTypeList(m.id),
                           IfLoginShows = GetIfLoginShowList(m.id),
                           Logourl = m.logourl,
                           Passed = GetPassedListExamine(m.id),
                       };

            return apps;
        }

        public showApplication GetAppByID(int id) //通过ID得到一个APP的信息
        {
            IList<showApplication> result;
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            var res = (from Apps in db.Application
                       where Apps.id == id
                       select new
                       {
                           id = Apps.id,
                           appname = Apps.appname,
                           priority = Apps.priority,
                           note = Apps.note,
                           logourl = Apps.logourl,
                           publishto = Apps.publishto == null ? "" : Apps.publishto,
                           tags = Apps.tags,
                           ispublish = Apps.ispublish,
                           IfLoginShow = Apps.IfLoginShow.Value,
                       }).ToList();
            result = (from a in res
                      select new showApplication
                      {
                          id = a.id,
                          Appname = a.appname,
                          Priority = a.priority == null ? 0 : a.priority.Value,
                          Note = a.note == null ? "" : a.note,
                          Logourl = a.logourl == null ? "" : a.logourl,
                          Publishto = GetPublishtoList(a.id),
                          publishtoMR = a.publishto==null? "":a.publishto,
                          Tags = a.tags == null ? "" : a.tags,
                          Ispublish = a.ispublish == null ? false : a.ispublish.Value,
                          Ispublishstring = GetIspublishListExamine(a.id),
                          IfLoginShow = a.IfLoginShow == null ? false : a.IfLoginShow,
                          Passed = GetPassedListExamine(a.id),
                      }).ToList();
            return result.FirstOrDefault();

        }

        public string GetPassedListExamine(int pid)  //降APP的审核状态转换为汉字
        {
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            int res = (from rp in db.Application where rp.id == pid select rp.passed).FirstOrDefault().Value;
            string pa;
            switch (res)
            {
                case 0: pa = "未审核"; break;
                case 1: pa = "审核未通过"; break;
                case 2: pa = "审核通过"; break;
                default: pa = ""; break;
            }
            return pa;
        }

        public string GetIspublishListExamine(int pid)  //将Ispublish（是否发布）属性值准换为汉字
        {
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            bool res = (from rp in db.Application where rp.id == pid select rp.ispublish).FirstOrDefault().Value;
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

        public showApplication GetAppInfoByID(int id)//显示选中APP的信息 用于窗口显示
        {
            IList<showApplication> result;
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            var res = (from Apps in db.Application
                       where Apps.id == id
                       select new
                       {
                           id = Apps.id,
                           appname = Apps.appname,
                           note = Apps.note,
                           logourl = Apps.logourl,
                           publishto = Apps.publishto,
                           tags = Apps.tags,
                            
                       }).ToList();
            result = (from a in res
                      select new showApplication
                      {
                          id = a.id,
                          Appname = a.appname,
                          Note = a.note,
                          Logourl = a.logourl,
                          Publishto = a.publishto,
                          Tags = a.publishto,
                          
                      }).ToList();
            return result.FirstOrDefault();
        }

        public bool AppApplyForInsert(Application ad)  //APP申请的插入数据库（APP名称不能同名）
        {
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            if (db.Application.Where(c => c.appname == ad.appname).FirstOrDefault() == null)
            {
                db.Application.InsertOnSubmit(ad);
                db.SubmitChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public void UpdateAppInfo(showApplication ae) //用户修改自己申请的APP
        {
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            var apps = db.Application.FirstOrDefault(a => a.id == ae.id);
            apps.appname = ae.Appname == null ? "" : ae.Appname;
            apps.logourl = ae.Logourl == null ? "" : ae.Logourl;
            apps.note = ae.Note == null ? "" : ae.Note;
            apps.publishto = ae.Publishto == null ? "" : ae.Publishto;
            apps.tags = ae.Tags == null ? "" : ae.Tags;
            db.SubmitChanges();
            return;

        }

        public string FileUpLoad(string FileURL, string UPURL) //文件上传
        {
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();

            string filename = FileURL.Substring(FileURL.LastIndexOf("\\") + 1);
            var rs = from m in db.Application where m.logourl == filename select m;
            if (rs.Count() == 0)
            {
                string suourl = UPURL + filename;
                return suourl;

            }
            else
            {
                return "1";
            }
        }

        public int ExaminePassed(int id, bool pa) //修改审核的状态
        {
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            var res = db.Application.Where(c => c.id == id).FirstOrDefault();
            try
            {
                if (pa)
                {
                    res.passed = 2;//审核通过
                    db.SubmitChanges();
                    return 2;
                }
                else
                {
                    res.passed = 1;//审核未通过
                    db.SubmitChanges();
                    return 1;
                }

            }
            catch
            {
                return 0;
            }
        }

    }
}