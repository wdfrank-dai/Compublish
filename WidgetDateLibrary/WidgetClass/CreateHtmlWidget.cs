using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using WidgetDate.Models;
using WidgetDateLibrary.Models;
using WidgetDate.Repository;
using System.Web.Script.Serialization;


namespace System.Web.Mvc.Html
{
    public static class CreateHtmlWidgetExtensions
    {
        //方法按AppUnits的id传参即参数pageid，theparams由页面传入，或者来源appunits表中的参数。需要和Datasource中的dsparams字段（参数规则）匹配。
        //初始显示一个units的时候需要读取字段unitstheparams和Datasource中的dsparams字段匹配生成去读取json数据，然后生成的每一条数据都应该有2个参数，一个是下一个units的id，一个是与下一个units的Datasource的参数匹配的实际参数，
        //即?nextid=xx&theparams=xxx,xxx,xxx,xxx(第二个参数来源于json的返回，格式和Datasource中的设置参数类型数量匹配,用，号分格，参数的拼接在GetPageContent（）方法中完成，与视图生成类ControlTemplate无关).也就是说数据商在他的Datasource中设定了什么参数那么相应的返回值也应该符合这个参数，平台只负责匹配和拼接这些参数。
        //那么视图生成类ControlTemplate中的一些方法需要按照实际要求修改下参数。（以前的已修改,后面的视图需要次规则.）
        public static MvcHtmlString GetPageContent(this HtmlHelper helper, int pageid, string theparams)
        {
            AppRepository ar = new AppRepository();
            AnalysisJson jr = new AnalysisJson();

            string addstr = "";//需要添加的控件字符串


                    ApplicationUnits appunits = ar.GetAppApplicationUnitByID(pageid);
                    if (appunits == null)
                    {
                        return MvcHtmlString.Create("未找到AppUnits控件！");
                    }
                    var appDatasource = ar.GetApplicationDatasourceByID(appunits.datasourceid.Value);
                    string datasurl = appDatasource.dsurl;
                    //解析参数
                    if (theparams == null || theparams == "")
                    {
                        //根据参数获取Json数据
                        var theparamset = appunits.unitparams != null ? appunits.unitparams.Split(',') : "".Split(',');

                        var paramset = appDatasource.dsparams != null ? appDatasource.dsparams.Split(',') : "".Split(',');

                        if (theparamset.Count() != paramset.Count())
                            return MvcHtmlString.Create("内容数据读取失败，错误代码102，请与系统管理员联系！");

                        string paras = "";
                        for (int i = 0; i < theparamset.Count(); i++)
                        {
                            if (i != 0)
                                paras = paras + "&";
                            else
                                paras = "?";
                            paras = paras + paramset[i] + "=" + theparamset[i];
                        }
                        theparams = paras;
                    }
                    else
                        theparams = "?" + theparams;
                    

                    string thedatas = jr.GetUrlJsonByUrl(datasurl + theparams);
                    if (thedatas == "")
                    {
                        return MvcHtmlString.Create("读取控件的Json数据失败！");
                    }

                    //反系列化Json数据
                    JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                    var datemodel = jsonSerializer.Deserialize<GetDataSourceModel>(thedatas);

                    var unitscss = ar.ApplicationUnitsCssByID(appunits.id);

               

                    //---------------------控件分别显示部分,CSS样式直接定义为固定字串，未使用CSS表--------------------
                    #region 控件分类型显示部分,使用IF
                    var AppUnitsShowType = ar.GetAllApplicationUnitsShowType();
                    //"SimpleTextLable"
                    var showtypetemp = AppUnitsShowType.Where(rep => rep.name == datemodel.DataSourceType).FirstOrDefault();
                    int showtypeid = 0;
                    if (showtypetemp != null)
                        showtypeid = showtypetemp.id;
                    try
                    {
                        switch (showtypeid)
                        {
                            case 0:
                                addstr = "显示模式有误";
                                break;
                            case 1://"SimpleTextLable": 
                                var datalist = jsonSerializer.Deserialize<SimpleTextLableModel>(datemodel.Datas);
                                addstr = new WidgetDate.Class.ControlTemplate().CreateLable(datemodel.DataSourceName, "SimpleTextLable", datalist);
                                break;
                            case 2://"HtmlLable": 
                                var htmllabledatalist = jsonSerializer.Deserialize<HtmlLableModel>(datemodel.Datas);
                                addstr = htmllabledatalist.LableHtml;
                                break;
                            case 3://DataList
                                var DataListdatalist = jsonSerializer.Deserialize<List<DataListModel>>(datemodel.Datas);
                                addstr = new WidgetDate.Class.ControlTemplate().CreateDataList(datemodel.DataSourceName, "DataList", DataListdatalist);
                                break;
                            case 4://NewList
                                var NewListdatalist = jsonSerializer.Deserialize<List<NewsListModel>>(datemodel.Datas);
                                addstr = new WidgetDate.Class.ControlTemplate().CreateNewList(datemodel.DataSourceName, "NewsList",appunits.NextUnitShowType, NewListdatalist, appunits.NextUnit.Value);
                                break; 
                            case 5://NewsContent
                                var NewsContentdatalist = jsonSerializer.Deserialize<NewsContentModel>(datemodel.Datas);
                                addstr = new WidgetDate.Class.ControlTemplate().CreateNewsContent(datemodel.DataSourceName, "NewsContent", NewsContentdatalist);
                                break;
                            case 6://HtmForm
                                var HtmFormdatalist = jsonSerializer.Deserialize<HtmlFormModel>(datemodel.Datas);
                                addstr = HtmFormdatalist.FormHtml;
                                break;
                            case 7://ImgNewList
                                var ImgNewListdatalist = jsonSerializer.Deserialize<List<ImgNewsListModel>>(datemodel.Datas);
                                addstr = new WidgetDate.Class.ControlTemplate().CreateImgNewList(datemodel.DataSourceName, "ImgNewsList", appunits.NextUnitShowType, ImgNewListdatalist, appunits.NextUnit.Value);
                                break;
                            default:
                                break;
                        }
                    }
                    catch
                    {
                        return MvcHtmlString.Create("无法解析的第三方数据格式！");
                    }

                    #endregion

              
            return MvcHtmlString.Create(addstr); 
        }



