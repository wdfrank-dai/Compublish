using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WiGetMSModelsLibrary;

namespace WiGetMS.Models.Repository
{
    public class UnitsCssRepository
    {
        public IEnumerable<ShowUnitsCss> GetAllUnitsCss(int unitsid)
        {
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            var res = db.ApplicationUnitsCss.Where(c => c.unitsid == unitsid).ToList();
            //var cl = new addressbookDataContext().classinfo.ToList();
            var rs = from m in res
                     select new ShowUnitsCss
                     {
                         id = m.id,
                         name = m.name == null ? "" : m.name,
                         unitsid = m.unitsid == null ? 0 : m.unitsid.Value,
                         unitname = (from re in db.ApplicationUnits where re.id == m.unitsid select re.unitname).FirstOrDefault(),
                         css = m.css == null ? "" : m.css
                     };
            return rs;
        }

        //删除刷新
        public IEnumerable<ShowUnitsCss> GetAllUnitsCssForDel()
        {
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            var res = db.ApplicationUnitsCss.ToList();
            //var cl = new addressbookDataContext().classinfo.ToList();
            var rs = from m in res
                     select new ShowUnitsCss
                     {
                         id = m.id,
                         name = m.name == null ? "" : m.name,
                         unitsid = m.unitsid == null ? 0 : m.unitsid.Value,
                         unitname = (from re in db.ApplicationUnits where re.id == m.unitsid select re.unitname).FirstOrDefault(),
                         css = m.css == null ? "" : m.css
                     };
            return rs;
        }

        public ApplicationUnitsCss GetUnitsCssByID(int id)     //通过传递过来的ID来查找模块的全部信息
        {
            using (WiGetMSLinqDataContext systemdc = new WiGetMSLinqDataContext())
            {
                try
                {
                    return systemdc.ApplicationUnitsCss.FirstOrDefault(o => o.id == id);
                }
                catch { return null; }

            }
        }


        public ShowUnitsCss GetUnitsCssInfo(int confid)
        {
            IList<ShowUnitsCss> result1;
            //if (confid == null)
            //    return null;
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            var temp = (from m in new WiGetMSLinqDataContext().ApplicationUnitsCss
                        where m.id == confid
                        select new
                        {
                            id = m.id,
                            name = m.name == null ? "" : m.name,
                            unitsid = m.unitsid == null ? 0 : m.unitsid.Value,
                            unitname = m.ApplicationUnits.unitname,
                            css = m.css == null ? "" : m.css
                        }).ToList();
            result1 = (from m in temp
                       select new ShowUnitsCss
                       {
                           id = m.id,
                           name = m.name,
                           unitsid = m.unitsid,
                           unitname = m.unitname,
                           css = m.css
                       }
                     ).ToList();

            return result1.FirstOrDefault();
        }


        public bool UpdateUnitsCssByAdmin(ApplicationUnitsCss uc)
        {
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            var pa = db.ApplicationUnitsCss.Where(e => e.id == uc.id).FirstOrDefault();
            try
            {
                if (pa != null)
                {
                    pa.id = uc.id;
                    pa.name = uc.name;
                    pa.unitsid = uc.unitsid;
                    //pa.unitname =uc.unitname;
                    pa.css = uc.css;
                    db.SubmitChanges();

                }
                return true;
            }
            catch { return false; }
        }


        public bool InsertUnitsCssByAdmin(ApplicationUnitsCss uc)
        {
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            try
            {
                db.ApplicationUnitsCss.InsertOnSubmit(uc);
                db.SubmitChanges();
                return true;
            }
            catch { return false; }

        }

        //Css删除
        public void DeleteUnitsCssByAdmin(int id)
        {
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            var Pa = db.ApplicationUnitsCss.Where(e => e.id == id).FirstOrDefault();
            try
            {
                if (Pa != null)
                {
                    db.ApplicationUnitsCss.DeleteOnSubmit(Pa);
                    db.SubmitChanges();
                    return;
                }
                return;
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: '{0}'", e);
            }
        }

        //删除Units时联动删除Css
        public void DeleteUnitsCssByAdminForUnits(int id)
        {
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            ApplicationUnitsCss units = db.ApplicationUnitsCss.Where(e => e.unitsid == id).FirstOrDefault();
            int unitsid = units.id;
            var dataforunits = db.ApplicationUnitsCss.Where(e => e.id == unitsid).FirstOrDefault();
            try
            {
                if (dataforunits != null)
                {
                    db.ApplicationUnitsCss.DeleteOnSubmit(dataforunits);
                    db.SubmitChanges();
                    return;
                }
                return;
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: '{0}'", e);
            }
        }


        //获取搜索的Css
        public IEnumerable<ShowUnitsCss> GetAllCssByUnitsname(string cssname)
        {

            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            var res = new WiGetMSLinqDataContext().ApplicationUnitsCss.Where(m => m.name.Contains(cssname)).ToList();
            var rs = from m in res
                     select new ShowUnitsCss
                     {
                         id = m.id,
                         name = m.name == null ? "" : m.name,
                         css = m.css  == null ? "" : m.css,
                         unitsid = m.unitsid == null ? 0 : m.unitsid.Value,
                         unitname = (from re in db.ApplicationUnits where re.id == m.unitsid select re.unitname).FirstOrDefault(),
                     };
            return rs;
        }
    }
}