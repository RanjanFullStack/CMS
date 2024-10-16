using CMS.Models.DbModel;

namespace CMS.Models.DatabaseContext
{
    using System.IO;
    using System.Text.Json;
    using Microsoft.EntityFrameworkCore;

    public class JsonDbContext : DbContext
    {
        private const string FilePath = "contacts.json"; // Path to your JSON file
        private List<Contact> _contacts;

        public JsonDbContext(DbContextOptions<JsonDbContext> options) : base(options)
        {
            LoadData();
        }

        public DbSet<Contact> Contacts => Set<Contact>();

        private void LoadData()
        {
            if (File.Exists(FilePath))
            {
                var json = File.ReadAllText(FilePath);
                _contacts = JsonSerializer.Deserialize<List<Contact>>(json) ?? new List<Contact>();
            }
            else
            {
                _contacts = new List<Contact>();
            }
        }

        public override int SaveChanges()
        {
            var json = JsonSerializer.Serialize(_contacts);
            File.WriteAllText(FilePath, json);
            return base.SaveChanges();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contact>().HasKey(c => c.Id);
        }
    }

}
