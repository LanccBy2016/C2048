using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class GameInfo
    {
        /// <summary>
        /// 游戏得分
        /// </summary>
        public int Score { get; set; }
        /// <summary>
        /// 游戏状态
        /// </summary>
        public int[,] Data;
        /// <summary>
        /// 游戏是否结束
        /// </summary>
        public bool IsOver;
        /// <summary>
        /// 游戏是否提交
        /// </summary>
        public bool IsSubmit;
        public GameInfo()
        {
            this.Data = new int[,] { { 0,0,0,0}, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } };
            this.IsOver = false;
            this.IsSubmit = false;
        }
    }
}
