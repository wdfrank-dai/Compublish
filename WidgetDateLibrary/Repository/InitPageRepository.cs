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
        public List<InitPageAppInfoModel> GetAuthedInitPageAppInfo(string userEmpNO)
        {
            try
            {
                WidgetDataContext db = new WidgetDataContext();

                List<InitPageAppInfoModel> rs =
               (from m in db.Application
                where m.publishto.IndexOf('W') >= 0 && m.ispublish == true
                orderby m.priority
                select new InitPageAppInfoModel
                {
                    AppName = m.appname,
                    ShowType = m.ShowType.Value,
                    FirstPageId = db.ApplicationPage.Where(o => o.appid == m.id && o.isStart == true).FirstOrDefault() == null ? 0 : db.ApplicationPage.Where(o => o.appid == m.id && o.isStart == true).FirstOrDefault().id
                }).ToList();

                return rs;
            }catch
            {
                return null;
            }
        }

        public List<InitPageAppInfoModel> GetInitPageAppInfo()
        {
            try
            {
                WidgetDataContext db = new WidgetDataContext();

                List<InitPageAppInfoModel> rs =
               (from m in db.Application
                where m.publishto.IndexOf('W') >= 0 && m.ispublish == true && m.IfLoginShow == false
                orderby m.priority
                select new InitPageAppInfoModel
                {
                    AppName = m.appname,
                    ShowType = m.ShowType.Value,
                    FirstPageId = db.ApplicationPage.Where(o => o.appid == m.id && o.isStart == true).FirstOrDefault() == null ? 0 : db.ApplicationPage.Where(o => o.appid == m.id && o.isStart == true).FirstOrDefault().id
                }).ToList();

                return rs;
            }
            catch
            {
                return null;
            }
        }
    }
}
