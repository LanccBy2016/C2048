using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class ConnectionFactory
    {

        /// <summary>
        /// 获取自适应数据库类型的连接对象
        /// </summary>
        /// <param name="linkName">连接字符串名</param>
        /// <param name="DBType">1:MSSql 2:MySql</param>
        /// <returns></returns>
        public static DbConnection GetOpen(string linkName,int DBType=0)
        {
            DBType = DBType == 0 ? int.Parse(ConfigurationManager.AppSettings.Get("DBType")) : DBType;

            if (DBType == 1)
            {
                return MSSQL_GetOpen(linkName);
            }
            else if (DBType == 2)
            {
                return MySQL_GetOpen(linkName);
            }
            return null;
        }


        //返回MSSQL的连接对象
        public static MySqlConnection MySQL_GetOpen(string linkName)
        {
            try
            {
                var connectionString = ConfigurationManager.ConnectionStrings[linkName].ConnectionString;
                var connection = new MySqlConnection(connectionString);
                connection.Open();
                return connection;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 返回MySQL的连接对象
        /// </summary>
        /// <param name="linkName"></param>
        /// <returns></returns>
        public static SqlConnection MSSQL_GetOpen(string linkName)
        {
            try
            {
                var connectionString = ConfigurationManager.ConnectionStrings[linkName].ConnectionString;
                var connection = new SqlConnection(connectionString);
                connection.Open();
                return connection;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
