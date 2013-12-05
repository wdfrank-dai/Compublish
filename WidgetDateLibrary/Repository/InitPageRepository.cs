using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WidgetDate.Models;
using WidgetDateLibrary.Models;

namespace WidgetDate.Repository
{
    public class InitPageRepository
    {
        //还需要判断是否为桌面平台和是否允许发布。
        public List<WebPageUnits> GetAuthedInitPageAppInfo(string userEmpNO)
        {
            try
            {
                WidgetDataContext db = new WidgetDataContext();

                List<WebPageUnits> rs =
                (from m in db.WebPageUnits
                 //where m.publishto.IndexOf('W') >= 0 && m.ispublish == true  //未确定显示平台的字段，后期修正。
                 //orderby m.priority
                 where  m.widgettype != "2"
                 select m).ToList();

                return rs;

             
            }
            catch
            {
                return null;
            }
        }
        //public List<InitPageAppInfoModel> GetAuthedInitPageAppInfo(string userEmpNO)
        //{
        //    try
        //    {
        //        WidgetDataContext db = new WidgetDataContext();

        //        List<InitPageAppInfoModel> rs =
        //       (from m in db.Application
        //        where m.publishto.IndexOf('W') >= 0 && m.ispublish == true
        //        orderby m.priority
        //        select new InitPageAppInfoModel
        //        {
        //            AppName = m.appname,
        //            ShowType = m.ShowType.Value,
        //            FirstPageId = db.ApplicationPage.Where(o => o.appid == m.id && o.isStart == true).FirstOrDefault() == null ? 0 : db.ApplicationPage.Where(o => o.appid == m.id && o.isStart == true).FirstOrDefault().id
        //        }).ToList();

        //        return rs;
        //    }catch
        //    {
        //        return null;
        //    }
        //}

        public List<WebPageUnits> GetInitPageAppInfo()
        {
            try
            {
                WidgetDataContext db = new WidgetDataContext();

                List<WebPageUnits> rs =
                (from m in db.WebPageUnits
                 //where m.publishto.IndexOf('W') >= 0 && m.ispublish == true  //未确定显示平台的字段，后期修正。
                 //orderby m.priority
                 where m.ifloginshow == true && m.widgettype == "2"
                 select m).ToList();

                return rs;
            }
            catch
            {
                return null;
            }
        }

        public List<WebPageUnits> GetInitPageAppInfoIndex()
        {
            try
            {
                WidgetDataContext db = new WidgetDataContext();

                List<WebPageUnits> rs =
                (from m in db.WebPageUnits
                 //where m.publishto.IndexOf('W') >= 0 && m.ispublish == true  //未确定显示平台的字段，后期修正。
                 //orderby m.priority
                 where m.ifloginshow == false
                 select m).ToList();

                return rs;
            }
            catch
            {
                return null;
            }
        }
    }
}