        //修改后方法，按WebPage后的版本。
        public static MvcHtmlString GetWebPageContent(this HtmlHelper helper, int webpageid)
        {
            AppRepository ar = new AppRepository();
            AnalysisJson jr = new AnalysisJson();


            WebPageSet webpage = ar.GetOneWebPageByAppID(webpageid);
            if (webpage == null)
            {
                return MvcHtmlString.Create("未找到WebPageSet！");
            }
            List<WebPageUnits> webpageunits = ar.GetOneWebPageUnitsByAppID(webpageid);
            string retstr1 = "<ul id='column1' class='column'>";
            string retstr2 = "<ul id='column1' class='column'>";
            string retstr = "";
            bool retstrif = true;
            foreach(var m in webpageunits)
            {

                retstr = "<li class='widget my-widget1'id='intro'><div class='widget-head'><h3 class='h3title'>"+m.showtitle+"</h3></div><div class='widget-content' style=''>";
                //----填充Units内容----
                ApplicationUnits appunits = ar.GetAppApplicationUnitByID(m.pageunitid);
                if (appunits == null)
                {
                    return MvcHtmlString.Create("未找到AppUnits控件！");
                }

                var appDatasource = ar.GetApplicationDatasourceByID(appunits.datasourceid.Value);
                string datasurl = appDatasource.dsurl;

                string thedatas = jr.GetUrlJsonByUrl(datasurl);
                if (thedatas == "")
                {
                    return MvcHtmlString.Create("读取控件的Json数据失败！");
                }

                //反系列化Json数据
                JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                var datemodel = jsonSerializer.Deserialize<GetDataSourceModel>(thedatas);

                //---------------------控件分别显示部分,CSS样式直接定义为固定字串，未使用CSS表--------------------
                #region 控件分类型显示部分,使用IF
                string addstr = "";
                var AppUnitsShowType = ar.GetAllApplicationUnitsShowType();
                //"SimpleTextLable"
                var showtypetemp = AppUnitsShowType.Where(rep => rep.name == datemodel.DataSourceType).FirstOrDefault();
                int showtypeid = 0;
                if (showtypetemp != null)
                    showtypeid = showtypetemp.id;
                try
                {
                    switch (showtypeid)
                    {
                        case 0:
                            addstr = "显示模式有误";
                            break;
                        case 1://"SimpleTextLable": 
                            var datalist = jsonSerializer.Deserialize<SimpleTextLableModel>(datemodel.Datas);
                            addstr = new WidgetDate.Class.ControlTemplate().CreateLable(datemodel.DataSourceName, "SimpleTextLable", datalist);
                            break;
                        case 2://"HtmlLable": 
                            var htmllabledatalist = jsonSerializer.Deserialize<HtmlLableModel>(datemodel.Datas);
                            addstr = htmllabledatalist.LableHtml;
                            break;
                        case 3://DataList
                            var DataListdatalist = jsonSerializer.Deserialize<List<DataListModel>>(datemodel.Datas);
                            addstr = new WidgetDate.Class.ControlTemplate().CreateDataList(datemodel.DataSourceName, "DataList", DataListdatalist);
                            break;
                        case 4://NewList
                            var NewListdatalist = jsonSerializer.Deserialize<List<NewsListModel>>(datemodel.Datas);
                            //addstr = new WidgetDate.Class.ControlTemplate().CreateNewList(datemodel.DataSourceName, "NewsList", appunits.NextPageShowType, NewListdatalist, appunits.NextPage.Value);
                            break;
                        case 5://NewsContent
                            var NewsContentdatalist = jsonSerializer.Deserialize<NewsContentModel>(datemodel.Datas);
                            addstr = new WidgetDate.Class.ControlTemplate().CreateNewsContent(datemodel.DataSourceName, "NewsContent", NewsContentdatalist);
                            break;
                        case 6://HtmForm
                            var HtmFormdatalist = jsonSerializer.Deserialize<HtmlFormModel>(datemodel.Datas);
                            addstr = HtmFormdatalist.FormHtml;
                            break;
                        case 7://ImgNewList
                            var ImgNewListdatalist = jsonSerializer.Deserialize<List<ImgNewsListModel>>(datemodel.Datas);
                            //addstr = new WidgetDate.Class.ControlTemplate().CreateImgNewList(datemodel.DataSourceName, "ImgNewsList", appunits.NextPageShowType, ImgNewListdatalist, appunits.NextPage.Value);
                            break;
                        default:
                            break;
                    }
                }
                catch
                {
                    return MvcHtmlString.Create("无法解析的第三方数据格式！");
                }

                #endregion




                //--------------------
                retstr += addstr;
                retstr += "</div></li>";
                if (retstrif)
                {
                    retstr1 += retstr;
                    retstrif = !retstrif;
                }
                else
                {
                    retstr2 += retstr;
                    retstrif = !retstrif;
                }
                
            }
            retstr1 += "</ul>";
            retstr2 += "</ul>";

            return MvcHtmlString.Create(retstr1 + retstr2);


         
        }



