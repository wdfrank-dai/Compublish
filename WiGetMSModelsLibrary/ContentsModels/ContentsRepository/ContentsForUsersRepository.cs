using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WiGetMSModelsLibrary;
using WiGetMS.Models;

namespace WiGetMS.Models
{
    public class ContentsForUsersRepository
    {
        public string GetPassedList(int pid)
        {
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            string res = (from rp in db.Content where rp.id == pid select rp.passed).FirstOrDefault();
            string last = "";
                var a = res;
                if (a == "0")
                    last = "未审核";
                if (a == "1")
                    last = "审核未通过";
                if (a == "2")
                    last = "需修改";
                if (a == "3")
                    last = "审核通过";
            return last;
        }

        public IEnumerable<ContentsModel> GetAllUsersContents()
        {
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            var rep = new WiGetMSLinqDataContext().Content.ToList();
            var re = from ad in rep
                     select new ContentsModel
                     {
                         ID = ad.id,
                         Title = ad.title == null ? "" : ad.title,
                         T_content = ad.t_content == null ? "" : ad.t_content,
                         Priority = ad.priority == null ? 0 : ad.priority.Value,
                         Stick = ad.stick == null ? false : ad.stick.Value,
                         Hotspot = ad.hotspot == null ? false : ad.hotspot.Value,
                         Tags = ad.tags == null ? "" : ad.tags,
                         Sorlink = ad.sorlink == null ? "" : ad.sorlink,
                         Createtime = ad.createtime.HasValue ? ad.createtime.Value.ToString("yyyy-MM-dd HH:mm") : "",
                         Creator = ad.creator == null ? "" : ad.creator,
                         Operator = ad.@operator == null ? "" : ad.@operator,
                         Lastmodifytime = ad.lastmodifytime.HasValue ? ad.lastmodifytime.Value.ToString("yyyy-MM-dd HH:mm") : "",
                         Verifier = ad.verifier == null ? "" : ad.verifier,
                         Verifytime = ad.verifytime.HasValue ? ad.verifytime.Value.ToString("yyyy-MM-dd HH:mm") : "",
                         Passed = GetPassedList(ad.id),
                         Summary = ad.summary == null ? "" : ad.summary,
                     };
            return re;
        }

        public bool Insertcontents(Content data)
        {
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            Content ads = new Content();
            ads.title = data.title;
            //var tit = 
            if ((from m in db.Content where m.title == data.title select m.title).FirstOrDefault() != null)
                return false ;
            ads.t_content = data.t_content;
            ads.createtime = DateTime.Now.Date;
            ads.passed = data.passed;
            ads.sorlink = data.sorlink;
            ads.priority = data.priority;
            ads.summary = data.summary;
            ads.hotspot = data.hotspot;
            ads.stick = data.stick;
            ads.tags = data.tags;
            ads.creator = data.creator;
            ads.@operator = data.@operator;
            ads.lastmodifytime = data.lastmodifytime;
            db.Content.InsertOnSubmit(ads);
            db.SubmitChanges();
            return true;
        }

