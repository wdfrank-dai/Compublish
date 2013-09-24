using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserGovern.Models
{
        [Serializable]
        public class RoleTreeModel
        {
            public string data;
            public JsTreeAttribute attr;
            public string state;
            public List<RoleTreeModel> children;
        }

        [Serializable]
        public class JsTreeAttribute
        {
            public int id;
            public string ifPerson = "N"; //初始化都标识不是人员节点
        }
        [Serializable]
        public class nodedata
        {
            public string title;
            public string icon;
            public data_attr attr;
        }
        [Serializable]
        public class data_attr
        {
            public string href = "";
        }
}