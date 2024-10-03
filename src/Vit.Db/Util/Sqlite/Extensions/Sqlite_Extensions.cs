using System.Runtime.CompilerServices;


namespace Vit.Extensions.Db_Extensions
{
    public static partial class Sqlite_Extensions
    {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BackupTo(this Microsoft.Data.Sqlite.SqliteConnection sourceConn, Microsoft.Data.Sqlite.SqliteConnection destinationConn)
        {
            sourceConn.BackupDatabase(destinationConn);
        }



        #region Vacuum
        /// <summary>
        /// execute VACUUM command . Used to reorganize the entire database for compact purposes, such as deleting deleted items completely
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="commandTimeout">The time in seconds to wait for the command to execute. The default is 30 seconds.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Vacuum(this Microsoft.Data.Sqlite.SqliteConnection conn, int? commandTimeout = null)
        {
            conn.Execute("VACUUM", commandTimeout: commandTimeout);
        }
        #endregion

    }


}
