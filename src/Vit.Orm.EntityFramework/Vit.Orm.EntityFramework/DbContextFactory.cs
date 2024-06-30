using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Vit.Core.Util.ConfigurationManager;
using Vit.Extensions;

namespace Vit.Orm.EntityFramework
{

    public class DbContextFactory : DbContextFactory<DbContext> { }


    public class DbContextFactory<TContext>
         where TContext : DbContext
    {
        ServiceCollection serviceCollection;
        ServiceProvider provider;

        /// <summary>
        ///
        /// </summary>
        /// <param name="info"> demo : {"type":"MySql","connectionString":"xxx"}</param>
        public DbContextFactory<TContext> Init(ConnectionInfo info)
        {
            serviceCollection = new ServiceCollection();
            serviceCollection.UseEntityFramework<TContext>(info);

            provider = serviceCollection.BuildServiceProvider();
            return this;
        }

        /// <summary>
        ///  init with config from appsettings.json
        /// </summary>
        /// <param name="configPath">config path in file appsettings.json. default："App.Db"</param>
        public DbContextFactory<TContext> Init(string configPath = null)
        {
            return Init(Appsettings.json.GetByPath<ConnectionInfo>(configPath ?? "App.Db"));
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public IServiceScope CreateDbContext(out TContext context)
        {
            var scope = provider.CreateScope();
            context = scope.ServiceProvider.GetService<TContext>();
            return scope;
        }

    }
}
