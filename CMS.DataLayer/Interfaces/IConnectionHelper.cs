namespace CMS.DataLayer.Interfaces
{
    public interface IConnectionHelper
    {
        bool UseJsonDatabase();
        string GetJsonFilePath();
        string GetSqlConnectionString();
    }
}
