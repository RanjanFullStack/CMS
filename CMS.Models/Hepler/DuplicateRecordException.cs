namespace CMS.Models.Hepler
{
    public class DuplicateRecordException : Exception
    {
        public DuplicateRecordException(string message) : base(message)
        {
        }
    }
}
