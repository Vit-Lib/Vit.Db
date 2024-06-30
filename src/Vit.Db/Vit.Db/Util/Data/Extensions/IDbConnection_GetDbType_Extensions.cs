using System.Data;
using System.Runtime.CompilerServices;
using Vit.Db.Util.Data;

namespace Vit.Extensions.Db_Extensions
{

    public static partial class IDbConnection_GetDbType_Extensions
    {

        #region GetDbType
        /// <summary>
        /// get database type, example:  mysql/mssql/sqlite
        /// </summary>
        /// <param name="conn"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static EDbType? GetDbType(this IDbConnection conn)
        {
            if (conn is Microsoft.Data.SqlClient.SqlConnection)
            {
                return EDbType.SqlServer;
            }

            if (conn is MySqlConnector.MySqlConnection)
            {
                return EDbType.MySql;
            }

            //if (conn is System.Data.SQLite.SQLiteConnection)
            //{
            //    return EDbType.sqlite;
            //}
            if (conn is Microsoft.Data.Sqlite.SqliteConnection)
            {
                return EDbType.Sqlite;
            }

            return GetDbTypeFromTypeName(conn);
        }
        #endregion


        #region GetDbTypeFromTypeName
        /// <summary>
        /// get database type, example:  mysql/mssql/sqlite
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static EDbType? GetDbTypeFromTypeName(object obj) 
        {
            var typeFullName = obj?.GetType().FullName.ToLower();

            if (string.IsNullOrEmpty(typeFullName)) return null;

            if (typeFullName.Contains("sqlite"))
            {
                return EDbType.Sqlite;
            }

            if (typeFullName.Contains("mysql"))
            {
                return EDbType.MySql;
            }

            if (typeFullName.Contains("mssql")|| typeFullName.Contains("sqlserver") || typeFullName.Contains(".sqlconnection") || typeFullName.Contains(".sqlclient."))
            {
                return EDbType.SqlServer;
            }

            return null;
        }
        #endregion

    }
}
