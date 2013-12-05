using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WiGetMSModelsLibrary;
using WiGetMS.Models;

namespace WiGetMS.Models.Repository
{
    public class DatasourceRepository
    {
        public IEnumerable<Datasource> GetAllData()
        {
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            var rep = new WiGetMSLinqDataContext().ApplicationDatasource.ToList();
            var re = from ad in rep
                     select new Datasource
                     {
                         id = ad.id,
                         dsname = ad.dsname,
                         dataitems = ad.dataitems == null ? "" : ad.dataitems,
                         dsparams = ad.dsparams == null ? "" : ad.dsparams,
                         dsurl = ad.dsurl == null ? "" : ad.dsurl,
                         //modifydate = ad.modifydate.HasValue ? ad.modifydate.Value.ToString("yyyy-MM-dd HH:mm") : "",
                         isefficetiveS = GetisefficetiveConvert(ad.id),
                         unitsid = ad.unitsid == null ? 0 : ad.unitsid.Value,
                     };
            return re;
        }

        public IEnumerable<Datasource> GetAllDataByAppId(int appid)
        {
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            var rep = new WiGetMSLinqDataContext().ApplicationDatasource.Where(d=>d.applicationid==appid).ToList();
            var re = from ad in rep
                     select new Datasource
                     {
                         id = ad.id,
                         dsname = ad.dsname,
                         dataitems = ad.dataitems == null ? "" : ad.dataitems,
                         dsparams = ad.dsparams == null ? "" : ad.dsparams,
                         dsurl = ad.dsurl == null ? "" : ad.dsurl,
                         //modifydate = ad.modifydate.HasValue ? ad.modifydate.Value.ToString("yyyy-MM-dd HH:mm") : "",
                         isefficetiveS = GetisefficetiveConvert(ad.id),
                         unitsid = ad.unitsid == null ? 0 : ad.unitsid.Value,
                     };
            return re;
        }

        //添加数据元
        public IEnumerable<ApplicationDatasource> Insert(ApplicationDatasource ad)
        {
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            db.ApplicationDatasource.InsertOnSubmit(ad);
            db.SubmitChanges();
            return null;
        }

        public Units GetViewDataForDataSource(int config)
        {
            IList<Units> result1;
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            var res = (from user in db.ApplicationDatasource
                       where user.id == config
                       select new
                       {
                           unitname = (from re in db.ApplicationUnits where re.datasourceid == config select re.unitname).FirstOrDefault(),
                       }).ToList();
            result1 = (from u in res
                       select new Units
                       {
                           unitname = u.unitname == null ? "" : u.unitname,
                       }).ToList();
            return result1.FirstOrDefault();
        }

        public Datasource MeetingDetailInfo(int confid)
        {
            IList<Datasource> result1;

            var temp = (from m in new WiGetMSLinqDataContext().ApplicationDatasource
                        where m.id == confid
                        select new 
                        {
                            id = m.id,
                            isefficetive = m.isefficetive == null ? false : m.isefficetive,
                            dataitems = m.dataitems == null ? "" : m.dataitems,
                            dsname = m.dsname == null ? "" : m.dsname,
                            dsparams = m.dsparams == null ? "" : m.dsparams,
                            dsurl = m.dsurl == null ? "" : m.dsurl,
                            @operator = m.@operator == null ? "杨佳" : m.@operator,
                            unitsid = m.unitsid == null ? 0 : m.unitsid.Value,
                           // modifydate = m.modifydate,
                        }).ToList();
            result1 = (from m in temp
                       select new Datasource
                       {
                           id = m.id,
                           isefficetive = m.isefficetive == null ? false : m.isefficetive.Value,
                           dataitems = m.dataitems,
                           dsname = m.dsname,
                           dsparams = m.dsparams,
                           dsurl = m.dsurl,
                           operators = m.@operator,
                           unitsid = m.unitsid,
                          // modifydate = m.modifydate.HasValue ? m.modifydate.Value.ToString("yyyy-MM-dd HH:mm") : "",        
                       }
                     ).ToList();

            return result1.FirstOrDefault();
        }

        //编辑数据元
        public void GetEditData(Datasource data)
        { 
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            ApplicationDatasource datasource = db.ApplicationDatasource.First(c => c.id == data.id);
            datasource.isefficetive = data.isefficetive;
            datasource.dataitems = data.dataitems;
            datasource.dsname = data.dsname;
            datasource.dsparams = data.dsparams;
            datasource.dsurl = data.dsurl;
            //datasource.@operator = data.operators;
            datasource.unitsid = data.unitsid;
            //datasource.modifydate = DateTime.Parse(data.modifydate);
            db.SubmitChanges();
            return;
        }

        //删除数据元
        public void Delete(int id)
        {
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            var data = db.ApplicationDatasource.Where(e => e.id == id).FirstOrDefault();
            if (data != null)
            {
                db.ApplicationDatasource.DeleteOnSubmit(data);
                db.SubmitChanges();
                return;
            }
            return;
        }

        //删除刷新
        public IEnumerable<Datasource> GetAllDatasource(int dataid)
        {
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            var result = (from user in db.ApplicationDatasource
                          select new Datasource
                          {
                              id = user.id,
                              unitsid = user.unitsid.Value,
                              operators = user.@operator,
                              dsurl = user.dsurl,
                              dsparams = user.dsparams,
                              dsname = user.dsname,
                              dataitems = user.dataitems,
                              isefficetive = user.isefficetive == null ? false : user.isefficetive.Value,
                              modifydate = user.modifydate.HasValue ? user.modifydate.Value.ToString("yyyy-MM-dd HH:mm") : "",
                          });
            return result;
        }

        public string GetisefficetiveConvert(int pid)  //将Ispublish（是否发布）属性值准换为汉字
        {
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            var bo = (from rp in db.ApplicationDatasource where rp.id == pid select rp.isefficetive).FirstOrDefault();
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