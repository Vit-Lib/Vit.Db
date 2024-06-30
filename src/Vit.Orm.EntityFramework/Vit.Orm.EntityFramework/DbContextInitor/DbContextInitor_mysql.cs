using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Vit.Orm.EntityFramework.DbContextInitor
{
    public partial class DbContextInitor_mysql : IDbContextInitor
    {
        public void AddDbContext<TContext>(IServiceCollection data, ConnectionInfo info) where TContext : DbContext
        {
            // for Pomelo.EntityFrameworkCore
            data.AddDbContext<TContext>(opt =>
            {
                opt.UseMySql(info.connectionString, ServerVersion.AutoDetect(info.connectionString));
            });
        }
    }
}
