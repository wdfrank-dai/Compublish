using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using WidgetDate.Models;

namespace WidgetDate.Class
{
    public class ControlTemplate
    {
        //创造一个Label
        public string CreateLable(string name, string css, SimpleTextLableModel date)
        {
            try
            {
                StringBuilder str = new StringBuilder();
                name = name + "_Lable";
                str.Append("<a class='lable_a' name=" + name + " id=" + name + " class=" + css + ">");
                str.Append(date.LableText);
                str.Append("</a>");
                return str.ToString();

            }
            catch
            {
                return "生成视图出错";
            }
        }
        //创造一个不带css样式的小图片
        public string CreateImg(string url)
        {
            //../../Images/list_dot.png
            try
            {
                StringBuilder str = new StringBuilder();
                str.Append("<img style='float:left' src="+ url +" />");
 
                return str.ToString();

            }
            catch
            {
                return "";
            }
        }
        //创造一个NewList
        public string CreateNewList(string name, string css,string showtype, List<NewsListModel> date,int nextpageid)
        {
            try
            {
                StringBuilder str = new StringBuilder();
                name = name + "_NewList";
                str.Append("<div name=" + name + " id=" + name + " class=" + css + ">");
                foreach (var m in date)
                {
                    //m.title = m.title.Substring(0,22);
                    str.Append("<p>");
                    if (showtype == null || showtype == "")
                    {
                        if (m.NewsUrl == null || m.NewsUrl == "")
                        {
                            str.Append("<span>" + m.NewsTitle + "</span>");
                        }
                        else
                        {
                            str.Append("<a href="+m.NewsUrl+" target='_blank' title='" + m.NewsTitle + "'>" + m.NewsTitle + "</a>");
                        }
                        
                    }else
                    {
                        if (m.NewsUrl == null || m.NewsUrl == "")
                        {
                            if (showtype == "ShowDataDetail")
                                str.Append("<a href=/WidgetDateView/Show/ShowDataDetail?pageid=" + nextpageid + "&theparams=id=" + m.NewsId + " target='_blank' title='" + m.NewsTitle + "'>" + m.NewsTitle + "</a>");
                            if (showtype == "OnWindow")
                                str.Append("<a href='javascript:void(0);' title='" + m.NewsTitle + "' onclick=OnWindows(" + nextpageid + ",'id=" + m.NewsId + "')> " + m.NewsTitle + "</a>");
                        }
                        else
                            str.Append("<a href=" + m.NewsUrl + " target='_blank' title='" + m.NewsTitle + "'>" + m.NewsTitle + "</a>");
                    }
                    str.Append("<span>" + m.NewsDataTime + "</span>");
                    str.Append("</p>");
                }
                str.Append("</div>");
                return str.ToString();

            }
            catch
            {
                return "生成视图出错";
            }
        }

        //创建一个带图片的新闻列表
        public string CreateImgNewList(string name, string css, string showtype, List<ImgNewsListModel> date, int nextpageid)
        {
            try
            {
                StringBuilder str = new StringBuilder();
                name = name + "_NewList";
                str.Append("<div name=" + name + " id=" + name + " class=" + css + ">");
                foreach (var m in date)
                {
                    //m.title = m.title.Substring(0,22);
                    str.Append("<p>");
                    str.Append("<img class='Newslist_img' src='" + m.NewsImgUrl + "' />");
                    if (showtype == null)
                    {
                        str.Append("<span>" + m.NewsTitle + "</span>");
                    }
                    else
                    {
                        if (showtype == "ShowDataDetail")
                            str.Append("<a class='Newslist_a' href=/WidgetDateView/Show/ShowDataDetail?pageid=" + nextpageid + "&theparams=id=" + m.NewsId + " target='_blank' title='" + m.NewsTitle + "'>" + m.NewsTitle + "</a>");
                        if (showtype == "OnWindow")
                            str.Append("<a href='javascript:void(0);' title='" + m.NewsTitle + "' onclick=OnWindows(" + nextpageid + ",'id=" + m.NewsId + "')> " + m.NewsTitle + "</a>");
                    }
                    str.Append("<span class='Newslist_time'>" + m.NewsDataTime + "</span>");
                    str.Append("</p>");
                }
                str.Append("</div>");
                return str.ToString();

            }
            catch
            {
                return "生成视图出错";
            }
        }

