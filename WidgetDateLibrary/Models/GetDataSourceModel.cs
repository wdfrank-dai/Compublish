using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace WidgetDate.Models
{
    [Serializable]
    public class GetDataSourceModel
    {
        public string DataSourceType {get; set;}//约定就是SimpleTextLable、HtmlLable、DataList、NewsList、NewsContent、HtmlForm、ImgNewList
        public string DataSourceName { get; set; }
        public string Datas { get; set; }
    }

    [Serializable]
    public class SimpleTextLableModel
    {
        public string LableText { get; set; }
    }

    [Serializable]
    public class HtmlLableModel
    {
        public string LableHtml { get; set; }
    }

    [Serializable]
    public class DataListModel
    {
        public string DataItem { get; set; }
        public string DataValue { get; set; } 
    }

    [Serializable]
    public class NewsListModel
    {
        public string NewsId { get; set; }
        public string NewsUrl { get; set; }
        public string NewsTitle { get; set; }
        public string NewsIntro { get; set; }
        public string NewsDataTime { get; set; }
    }

    [Serializable]
    public class NewsContentModel
    {
        public string NewsTitle { get; set; }
        public string NewsCollateralTitle { get; set; }
        public string NewsText { get; set; }
        public string NewsEnd { get; set; }
    }

    [Serializable]
    public class HtmlFormModel
    {
        public string FormHtml { get; set; }
    }

    [Serializable]
    public class ImgNewsListModel
    {
        public string NewsId { get; set; }
        public string NewsImgUrl { get; set; }
        public string NewsUrl { get; set; }
        public string NewsTitle { get; set; }
        public string NewsDataTime { get; set; }
    }
}
