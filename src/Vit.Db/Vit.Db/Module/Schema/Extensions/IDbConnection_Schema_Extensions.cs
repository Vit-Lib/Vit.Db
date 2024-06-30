using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using Vit.Db.Module.Schema;
using Vit.Db.Util.Data;

namespace Vit.Extensions.Db_Extensions
{

    public static partial class IDbConnection_Schema_Extensions
    {

        #region GetAllTableName
        /// <summary>
        /// get all table names
        /// </summary>
        /// <param name="conn"></param>
        /// <returns></returns>
        public static List<string> GetAllTableName(this IDbConnection conn)
        {
            switch (conn.GetDbType())
            {
                case EDbType.SqlServer: return conn.MsSql_GetAllTableName();
                case EDbType.MySql: return conn.MySql_GetAllTableName();
                case EDbType.Sqlite: return conn.Sqlite_GetAllTableName();
            }

            throw new NotImplementedException($"NotImplementedException from IDbConnection.{nameof(GetAllTableName)} in {nameof(IDbConnection_Schema_Extensions)}.cs");

        }
        #endregion


        #region GetAllData
        /// <summary>
        /// get all data from all tables
        /// </summary>
        /// <param name="conn"></param>
        /// <returns></returns>
        public static DataSet GetAllData(this IDbConnection conn)
        {
            var tableNames = conn.GetAllTableName();
            var sql = "select * from " + String.Join(";select * from ", tableNames.Select(conn.Quote)) + ";";
            var ds = conn.ExecuteDataSet(sql);
            for (int t = 0; t < tableNames.Count; t++)
            {
                ds.Tables[t].TableName = tableNames[t];
            }
            return ds;
        }
        #endregion


        #region GetSchema
        /// <summary>
        /// get database table schema infos
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="tableNames">will get all table schema if null </param>
        /// <returns></returns>
        public static List<TableSchema> GetSchema(this IDbConnection conn, IEnumerable<string> tableNames = null)
        {
            switch (conn.GetDbType())
            {
                case EDbType.SqlServer: return conn.MsSql_GetSchema();
                case EDbType.MySql: return conn.MySql_GetSchema();
                case EDbType.Sqlite: return conn.Sqlite_GetSchema();
            }

            throw new NotImplementedException($"NotImplementedException from IDbConnection.{nameof(GetSchema)} in {nameof(IDbConnection_Schema_Extensions)}.cs");

        }
        #endregion


    }
}
