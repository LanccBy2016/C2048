using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class UserGameLog
    {
        public string UserID { get; set; }
        public int Score { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
