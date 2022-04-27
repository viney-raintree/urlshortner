namespace API.Models
{
    public interface IShortURLDatabaseSettings
    {
        string ShortURLCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}