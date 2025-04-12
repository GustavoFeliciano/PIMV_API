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
    public class OrdersController : ControllerBase
    {
        private readonly AppDbContext _context;
        public OrdersController(AppDbContext context)
        {
            _context = context;
        }

        // POST /create-order
        [HttpPost("create-orders")]
        public async Task<IActionResult> CreateOrders([FromBody] List<CreateOrderRequest> requests)
        {

            if (requests == null || !requests.Any())
            {
                return BadRequest(new { message = "Nenhum pedido foi enviado." });
            }

            foreach(var request in requests){
                var user = await _context.Users.FindAsync(request.Cpf);
                var product = await _context.Products.FindAsync(request.ProductId);

                if (user == null || product == null)
                    return BadRequest(new { message = "Usuário ou produto não encontrado." });

                var order = new Order
                {
                    Status = true, // Pedido iniciado como ativo
                    Cpf = request.Cpf,
                    ProductId = request.ProductId,
                    Quantity = request.Quantity
                };    

                _context.Orders.Add(order);
            }

            await _context.SaveChangesAsync();

            return Ok(new { message = "Pedido criado com sucesso." });
        }

        // PUT /close-orders
        [HttpPut("close-orders")]
        public async Task<IActionResult> CloseOrder([FromBody] List<CloseOrderRequest> requests)
        {

            foreach(var request in requests){
                var order = await _context.Orders.FindAsync(request.OrderId);
                if (order == null)
                    return NotFound(new { message = "Pedido não encontrado." });

                order.Status = false;
                await _context.SaveChangesAsync();

            }
            
            return Ok(new { message = "Pedidos finalizados com sucesso." });
        }

        // POST /search-orders
        [HttpPost("search-orders")]
        public async Task<IActionResult> SearchOrders([FromBody] SearchOrdersRequest request)
        {
            if (request.Cpf == null)
            {
                return BadRequest(new { message = "Forneça um CPF para pesquisa." });
            }

            // Pesquisa por CPF
            var orders = await _context.Orders
                .Where(o => o.Cpf == request.Cpf && o.Status == true)
                .Include(o => o.User)
                .Include(o => o.Product)
                .ToListAsync();                

            return Ok(orders);
        }
    }

    public class CreateOrderRequest
    {
        public long Cpf { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public class CloseOrderRequest
    {
        public int OrderId { get; set; }
    }

    public class SearchOrdersRequest
    {
        public long? Cpf { get; set; }
    }
}
