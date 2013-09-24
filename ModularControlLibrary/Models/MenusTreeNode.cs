using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ModularControl.Repository;

namespace ModularControl.Models
{


    public class MenuNodeModel
    {
        public nodedata data;
        public NodeAttribute attr;
        public string state;
        public List<MenuNodeModel> children;
    }

    public class NodeAttribute
    {
        public string id;
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

    //Telerik横向菜单
    public class AttendMenus
    {
        public enum ShowIcon { 排版查询与管理 = 0, 班次调整管理 = 1, 考勤登记与管理 = 2, 班车查询与管理 = 3, 校历设定管理 = 4, 节假日放假方案设定 = 5, 班次设定管理 = 6, 人员排班管理 = 7, 考勤点设定管理 = 8, 班次查询 = 9, 班次对调 = 10, 顶班处理 = 11, 休息调整 = 12, 差旅假加班补勤登记 = 13, 差旅假加班补勤审核 = 14, 差旅假销假处理 = 15, 差旅假销假历史数据 = 16, 考勤汇总信息生成与查询 = 17, 班车登记管理 = 18, 班车排班与乘车人员管理 = 19, 班车误点处理 = 20, 班车线路查询 = 21 };
        public string[] IconArray = { "Suites/sl.png", "Suites/ajax.png", "Suites/orm.png", "Suites/rep.png", "Mvc/Grid.png", "Mvc/Menu.png", "Mvc/PanelBar.png", "Mvc/TabStrip.png", "Mvc/Grid.png", "Mvc/PanelBar.png", "Ajax/Editor.png", "Ajax/Grid.png", "Ajax/Scheduler.png", "Sl/Chart.gif", "Sl/Docking.gif", "Sl/GridView.gif", "Sl/Scheduler.gif", "Sl/Chart.gif", "Mvc/Grid.png", "Mvc/Menu.png", "Mvc/PanelBar.png", "Mvc/TabStrip.png" };

        public IEnumerable<MenuNodeModel> GetAttendMenu(string UserID, string SystemID)
        {
            if (UserID == null || SystemID == null)
                return null;
            string dNo = SystemID;

            var menus = ModuleRepository.GetMenusByUserID(UserID);

            //var ms = menus.Where(m => m.IsDelete == false);
            var ms = menus;

            List<MenuNodeModel> nodes = new List<MenuNodeModel>();
            if (ms != null)
            {

                //------------------

                var rs = ms.Where(c => c.ModuleParentID.ToString() == dNo).ToList();

                foreach (var d in rs)
                {

                    // create a new node
                    MenuNodeModel t = new MenuNodeModel();
                    t.attr = new NodeAttribute();
                    t.attr.id = d.ModuleID.ToString();
                    t.data = new nodedata();
                    t.data.title = d.ModuleName;
                    if (d.RouteName != null && d.RouteName != "")
                    {
                        if (ShowIcon.IsDefined(typeof(ShowIcon), t.data.title))
                        {
                            int iconno = (int)ShowIcon.Parse(typeof(ShowIcon), t.data.title);
                            t.data.icon = IconArray[iconno];
                        }
                        else
                            t.data.icon = "Suites/mvc.png";
                        t.data.attr = new data_attr();
                        t.data.attr.href = d.RouteName;//Url.Content("~/Menus/Dispatch?routeName=") + 
                        t.children = null;
                    }
                    else
                    {
                        if (ShowIcon.IsDefined(typeof(ShowIcon), t.data.title))
                        {
                            int iconno = (int)ShowIcon.Parse(typeof(ShowIcon), t.data.title);
                            t.data.icon = IconArray[iconno];
                        }
                        else
                            t.data.icon = "Suites/mvc.png";
                        t.children = new List<MenuNodeModel>();
                        PopulateTree(d.ModuleID.ToString(), t, ms);
                        t.state = "closed";
                    }
                    nodes.Add(t);
                }

                return nodes;
            }
            else
            {
                return null;
            }
        }

        public void PopulateTree(string dNo, MenuNodeModel node, IEnumerable<Module> ms)
        {
            var rs1 = ms.Where(c => c.ModuleParentID.ToString() == dNo).ToList();

            foreach (var d in rs1)
            {
                MenuNodeModel t = new MenuNodeModel();
                t.attr = new NodeAttribute();
                t.attr.id = d.ModuleID.ToString();
                t.data = new nodedata();
                t.data.title = d.ModuleName;
                if (ShowIcon.IsDefined(typeof(ShowIcon), t.data.title))
                {
                    int iconno = (int)ShowIcon.Parse(typeof(ShowIcon), t.data.title);
                    t.data.icon = IconArray[iconno];
                }
                else
                    t.data.icon = "Suites/mvc.png";
                t.children = null;

                PopulateTree(d.ModuleID.ToString(), t, ms);

                if (t.children == null)
                {
                    //t.data.icon = "/Scripts/plusIns/jsTree/themes/classic/TreeLeaf.png";
                    if (d.RouteName != "" && d.RouteName != null)
                    {
                        t.data.attr = new data_attr();
                        t.data.attr.href = d.RouteName;
                    }
                    t.state = "close";
                }
                else
                {
                    //t.data.icon = "/Scripts/plusIns/jsTree/themes/classic/TreeMenu.png";
                }

                if (node.children == null)
                    node.children = new List<MenuNodeModel>();
                node.children.Add(t); // add the node to the "master" node

            }
        }
    }

}