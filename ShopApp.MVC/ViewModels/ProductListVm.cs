namespace ShopApp.MVC.ViewModels
{
    public class ProductListVm
    {
        public int Page { get; set; }
        public int TotalCount { get; set; }
        public List<ProductListItemVm> Items { get; set; }
    }
    public class ProductListItemVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal SalePrice { get; set; }
        public decimal CostPrice { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public CategoryInProductReturnVm Category { get; set; }
    }
    public class CategoryInProductReturnVm
    {
        public string Name { get; set; }
        public int ProductsCount { get; set; }
    }
}