        //创造一个NewDate
        public string CreateNewsContent(string name, string css, NewsContentModel date)
        {
            try
            {
                StringBuilder str = new StringBuilder();
                name = name + "_NewsContent";
                str.Append("<div name=" + name + " id=" + name + " class=" + css + ">");

                    str.Append("<h3 class='DataDetail_Title'>");
                    str.Append(date.NewsTitle);
                    str.Append("</h3>");
                    str.Append("<div class='DataDetail_Time'>");
                    str.Append(date.NewsCollateralTitle);
                    str.Append("</div>");
                    str.Append("<div class='DataDetail_Main'>");
                    str.Append("<p>");
                    str.Append(date.NewsText);
                    str.Append("</p>");
                    str.Append("<div class='DataDetail_Writer'>");
                    str.Append("<p>");
                    str.Append(date.NewsEnd);
                    str.Append("</p>");
                    str.Append("</div>");
                    str.Append("</div>");
                    

                str.Append("</div>");
                return str.ToString();

            }
            catch
            {
                return "生成视图出错";
            }
        }

        public string CreateDataList(string name, string css, List<DataListModel> date)
        {
            try
            {
                StringBuilder str = new StringBuilder();
                name = name + "_DataList";
                str.Append("<div name=" + name + " id=" + name + " class=" + css + ">");
                foreach (var m in date)
                {
                    str.Append("<p class='Datalist_p'>");
                    str.Append("<span>" + m.DataItem + "</span>");
                    str.Append("<span>" + m.DataValue + "</span>");
                    str.Append("</p>");
                }
                str.Append("</div>");
                return str.ToString();

            }
            catch
            {
                return "生成视图出错";
            }
        }

        //创造一个form,验证需要页面中添加验证控件和JQ。
        //<script src="../../Scripts/jquery-1.5.1.min.js" type="text/javascript"></script>
        //script src="../../Scripts/jquery.validate.min.js" type="text/javascript"></script>
        //<script src="../../Scripts/jquery.validate.unobtrusive.min.js" type="text/javascript"></script>
        //以及css文件
/*
.field-validation-error
{
    color: #ff0000;
    font-family:微软雅黑;
    font-size:12px;
}

.field-validation-valid
{
    display: none;
}

.input-validation-error
{
    border: 1px solid #ff0000;
    background-color: #ffeeee;
    font-family:微软雅黑;
    font-size:12px;
}

.validation-summary-errors
{
    font-weight: bold;
    font-family:微软雅黑;
    color: #ff0000;
    font-size:12px;
}

.validation-summary-valid
{
    display: none;
}
*/
        public string CreateForm(string name,string method,string target, string action,string css, List<FormInputModel> date)
        {
            try
            {
                StringBuilder str = new StringBuilder();
                name = name + "_Form";
                str.Append("<form name='"+name+"' action='"+action+"' method='"+method+"' target='"+target+"' class='"+css+"'>");

                foreach (var m in date)
                {
                    str.Append("<div>");
                    string v ="";
                    string v1 = "";
                        if(m.Funll!="")
                        {
                            v1="data-val='true' data-val-required='"+m.title+" 字段是必需的。' class=' required '";
                            v="<span style='color:Red'>*</span><label data-valmsg-for='"+m.name+"'></label>";
                        }
                    string c ="";
                    if (m.Fchecked != "")
                    {
                        c = "checked";
                    }
                    str.Append(m.title+"<input id='"+ m.name +"' type='"+ m.type +"' name='"+ m.name +"' "+ c +" value='"+ m.value +"' "+v1+" />" + v +"<span>"+ m.endtitle+"</span>");
                    str.Append("</div>");
                }

                str.Append("<input type='submit' value='提交' />");
                str.Append("</form>");
                return str.ToString();

            }
            catch
            {
                return "生成视图出错";
            }
        }
    }
}