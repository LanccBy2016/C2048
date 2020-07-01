using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace DAL
{
    public class UserDal
    {
        /// <summary>
        /// 新增一个用户
        /// </summary>
        /// <param name="u"></param>
        /// <returns></returns>
        public int AddUser(UserInfo u)
        {
            string sql = @"insert into Users values(@UserID,@Pwd,@Date) ";
            using (var conn = DataAccess.ConnectionFactory.GetOpen("c2048"))
            {
                return conn.Execute(sql, new {u.UserID,u.Pwd, Date = DateTime.Now });
            }
        }

        /// <summary>
        /// 返回用户信息
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public UserInfo GetUser(string uid, string pwd)
        {
            string sql = @"SELECT * FROM Users WHERE UserID=@UserID AND Pwd=@Pwd ";
            using (var conn = DataAccess.ConnectionFactory.GetOpen("c2048"))
            {
                return conn.Query<UserInfo>(sql, new { UserID=uid,Pwd=pwd }).FirstOrDefault();
            }
        }

        /// <summary>
        /// 判断用户是否存在
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public UserInfo ExistUser(string uid)
        {
            string sql = "select * from Users where UserID=@uid";
            using (var conn = DataAccess.ConnectionFactory.GetOpen("c2048"))
            {
                return conn.Query<UserInfo>(sql, new { uid}).FirstOrDefault();
            }
        }
    }
}
