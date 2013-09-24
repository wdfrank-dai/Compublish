using System;
using System.Collections;
using System.Collections.Generic;
using UserGovern.Models;

namespace UserGovern.Repository
{
    public interface IAuthUserRepository :IDisposable
    {
        //获取用户信息
        IEnumerable<AuthUser> GetAllUserFromAuth();
        IEnumerable<roleusershowmodel> GetAllUserRoleInfoFromAuth(string roleid);
        IEnumerable<AuthUser> GetAllUserFromAuthByDepNO(string depno);
        IEnumerable<AuthUser> GetAllUserFromAuthByType(string typeid);
        IEnumerable<AuthUser> GetAllUserFromAuthByRole(int roleid);
        //AuthUser GetUserFromAuthByUserID(string userid);
        //AuthUser GetUserFromAuthByAccountNo(int accountno);
        //AuthUser GetUserFromAuthByIdentify(string identify);
        //AuthUser GetUserFromAuthByTelephone(string telephone);
        //AuthUser GetUserFromAuthByUserIDandPassword(string userid, string password);

        //基本操作
        
        //string UpdateUserInfo(AuthUserEditModel userinfo);
        //string ChangeAuthUserPassword(string userid, string newpassword);

    }
}
