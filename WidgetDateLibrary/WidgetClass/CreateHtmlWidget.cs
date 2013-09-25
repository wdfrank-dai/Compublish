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




        public static MvcHtmlString GetPageContent(this HtmlHelper helper, int pageid, string theparams)//按pageid显示Widget,theparams为显示下一个page的参数，初始的使用可以为null
        {
            AppRepository ar = new AppRepository();
            AnalysisJson jr = new AnalysisJson();


            ApplicationPage appage = ar.GetOneApplicationPageByAppID(pageid);
            if (appage == null)
            {
                return MvcHtmlString.Create("未找到App页面控制！");
            }
            #region page的模板控制部分
            ApplicationShowType appShow = ar.ApplicationShowTypeByID(appage.showtypeid.Value);

            string ShowType = appShow==null?"" : appShow.viewdate;
            string[] c = new string[1];
            c[0] = appShow == null ? "" : appShow.FillMark;
            string[] aArray = ShowType.Split(c, System.StringSplitOptions.None);
            string[] unitsid = appage.actionunits.Split(',');

            bool appshowIF;
            if (appShow == null)//要是没有模板，就按默认顺着显示。但是这样需要考虑版面无法控制。不受控制的情况一般不需要出现。
            {
                appshowIF = false;
            }
            else
            {

                int unitsids = unitsid.Length;
                if (unitsid[unitsid.Length - 1] == "")
                {
                    unitsids = unitsids - 1;
                }

                string r = ShowType.Replace(appShow.FillMark, "");
                int num = ((ShowType.Length - r.Length) / appShow.FillMark.Length) / 2;
                if (num != appShow.FillCount || num != unitsids || appShow.FillCount != unitsids)
                {
                    return MvcHtmlString.Create("要填充的控件和显示模板的控件数量不符！");
                }
                appshowIF = true;
            }
            #endregion
            string returnstr = "";
            string addstr = "";//需要添加的控件字符串

            foreach (var m in unitsid)
            {
                if (m != "")
                {
                    ApplicationUnits appunits = ar.GetAppApplicationUnitByID(Convert.ToInt32(m));
                    if (appunits == null)
                    {
                        return MvcHtmlString.Create("未找到AppUnits控件！");
                    }
                    //根据参数获取Json数据
                    var appDatasource = ar.GetApplicationDatasourceByID(appunits.datasourceid.Value);
                    string datasurl = appDatasource.dsurl;

                    //string thedatas = "";
                    #region 拼接参数部分
                    if (theparams != null)
                    {
                        string[] theparamsArre = theparams.Split(',');
                        string theparamsStr = "";
                        foreach(string s in theparamsArre)
                        {
                            if (appDatasource.dataitems == s.Substring(0, appDatasource.dataitems.Length))
                                if (theparamsStr == "")
                                    theparamsStr = s;
                                else
                                    theparamsStr = "&" + s;
                        }

                        datasurl = datasurl + "?" + theparamsStr;


                    }
                    #endregion

                    string thedatas = jr.GetUrlJsonByUrl(datasurl);
                    if (thedatas == "")
                    {
                        return MvcHtmlString.Create("读取控件的Json数据失败！");
                    }

                    //反系列化Json数据
                    JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                    var datemodel = jsonSerializer.Deserialize<GetDataSourceModel>(thedatas);

                    var unitscss = ar.ApplicationUnitsCssByID(appunits.id);

                    //------------------未使用数据库的css，先取消--------------
                    #region 控件的Css部分
                    //string css = "";
                    //foreach (var u in unitscss)
                    //{
                    //    if (css == "")
                    //    {
                    //        css = u.css;
                    //    }
                    //    else
                    //    {
                    //        css = css + "," + u.css;
                    //    }
                    //}
                    #endregion
                    //--------------------------------------------------------

                    //---------------------控件分别显示部分,CSS样式直接定义为固定字串，未使用CSS表--------------------
                    #region 控件分类型显示部分,使用IF
                    var AppUnitsShowType = ar.GetAllApplicationUnitsShowType();
                    //"SimpleTextLable"
                    if (datemodel.DataSourceType == AppUnitsShowType[0].name)
                    {
                        try
                        {
                            var datalist = jsonSerializer.Deserialize<SimpleTextLableModel>(datemodel.Datas);
                            addstr = new WidgetDate.Class.ControlTemplate().CreateLable(datemodel.DataSourceName, "SimpleTextLable", datalist);
                        }
                        catch
                        {
                            return MvcHtmlString.Create("无法解析的第三方数据格式！");
                        }
                    }
                    //"HtmlLable":  
                    if (datemodel.DataSourceType == AppUnitsShowType[1].name)
                    {
                        try
                        {
                            var datalist = jsonSerializer.Deserialize<HtmlLableModel>(datemodel.Datas);
                            addstr = datalist.LableHtml;
                        }
                        catch
                        {
                            return MvcHtmlString.Create("无法解析的第三方数据格式！");
                        }
                        break;
                    }
                    //DataList
                    if (datemodel.DataSourceType == AppUnitsShowType[2].name)
                    {
                        try
                        {
                            var datalist = jsonSerializer.Deserialize<List<DataListModel>>(datemodel.Datas);
                            addstr = new WidgetDate.Class.ControlTemplate().CreateDataList(datemodel.DataSourceName, "DataList", datalist);
                        }
                        catch
                        {
                            return MvcHtmlString.Create("无法解析的第三方数据格式！");
                        }
                    }
                    //NewList
                    if (datemodel.DataSourceType == AppUnitsShowType[3].name)
                    {
                        try
                        {
                            var datalist = jsonSerializer.Deserialize<List<NewsListModel>>(datemodel.Datas);
                            addstr = new WidgetDate.Class.ControlTemplate().CreateNewList(datemodel.DataSourceName, "NewsList", appunits.NextPageShowType, datalist, appunits.NextPage.Value);
                        }
                        catch
                        {
                            return MvcHtmlString.Create("无法解析的第三方数据格式！");
                        }
                    }
                    //"NewsContent":  
                    if (datemodel.DataSourceType == AppUnitsShowType[4].name)
                    {
                        try
                        {
                            var datalist = jsonSerializer.Deserialize<NewsContentModel>(datemodel.Datas);
                            addstr = new WidgetDate.Class.ControlTemplate().CreateNewsContent(datemodel.DataSourceName, "NewsContent", datalist);
                        }
                        catch
                        {
                            return MvcHtmlString.Create("无法解析的第三方数据格式！");
                        }
                    }
                    //HtmForm
                    if (datemodel.DataSourceType == AppUnitsShowType[5].name)
                    {
                        try
                        {
                            var datalist = jsonSerializer.Deserialize<HtmlFormModel>(datemodel.Datas);
                            addstr = datalist.FormHtml;
                        }
                        catch
                        {
                            return MvcHtmlString.Create("无法解析的第三方数据格式！");
                        }
                    }
                    //ImgNewList
                    if (datemodel.DataSourceType == AppUnitsShowType[6].name)
                    {
                        try
                        {
                            var datalist = jsonSerializer.Deserialize<List<ImgNewsListModel>>(datemodel.Datas);
                            addstr = new WidgetDate.Class.ControlTemplate().CreateImgNewList(datemodel.DataSourceName, "ImgNewsList", appunits.NextPageShowType, datalist, appunits.NextPage.Value);
                        }
                        catch
                        {
                            return MvcHtmlString.Create("无法解析的第三方数据格式！");
                        }
                    }
                    #endregion
                    #region 控件分类型显示部分，使用Swith
                    //switch (datemodel.DataSourceType)
                    //{
                    //    case "SimpleTextLable":
                    //        try
                    //        {
                    //            var datalist = jsonSerializer.Deserialize <SimpleTextLableModel> (datemodel.Datas);
                    //            addstr = new WidgetDate.Class.ControlTemplate().CreateLable(datemodel.DataSourceName, "SimpleTextLable", datalist);
                    //        }
                    //        catch
                    //        {
                    //            return MvcHtmlString.Create("无法解析的第三方数据格式！");
                    //        }
                    //        break;
                    //    case "HtmlLable":  
                    //        try
                    //        {
                    //            var datalist = jsonSerializer.Deserialize<HtmlLableModel>(datemodel.Datas);
                    //            addstr = datalist.LableHtml;
                    //        }
                    //        catch
                    //        {
                    //            return MvcHtmlString.Create("无法解析的第三方数据格式！");
                    //        }
                    //        break;
                    //    case "DataList":
                    //        try
                    //        {
                    //            var datalist = jsonSerializer.Deserialize < List<DataListModel>>(datemodel.Datas);
                    //            addstr = new WidgetDate.Class.ControlTemplate().CreateDataList(datemodel.DataSourceName, "DataList", datalist);
                    //        }
                    //        catch
                    //        {
                    //            return MvcHtmlString.Create("无法解析的第三方数据格式！");
                    //        }
                    //        break;
                    //    case "NewsList": 
                    //        try
                    //        {
                    //            var datalist = jsonSerializer.Deserialize<List<NewsListModel>>(datemodel.Datas);
                    //            addstr = new WidgetDate.Class.ControlTemplate().CreateNewList(datemodel.DataSourceName, "NewsList", appunits.NextPageShowType, datalist, appunits.NextPage.Value);
                    //        }
                    //        catch
                    //        {
                    //            return MvcHtmlString.Create("无法解析的第三方数据格式！");
                    //        }

                    //        break;
                    //    case "NewsContent":  
                    //        try
                    //        {
                    //            var datalist = jsonSerializer.Deserialize<NewsContentModel>(datemodel.Datas);
                    //            addstr = new WidgetDate.Class.ControlTemplate().CreateNewsContent(datemodel.DataSourceName, "NewsContent", datalist);
                    //        }
                    //        catch
                    //        {
                    //            return MvcHtmlString.Create("无法解析的第三方数据格式！");
                    //        }

                    //        break;
                    //    case "HtmlForm":  
                    //        try
                    //        {
                    //            var datalist = jsonSerializer.Deserialize<HtmlFormModel>(datemodel.Datas);
                    //            addstr = datalist.FormHtml;
                    //        }
                    //        catch
                    //        {
                    //            return MvcHtmlString.Create("无法解析的第三方数据格式！");
                    //        }
                    //        break;
                    //    case "ImgNewsList":
                    //        try
                    //        {
                    //            var datalist = jsonSerializer.Deserialize<List<ImgNewsListModel>>(datemodel.Datas);
                    //            addstr = new WidgetDate.Class.ControlTemplate().CreateImgNewList(datemodel.DataSourceName, "ImgNewsList", appunits.NextPageShowType, datalist, appunits.NextPage.Value);
                    //        }
                    //        catch
                    //        {
                    //            return MvcHtmlString.Create("无法解析的第三方数据格式！");
                    //        }

                    //        break;
                    //}
                    #endregion
                    //------------------------------------
                    //按名称填充模板中的控件
                    if (appshowIF)
                    {
                        for (int j = 1; j < aArray.Count(); j++)//foreach(string n in aArray)
                        {
                            if (aArray[j] == ar.GetApplicationUnitsShowTypeByID(appunits.showstyleid.Value).name )
                            {
                                aArray[j] = addstr;
                            }
                        }
                    }
                    else
                        returnstr += addstr;
                }
            }
            //拼接所有控件和模板样式
            if (appshowIF)
            {
                foreach (string n in aArray)
                {
                    returnstr += n;
                }
            }

            return MvcHtmlString.Create(returnstr);
        }

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
