using System.Data;
using System.Runtime.CompilerServices;

namespace Vit.Extensions.Db_Extensions
{

    public static partial class IDbConnection_GetDbName_Extensions
    {

        #region MySql_GetDbName

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string MySql_GetDbName(this IDbConnection conn)
        {
            return new MySqlConnector.MySqlConnectionStringBuilder(conn.ConnectionString).Database;
        }
        #endregion




        #region MsSql_GetDbName

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string MsSql_GetDbName(this IDbConnection conn)
        {
            return new Microsoft.Data.SqlClient.SqlConnectionStringBuilder(conn.ConnectionString).InitialCatalog;
        }
        #endregion
    }
}
