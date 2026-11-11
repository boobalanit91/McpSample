using Microsoft.AspNetCore.Mvc;
using MyProduct.Api.Models;
using MyProduct.Api.Repository;

namespace MyProduct.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(IProductRepository _productRespository) : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_productRespository.GetAllProducts());
        }

        [HttpGet("{id:int}")]
        public IActionResult GetProductById([FromRoute] int id)
        {
            return Ok(_productRespository.GetProductById(id));
        }

        [HttpGet("category/{categoryname}")]
        public IActionResult GetProductsByCategory([FromRoute] string categoryname)
        {
            return Ok(_productRespository.GetProductsByCategory(categoryname));
        }

        [HttpGet("search/{productname}")]
        public IActionResult SearchProductByName([FromRoute] string productname)
        {
            return Ok(_productRespository.SearchProducts(productname));
        }
    }
}
