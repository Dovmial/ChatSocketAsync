
using DataStorageLayer.Helpers;
using DataStorageLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DataStorageLayer.Contexts
{
    internal class EfContext : DbContext
    {
        internal EfContext(DbContextOptions<EfContext> options) : base(options) { }
        internal EfContext(bool doMigrate = false)
        {
            SQLitePCL.Batteries_V2.Init();
            if (doMigrate)
                Database.Migrate();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlite(ConnectionStringBuilder.ConnectionString);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(u => u.Login).IsUnique();
            modelBuilder.Entity<User>().HasIndex(u => u.HashedPassword).IsUnique();
        }

        #region DbSets
        internal DbSet<User> Users { get; set; }
        internal DbSet<MessageModel> Messages { get; set; }
        internal DbSet<UserMessageReceivers> MessageReceivers{get;set;}
        #endregion


    }
}
