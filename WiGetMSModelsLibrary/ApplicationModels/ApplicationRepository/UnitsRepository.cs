using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WiGetMSModelsLibrary;

namespace WiGetMS.Models.Repository
{
    public class UnitsRepository
    {
        public IEnumerable<Units> GetAllUnits()
        {
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            var rep = new WiGetMSLinqDataContext().ApplicationUnits.ToList();
            var re = from ad in rep
                     select new Units
                     {
                         id = ad.id,
                         unitname = ad.unitname == null ? "" : ad.unitname,
                         dsname = ad.ApplicationDatasource == null ? "" : ad.ApplicationDatasource.dsname,//dsname!??!  //(from rs in db.ApplicationDatasource where rs.id ==ad.datasourceid select rs.dsname).FirstOrDefault(),
                         theparams = ad.theparams,
                         utype = ad.utype == null ? 0 : ad.utype.Value,
                         ucontent = ad.ucontent == null ? "" : ad.ucontent,
                         showstylename = ad.ApplicationUnitsShowStyle == null ? "" : ad.ApplicationUnitsShowStyle.name,
                         iseffictiveS = GetiseffictiveConvert(ad.id),
                         lastmodifytime = ad.lastmodifytime.HasValue ? ad.lastmodifytime.Value.ToString("yyyy-MM-dd HH:mm") : "",
                     };
            return re;
        }

