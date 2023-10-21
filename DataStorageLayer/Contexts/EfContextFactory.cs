using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using DataStorageLayer.Helpers;

namespace DataStorageLayer.Contexts
{
    internal class EfContextFactory : IDesignTimeDbContextFactory<EfContext>
    {
        public EfContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<EfContext> optionBuidler = new();
            optionBuidler.UseSqlite($"Data Source={ConnectionStringBuilder.ConnectionString}");
            return new EfContext(optionBuidler.Options);
        }
    }
}
