using System;

using Microsoft.Data.Sqlite;

using Vit.Db.Util.Data;

namespace Vit.Db.Util.Sqlite
{
    /// <summary>
    /// 
    /// backup sourceConn to destinationConn：
    /// <para> // method 1:                                                                                                     </para>
    /// <para> using (var sourceConn = ConnectionFactory.Sqlite_GetOpenConnectionByFilePath(sourceFilePath))                    </para>
    /// <para> using (new SQLiteBackup(sourceConn, destinationConnectionString: destinationConnectionString ))                  </para>
    /// <para> {                                                                                                                </para>
    /// <para>     //do something with  sourceConn                                                                              </para>
    /// <para> }                                                                                                                </para>
    /// <para>                                                                                                                  </para>
    /// <para> // method 2:                                                                                                     </para>
    /// <para> using (var sourceConn = ConnectionFactory.Sqlite_GetOpenConnectionByFilePath(sourceFilePath))                    </para>
    /// <para> using (new SQLiteBackup(sourceConn, destinationFilePath:destinationFilePath))                                    </para>
    /// <para> {                                                                                                                </para>
    /// <para>     //do something with  sourceConn                                                                              </para>
    /// <para> }                                                                                                                </para>
    /// <para>                                                                                                                  </para>
    /// <para> // method 3:                                                                                                     </para>
    /// <para> using (var sourceConn = ConnectionFactory.Sqlite_GetOpenConnectionByFilePath(sourceFilePath))                    </para>
    /// <para> using (var connDestination = ConnectionFactory.Sqlite_GetOpenConnectionByFilePath(destinationFilePath))          </para>
    /// <para> {                                                                                                                </para>
    /// <para>     //do something connSource                                                                                    </para>
    /// <para>     sourceConn.BackupDatabase(connDestination);                                                                  </para>
    /// <para> }                                                                                                                </para>
    /// 
    /// </summary>
    public class SQLiteBackup : IDisposable
    {

        SqliteConnection sourceConn = null;
        string destinationConnectionString = null;
        public SQLiteBackup(SqliteConnection sourceConn, string destinationConnectionString = null, string destinationFilePath = null)
        {
            this.sourceConn = sourceConn;

            if (destinationConnectionString == null && destinationFilePath != null)
            {
                destinationConnectionString = ConnectionFactory.Sqlite_GetConnectionString(destinationFilePath);
            }

            this.destinationConnectionString = destinationConnectionString;
        }
        public void Dispose()
        {
            if (sourceConn == null || destinationConnectionString == null)
                return;

            using (var connDestination = ConnectionFactory.Sqlite_GetOpenConnection(destinationConnectionString))
            {
                sourceConn.BackupDatabase(connDestination);
            }
        }
    }
}
