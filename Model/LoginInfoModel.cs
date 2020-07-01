using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class LoginInfoModel
    {
        /// <summary>
        /// 登陆的用户对象
        /// </summary>
        public UserInfo UserInfo { get; set; }

        /// <summary>
        /// 游客Cookie
        /// </summary>
        public string ykCookie { get; set; }
    }
}
