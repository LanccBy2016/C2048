using Common;
using DAL;
using Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    /// <summary>
    /// 登陆相关
    /// </summary>
    public class LoginBLL
    {
        
        /// <summary>
        /// 登陆/注册
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="pwd"></param>
        /// <returns>用户对象</returns>
        public UserInfo LoginIn(string uid, string pwd)
        {
            //用户密码加密保存
            pwd = EncryptAndDecrypt.Encrypt(pwd);

            var dal = new UserDal();

            var tryuser=dal.ExistUser(uid);
            if (tryuser == null)
            {
                //若尝试登录的用户ID不存在,则直接新增该用户
                dal.AddUser(new UserInfo { UserID = uid, Pwd = pwd });
            }

            var user= new UserDal().GetUser(uid, pwd);
            if (user !=null)
            {
                SetLoginCookie(user);
            }
            return user;
        }

        /// <summary>
        /// 返回用户信息
        /// </summary>
        /// <param name="userId">用户id</param>
        ///  <param name="userType">用户类型</param>
        /// <returns></returns>
        public UserInfo GetUserInfo(string CookieStr)
        {
            var userInfo = new UserInfo();

            try { 
                var JsonStr= EncryptAndDecrypt.Decrypt(CookieStr);
                userInfo = JsonConvert.DeserializeObject<UserInfo>(JsonStr);
            }
            catch (Exception ex)
            {
                return null;
            }
            return userInfo;
        }


        /// <summary>
        /// 设置登陆cookie
        /// </summary>
        /// <param name="user"></param>
        public void SetLoginCookie(UserInfo user)
        {
            var userInfo=JsonConvert.SerializeObject(new { user.UserID, user.CreateTime });
            var cookieStr = EncryptAndDecrypt.Encrypt(userInfo);
            CookiesHelper.AddCookie(WebConfigOperation.CookieName, cookieStr);
            CookiesHelper.SetCookie(WebConfigOperation.CookieName, DateTime.Now.AddMonths(1));
        }

    }
}
