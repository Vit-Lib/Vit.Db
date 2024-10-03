

namespace Vit.Db.Util.Data
{
    public class ConnectionInfo
    {
        /// <summary>
        ///  database type, could be : MySql SqlServer Sqlite
        /// </summary>
        public string type { get; set; }

        public string connectionString { get; set; }

        public object ext { get; set; }
    }
}
