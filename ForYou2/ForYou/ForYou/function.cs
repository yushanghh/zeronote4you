using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForYou
{
    class function
    {
        /// <summary>
        /// 将所有分组信息从数据库读出
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> get_group<T>()
        {
            string sql = "select * from t_group";
            sqlite_operation.operation ope = new sqlite_operation.operation();
            return ope.ExecuteList<T>(sql);
        }

       /// <summary>
       /// 删除分组
       /// </summary>
       /// <param name="group_id">待删除的分组id</param>
        public static void delete_group(int group_id)
        {
            string sql = "delete from t_group where group_id='"+group_id+"'";
            sqlite_operation.operation ope = new sqlite_operation.operation();
            ope.ExecuteNonQuery(sql);
        }
        /// <summary>
        /// 修改分组名
        /// </summary>
        /// <param name="group_id">分组id</param>
        /// <param name="new_name">修改后的组名</param>
        public static void modify_groupname(int group_id,string new_name)
        {
            string sql = "update t_group set group_name='"+new_name+ "' where group_id=" + group_id+"";
            sqlite_operation.operation ope = new sqlite_operation.operation();
            ope.ExecuteNonQuery(sql);
        }
        /// <summary>
        /// 新建分组
        /// </summary>
        /// <param name="group_name">组名</param>
        public static void insert_group(string group_name)
        {
            string sql = "insert into t_group(group_name)values('"+group_name+"')";
            sqlite_operation.operation ope = new sqlite_operation.operation();
            ope.ExecuteNonQuery(sql);
        }
        /// <summary>
        /// 插入笔记
        /// </summary>
        /// <param name="group_id">组号</param>
        /// <param name="html_path">html路径</param>
        public static void insert_note(int group_id)
        {
            string sql = "insert into t_note(group_id,modify_time)values("+group_id+",'"+DateTime.Now.ToString()+"')";
            sqlite_operation.operation ope = new sqlite_operation.operation();
            ope.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 将数据库中所有笔记信息取出(未测试)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="group_id">得到所处于哪个组</param>
        /// <returns></returns>
        public static List<T> get_note<T>(int group_id)
        {
            string sql = "select * from t_note where group_id="+group_id+"";
            sqlite_operation.operation ope = new sqlite_operation.operation();
            return ope.ExecuteList<T>(sql);
        }
        /// <summary>
        /// 修改笔记的内容，修改的时候将最新的修改时间写入数据库(未测试)
        /// </summary>
        /// <param name="group_id">修改的笔记所在组的id</param>
        /// <param name="note_id">修改的笔记id</param>
        /// <param name="html_path">html的路径</param>
        /// <param name="new_path">修改之后的图片路径</param>
        public static void modify_note(int group_id,int note_id,string html_path,List<string> new_path)
        {
            //先删除原来图片路径数据库中跟这个笔记有关的所有图片记录，然后将新的图片路径写入到数据库
            string delete_sql = "delete from t_picture where group_id="+group_id+" and note_id="+note_id+"";
            sqlite_operation.operation ope = new sqlite_operation.operation();
            ope.ExecuteNonQuery(delete_sql);

            //现在不知道怎么获取当前时间，以后再说
            string time_sql = "update t_note set modify_time='"+ DateTime.Now.ToString() + "',note_html='"+html_path+"' where group_id="+group_id+" and note_id="+note_id+"";
            ope.ExecuteNonQuery(time_sql);

            foreach(string item in new_path)
            {
                string insert_sql = "insert into t_picture(pic_path,group_id,note_id) values('"+item+"',"+group_id+","+note_id+")";
                ope.ExecuteNonQuery(insert_sql);
            }
        }
        /// <summary>
        /// 删除笔记
        /// </summary>
        /// <param name="group_id">笔记所在分组的组号</param>
        /// <param name="note_id">笔记id</param>
        public static void delete_note(int group_id,int note_id)
        {
            string delete_sql = "delete from t_note where group_id="+group_id+" and note_id="+note_id+"";
            sqlite_operation.operation ope = new sqlite_operation.operation();
            ope.ExecuteNonQuery(delete_sql);
            delete_sql = "delete from t_picture where group_id="+group_id+" and note_id="+note_id+"";
            ope.ExecuteNonQuery(delete_sql);
        }
        /// <summary>
        /// 得到最大的id
        /// </summary>
        /// <param name="type">group_id还是note_id</param>
        /// <returns></returns>
        public static int get_maxid(int type)
        {
            int id = 0;
            string sql = "";
            sqlite_operation.operation ope = null;
            if (type == 0)
            {
                sql = "select max(group_id) from t_group";
                ope = new sqlite_operation.operation();
                id=(int)ope.get_id(sql);
                return id;
            }
            else
            {
                sql = "select max(note_id) from t_note";
                ope = new sqlite_operation.operation();
                id = (int)ope.get_id(sql);
                return id;
            }
        }
        /// <summary>
        /// 修改笔记标题
        /// </summary>
        /// <param name="group_id">组号</param>
        /// <param name="note_id">笔记号</param>
        /// <param name="note_name">笔记标题</param>
        public static void modify_notename(int group_id,int note_id,string note_name)
        {
            string sql = "update t_note set note_name='"+note_name+"' where group_id="+group_id+" and note_id="+note_id+"";
            sqlite_operation.operation ope = new sqlite_operation.operation();
            ope.ExecuteNonQuery(sql);
        }
        /// <summary>
        /// 获得图片
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="group_id"></param>
        /// <param name="note_id"></param>
        /// <returns></returns>
        public static List<T> get_pic<T>(int group_id,int note_id)
        {
            string sql = "select * from t_picture where group_id="+group_id+" and note_id="+note_id+"";
            sqlite_operation.operation ope = new sqlite_operation.operation();
            return ope.ExecuteList<T>(sql);
        }
    }
}
