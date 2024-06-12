
using Newtonsoft.Json.Linq;

using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using Vit.Db.Util.Data;

namespace Vit.Orm.Dapper.Orm
{
    /// <summary>
    /// SqlServer
    /// </summary>
    public class TinyOrm
    {

        public static Func<System.Data.IDbConnection> DefaultCreator { get; set; } = ConnectionFactory.GetConnectionCreator("App.Db");

        public static Func<SqlConnection> CreateConn
            //=    () => new SqlConnection(System.Configuration.ConfigurationManager.AppSettings[0]); 

            //"App.Db"
            = () => (DefaultCreator() as SqlConnection);


        #region Insert
        /// <summary>
        /// 把数据模型插入到数据库表中
        /// <para> 返回值：新插入数据自增列的值</para>
        /// <para> 若数据模型中没有有效数据则抛异常 </para>
        /// <para> 会清空所有数据库参数 </para>
        /// </summary>    
        /// <param name="model"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static int Insert(JObject model, String tableName)
        {

            using (var conn = CreateConn())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;

                if (ConnectionFactory.CommandTimeout.HasValue)
                    cmd.CommandTimeout = ConnectionFactory.CommandTimeout.Value;

                StringBuilder builder1 = new StringBuilder(" insert into [").Append(tableName).Append("] ( [");
                StringBuilder builder2 = new StringBuilder("@");


                String fieldName;

                Object value;
                foreach (var entry in model)
                {
                    if (null != (value = entry.Value.Value<string>()))
                    {
                        builder1.Append(fieldName = entry.Key).Append("],[");
                        builder2.Append(fieldName).Append(",@");
                        cmd.Parameters.AddWithValue(fieldName, value);
                    }
                }
                if (builder2.Length < 2)
                {
                    throw new Exception("没有传入有效数据");
                }

                builder1.Length -= 2;
                builder2.Length -= 2;
                builder1.Append(") values(").Append(builder2).Append(");select convert(int,isnull(SCOPE_IDENTITY(),-1));");

                cmd.CommandText = builder1.ToString();
                return (int)cmd.ExecuteScalar();
            }
        }

        #endregion


        #region Update
        /// <summary>
        /// 把数据模型中的数据更新到数据库。
        /// <para> 返回值：数据库表中受影响的行数</para>
        /// <para> 若数据模型中没有有效数据则抛异常 </para>
        /// <para> 会清空所有数据库参数 </para>
        /// </summary>
        /// <param name="model"></param>
        /// <param name="tableName"></param>
        /// <param name="sqlWhere">如 " and id=5"</param>
        /// <returns></returns>
        public static int UpdateByWhere(JObject model, String tableName, string sqlWhere)
        {

            using (var conn = CreateConn())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                if (ConnectionFactory.CommandTimeout.HasValue)
                    cmd.CommandTimeout = ConnectionFactory.CommandTimeout.Value;


                StringBuilder builder = new StringBuilder(" update [").Append(tableName).Append("] set [");


                String fieldName;

                Object value;
                foreach (var entry in model)
                {
                    if (null != (value = entry.Value.Value<string>()))
                    {
                        builder.Append(fieldName = entry.Key).Append("]= @").Append(fieldName).Append(",[");
                        cmd.Parameters.AddWithValue(fieldName, value);
                    }
                }

                if (',' != builder[builder.Length - 2])
                {
                    throw new Exception("没有传入有效数据");
                }

                builder.Length -= 2;
                builder.Append(" where  1=1 ").Append(sqlWhere);
                cmd.CommandText = builder.ToString();
                return cmd.ExecuteNonQuery();

            }
        }
        #endregion


        /// <summary>
        /// 执行查询，返回结果集所有数据。
        /// </summary>
        /// <param name="strSql">sql语句或存储过程名称</param>
        /// <returns></returns>
        public static DataSet ExecuteDataSet(string strSql)
        {
            using (var conn = CreateConn())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                if (ConnectionFactory.CommandTimeout.HasValue)
                    cmd.CommandTimeout = ConnectionFactory.CommandTimeout.Value;

                cmd.Connection = conn;
                cmd.CommandText = strSql;
                DataSet myDs = new DataSet();
                using (SqlDataAdapter Adapter = new SqlDataAdapter(cmd))
                {
                    Adapter.Fill(myDs);
                }
                return myDs;
            }
        }

    }
}
