using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WiGetMSModelsLibrary;
using WiGetMS.Models;

namespace WiGetMS.Models.Repository
{
    public class ShowTypeRepository
    {
        public IEnumerable<ShowTypeModel> GetAllShowType()
        {
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            var res = db.ApplicationShowType.ToList();
            var rs = from m in res
                     select new ShowTypeModel
                     {
                         id = m.id,
                         name=m.name,
                         FillMark=m.FillMark,
                         FillCount=m.FillCount,
                         remark=m.remark,
                         viewdate=m.viewdate
                     };
            return rs;
        
        }

        public IEnumerable<ApplicationShowType> InsertShowType(ApplicationShowType ad)
        {
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            db.ApplicationShowType.InsertOnSubmit(ad);
            db.SubmitChanges();
            return null;
        }

        public bool UpdateShowTypeByAdmin(ApplicationShowType ap)
        {
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            var pa = db.ApplicationShowType.First(e => e.id == ap.id);

            try
            {
                if (pa != null)
                {
                    pa.id = ap.id;
                    pa.name = ap.name;
                    pa.FillCount = ap.FillCount;
                    pa.FillMark = ap.FillMark;
                    pa.remark = ap.remark;
                    pa.viewdate = ap.viewdate;
                    db.SubmitChanges();        
                }
                return true;
            }
            catch { return false; }
        }

        public bool DeleteShowTypeByAdmin(int id)
        {
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            var Pa = db.ApplicationShowType.Where(e => e.id == id).FirstOrDefault();
            try
            {
                if (Pa != null)
                {
                    db.ApplicationShowType.DeleteOnSubmit(Pa);
                    db.SubmitChanges();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public ShowTypeModel GetShowTypeInfoByID(int confid)
        {
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            IList<ShowTypeModel> result1;
            var temp = (from m in new WiGetMSLinqDataContext().ApplicationShowType
                        where m.id == confid
                        select new
                        {
                            id = m.id,
                            name = m.name == null ? "" : m.name,
                            fillmark = m.FillMark == null ? "" : m.FillMark,
                            fillcount = m.FillCount==null ? 0 : m.FillCount,
                            remark = m.remark == null ? "" : m.remark,
                            viewdate = m.viewdate == null ? "" : m.viewdate,
                          
                        }).ToList();
            result1 = (from m in temp
                       select new ShowTypeModel
                       {
                           id = m.id,
                           name = m.name,
                           FillMark = m.fillmark,
                           FillCount = m.fillcount,      
                           remark = m.remark,
                           viewdate = m.viewdate,                       
                       }
                     ).ToList();

            return result1.FirstOrDefault();
        }
    }
}
