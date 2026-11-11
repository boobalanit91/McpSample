using McpTest.Models;

namespace McpTest.Interfaces
{
    public interface IProductService
    {
        List<Product> GetAllProducts();
        Product? GetProductById(int id);
        List<Product> GetProductsByCategory(string category);
        List<Product> SearchProducts(string searchTerm);
    }
}