        /// <summary>  
        /// 参数：App的id,控件名称的统一标头.
        /// 
        /// </summary>  
        /// <param name="helper"></param>  
        /// <returns></returns>
        /// 
        //public static MvcHtmlString GetPageContent(this HtmlHelper helper, int pageid,string theparams)
        //{
        //    #region 初始化定义与处理

        //    string returnstr = "";
        //    AppRepository ar = new AppRepository();
        //    AnalysisJson jr = new AnalysisJson();

        //    #endregion
            
        //    #region 获取Page的所包含的Units

        //    var units = ar.GetOneApplicationPageByID(pageid).actionunits.Split(',');

        //    #endregion
            
        //    #region 循环分别对不同的Unit类型进行不同的数据装配
        //    //需要考虑多种情况，数据源获取失败怎么办，应该返回两种默认内容：
        //    //一个只是提示出错，一个因帐号密码不正确要求重新输入访问第三方应用系统的访问密码。可以先只考虑提示出错
        //    for (int i = 0; i < units.Count(); i++)
        //    {
        //        var nowunit = ar.GetAppApplicationUnitByID(Convert.ToInt32(units[i]));
        //        string datas = ar.GetApplicationDatasourceByID(nowunit.datasourceid.Value).dsurl;
        //        string date = jr.GetUrlJsonByUrl(datas);
        //        JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
        //        var datemodel = jsonSerializer.Deserialize<GetDataSourceModel>(date);
        //        string addstr = "";
        //        switch (datemodel.DataSourceType)
        //        {
        //            case "Lable":  //Lable
        //                break;
        //            case "NewsList":  //newlist
        //                try
        //                {
        //                    var datalist = jsonSerializer.Deserialize<List<NewsListModel>>(datemodel.Datas);
        //                    addstr = new WidgetDate.Class.ControlTemplate().CreateNewList(datemodel.DataSourceName, "DataList", nowunit.NextPageShowType, datalist, nowunit.NextPage.Value);
        //                }
        //                catch
        //                {
        //                    return MvcHtmlString.Create("无法解析的第三方数据格式！");
        //                }

