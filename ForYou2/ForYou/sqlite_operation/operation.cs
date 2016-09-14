using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sqlite_operation
{
    public class operation
    {
        /// <summary>
        /// 创建sqlite数据库,记得改路径
        /// </summary>
        /// <returns></returns>
        public SQLiteConnection create_connection()
        {
            try
            {
                string path = this.GetType().Assembly.Location;
                path = System.IO.Path.GetDirectoryName(path);
                SQLiteConnection conn = new SQLiteConnection("Data Source="+path+"//zeronote4you.db");
                return conn;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 创建cmd
        /// </summary>
        /// <param name="commandText">cmd语句</param>
        /// <param name="commandType">类型</param>
        /// <param name="connection">连接</param>
        /// <param name="paramList">参数</param>
        /// <returns></returns>
        public SQLiteCommand CreateCommand(string commandText, SQLiteConnection connection)
        {
            var cmd = new SQLiteCommand(commandText, connection);
            cmd.CommandType = CommandType.Text;
            return cmd;
        }
        /// <summary>
        /// 执行非查询语句
        /// </summary>
        /// <param name="commandText">cmd类型</param>
        /// <param name="paramList">语句</param>
        /// <returns></returns>
        public int ExecuteNonQuery(string commandText)
        {
            try
            {
                var conn = create_connection();
                conn.Open();
                var cmd = CreateCommand(commandText, conn);
                cmd.ExecuteNonQuery();
                if (conn != null)
                {
                    conn.Close();
                }
                return 1;
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 执行SQL语句,得到DataSet
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="paramList"></param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(string commandText)
        {
            try
            {
                var conn = create_connection();
                conn.Open();
                SQLiteCommand cmd = CreateCommand(commandText, conn);
                SQLiteDataAdapter ada = new SQLiteDataAdapter(cmd);
                DataSet ds = new DataSet();
                ada.Fill(ds);
                conn.Close();
                return ds;
            }
            catch(Exception e)
            {
                string s=e.Message;
                return null;
            }
        }

        /// <summary>
        /// 执行SQL语句,得到第一个DataTable
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="paramList"></param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(string commandText)
        {
            var ds = ExecuteDataSet(commandText);
            if (ds != null)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 执行SQL语句,返回数据集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="commandText"></param>
        /// <param name="paramList"></param>
        /// <returns></returns>
        public List<T> ExecuteList<T>(string commandText)
        {
            var dt = ExecuteDataTable(commandText);
            return CollectionHelper.ConvertTo<T>(dt);
        }
        /// <summary>
        /// 
        /// 
        /// 
        /// 执行SQL语句,返回第一个数据,没写完
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="commandText"></param>
        /// <param name="paraList"></param>
        /// <returns></returns>
        //public T ExecuteItem<T>(string commandText, params SQLiteParameter[] paraList)
        //{
        //    var dt = ExecuteDataTable(commandText, paraList);
        //    if (dt.Rows.Count == 0) return default(T);
        //    var dr = dt.Rows[0];
        //    return default<T>;
        //}
        /// <summary>
        /// 没写完
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="paramList"></param>
        /// <returns></returns>
        public object ExecuteScalar(string commandText, params SQLiteParameter[] paramList)
        {
            //using (var conn = CreateConnection())
            //{
            //    conn.Open();
            //    using (var cmd = CreateCommand(commandText, CommandType.Text, conn, paramList))
            //    {
            //        return cmd.ExecuteScalar();
            //    }
            //}
            return null;
        }
        /// <summary>
        /// 返回最新的id
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public System.Int64 get_id(string commandText)
        {
            var conn = create_connection();
            System.Int64 id = 0;
            try {
                conn.Open();
                var cmd = CreateCommand(commandText, conn);
                SQLiteDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    if (reader.Read())
                    {
                        id = reader.GetInt64(0);
                        reader.Close();//记得把reader关掉
                    }
                }
            }
            catch
            {
            }
            if (conn != null)
            {
                conn.Close();
            }
            return id;
        }
    }
}
