using CMS.Models.DbModel;
using Microsoft.EntityFrameworkCore;

namespace CMS.DataLayer.Context
{
    public class JsonDbContext : DbContext
    {
        public JsonDbContext(DbContextOptions<JsonDbContext> options) : base(options) { }

        public DbSet<Contact> Contacts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Contact>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();  // Configure Id to be auto-incremented
            });
        }
    }
}
