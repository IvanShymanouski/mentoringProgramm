using LinqToDB.Mapping;

namespace linq2dbTask.Entities
{
    [Table("Categories")]
    public class Category
    {
        [Column("CategoryID"), PrimaryKey, Identity]
        public int Id { get; set; }

        [Column("CategoryName")]
        public string Name { get; set; }

        [Column]
        public string Description { get; set; }
    }
}