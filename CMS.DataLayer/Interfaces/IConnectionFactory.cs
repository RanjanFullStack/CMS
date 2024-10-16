namespace CMS.DataLayer.Interfaces
{
    public interface IConnectionFactory
    {
        bool UseJsonDatabase();
        string GetConnectionString();
    }

}
