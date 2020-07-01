using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Common
{

    /// <summary>
    /// 批量入库
    /// </summary>
    public static class BulkToDBHelper
    {
        public static int BulkToDB(string tableName, DataTable dt, SqlConnection conn)
        {
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(conn))
            {
                bulkCopy.DestinationTableName = tableName;
                bulkCopy.BatchSize = dt.Rows.Count;
                if (dt != null && dt.Rows.Count != 0)
                {
                    try
                    {
                        bulkCopy.WriteToServer(dt);
                    }
                    catch
                    {
                        throw;
                    }

                }
                return dt.Rows.Count;
            }
        }

        public static DataTable ToDataTableTow(IList list)
        {
            DataTable result = new DataTable();
            if (list.Count > 0)
            {
                PropertyInfo[] propertys = list[0].GetType().GetProperties();

                foreach (PropertyInfo pi in propertys)
                {
                    result.Columns.Add(pi.Name, pi.PropertyType);
                }
                for (int i = 0; i < list.Count; i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in propertys)
                    {
                        object obj = pi.GetValue(list[i], null);
                        tempList.Add(obj);
                    }
                    object[] array = tempList.ToArray();
                    result.LoadDataRow(array, true);
                }
            }
            return result;
        }

        public static int BulkToDB(string tableName, DataTable dt, SqlConnection conn, SqlTransaction transaction)
        {
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(conn, SqlBulkCopyOptions.Default, transaction))
            {
                bulkCopy.DestinationTableName = tableName;
                bulkCopy.BatchSize = dt.Rows.Count / 10;
                bulkCopy.BulkCopyTimeout = 600;
                if (dt != null && dt.Rows.Count != 0)
                {
                    try
                    {
                        bulkCopy.WriteToServer(dt);
                    }
                    catch
                    {
                        throw;
                    }

                }
                return dt.Rows.Count;
            }
        }
    }
}
