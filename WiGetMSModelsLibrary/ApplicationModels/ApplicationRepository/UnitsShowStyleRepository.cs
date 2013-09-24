using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WiGetMSModelsLibrary;

namespace WiGetMS.Models.Repository
{
    public class UnitsShowStyleRepository
    {
        public IEnumerable<UnitsShowStyle> GetAllUnitsShowStyle()
        {
            var rep = new WiGetMSLinqDataContext().ApplicationUnitsShowStyle.ToList();
            var re = from ad in rep
                     select new UnitsShowStyle
                     {
                         id = ad.id,
                         name = ad.name == null ? "" : ad.name,
                         note = ad.note == null ? "" : ad.note,
                     };
            return re;
        }

        //添加控件展示方式
        public IEnumerable<ApplicationUnitsShowStyle> Insert(ApplicationUnitsShowStyle ads)
        {
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            db.ApplicationUnitsShowStyle.InsertOnSubmit(ads);
            db.SubmitChanges();
            return null;
        }

        public UnitsShowStyle MeetingDetailInfo(int confid)
        {
            IList<UnitsShowStyle> result1;

            var temp = (from m in new WiGetMSLinqDataContext().ApplicationUnitsShowStyle
                        where m.id == confid
                        select new
                        {
                            id = m.id,
                            name = m.name,
                            note = m.note,
                        }).ToList();
            result1 = (from m in temp
                       select new UnitsShowStyle
                       {
                           id = m.id,
                           name = m.name,
                           note = m.note == null ? "" : m.note,
                       }
                     ).ToList();

            return result1.FirstOrDefault();
        }

        //编辑控件展示方式
        public void GetEditUnitsShowStyle(UnitsShowStyle data)
        {
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            ApplicationUnitsShowStyle datasource = db.ApplicationUnitsShowStyle.First(c => c.id == data.id);
            datasource.name = data.name;
            datasource.note = data.note;
            db.SubmitChanges();
            return;
        }

        //删除控件展示方式
        public void Delete(int id)
        {
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            var dataforshowstyle = db.ApplicationUnitsShowStyle.Where(e => e.id == id).FirstOrDefault();
            if (dataforshowstyle != null)
            {
                db.ApplicationUnitsShowStyle.DeleteOnSubmit(dataforshowstyle);
                db.SubmitChanges();
                return;
            }
            return;
        }

        //删除刷新
        public IEnumerable<UnitsShowStyle> GetAllUnitsShowStyle(int unitsshowid)
        {
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            var result = (from user in db.ApplicationUnitsShowStyle
                          select new UnitsShowStyle
                          {
                              id = user.id,
                              name = user.name,
                              note = user.note,                             
                          });
            return result;
        }
    }
}