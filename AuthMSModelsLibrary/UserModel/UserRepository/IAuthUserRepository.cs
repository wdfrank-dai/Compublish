using System;
using System.Collections;
using System.Collections.Generic;
using AuthUserDataLibrary.Models;

namespace AuthUserDataLibrary.Repository
{
    public interface IAuthUserRepository : IDisposable
    {
        //获取用户信息
        IEnumerable<AuthUserM> GetAllUserFromAuth();
        //IEnumerable<AuthUserM> GetAllUserFromAuthByDepNO(string depno);
        //IEnumerable<AuthUserM> GetAllUserFromAuthByType(string typeid);
        //IEnumerable<AuthUserM> GetAllUserFromAuthByRole(int roleid);
        //AuthUserM GetUserFromAuthByUserID(string userid);
        //AuthUserM GetUserFromAuthByAccountNo(int accountno);
        //AuthUserM GetUserFromAuthByIdentify(string identify);
        //AuthUserM GetUserFromAuthByTelephone(string telephone);
        IEnumerable<AuthUserM> GetUserFromAuthByUserIDandPassword(string userid, string password);


        //基本操作
        string UpdateUserInfo(AuthUserEditModel userinfo);
        string ChangeAuthUserPassword(string userid, string newpassword);

    }
}
