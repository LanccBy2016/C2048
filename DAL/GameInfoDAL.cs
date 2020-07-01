using Common;
using Dapper;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class GameInfoDAL
    {
        /// <summary>
        /// 添加游戏记录
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        public int AddGameLog(UserGameLog log)
        {
            string sql = @"insert into UserScoreLog values(@UserID,@Score,@CreateTime) ";
            using (var conn = DataAccess.ConnectionFactory.GetOpen("c2048"))
            {
                return conn.Execute(sql,log);
            }
        }

        /// <summary>
        /// 返回最高得分
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public int GetMaxScore(string UserID)
        {
            string sql = "select max(Score) from UserScoreLog where UserID=@UserID ";

            try
            {
                using (var conn = DataAccess.ConnectionFactory.GetOpen("c2048"))
                {
                    return conn.Query<int>(sql, new { UserID = UserID }).FirstOrDefault();
                }
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 返回得分排行榜
        /// </summary>
        /// <returns></returns>
        public List<UserGameLog> GetRankingList()
        {
            string sql = "";

            if (WebConfigOperation.DBType == 1)
            {
                sql = "SELECT TOP 20 * FROM [UserScoreLog] ORDER BY Score DESC,CreateTime DESC";
            }
            else
            {
                sql = "SELECT * FROM `UserScoreLog` ORDER BY Score DESC,CreateTime DESC LIMIT 20";
            }
            using (var conn = DataAccess.ConnectionFactory.GetOpen("c2048"))
            {
                return conn.Query<UserGameLog>(sql).ToList();
            }
        }
    }
}