        //                break;
        //        }
        //        returnstr = returnstr + addstr;

        //    }
        //    #endregion

        //    return MvcHtmlString.Create(returnstr);
        //}



        //改有pageid参数变成unitsid
        //public static MvcHtmlString GetPageContent(this HtmlHelper helper, int pageid, string theparams)//按pageid显示Widget,theparams为显示下一个page的参数，初始的使用可以为null
        //{
        //    AppRepository ar = new AppRepository();
        //    AnalysisJson jr = new AnalysisJson();


        //    ApplicationPage appage = ar.GetOneApplicationPageByAppID(pageid);
        //    if (appage == null)
        //    {
        //        return MvcHtmlString.Create("未找到App页面控制！");
        //    }
        //    #region page的模板控制部分
        //    ApplicationShowType appShow = ar.ApplicationShowTypeByID(appage.showtypeid.Value);

        //    string ShowType = appShow == null ? "" : appShow.viewdate;
        //    string[] c = new string[1];
        //    c[0] = appShow == null ? "" : appShow.FillMark;
        //    string[] aArray = ShowType.Split(c, System.StringSplitOptions.None);
        //    string[] unitsid = appage.actionunits.Split(',');

        //    bool appshowIF;
        //    if (appShow == null)//要是没有模板，就按默认顺着显示。但是这样需要考虑版面无法控制。不受控制的情况一般不需要出现。
        //    {
        //        appshowIF = false;
        //    }
        //    else
        //    {

        //        int unitsids = unitsid.Length;
        //        if (unitsid[unitsid.Length - 1] == "")
        //        {
        //            unitsids = unitsids - 1;
        //        }

        //        string r = ShowType.Replace(appShow.FillMark, "");
        //        int num = ((ShowType.Length - r.Length) / appShow.FillMark.Length) / 2;
        //        if (num != appShow.FillCount || num != unitsids || appShow.FillCount != unitsids)
        //        {
        //            return MvcHtmlString.Create("要填充的控件和显示模板的控件数量不符！");
        //        }
        //        appshowIF = true;
        //    }
        //    #endregion
        //    string returnstr = "";
        //    string addstr = "";//需要添加的控件字符串

        //    foreach (var m in unitsid)
        //    {
        //        if (m != "")
        //        {
        //            ApplicationUnits appunits = ar.GetAppApplicationUnitByID(Convert.ToInt32(m));
        //            if (appunits == null)
        //            {
        //                return MvcHtmlString.Create("未找到AppUnits控件！");
        //            }
        //            var appDatasource = ar.GetApplicationDatasourceByID(appunits.datasourceid.Value);
        //            string datasurl = appDatasource.dsurl;
        //            if (theparams == null || theparams == "")
        //            {
        //                //根据参数获取Json数据
        //                var theparamset = appunits.theparams != null ? appunits.theparams.Split(',') : "".Split(',');

        //                var paramset = appDatasource.dsparams != null ? appDatasource.dsparams.Split(',') : "".Split(',');

