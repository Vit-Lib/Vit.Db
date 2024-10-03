using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;


namespace Vit.Extensions.Db_Extensions
{
    public static partial class IDbConnection_Execute_Extensions
    {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Execute(this IDbConnection conn, string sql, IDictionary<string, object> param = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return SqlExecutor.Instance.Execute(conn: conn, sql: sql, param: param, transaction: transaction, commandTimeout: commandTimeout);
        }



        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static object ExecuteScalar(this IDbConnection conn, string sql, IDictionary<string, object> param = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return SqlExecutor.Instance.ExecuteScalar(conn: conn, sql: sql, param: param, transaction: transaction, commandTimeout: commandTimeout);
        }



        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IDataReader ExecuteReader(this IDbConnection conn, string sql, IDictionary<string, object> param = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return SqlExecutor.Instance.ExecuteReader(conn: conn, sql: sql, param: param, transaction: transaction, commandTimeout: commandTimeout);
        }


    }
}
