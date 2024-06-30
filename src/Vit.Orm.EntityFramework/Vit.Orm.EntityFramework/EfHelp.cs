using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using Vit.Extensions;
using Vit.Orm.EntityFramework.DbContextInitor;

namespace Vit.Orm.EntityFramework
{
    public class EfHelp
    {

        #region DbContextInitor
        public static ConcurrentDictionary<string, IDbContextInitor> DbContextInitorMap
            = new ConcurrentDictionary<string, IDbContextInitor>();


        static EfHelp()
        {
            DbContextInitorMap["SqlServer"] = new DbContextInitor_mssql();
            DbContextInitorMap["MySql"] = new DbContextInitor_mysql();
            DbContextInitorMap["Sqlite"] = new DbContextInitor_sqlite();
        }
        #endregion



        #region CreateDbContext
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="configPath">config path in file appsettings.json. default："App.Db"</param>
        /// <returns></returns>
        public static TContext CreateDbContext<TContext>(string configPath = "App.Db")
            where TContext : DbContext
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.UseEntityFramework<TContext>(configPath);

            return serviceCollection.BuildServiceProvider().GetService<TContext>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configPath">config path in file appsettings.json. default："App.Db"</param>
        /// <returns></returns>
        public static DbContext CreateDbContext(string configPath = "App.Db")
        {
            return CreateDbContext<DbContext>(configPath);
        }
        #endregion

    }
}