        //                if (theparamset.Count() != paramset.Count())
        //                    return MvcHtmlString.Create("内容数据读取失败，错误代码102，请与系统管理员联系！");

        //                string paras = "";
        //                for (int i = 0; i < theparamset.Count(); i++)
        //                {
        //                    if (i != 0)
        //                        paras = paras + "&";
        //                    else
        //                        paras = "?";
        //                    paras = paras + paramset[i] + "=" + theparamset[i];
        //                }
        //                theparams = paras;
        //            }
        //            else
        //                theparams = "?" + theparams;

        //            string thedatas = jr.GetUrlJsonByUrl(datasurl + theparams);
        //            if (thedatas == "")
        //            {
        //                return MvcHtmlString.Create("读取控件的Json数据失败！");
        //            }

        //            //反系列化Json数据
        //            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
        //            var datemodel = jsonSerializer.Deserialize<GetDataSourceModel>(thedatas);

        //            var unitscss = ar.ApplicationUnitsCssByID(appunits.id);

        //            //------------------未使用数据库的css，先取消--------------
        //            #region 控件的Css部分
        //            //string css = "";
        //            //foreach (var u in unitscss)
        //            //{
        //            //    if (css == "")
        //            //    {
        //            //        css = u.css;
        //            //    }
        //            //    else
        //            //    {
        //            //        css = css + "," + u.css;
        //            //    }
        //            //}
        //            #endregion
        //            //--------------------------------------------------------

        //            //---------------------控件分别显示部分,CSS样式直接定义为固定字串，未使用CSS表--------------------
        //            #region 控件分类型显示部分,使用IF
        //            var AppUnitsShowType = ar.GetAllApplicationUnitsShowType();
        //            //"SimpleTextLable"
        //            var showtypetemp = AppUnitsShowType.Where(rep => rep.name == datemodel.DataSourceType).FirstOrDefault();
        //            int showtypeid = 0;
        //            if (showtypetemp != null)
        //                showtypeid = showtypetemp.id;
        //            try
        //            {
        //                switch (showtypeid)
        //                {
        //                    case 0:
        //                        addstr = "显示模式有误";
        //                        break;
        //                    case 1://"SimpleTextLable": 
        //                        var datalist = jsonSerializer.Deserialize<SimpleTextLableModel>(datemodel.Datas);
        //                        addstr = new WidgetDate.Class.ControlTemplate().CreateLable(datemodel.DataSourceName, "SimpleTextLable", datalist);
        //                        break;
        //                    case 2://"HtmlLable": 
        //                        var htmllabledatalist = jsonSerializer.Deserialize<HtmlLableModel>(datemodel.Datas);
        //                        addstr = htmllabledatalist.LableHtml;
        //                        break;
        //                    case 3://DataList
        //                        var DataListdatalist = jsonSerializer.Deserialize<List<DataListModel>>(datemodel.Datas);
        //                        addstr = new WidgetDate.Class.ControlTemplate().CreateDataList(datemodel.DataSourceName, "DataList", DataListdatalist);
        //                        break;
        //                    case 4://NewList
        //                        var NewListdatalist = jsonSerializer.Deserialize<List<NewsListModel>>(datemodel.Datas);
        //                        addstr = new WidgetDate.Class.ControlTemplate().CreateNewList(datemodel.DataSourceName, "NewsList", appunits.NextPageShowType, NewListdatalist, appunits.NextPage.Value);
        //                        break;
        //                    case 5://NewsContent
        //                        var NewsContentdatalist = jsonSerializer.Deserialize<NewsContentModel>(datemodel.Datas);
        //                        addstr = new WidgetDate.Class.ControlTemplate().CreateNewsContent(datemodel.DataSourceName, "NewsContent", NewsContentdatalist);
        //                        break;
        //                    case 6://HtmForm
        //                        var HtmFormdatalist = jsonSerializer.Deserialize<HtmlFormModel>(datemodel.Datas);
        //                        addstr = HtmFormdatalist.FormHtml;
        //                        break;
        //                    case 7://ImgNewList
        //                        var ImgNewListdatalist = jsonSerializer.Deserialize<List<ImgNewsListModel>>(datemodel.Datas);
        //                        addstr = new WidgetDate.Class.ControlTemplate().CreateImgNewList(datemodel.DataSourceName, "ImgNewsList", appunits.NextPageShowType, ImgNewListdatalist, appunits.NextPage.Value);
        //                        break;
        //                    default:
        //                        break;
        //                }
        //            }
        //            catch
        //            {
        //                return MvcHtmlString.Create("无法解析的第三方数据格式！");
        //            }

