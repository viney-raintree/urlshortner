namespace API.Models
{
    public class ShortURLDatabaseSettings : IShortURLDatabaseSettings
    {
        public string ShortURLCollectionName { get; set; } = String.Empty;
        public string ConnectionString { get; set; } = String.Empty;
        public string DatabaseName { get; set; } = String.Empty;
    }
}