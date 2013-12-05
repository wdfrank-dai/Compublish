using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Web.Script.Serialization;
using WidgetDate.Models;
using WiGetMS.Models;

namespace ComPublishWeb.Controllers
{
    public class ReturnJsonController : Controller
    {
        //
        // GET: /ReturnJson/
        public JsonResult DataList()
        {
            GetDataSourceModel theresult = new GetDataSourceModel();
            theresult.DataSourceType = "DataList";
            theresult.DataSourceName = "DataList1";
            List<DataListModel> nownewslist = new List<DataListModel>();
            DataListModel re = new DataListModel();
            re.DataItem = "新闻：";
            re.DataValue = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";


            nownewslist.Add(re);
            nownewslist.Add(re);
            nownewslist.Add(re);
            nownewslist.Add(re);
            nownewslist.Add(re);
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            theresult.Datas = jsonSerializer.Serialize(nownewslist);

            return Json(theresult, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Index()
        {
            GetDataSourceModel theresult = new GetDataSourceModel();
            theresult.DataSourceType = "NewsList";
            theresult.DataSourceName = "newlist1";
            List<NewsListModel> nownewslist = new List<NewsListModel>();
            NewsListModel re = new NewsListModel();
            re.NewsTheparams = "386";
            re.NewsTitle = "转发－－关于2014年度省教育厅人文社会科学研究项目（专项任务项目）申报工作的通知";
            re.NewsDataTime = "2013-7-15";
            

            nownewslist.Add(re);
            nownewslist.Add(re);
            nownewslist.Add(re);
            nownewslist.Add(re);
            nownewslist.Add(re);
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            theresult.Datas =  jsonSerializer.Serialize(nownewslist);

            return Json(theresult, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ImgNewList()
        {
            GetDataSourceModel theresult = new GetDataSourceModel();
            theresult.DataSourceType = "ImgNewsList";
            theresult.DataSourceName = "ImgNewsList1";
            List<ImgNewsListModel> nownewslist = new List<ImgNewsListModel>();
            ImgNewsListModel re = new ImgNewsListModel();
            re.NewsTheparams = "386";
            re.NewsTitle = "转发－－关于2014年度省教育厅人文社会科学研究项目（专项任务项目）申报工作的通知";
            re.NewsDataTime = "2013-7-15";
            re.NewsImgUrl = "../../Content/ContentImg/bq1.png";


            nownewslist.Add(re);
            nownewslist.Add(re);
            nownewslist.Add(re);
            nownewslist.Add(re);
            nownewslist.Add(re);
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            theresult.Datas = jsonSerializer.Serialize(nownewslist);

            return Json(theresult, JsonRequestBehavior.AllowGet);
        }

        public JsonResult IndexGetID(int id)
        {
            GetDataSourceModel theresult = new GetDataSourceModel();
            theresult.DataSourceType = "NewsContent";
            theresult.DataSourceName = "NewsContent1";
            NewsContentModel re = new NewsContentModel();
            var rep = new ContentsRepository();
            var thedata = rep.PassedForContents(id);
            re.NewsTitle = thedata.Title;
            re.NewsCollateralTitle = thedata.Createtime;
            re.NewsText = thedata.T_content;
            re.NewsEnd = "作者："+thedata.Creator;

       
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            theresult.Datas = jsonSerializer.Serialize(re);

            return Json(theresult, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Label()
        {
            GetDataSourceModel theresult = new GetDataSourceModel();
            theresult.DataSourceType = "SimpleTextLable";
            theresult.DataSourceName = "SimpleTextLable1";
            SimpleTextLableModel re = new SimpleTextLableModel();
            re.LableText = "中方：美国若停止对台军售 中方可考虑调整军事部署 2013-07-07";


            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            theresult.Datas = jsonSerializer.Serialize(re);

            return Json(theresult, JsonRequestBehavior.AllowGet);
        }

        public JsonResult NewDateTitle()
        {
            string[] sss = new string[2];
            sss[0] = "中南民族大学图书馆";
            sss[1] = "新闻";

            return Json(sss, JsonRequestBehavior.AllowGet);
        }

        public JsonResult NewDate()
        {
            List<string[]> sss = new List<string[]>();
            string[] a = new string[3];
            a[0] = "2013年暑假图书馆开放通知";
            a[1] = "http://www.lib.scuec.edu.cn/HomeSubsidiary/NewText?dataid=385";
            a[2] = "2013-07-13";
            sss.Add(a);

            string[] b = new string[3];
            b[0] = "“中南民族大学学术成果库”正式发布";
            b[1] = "http://www.lib.scuec.edu.cn/HomeSubsidiary/NewText?dataid=384";
            b[2] = "2013-07-03";
            sss.Add(b);

            string[] c = new string[3];
            c[0] = "CCER中国经济金融数据库开通试用";
            c[1] = "http://www.lib.scuec.edu.cn/HomeSubsidiary/NewText?dataid=383";
            c[2] = "2013-07-02";
            sss.Add(c);

            string[] d = new string[3];
            d[0] = "6月27日（周四）下午政治、业务学习（闭馆）通知";
            d[1] = "http://www.lib.scuec.edu.cn/HomeSubsidiary/NewText?dataid=381";
            d[2] = "2013-06-26";
            sss.Add(d);

            return Json(sss, JsonRequestBehavior.AllowGet);
        }

        //所有的单位都是input，每项以，号分割,第一项不用分割，第一项只有一个值就是form的参数：(method,action)。分7个部分(空串就代表没有值)：标头说明,控件名字，控件类型，value值，checked值,可否为空(空串就可以为空)，说明项。
        public JsonResult Form()
        {
            List<string> sss = new List<string>();
            sss.Add("post,ReturnJson/Post");
            sss.Add("姓名,name,text,,,1,输入姓名");
            sss.Add("年龄,year,text,,,,输入年龄");
            sss.Add("男,sex,radio,男,1,,");
            sss.Add("女,sex,radio,女,,,");

            return Json(sss, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Form1()
        {

            List<string> sss = new List<string>();
            sss.Add("get,http://coin.lib.scuec.edu.cn/opac/openlink.php");
            sss.Add("请输入检索词：,txtBaseSearchValue,text,,,1,");
            sss.Add("男,v_index,radio,TITLE,1,,");
            sss.Add("女,v_index,radio,AUTHOR,,,");

            return Json(sss, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult Post()
        {
            return Json(Request.Form["name"].ToString() + "," + Request.Form["year"].ToString() + "," + Request.Form["sex"].ToString());
        }

        public JsonResult getAnnounceList(int startitemid, int listnum)
        {
            var theresult = getContentList("公告", "通知", startitemid, listnum);
            return theresult;
        }

        public JsonResult getPolicyList(int startitemid, int listnum)
        {
            var theresult = getContentList("国家政策法规", null, startitemid, listnum);
            return theresult;
        }

        public JsonResult getRegulationsList(int startitemid, int listnum)
        {
            var theresult = getContentList("校内规章", null, startitemid, listnum);
            return theresult;
        }

        //public JsonResult getRegulationsList(int startitemid, int listnum)
        //{
        //    var theresult = getContentList("办事流程", null, startitemid, listnum);
        //    return theresult;
        //}

        public JsonResult getContentList(string musthaskeywords, string maybehaskeywords, int startitemid, int listnum)
        {
            ContentsRepository rp = new ContentsRepository();
            GetDataSourceModel theresult = new GetDataSourceModel();
            theresult.DataSourceType = "NewsList";
            theresult.DataSourceName = musthaskeywords;

            var res = rp.GetContentListByTag(musthaskeywords, maybehaskeywords, startitemid, listnum);
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            List<NewsListModel> listresult = new List<NewsListModel>();
            for (int i = 0; i < res.Count; i++)
            {
                NewsListModel temp = new NewsListModel();
                var datatemp = res.ElementAt(i);
                temp.NewsTheparams = datatemp.ID.ToString();
                temp.NewsTitle = datatemp.Title;
                temp.NewsIntro = datatemp.Summary;
                temp.NewsUrl = datatemp.Sorlink;
                temp.NewsDataTime = datatemp.Createtime;
                listresult.Add(temp);
            }
            if (res.Count < listnum)
            {
                for (int i = res.Count; i < listnum; i++)
                {
                    NewsListModel temp = new NewsListModel();
                    temp.NewsTheparams = "";
                    temp.NewsTitle = "";
                    temp.NewsIntro = "";
                    temp.NewsUrl = "";
                    temp.NewsDataTime = "";
                    listresult.Add(temp);
                }
            }
            theresult.Datas = jsonSerializer.Serialize(listresult);

            return Json(theresult, JsonRequestBehavior.AllowGet);
        }

    }
}
