using MyProduct.Api.Models;

namespace MyProduct.Api.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly List<Product> _products = new()
        {
            new Product { Id = 1, Name = "iPhone 16", Category = "Electronics", Price = 79999, StockQuantity = 25, Description = "Apple smartphone with advanced camera" },

            new Product { Id = 2, Name = "Samsung Galaxy S25", Category = "Electronics", Price = 84999, StockQuantity = 30, Description = "Samsung flagship Android smartphone" },

            new Product { Id = 3, Name = "MacBook Air M3", Category = "Laptops", Price = 114999, StockQuantity = 15, Description = "Apple laptop powered by M3 processor" },

            new Product { Id = 4, Name = "Dell XPS 15", Category = "Laptops", Price = 159999, StockQuantity = 10, Description = "Premium Dell laptop for professionals" },

            new Product { Id = 5, Name = "HP Pavilion 14", Category = "Laptops", Price = 65999, StockQuantity = 20, Description = "Everyday laptop for students and professionals" },

            new Product { Id = 6, Name = "Sony WH-1000XM5", Category = "Accessories", Price = 29999, StockQuantity = 18, Description = "Wireless noise cancelling headphones" },

            new Product { Id = 7, Name = "Apple AirPods Pro", Category = "Accessories", Price = 24999, StockQuantity = 40, Description = "Wireless earbuds with active noise cancellation" },

            new Product { Id = 8, Name = "Samsung 55 Inch TV", Category = "Televisions", Price = 74999, StockQuantity = 12, Description = "4K UHD Smart LED television" },

            new Product { Id = 9, Name = "LG OLED 65 Inch TV", Category = "Televisions", Price = 149999, StockQuantity = 8, Description = "Premium OLED Smart television" },

            new Product { Id = 10, Name = "Apple iPad Air", Category = "Tablets", Price = 59999, StockQuantity = 22, Description = "Lightweight tablet for productivity" },

            new Product { Id = 11, Name = "Samsung Galaxy Tab S10", Category = "Tablets", Price = 69999, StockQuantity = 14, Description = "Android tablet with S Pen support" },

            new Product { Id = 12, Name = "Logitech MX Master 3S", Category = "Accessories", Price = 9999, StockQuantity = 50, Description = "Advanced wireless productivity mouse" },

            new Product { Id = 13, Name = "Keychron K2 Keyboard", Category = "Accessories", Price = 8499, StockQuantity = 35, Description = "Mechanical wireless keyboard" },

            new Product { Id = 14, Name = "Canon EOS R50", Category = "Cameras", Price = 69999, StockQuantity = 9, Description = "Mirrorless camera for photography enthusiasts" },

            new Product { Id = 15, Name = "Sony Alpha A7 IV", Category = "Cameras", Price = 219999, StockQuantity = 6, Description = "Full-frame mirrorless professional camera" },

            new Product { Id = 16, Name = "Apple Watch Series 10", Category = "Wearables", Price = 45999, StockQuantity = 28, Description = "Smartwatch with fitness tracking features" },

            new Product { Id = 17, Name = "Samsung Galaxy Watch 7", Category = "Wearables", Price = 29999, StockQuantity = 19, Description = "Smartwatch with health and fitness tracking" },

            new Product { Id = 18, Name = "JBL Flip 6", Category = "Audio", Price = 9999, StockQuantity = 32, Description = "Portable waterproof Bluetooth speaker" },

            new Product { Id = 19, Name = "OnePlus 13", Category = "Electronics", Price = 69999, StockQuantity = 16, Description = "High performance Android smartphone" },

            new Product { Id = 20, Name = "Mi Power Bank 20000mAh", Category = "Accessories", Price = 2499, StockQuantity = 60, Description = "High capacity portable power bank" }
        };

        public List<Product> GetAllProducts()
        {
            return _products;
        }

        public Product? GetProductById(int id)
        {
            return _products.FirstOrDefault(p => p.Id == id);
        }

        public List<Product> GetProductsByCategory(string category)
        {
            return [.. _products
                .Where(p => p.Category.Equals(category,
                    StringComparison.OrdinalIgnoreCase))];
        }

        public List<Product> SearchProducts(string searchTerm)
        {
            return [.. _products
                .Where(p => p.Name.Contains(searchTerm,
                    StringComparison.OrdinalIgnoreCase))];
        }
    }
}
