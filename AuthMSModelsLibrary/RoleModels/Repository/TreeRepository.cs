using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Data.Linq;
using UserGovern.Models;
using AuthMSModelsLibrary;

namespace UserGovern.Repository
{
    public class TreeRepository
    {
        AuthMSLinqDataContext db = new AuthMSLinqDataContext();

        //public void GetRoleTree(string dhid, RoleTreeModel node)
        //{
        //    if (node.children == null)
        //    {
        //        node.children = new List<RoleTreeModel>();
        //    }
        //    var dp = db.T_Role.Where(c => c.RoleParentID.ToString() == dhid).FirstOrDefault();
        //    var hid = dp.RoleID; //hid为跟节点id
        //    foreach (var p in db.T_Role.Where(e => e.RoleParentID == hid).OrderBy(e => e.RoleName).ToList())
        //    {
        //        RoleTreeModel t = new RoleTreeModel();
        //        t.attr = new JsTreeAttribute();
        //        t.attr.id = p.RoleID;
        //        t.attr.ifPerson = "Y"; //表明是人员节点
        //        t.data = p.RoleName;
        //        t.state = "close";
        //        t.children = null;
        //        node.children.Add(t);
        //    }
        //}

        public void GetRoleTree(string dhid, RoleTreeModel node)
        {
            if (node.children == null)
            {
                node.children = new List<RoleTreeModel>();
            }
            var dp = db.T_Role.Where(c => c.RoleParentID.ToString() == dhid).ToList();//dp为跟节点
            var hid = dhid;//hid为根节点id
            foreach (var d in dp.OrderBy(e => e.RoleName).ToList())
            {
                RoleTreeModel t = new RoleTreeModel();
                t.attr = new JsTreeAttribute();
                t.attr.id = d.RoleID;
                t.data = d.RoleName;
                GetRoleTree(d.RoleID.ToString(), t);
                node.children.Add(t); // add the node to the "master" node
            }
        }

        //角色树
        public IQueryable<T_Role> GetAllRole()
        {
            AuthMSLinqDataContext systemdc = new AuthMSLinqDataContext();
            return systemdc.T_Role;
        }

        //模块树
        public IQueryable<T_Module> GetAllModule()
        {
            AuthMSLinqDataContext systemdc = new AuthMSLinqDataContext();
            return systemdc.T_Module.Where(o => o.IsDelete == false).OrderBy(o => o.ModulePriority);
        }

        //显示选择角色的All
        public static IList<roleusershowmodel> RoleTreeAll(string[] checkedRecords)
        {
            AuthMSLinqDataContext db = new AuthMSLinqDataContext();
            checkedRecords = checkedRecords ?? new string[] { };
            var plist = checkedRecords.ToList();

            IList<roleusershowmodel> rs =
           (from rs_event in db.T_Role
            select new roleusershowmodel
            {
                RoleID = rs_event.RoleID,
                RoleName = rs_event.RoleName,
                RoleParentID = rs_event.RoleParentID,
                IsChildrenRole = rs_event.IsChildrenRole,
                RoleFunctionNotes = rs_event.RoleFunctionNotes,

            }).Where(o => checkedRecords.Contains(o.RoleID.ToString())).ToList();

            var result = (from re in plist join ps in rs on re equals ps.RoleID.ToString() select ps).ToList();

            return result;
        }
        //提取用户信息
        public IEnumerable<AuthUser> UserTreeAll(IEnumerable<string> roleid)
        {
            AuthMSLinqDataContext db = new AuthMSLinqDataContext();
            var plist = roleid.ToList();
            var role = db.T_Role.Where(r => r.RoleID.ToString() == plist.ToString());
            try
            {
                IList<AuthUser> rs = (from rs_event in db.T_User
                                      select new AuthUser
                                      {
                                          UserID = rs_event.UserID,
                                          UserName = rs_event.UserName,
                                          DepNO = (from dep in db.T_Department where dep.DepNO == rs_event.DepNO select dep.DepName).FirstOrDefault(),
                                          InRole = roleid.Contains(rs_event.UserID) ? true : false
                                      }).Where(o => roleid.Contains(o.UserID.ToString())).ToList();
                return rs;
            }
            catch {
                return null;
            }
        }

    }
}