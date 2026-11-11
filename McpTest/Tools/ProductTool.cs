using McpTest.Interfaces;
using McpTest.Models;
using ModelContextProtocol.Server;
using System.ComponentModel;

namespace McpTest.Tools
{
    [McpServerToolType]
    public class ProductTool(IProductService _productService)
    {
        [McpServerTool]
        [Description("Retrieves all the product list")]
        public List<Product> GetAllProducts()
        {
            return _productService.GetAllProducts();
        }

        [McpServerTool]
        [Description("Get product related information by product id")]
        public Product? GetProductById(int id)
        {
            return _productService.GetProductById(id);
        }

        [McpServerTool]
        [Description("Get product related information by product category name")]
        public List<Product> GetProductsByCategory(string categoryname)
        {
            return _productService.GetProductsByCategory(categoryname);
        }

        [McpServerTool]
        [Description("Search product related information by product name")]
        public List<Product> SearchProducts(string search)
        {
            return _productService.SearchProducts(search);
        }
    }
}