        //添加控件
        public bool  Insert(ApplicationUnits ad)
        {
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            try
            {
                db.ApplicationUnits.InsertOnSubmit(ad);
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        //测试
        public IEnumerable<ApplicationUnitsCss> InsertCss(string res)
        {
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            int[] array = res.Split(',').Where(t => t.Trim() != "").Select(t => Convert.ToInt32(t.Trim())).ToArray();
            ApplicationUnits ad = new ApplicationUnits();

            //db.ApplicationUnits.InsertOnSubmit(ad.res1);
            db.SubmitChanges();
            //foreach( int i in array )
            //{
            //    ad.id = i;
            //    db.ApplicationUnitsCss.InsertOnSubmit(ad);
            //    db.SubmitChanges();
            //}
            return null;
        }
        
        public Units MeetingDetailInfo(int confid)
        {
            IList<Units> result1;
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            var temp = (from m in new WiGetMSLinqDataContext().ApplicationUnits
                        where m.id == confid
                        select new
                        {
                            id = m.id,
                            unitname = m.unitname,
                            datasourceid = m.datasourceid,
                            dsname = m.ApplicationDatasource.dsname,//dsname!??!
                            utype = m.utype,
                            ucontent = m.ucontent,
                            showstyleid = m.showstyleid,
                            showstylename = m.ApplicationUnitsShowStyle.name,
                            iseffictive = m.iseffictive,
                           // lastmodifytime = m.lastmodifytime,
                        }).ToList();
            result1 = (from m in temp
                       select new Units
                       {
                           id = m.id,
                           unitname = m.unitname,
                           datasourceid = m.datasourceid == null ? 0 : m.datasourceid.Value,
                           dsname = m.dsname == null ? "" : m.dsname,
                           utype = m.utype == null ? 0 : m.utype.Value,
                           ucontent = m.ucontent == null ? "" : m.ucontent,
                           showstyleid = m.showstyleid == null ? 0 : m.showstyleid.Value,
                           showstylename = m.showstylename == null ? "" : m.showstylename,
                           iseffictive = m.iseffictive == null ? false : m.iseffictive.Value,
                          // lastmodifytime = m.lastmodifytime.HasValue ? m.lastmodifytime.Value.ToString("yyyy-MM-dd HH:mm") : "",
                     }).ToList();

            return result1.FirstOrDefault();
        }

        //编辑控件
        public void GetEditUnits(Units data)
        {
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            ApplicationUnits datasource = db.ApplicationUnits.First(c => c.id == data.id);
            try
            {
                if (data.datasourceid != 0)
                    datasource.datasourceid = data.datasourceid;
                datasource.theparams = data.theparams;

                if (data.showstyleid != 0)
                    datasource.showstyleid = data.showstyleid;
                if (data.NextPage != 0)
                    datasource.NextPage = data.NextPage;
                datasource.iseffictive = data.iseffictive;
                datasource.ucontent = data.ucontent;
                datasource.utype = data.utype;
                datasource.unitname = data.unitname;
                //datasource.lastmodifytime = DateTime.Parse(data.lastmodifytime);
                db.SubmitChanges();
                return;
            }
            catch
            {
                return ;
            }
        }

        //删除控件
        public void Delete(int id)
        {
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            var data = db.ApplicationUnits.Where(e => e.id == id).FirstOrDefault();
            if (data != null)
            {
                db.ApplicationUnits.DeleteOnSubmit(data);
                db.SubmitChanges();
                return;
            }
            return;
        }

        //删除刷新
        public IEnumerable<Units> GetAllUnits(int unitsid)
        {
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            var result = (from user in db.ApplicationUnits
                          select new Units
                          {
                              id = user.id,
                              unitname = user.unitname,
                              datasourceid = user.datasourceid.Value,
                              theparams = user.theparams,
                              utype = user.utype.Value,
                              ucontent = user.ucontent,
                              showstyleid = user.showstyleid.Value,
                              iseffictive = user.iseffictive == null ? false : user.iseffictive.Value,
                              lastmodifytime = user.lastmodifytime.HasValue ? user.lastmodifytime.Value.ToString("yyyy-MM-dd HH:mm") : "",
                          });
            return result;
        }

        public IQueryable dsnamelist()
        {
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            var dsname = from m in db.ApplicationDatasource select m;
            return dsname;
        }

        public IQueryable showstyle()
        {
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            var showstylename = from m in db.ApplicationUnitsShowStyle select m;
            return showstylename;
        }

        public IQueryable pagelist()
        {
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            var pagename = from m in db.ApplicationPage select m;
            return pagename;
        }

        public IQueryable nextpageshowtype()
        {
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            var NextPageShowType = from m in db.ApplicationUnits select m;
            return NextPageShowType;
        }

        public Units GetViewDataForUnits(int config)
        {
            IList<Units> result1;
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            var res = (from user in db.ApplicationUnits
                       where user.id == config
                       select new
                       {
                           datesourceid=user.datasourceid,
                           showstyleid = user.showstyleid,
                       }).ToList();
            result1 = (from u in res
                       select new Units
                       {
                           datasourceid = u.datesourceid!=null ?u.datesourceid.Value:0,
                           showstyleid = u.showstyleid!=null ?u.showstyleid.Value : 0,
                       }).ToList();
            return result1.FirstOrDefault(); 
        }

        public Units GetViewDataForCss(int config)
        {
            IList<Units> result1;
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            var res = (from user in db.ApplicationUnitsCss
                       where user.id == config
                       select new
                       {
                           unitname = user.ApplicationUnits == null ? "" : user.ApplicationUnits.unitname,
                       }).ToList();
            result1 = (from u in res
                       select new Units
                       {
                           unitname = u.unitname,
                       }).ToList();
            return result1.FirstOrDefault();
        }

        public Units GetViewDatanextpage(int config)
        {
            IList<Units> result1;
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            var res = (from user in db.ApplicationUnits
                       where user.id == config
                       select new
                       {
                           nextpage = user.NextPage,
                       }).ToList();
            result1 = (from u in res
                       select new Units
                       {
                           NextPage = u.nextpage == null ? 0 : u.nextpage.Value,
                       }).ToList();
            return result1.FirstOrDefault();
        }

        public Units GetViewDatanextpageshowtype(int config)
        {
            IList<Units> result1;
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            var res = (from user in db.ApplicationUnits
                       where user.id == config
                       select new
                       {
                           nextpageshowtype = user.NextPageShowType,
                       }).ToList();
            result1 = (from u in res
                       select new Units
                       {
                           nextpageshowtype = u.nextpageshowtype,
                       }).ToList();
            return result1.FirstOrDefault();
        }
        public string GetiseffictiveConvert(int pid)  //将Ispublish（是否发布）属性值准换为汉字
        {
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            var bo = (from rp in db.ApplicationUnits where rp.id == pid select rp.iseffictive).FirstOrDefault();
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