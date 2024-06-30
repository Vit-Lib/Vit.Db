using System;
using Microsoft.Data.SqlClient;
using System.Runtime.CompilerServices;

using Vit.Core.Util.Common;
using Vit.Core.Util.ConfigurationManager;

namespace Vit.Db.Util.Data
{

    public class ConnectionFactory
    {

        #region CommandTimeout
        /// <summary>
        /// default value is coming from  appsettings.json::Vit.Db.CommandTimeout
        /// </summary>
        public static int? CommandTimeout = Vit.Core.Util.ConfigurationManager.Appsettings.json.GetByPath<int?>("Vit.Db.CommandTimeout");

        #endregion



        #region ConnectionCreator

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configPath">config path in file appsettings.json. default："App.Db"</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Func<System.Data.IDbConnection> GetConnectionCreator(string configPath = "App.Db")
        {
            return GetConnectionCreator(Appsettings.json.GetByPath<ConnectionInfo>(configPath ?? "App.Db"));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Func<System.Data.IDbConnection> GetConnectionCreator(ConnectionInfo info)
        {
            switch (info?.type?.ToLower())
            {
                // SqlServer
                case "sqlserver":
                case "mssql":
                    return () => MsSql_GetConnection(info.connectionString);

                // MySql
                case "mysql":
                    return () => MySql_GetConnection(info.connectionString);

                // Sqlite
                case "sqlite":
                    return () => Sqlite_GetConnection(info.connectionString);
            }
            return null;
        }
        #endregion

        #region GetConnection
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static System.Data.IDbConnection GetConnection(ConnectionInfo info)
        {
            switch (info?.type?.ToLower())
            {
                // SqlServer
                case "sqlserver":
                case "mssql":
                    return MsSql_GetConnection(info.connectionString);

                // MySql
                case "mysql":
                    return MySql_GetConnection(info.connectionString);

                // Sqlite
                case "sqlite":
                    return Sqlite_GetConnection(info.connectionString);
            }
            return null;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="configPath">config path in file appsettings.json. default："App.Db"</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static System.Data.IDbConnection GetConnection(string configPath = "App.Db")
        {
            return GetConnection(Appsettings.json.GetByPath<ConnectionInfo>(configPath ?? "App.Db"));
        }
        #endregion


        #region GetOpenConnection
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static System.Data.IDbConnection GetOpenConnection(ConnectionInfo info)
        {
            var connection = GetConnection(info);
            connection?.Open();
            return connection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configPath">config path in file appsettings.json. default："App.Db"</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static System.Data.IDbConnection GetOpenConnection(string configPath = "App.Db")
        {
            var connection = GetConnection(configPath);
            connection?.Open();
            return connection;
        }
        #endregion



        #region MsSql
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SqlConnection MsSql_GetConnection(string ConnectionString)
        {
            return new SqlConnection(ConnectionString);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SqlConnection MsSql_GetOpenConnection(string ConnectionString)
        {
            var connection = MsSql_GetConnection(ConnectionString);
            connection.Open();
            return connection;
        }
        #endregion



        #region MySql

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MySqlConnector.MySqlConnection MySql_GetConnection(string ConnectionString)
        {
            return new MySqlConnector.MySqlConnection(ConnectionString);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MySqlConnector.MySqlConnection MySql_GetOpenConnection(string ConnectionString)
        {
            var connection = MySql_GetConnection(ConnectionString);
            connection.Open();
            return connection;
        }
        #endregion


        #region Sqlite

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Microsoft.Data.Sqlite.SqliteConnection Sqlite_GetConnection(string ConnectionString)
        {
            return new Microsoft.Data.Sqlite.SqliteConnection(ConnectionString);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Microsoft.Data.Sqlite.SqliteConnection Sqlite_GetOpenConnection(string ConnectionString)
        {
            var conn = Sqlite_GetConnection(ConnectionString);
            conn.Open();
            return conn;
        }

        /// <summary>
        /// use memory if filePath is null
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="CacheSize"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Sqlite_GetConnectionString(string filePath = null, int? CacheSize = null)
        {
            // https://learn.microsoft.com/en-us/dotnet/standard/data/sqlite/compare#connection-strings
            // "Data Source=:memory:;Version=3;Cache Size=2000;Pooling=False"

            if (string.IsNullOrEmpty(filePath))
            {
                filePath = ":memory:";
            }
            else
            {
                filePath = CommonHelp.GetAbsPath(filePath);
            }

            var connectionStringBuilder = new Microsoft.Data.Sqlite.SqliteConnectionStringBuilder();
            connectionStringBuilder.DataSource = filePath;

            //if (Version.HasValue) connectionStringBuilder.Add("Version", Version.Value);
            if (CacheSize.HasValue) connectionStringBuilder.Add("Cache Size", CacheSize.Value);


            // to avoid SQLite keeps the database locked even after the connection is closed
            // > System.IO.IOException: 'The process cannot access the file '*' because it is being used by another process.'
            // > https://stackoverflow.com/questions/12532729/sqlite-keeps-the-database-locked-even-after-the-connection-is-closed
            // also work: SqliteConnection.ClearPool((SqliteConnection)connDestination);
            connectionStringBuilder.Pooling = false;

            return connectionStringBuilder.ConnectionString;
        }

        /// <summary>
        /// use memory if filePath is null
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="CacheSize"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Microsoft.Data.Sqlite.SqliteConnection Sqlite_GetConnectionByFilePath(string filePath = null, int? CacheSize = null)
        {
            return Sqlite_GetConnection(Sqlite_GetConnectionString(filePath, CacheSize: CacheSize));
        }

        /// <summary>
        /// use memory if filePath is null
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="CacheSize"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Microsoft.Data.Sqlite.SqliteConnection Sqlite_GetOpenConnectionByFilePath(string filePath = null, int? CacheSize = null)
        {
            var conn = Sqlite_GetConnectionByFilePath(filePath, CacheSize: CacheSize);
            conn.Open();
            return conn;
        }


        #endregion

    }
}
