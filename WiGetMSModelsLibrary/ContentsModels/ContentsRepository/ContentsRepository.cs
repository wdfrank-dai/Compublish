using System;
using System.Collections.Generic;
using System.Linq;
using WiGetMSModelsLibrary;
using System.Linq.Dynamic;

namespace WiGetMS.Models
{
    public class ContentsRepository
    {
        public IEnumerable<ContentsModel> GetAllContents()
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
                         Passed = new ContentsForUsersRepository().GetPassedList(ad.id),
                         Summary = ad.summary == null ? "" : ad.summary,
                         Suggestion = ad.suggestion == null ? "" : ad.suggestion,
                     };
            return re;
        }

        public List<ContentsModel> GetContentListByTag(string musthaskeywords, string maybehaskeywords, int startitemid, int listnum)
        {
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            var rep = new WiGetMSLinqDataContext().Content.ToList();
            string predicatestr = "";
            if (musthaskeywords != null)
            {
                var mtemp = musthaskeywords.Split(',');
                for (int i = 0; i < mtemp.Count();i++ )
                {
                    if (i !=0)
                        predicatestr = predicatestr + " && Tags.Contains(\"" + mtemp[i]+"\")";
                    else
                        predicatestr = "Tags.Contains(\"" + mtemp[i] + "\")";
                }
            }
            if (maybehaskeywords != null)
            {
                var mtemp = maybehaskeywords.Split(',');
                for (int i = 0; i < mtemp.Count(); i++)
                {
                    if (predicatestr == "" && i==0)
                        predicatestr = predicatestr + "Tags.Contains(\"" + mtemp[i] + "\")";
                    else
                        predicatestr = predicatestr+ " || Tags.Contains(\"" + mtemp[i] + "\")";
                }
            }

            var dbtemp = db.Content.ToList();
            var resulttemp = (from ad in dbtemp
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
                              Passed = new ContentsForUsersRepository().GetPassedList(ad.id),
                              Summary = ad.summary == null ? "" : ad.summary,
                              Suggestion = ad.suggestion == null ? "" : ad.suggestion,
                          }).AsQueryable();
            var result = resulttemp.Where(predicatestr).ToList();
            if (listnum != 0 && listnum <= result.Count && startitemid < result.Count)
                return result.GetRange(startitemid, listnum);
            else
                return result;
        }

        public ContentsModel PassedForContents(int nid)
        {
            IList<ContentsModel> result;
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            result = (from ad in db.Content
                        where ad.id == nid
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
                            Createtime = ad.createtime.ToString(),
                            Creator = ad.creator == null ? "" : ad.creator,
                            Operator = ad.@operator == null ? "" : ad.@operator,
                            //Lastmodifytime = ad.lastmodifytime.HasValue ? ad.lastmodifytime.Value.ToString("yyyy-MM-dd HH:mm") : "",
                            Verifier = ad.verifier == null ? "" : ad.verifier,
                            //Verifytime = ad.verifytime.HasValue ? ad.verifytime.Value.ToString("yyyy-MM-dd HH:mm") : "",
                            Passed = ad.passed == null ? "0" : ad.passed,
                            Summary = ad.summary == null ? "" : ad.summary,
                        }).ToList();

            return result.FirstOrDefault();
        }

        //编辑内容
        public void GetEditContents(ContentsModel data)
        {
            WiGetMSLinqDataContext db = new WiGetMSLinqDataContext();
            Content content = db.Content.First(c => c.id == data.ID);
            try
            {
                content.title = data.Title;
                content.t_content = data.T_content;
                content.priority = data.Priority;
                content.summary = data.Summary;
                content.hotspot = data.Hotspot;
                content.stick = data.Stick;
                content.sorlink = data.Sorlink;
                content.suggestion = data.Suggestion;
                content.passed = data.Passed;
                content.tags = data.Tags;
                content.creator = data.Creator;
                content.@operator = data.Operator;
                content.verifytime = Convert.ToDateTime(data.Verifytime);
                content.verifier = data.Verifier;

                db.SubmitChanges();
                return;
            }
            catch
            {
                return;
            }
        }
    }
}