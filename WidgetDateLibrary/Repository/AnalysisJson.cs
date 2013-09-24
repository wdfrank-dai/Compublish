using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WidgetDate.Models;
using System.Net;
using System.Text;

namespace WidgetDate.Repository
{
    public class AnalysisJson
    {
        //根据URL获取Json数据，以是否Http开头来判断是内部数据还是第三方数据。失败返回空字符串。
        public string GetUrlJsonByUrl(string url)
        {
            try
            {
                WebRequest wrt;
                string sdasd = url.Substring(0, 4);
                if (url.Length < 4 || !(url.Substring(0, 4) == "http" || url.Substring(0, 4) == "HTTP" || url.Substring(0, 4) == "Http"))
                {
                    wrt = WebRequest.Create("http://" + HttpContext.Current.Request.Url.Authority + "/" + url);
                }
                else
                {
                    wrt = WebRequest.Create(url);
                }
                wrt.Credentials = CredentialCache.DefaultCredentials;
                WebResponse wrp;
                wrp = wrt.GetResponse();
                return new System.IO.StreamReader(wrp.GetResponseStream(), Encoding.UTF8).ReadToEnd();
            }
            catch
            {
                return "";
            }
        }
        //带参数的获取Json数据
        //public string GetUrlJsonByUrlAndStr(string url)
        //{
        //    try
        //    {
        //        WebRequest wrt;
        //        string sdasd = url.Substring(0, 4);
        //        if (url.Length < 4 || !(url.Substring(0, 4) == "http" || url.Substring(0, 4) == "HTTP" || url.Substring(0, 4) == "Http"))
        //        {
        //            wrt = WebRequest.Create(HttpContext.Current.Request.Url.ToString() + url);
        //        }
        //        else
        //        {
        //            wrt = WebRequest.Create(url);
        //        }
        //        wrt.Credentials = CredentialCache.DefaultCredentials;
        //        WebResponse wrp;
        //        wrp = wrt.GetResponse();
        //        return new System.IO.StreamReader(wrp.GetResponseStream(), Encoding.UTF8).ReadToEnd();
        //    }
        //    catch
        //    {
        //        return "";
        //    }
        //}



        //讲Json数据解析成字符数组，失败返回null。
        public string[] JsonToStringArry(string date)
        {
            //"[\"qweqwe\",\"qwrqwr\",\"werewrew\",\"werwerwerwerwe\"]"
            try
            {
                date = date.Substring(2, date.Length - 4);
                string[] c = new string[1];

                c[0] = "\",\"";
                string[] arr = date.Split(c, System.StringSplitOptions.None);



                return arr;
            }
            catch
            {
                return null;
            }

        }

        //讲Json数据解析成新闻列表，失败返回null。
        public List<NewDateModel> JsonToListNewList(string date,int pageid)
        {
            //[[\"6月27日（周四）下午政治、业务学习（闭馆）通知\",\"http://www.lib.scuec.edu.cn/HomeSubsidiary/NewText?dataid=381\",\"2013-06-26\"],[null,null,null],[null,null,null],[null,null,null]]
            try
            {
                date = date.Substring(2, date.Length - 4);
                string[] c = new string[1];

                c[0] = "],[";
                string[] arr = date.Split(c, System.StringSplitOptions.None);

                List<NewDateModel> rl = new List<NewDateModel>();
                foreach (var m in arr)
                {
                    string s = m.Substring(1, m.Length - 2);
                    c[0] = "\",\"";
                    string[] arrr = s.Split(c, System.StringSplitOptions.None);


                    NewDateModel model = new NewDateModel();
                    model.imgurl = "../../Images/list_dot.png";
                    model.id = arrr[0];
                    model.title = arrr[1];
                    model.time = arrr[2];
                    model.url = "/WidgetDateView/AccordingToParameterGetDate/GetDateByStr?pageid=" + pageid + "&str=" + arrr[0];
                    rl.Add(model);
                }


                return rl;
            }
            catch
            {
                return null;
            }

        }
        //将数据解析成新闻详细列表
        public NewDateModel JsonToNewDate(string date)
        {
            try
            {
                date = date.Substring(3, date.Length - 6);
                string[] c = new string[1];

                c[0] = "\",\"";
                string[] arr = date.Split(c, System.StringSplitOptions.None);


                NewDateModel model = new NewDateModel();

                model.title = arr[0];
                model.time = arr[1];
                model.text = arr[2];


                return model;
            }
            catch
            {
                return null;
            }
        
        }


        //讲Json数据解析form表单。
        public List<FormInputModel> JsonToForm(out string a, out string m, string date)
        {
            //[\"post,ReturnJson/Post\",\"姓名,name,text,,,1,输入姓名\",\"年龄,year,text,,,,输入年龄\",\"男,sex,radio,男,1,,\",\"女,sex,radio,女,,,\"]

            try
            {

                date = date.Substring(2, date.Length - 4);


                List<FormInputModel> rl = new List<FormInputModel>();
                string[] c = new string[1];
                c[0] = "\",\"";
                string[] arr = date.Split(c, System.StringSplitOptions.None);
                string[] arrr = arr[0].Split(',');
                m = arrr[0]; a = arrr[1];
                for (int i = 1; i < arr.Count(); i++)//foreach (var t in arr)
                {
                    FormInputModel model = new FormInputModel();
                    string[] arrt = arr[i].Split(',');
                    model.title = arrt[0];
                    model.name = arrt[1];
                    model.type = arrt[2];
                    model.value = arrt[3];
                    model.Fchecked = arrt[4];
                    model.Funll = arrt[5];
                    model.endtitle = arrt[6];
                    rl.Add(model);

                }


                return rl;
            }
            catch
            {
                a = ""; m = "";
                return null;
            }
        }
    }
}