        //            #endregion

        //            //------------------------------------
        //            //按名称填充模板中的控件
        //            if (appshowIF)
        //            {
        //                for (int j = 1; j < aArray.Count(); j++)//foreach(string n in aArray)
        //                {
        //                    if (aArray[j] == ar.GetApplicationUnitsShowTypeByID(appunits.showstyleid.Value).name)
        //                    {
        //                        aArray[j] = addstr;
        //                    }
        //                }
        //            }
        //            else
        //                returnstr += addstr;
        //        }
        //    }
        //    //拼接所有控件和模板样式
        //    if (appshowIF)
        //    {
        //        foreach (string n in aArray)
        //        {
        //            returnstr += n;
        //        }
        //    }

        //    return MvcHtmlString.Create(returnstr);
        //}

    //    /// <summary>  
    //    /// 参数：id值，控件名称，css样式名称,标题图标url
    //    /// 
    //    /// </summary>  
    //    /// <param name="helper"></param>  
    //    /// <returns></returns>  
    //    public static MvcHtmlString CreateNewDate(this HtmlHelper helper, int id, string name, string csslabel,string cssnewdate,string imgurl)
    //    {
    //        //待解决问题：在有多个不同的控件要加载的时候，在循环中无法为不同的控件匹配视图方法。初步想法，根据units的名称来判断，用switch来匹配
    //        //但是通用性很差。整个程序除了这里其他地方基本通用。
    //        AppRepository ar = new AppRepository();
    //        AnalysisJson jr = new AnalysisJson();
    //        Application app = ar.GetApplicationByID(id);
    //        //外层ShouwType的模板框架
    //        ApplicationShowType appShow = ar.ApplicationShowTypeByID(app.showtypeid.Value);
    //        string ShowType = appShow.viewdate;
    //        string r = ShowType.Replace(appShow.FillMark, "");
    //        int num = ((ShowType.Length - r.Length) / appShow.FillMark.Length) / 2;
    //        if (num != appShow.FillCount)
    //        {
    //            return MvcHtmlString.Create("要填充的控件和显示模板的控件数量不符！");
    //        }
    //        string[] c = new string[1];

    //        c[0] = appShow.FillMark;
    //        string[] aArray = ShowType.Split(c, System.StringSplitOptions.None);
    //        //ShowType.Insert

    //        //内层循环嵌入控件
    //        List<ApplicationUnits> appage = ar.GetListAppApplicationUnitsByAppID(id);
    //        string returnstr = "";
    //        string addstr = "";//需要添加的控件字符串
    //        for (int i = 0; i < appage.Count(); i++)//foreach(var m in appage)
    //        {
    //            string datas = ar.GetApplicationDatasourceByID(appage[i].datasourceid.Value).dsurl;
    //            //根据数据源读取数据

    //            //String savePath = System.Web.HttpContext.Current.Server.MapPath("~") + datas;
    //            string date = jr.GetUrlJsonByUrl(datas);

    //            if (date == "")
    //            {
    //                return MvcHtmlString.Create("读取控件的Json数据失败！");
    //            }

    //            switch (appage[i].showstyle)
    //            {
    //                case "Lable":
    //                    string[] datearray1 = jr.JsonToStringArry(date);

    //                    if (datearray1 == null)
    //                    {
    //                        return MvcHtmlString.Create("解析Json数据失败！");
    //                    }

    //                    addstr = new WidgetDate.Class.ControlTemplate().CreateLable(name, csslabel, datearray1);

    //                    break;
    //                case "NewDate":
    //                    List<NewDateModel> datearray2 = jr.JsonToListNewDate(imgurl,date);

