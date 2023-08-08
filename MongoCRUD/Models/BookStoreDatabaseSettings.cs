namespace Mongo.CRUD.Models
{
    /// <summary>
    /// appsettings.json 의 모델링
    /// </summary>
    public class BookStoreDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string BooksCollectionName { get; set; } = null!;
    }
}
