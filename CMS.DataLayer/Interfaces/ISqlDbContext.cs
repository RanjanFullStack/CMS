using CMS.Models.DbModel;
using Microsoft.EntityFrameworkCore;

namespace CMS.DataLayer.Interfaces
{
    public interface ISqlDbContext
    {
        DbSet<Contact> Contacts { get; set; }
        DbSet<T> Set<T>() where T : class;
    }
}