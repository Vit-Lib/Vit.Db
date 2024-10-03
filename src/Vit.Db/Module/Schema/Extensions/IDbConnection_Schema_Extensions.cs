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
            return conn.GetDbType() switch
            {
                EDbType.SqlServer => conn.MsSql_GetAllTableName(),
                EDbType.MySql => conn.MySql_GetAllTableName(),
                EDbType.Sqlite => conn.Sqlite_GetAllTableName(),
                _ => throw new NotImplementedException($"NotImplementedException from IDbConnection.{nameof(GetAllTableName)} in {nameof(IDbConnection_Schema_Extensions)}.cs"),
            };
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
            return conn.GetDbType() switch
            {
                EDbType.SqlServer => conn.MsSql_GetSchema(tableNames),
                EDbType.MySql => conn.MySql_GetSchema(tableNames),
                EDbType.Sqlite => conn.Sqlite_GetSchema(tableNames),
                _ => throw new NotImplementedException($"NotImplementedException from IDbConnection.{nameof(GetSchema)} in {nameof(IDbConnection_Schema_Extensions)}.cs"),
            };
        }
        #endregion


    }
}
