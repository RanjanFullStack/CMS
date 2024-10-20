using Microsoft.EntityFrameworkCore;
using CMS.Models.DbModel;
using CMS.DataLayer.Interfaces;

namespace CMS.DataLayer.Context
{
    public class SqlDbContext : DbContext, ISqlDbContext
    {
        public DbSet<T> Set<T>() where T : class
        {
            return base.Set<T>();
        }

        public SqlDbContext(DbContextOptions<SqlDbContext> options) : base(options)
        {
        }

        public DbSet<Contact> Contacts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contact>()
                .HasKey(c => c.Id); // Ensures Id is the primary key

            // Configure other entity properties as needed
        }
    }
}
