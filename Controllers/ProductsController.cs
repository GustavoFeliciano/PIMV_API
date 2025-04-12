using EquipmentLoanApi.Data;
using EquipmentLoanApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace EquipmentLoanApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        // POST /search-products
        [HttpPost("search-products")]
        public async Task<IActionResult> SearchProducts([FromBody] SearchProductsRequest request)
        {
            var products = await _context.Products
                .Where(p => p.Name.ToLower().Contains(request.Name.ToLower()))
                .ToListAsync();
            return Ok(products);
        }
    }

    public class SearchProductsRequest
    {
        public string Name { get; set; }
    }
}
