using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using Vit.Core.Util.ConfigurationManager;
using Vit.Orm.EntityFramework;

namespace Vit.Extensions
{

    public static partial class IServiceCollection_UseEntityFramework_Extensions
    {

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="data"></param>
        /// <param name="configPath">config path in file appsettings.json. default："App.Db"</param>
        public static bool UseEntityFramework<TContext>(this IServiceCollection data,string configPath = "App.Db") where TContext : DbContext
        {
            return UseEntityFramework<TContext>(data, Appsettings.json.GetByPath<ConnectionInfo>(configPath ?? "App.Db"));
        }




        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="data"></param>
        /// <param name="info"> demo : {"type":"MySql","connectionString":"xxx"} </param>
        public static bool UseEntityFramework<TContext>(this IServiceCollection data, ConnectionInfo info) where TContext : DbContext
        {
            if (!EfHelp.DbContextInitorMap.TryGetValue(info.type, out var initor) || initor == null) return false;
            initor.AddDbContext<TContext>(data, info);

            return true;
        }

    }
}
