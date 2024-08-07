namespace ShopApp.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public bool IsDelete { get; set; }
        public List<Product> Products { get; set; }
    }
}
