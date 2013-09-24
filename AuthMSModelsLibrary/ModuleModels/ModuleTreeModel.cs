using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AuthModelManage.Models
{
    public class ModuleTreeModel
    {
        public string data;
        public JsTreeAttribute attr;
        public string state;
        public List<ModuleTreeModel> children;
    }

    public class JsTreeAttribute
    {
        public int id;
        public string ifPerson = "N"; //初始化都标识不是人员节点
    }

    public class JsTreeLeafModel //叶子节点
    {
        public string data;
        public JsTreeAttribute attr;
    }

    public class nodedata
    {
        public string title;
        public string icon;
        public data_attr attr;//=new data_attr() ;
    }

    public class data_attr
    {
        public string href = "";
    }
}