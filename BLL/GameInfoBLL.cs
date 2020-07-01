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
    public class GameInfoBLL
    {

        /// <summary>
        /// 添加游戏记录
        /// </summary>
        /// <param name="user"></param>
        /// <param name="Score"></param>
        /// <returns></returns>
        public bool AddGameLog(UserInfo user, int Score)
        {
            UserGameLog log = new UserGameLog { UserID = user.UserID, Score = Score,CreateTime=DateTime.Now };
            int cmd = new GameInfoDAL().AddGameLog(log);
            if (cmd == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 获取用户最高分
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int GetMaxScore(UserInfo user)
        {
            return new GameInfoDAL().GetMaxScore(user.UserID);
        }

        /// <summary>
        /// 获取积分排行榜
        /// </summary>
        /// <returns></returns>
        public List<UserGameLog> GetRankingList()
        {
            return new GameInfoDAL().GetRankingList();
        }

        /// <summary>
        /// 向上移动
        /// </summary>
        /// <param name="game"></param>
        public void MoveUP(ref GameInfo game)
        {
            var flag = false;
            for (var i = 0; i < 3; i++)
            {
                for (var j = 0; j <= 3; j++)
                {
                    var a = game.Data[i,j];
                    var b = game.Data[i + 1,j];
                    if (b != 0 && (a == b || a == 0))
                    {
                        game.Data[i,j]= a + b;
                        game.Data[i+1, j]= 0;
                        flag = true;
                        game.Score += a == b ? b : 0;
                    }
                }
            }
            if (flag)
            {
                MoveUP(ref game);
            }
        }

        /// <summary>
        /// 向下移动
        /// </summary>
        /// <param name="game"></param>
        public void MoveDown(ref GameInfo game)
        {
            var flag = false;
            for (var i = 3; i > 0; i--)
            {
                for (var j = 0; j <= 3; j++)
                {
                    var a = game.Data[i, j];
                    var b = game.Data[i - 1, j];
                    if (b != 0 && (a == b || a == 0))
                    {
                        game.Data[i,j] = a + b;
                        game.Data[i - 1,j] = 0;
                        flag = true;
                        game.Score += a == b ? b : 0;
                    }
                }
            }
            if (flag)
            {
                MoveDown(ref game);
            }
        }

        /// <summary>
        /// 向左移动
        /// </summary>
        /// <param name="game"></param>
        public void MoveLeft(ref GameInfo game)
        {
            var flag = false;
            for (var i = 0; i <= 3; i++)
            {
                for (var j = 0; j < 3; j++)
                {

                    var a = game.Data[i, j];
                    var b = game.Data[i, j + 1];
                    if (b != 0 && (a == b || a == 0))
                    {
                        game.Data[i,j] = a + b;
                        game.Data[i,j + 1] = 0;
                        flag = true;
                        game.Score += a == b ? b : 0;
                    }
                }
            }
            if (flag)
            {
                MoveLeft(ref game);
            }
        }

        /// <summary>
        /// 向右移动
        /// </summary>
        /// <param name="game"></param>
        public void MoveRight(ref GameInfo game)
        {
            var flag = false;
            for (var i = 0; i <= 3; i++)
            {
                for (var j = 3; j > 0; j--)
                {
                    var a = game.Data[i, j];
                    var b = game.Data[i, j - 1];
                    if (b != 0 && (a == b || a == 0))
                    {
                        game.Data[i,j] = a + b;
                        game.Data[i,j - 1] = 0;
                        flag = true;
                        game.Score += a == b ? b : 0;
                    }
                }
            }
            if (flag)
            {
                MoveRight(ref game);
            }
        }


        /// <summary>
        /// 随机生成新的种子
        /// </summary>
        /// <param name="game"></param>
        public void RefNum(ref GameInfo game)
        {
            //存储所有空白格子信息(key为编号，value为(X,Y)坐标)
            Dictionary<int, List<int>> map = new Dictionary<int, List<int>>();
            int k = 0;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (game.Data[i, j] == 0)
                    {
                        map.Add(k, new List<int> { i, j });
                        k++;
                    }
                }
            }            
            Random rd = new Random();
            //随机抽取一个空格子
            var map_index = rd.Next(0, map.Count);

            //生成2或4(先生成一个随机数，奇数返回4，偶数返回2)
            int num = rd.Next(0, 100);
            num = (num % 2)*2+2;
            game.Data[map[map_index][0], map[map_index][1]] = num;
        }

        /// <summary>
        /// 保存游戏数据
        /// </summary>
        /// <param name="game"></param>
        public void SetGameData(string key,GameInfo game)
        {
            //IIS缓存
            CacheExts<GameInfo>.SetValue(key, game,noSlidingExpiration:false);

            //客户端持久化存储
            var json = JsonConvert.SerializeObject(game);
            var cookieStr = EncryptAndDecrypt.Encrypt(json);
            CookiesHelper.AddCookie(key,cookieStr);
            CookiesHelper.SetCookie(key, DateTime.Now.AddMonths(1));
        }

        /// <summary>
        /// 获取用户的游戏状态
        /// </summary>
        /// <param name="key">UserID</param>
        /// <returns></returns>
        public GameInfo GetGameData(string key)
        {
            var data = new GameInfo();
            try
            {
                //首先从服务端缓存获取
                data = CacheExts<GameInfo>.GetValue(key);
                if(data==null)
                { 
                    //缓存不存在，则读取用户本地Cookie解析成游戏对象
                    var cookieStr = CookiesHelper.GetCookieValue(key);
                    var json = EncryptAndDecrypt.Decrypt(cookieStr);
                    data = JsonConvert.DeserializeObject<GameInfo>(json);
                }
            }
            catch{}
            if (data == null)
            {
                //如果都没有，则初始化一个游戏对象
                data = new GameInfo();
                RefNum(ref data);
            }
            return data;
        }

    }
}
