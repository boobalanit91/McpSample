using MyProduct.Api.Models;

namespace MyProduct.Api.Repository
{
    public interface IProductRepository
    {
        List<Product> GetAllProducts();
        Product? GetProductById(int id);
        List<Product> GetProductsByCategory(string category);
        List<Product> SearchProducts(string searchTerm);
    }
}