    //                    if (datearray2 == null)
    //                    {
    //                        return MvcHtmlString.Create("解析Json数据失败！");
    //                    }
                
    //                    addstr = new WidgetDate.Class.ControlTemplate().CreateNewDate(name, cssnewdate, datearray2);

    //                    break;               
    //            }

                

    //            //------------------------------

    //            for (int j = 1; j < aArray.Count(); j++)//foreach(string n in aArray)
    //            {
    //                if (aArray[j] == appage[i].unitname)
    //                {
    //                    aArray[j] = addstr;
    //                }
    //            }

    //            //returnstr = returnstr + addstr +aArray[i+1];
    //            //ShowType = ShowType.Insert(aArray[i].Length,addstr);
    //        }
    //        foreach (string n in aArray)
    //        {
    //            returnstr += n;
    //        }


    //        return MvcHtmlString.Create(returnstr);
    //    }


    //    /// <summary>  
    //    /// 参数：id值，控件名称,表单返回打开的方式，css样式名称
    //    /// 
    //    /// </summary>  
    //    /// <param name="helper"></param>  
    //    /// <returns></returns>  
    //    public static MvcHtmlString CreateForm(this HtmlHelper helper, int id, string name, string target, string css)
    //    {
    //        AppRepository ar = new AppRepository();
    //        AnalysisJson jr = new AnalysisJson();
    //        Application app = ar.GetApplicationByID(id);
    //        //外层ShouwType的模板框架
    //        ApplicationShowType appShow = ar.ApplicationShowTypeByID(app.showtypeid.Value);
    //        string ShowType = appShow.viewdate;
    //        string r = ShowType.Replace(appShow.FillMark, "");
    //        int num = ((ShowType.Length - r.Length) / appShow.FillMark.Length) / 2;
    //        if (num != appShow.FillCount)
    //        {
    //            return MvcHtmlString.Create("要填充的控件和显示模板的控件数量不符！");
    //        }
    //        string[] c = new string[1];

    //        c[0] = appShow.FillMark;
    //        string[] aArray = ShowType.Split(c, System.StringSplitOptions.None);
    //        //ShowType.Insert

    //        //内层循环嵌入控件
    //        List<ApplicationUnits> appage = ar.GetListAppApplicationUnitsByAppID(id);
    //        string returnstr = "";
    //        string addstr = "";//需要添加的控件字符串
    //        for (int i = 0; i < appage.Count(); i++)//foreach(var m in appage)
    //        {
    //            string datas = ar.GetApplicationDatasourceByID(appage[i].datasourceid.Value).dsurl;
    //            //根据数据源读取数据

    //            //String savePath = System.Web.HttpContext.Current.Server.MapPath("~") + datas;
    //            string date = jr.GetUrlJsonByUrl(datas);

    //            if (date == "")
    //            {
    //                return MvcHtmlString.Create("读取控件的Json数据失败！");
    //            }

    //            switch (appage[i].showstyle)
    //            {
    //                case "Form":
    //                    if (appage[i].utype == 0)
    //                    {
    //                        addstr = date;
    //                    }else
    //                    {
    //                        string a, m;
    //                        List<FormInputModel> datearray2 = jr.JsonToForm(out a, out m, date);
    //                        if (datearray2 == null)
    //                        {
    //                            return MvcHtmlString.Create("解析Json数据失败！");
    //                        }
    //                        addstr = new WidgetDate.Class.ControlTemplate().CreateForm(name,m, target,a,css, datearray2);                        
    //                    }
    //                    break;
    //            }



    //            //------------------------------

    //            for (int j = 1; j < aArray.Count(); j++)//foreach(string n in aArray)
    //            {
    //                if (aArray[j] == appage[i].unitname)
    //                {
    //                    aArray[j] = addstr;
    //                }
    //            }

    //            //returnstr = returnstr + addstr +aArray[i+1];
    //            //ShowType = ShowType.Insert(aArray[i].Length,addstr);
    //        }
    //        foreach (string n in aArray)
    //        {
    //            returnstr += n;
    //        }


    //        return MvcHtmlString.Create(returnstr);
    //    }

    }

}
