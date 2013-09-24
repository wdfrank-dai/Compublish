using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AuthMSModelsLibrary;


namespace AuthModelManage.Models.Repository
{
    public class ModuleTreeRepository
    {



        public IQueryable<T_Module> GetAllModule()   //获取T_Module表中IsDelete属性为false的全部数据
        {
            AuthMSLinqDataContext systemdc = new AuthMSLinqDataContext();
            return systemdc.T_Module.Where(o => o.IsDelete == false).OrderBy(o => o.ModulePriority);
        }

        public T_Module GetModuleByID(int id)     //通过传递过来的ID来查找模块的全部信息
        {
            using (AuthMSLinqDataContext systemdc = new AuthMSLinqDataContext())
            {
                try
                {
                    return systemdc.T_Module.FirstOrDefault(o => o.ModuleID == id);
                }
                catch { return null; }

            }
        }


        public T_Module GetModuleByName(string name)    //通过传递过来的名称来查找模块
        {
            using (AuthMSLinqDataContext systemdc = new AuthMSLinqDataContext())
            {
                try { return systemdc.T_Module.Where(o => o.ModuleName == name).FirstOrDefault(); }
                catch { return null; }
            }

        }


        public void GetModuleNodeChildren(string dhid, ModuleTreeModel node)
        {
            AuthMSLinqDataContext db = new AuthMSLinqDataContext();
            if (node.children == null)
            {
                node.children = new List<ModuleTreeModel>();
            }
            var dp = db.T_Module.Where(c => c.ModuleParentID.ToString() == dhid).ToList();//dp为跟节点
            var hid = dhid;//hid为根节点id
            foreach (var d in dp.OrderBy(e => e.ModulePriority).ToList())
            {
                ModuleTreeModel t = new ModuleTreeModel();
                t.attr = new JsTreeAttribute();
                t.attr.id = d.ModuleID;
                t.data = d.ModuleName;
                GetModuleNodeChildren(d.ModuleID.ToString(), t);
                node.children.Add(t); // add the node to the "master" node
            }
        }


        //public void GetModuleNodeChildren(int mid, ModuleTreeModel node)   //获取整棵树的节点的方法
        //{
        //    AuthMSLinqDataContext db = new AuthMSLinqDataContext();
        //    if (node.children == null)
        //    {
        //        node.children = new List<ModuleTreeModel>();
        //    }
        //    var dp = db.T_Module.Where(c => c.ModuleID == mid).FirstOrDefault();
        //    var uid = dp.ModuleID;
        //    var pid = dp.ModuleParentID;
        //    var Mo = db.T_Module.Where(c => c.ModuleParentID == uid).ToList().OrderBy(c=>c.ModulePriority);
        //    foreach (var d in Mo)
        //    {
        //        ModuleTreeModel t = new ModuleTreeModel();
        //        t.attr = new JsTreeAttribute();
        //        t.attr.id = d.ModuleID;
        //        t.data = d.ModuleName;
        //        GetModuleNodeChildren(d.ModuleID, t);

        //        node.children.Add(t);
        //    }
        //}


        public bool AddModule(T_Module m)   //向表中添加一个模块
        {
            using (AuthMSLinqDataContext systemdc = new AuthMSLinqDataContext())
            {
                try
                {
                    if (m.RouteName == null) m.RouteName = "";
                    systemdc.T_Module.InsertOnSubmit(m);
                    systemdc.SubmitChanges();
                    return true;
                }
                catch { return false; }

            }

        }

        public bool Update(T_Module model)   //更新表中的一个模块
        {
            using (AuthMSLinqDataContext systemdc = new AuthMSLinqDataContext())
            {
                try
                {
                    T_Module m = systemdc.T_Module.FirstOrDefault(o => o.ModuleID == model.ModuleID);
                    m.ModuleID = model.ModuleID;
                    m.IsDelete = false;
                    m.IsMenu = model.IsMenu;
                    m.ModuleHierarchy = model.ModuleHierarchy;
                    m.ModuleName = model.ModuleName;
                    m.ModuleNotes = model.ModuleNotes;
                    m.ModuleParentID = model.ModuleParentID;
                    m.ModulePriority = model.ModulePriority;
                    m.OperateDate = model.OperateDate;
                    //m.OperatorID = model.OperatorID;
                    //m.OperatorName = model.OperatorName;
                    if (model.RouteName != null)

                        m.RouteName = model.RouteName;
                    else m.RouteName = "";

                    systemdc.SubmitChanges();
                    return true;
                }
                catch { return false; }

            }
        }



        public bool Delete(int id)  //通过传过来的ID值删除一个模块
        {
            using (AuthMSLinqDataContext systemdc = new AuthMSLinqDataContext())
            {
                try
                {

                    T_Module m = systemdc.T_Module.SingleOrDefault(o => o.ModuleID == id);
                    //  m.IsDelete = true;
                    // var a = systemdc.T_RoleModule.Where(o => o.ModuleID == id);
                    //if (a != null)
                    //{
                    //    systemdc.T_RoleModule.DeleteAllOnSubmit(a);

                    //}
                    systemdc.T_Module.DeleteOnSubmit(m);//////////////////////////真正删除一个模块
                    systemdc.SubmitChanges();
                    return true;
                }
                catch { return false; }
            }
        }


        public Module ModuleTreeAll(int moduleid)  //通过ID值查找模块的信息，返回类型为EditModule类型
        {
            AuthMSLinqDataContext db = new AuthMSLinqDataContext();
            ////var res = new AuthUserDataContext().T_User;
            var re = (from rs_event in db.T_Module.Where(e => e.ModuleID == moduleid)
                      select new Module
                      {
                          ModuleID = rs_event.ModuleID,
                          ModuleParentID = rs_event.ModuleParentID,
                          ModuleName = rs_event.ModuleName,
                          ModuleNotes = rs_event.ModuleNotes,
                          ModuleHierarchy = rs_event.ModuleHierarchy,
                          ModulePriority = rs_event.ModulePriority,
                          RouteName = rs_event.RouteName,
                          IsMenu = rs_event.IsMenu,
                          IsDelete = rs_event.IsDelete,
                          OperatorID = rs_event.OperatorID,
                          OperatorName = rs_event.OperatorName,
                          OperateDate = rs_event.OperateDate
                      });
            return re.FirstOrDefault();
        }

    }
}