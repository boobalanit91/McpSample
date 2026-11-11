using McpTest.Interfaces;
using McpTest.Models;
using System.Text.Json;

namespace McpTest.Services
{
    public class ProductService(IHttpClientFactory _httpClientFactory) : IProductService
    {
        private HttpClient _httpClient = _httpClientFactory.CreateClient("myproductapi");
        private JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };

        public List<Product> GetAllProducts()
        {
            string products = Call("product");
            if (!string.IsNullOrEmpty(products))
            {
                return JsonSerializer.Deserialize<List<Product>>(products, jsonSerializerOptions)!;
            }
            return [];
        }

        public Product? GetProductById(int id)
        {
            string product = Call($"product/{id}");
            if (product != null)
            {
                return JsonSerializer.Deserialize<Product>(product, jsonSerializerOptions)!;
            }
            return null;
        }

        public List<Product> GetProductsByCategory(string category)
        {
            string products = Call($"product/category/{category}");
            if (!string.IsNullOrEmpty(products))
            {
                return JsonSerializer.Deserialize<List<Product>>(products, jsonSerializerOptions)!;
            }
            return [];
        }

        public List<Product> SearchProducts(string searchTerm)
        {
            string products = Call($"product/search/{searchTerm}");
            if (!string.IsNullOrEmpty(products))
            {
                return JsonSerializer.Deserialize<List<Product>>(products, jsonSerializerOptions)!;
            }
            return [];
        }


        private string Call(string url)
        {
            var response = _httpClient.GetAsync(url, CancellationToken.None).Result;
            response.EnsureSuccessStatusCode();
            return response.Content.ReadAsStringAsync().Result;
        }

    }
}