        public bool ContentInsert(Content ad)  //APP申请的插入数据库（APP名称不能同名）
        {
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            if (db.Content.Where(c => c.title == ad.title).FirstOrDefault() == null)
            {
                db.Content.InsertOnSubmit(ad);
                db.SubmitChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public IEnumerable<ContentsModel> GetAllContentInfoUser(string name)
        {
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            var res = new WiGetMSLinqDataContext().Content.Where(c => c.@operator == name).ToList();
            var apps = from m in res
                       select new ContentsModel
                       {
                           ID = m.id,
                           Title = m.title,
                           T_content = m.t_content==null? "":m.t_content,
                           Priority = m.priority==null? 0:m.priority.Value,
                           Hotspot = m.hotspot== null ? false : m.hotspot.Value,
                           HotspotS = GetHotspotListExamine(m.id),
                           Stick = m.stick == null ? false : m.stick.Value,
                           StickS = GetStickListExamine(m.id),
                           Tags = m.tags == null ? "": m.tags,
                           Sorlink = m.sorlink == null ? "": m.sorlink,
                           Summary = m.summary ==null ? "": m.summary,
                           Suggestion = m.suggestion == null ? "":m.suggestion,
                           Creator = m.creator == null ? "": m.creator,
                           Createtime = m.createtime.HasValue ? m.createtime.Value.ToString("yyyy-MM-dd HH:mm") : "",
                           Operator = m.@operator == null ? "": m.@operator,
                           Lastmodifytime = m.lastmodifytime.HasValue ? m.lastmodifytime.Value.ToString("yyyy-MM-dd HH:mm") : "",
                           Verifier =  m.verifier == null ? "": m.verifier,
                           Verifytime = m.verifytime.HasValue? m.verifytime.Value.ToString("yyyy-MM-dd HH:mm") : "",
                           Passed = GetPassedList(m.id),
                       };

            return apps;
        }

        public string GetStickListExamine(int pid)  //将Stick（是否发布）属性值准换为汉字
        {
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            bool res = (from rp in db.Content where rp.id == pid select rp.stick).FirstOrDefault().Value;
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

        public string GetHotspotListExamine(int pid)  //将Stick（是否发布）属性值准换为汉字
        {
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            bool res = (from rp in db.Content where rp.id == pid select rp.hotspot).FirstOrDefault().Value;
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

        public ContentsModel GetContentByID(int id) //通过ID得到一个APP的信息
        {
            IList<ContentsModel> result;
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            var res = (from m in db.Content
                       where m.id == id
                       select new 
                       {                           
                           id = m.id,
                           title = m.title,
                           t_content = m.t_content == null ? "" : m.t_content,
                           priority = m.priority == null ? 0 : m.priority.Value,
                           hotspot = m.hotspot == null ? false : m.hotspot.Value,
                           stick = m.stick == null ? false : m.stick.Value,
                           tags = m.tags == null ? "" : m.tags,
                           summary = m.summary == null ? "" : m.summary,
                           suggestion = m.suggestion == null ? "" : m.suggestion,
                           sorlink = m.sorlink == null ? "" : m.sorlink,
                           creator = m.creator == null ? "" : m.creator,
                           //createtime = m.createtime.HasValue ? m.createtime.Value.ToString("yyyy-MM-dd HH:mm") : "",
                           @operator = m.@operator == null ? "" : m.@operator,
                           //lastmodifytime = m.lastmodifytime.HasValue ? m.lastmodifytime.Value.ToString("yyyy-MM-dd HH:mm") : "",/    modifydate = ad.modifydate.HasValue ? ad.modifydate.Value.ToString("yyyy-MM-dd HH:mm") : "",
                           verifier = m.verifier == null ? "" : m.verifier,
                           // verifytime = m.verifytime.HasValue ? m.verifytime.Value.ToString("yyyy-MM-dd HH:mm") : "",
                       }).ToList();
            result = (from a in res
                      select new ContentsModel
                      {
                          ID = a.id,
                          Title = a.title,
                          T_content = a.t_content,
                          Priority = a.priority,
                          Hotspot = a.hotspot,
                          HotspotS = GetHotspotListExamine(a.id),
                          Stick = a.stick,
                          Summary = a.summary,
                          Suggestion = a.suggestion,
                          StickS = GetStickListExamine(a.id),
                          Tags = a.tags,
                          Sorlink = a.sorlink,
                          Creator = a.creator,
                          //Createtime = a.createtime,
                          Operator = a.@operator,
                          // Lastmodifytime = a.lastmodifytime,/    modifydate = ad.modifydate.HasValue ? ad.modifydate.Value.ToString("yyyy-MM-dd HH:mm") : "",
                          Verifier = a.verifier,
                          //Verifytime = a.verifytime,
                          Passed = GetPassedList(a.id),
                      }).ToList();
            return result.FirstOrDefault();

        }



        public bool UpdateContentByUser(Content ae) //用户修改自己申请的APP
        {
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            try
            {
                var apps = db.Content.FirstOrDefault(a => a.id == ae.id);
                apps.title = ae.title == null ? "" : ae.title;
                apps.tags = ae.tags == null ? "" : ae.tags;
                apps.t_content = ae.t_content == null ? "" : ae.t_content;
                apps.stick = ae.stick == null ? false : ae.stick;
                apps.hotspot = ae.hotspot == null ? false : ae.hotspot;
                apps.sorlink = ae.sorlink == null ? "" : ae.sorlink;
                apps.creator = ae.creator == null ? "" : ae.creator;
                apps.summary = ae.summary == null ? "" : ae.summary;
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}