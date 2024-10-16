using Microsoft.EntityFrameworkCore;
using CMS.Models.DbModel;

namespace CMS.DataLayer.Context
{
    public class SqlDbContext : DbContext
    {
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
