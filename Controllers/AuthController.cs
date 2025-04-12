using EquipmentLoanApi.Data;
using EquipmentLoanApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EquipmentLoanApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var user = _context.Users.Find(request.CPF);
            var password = request.Password;

            if (user == null || password == null)
            {
                return BadRequest(new { message = "Usuário ou senha não encontrados." });
            }

            if(password != "adm")
            {
                return Unauthorized(new {message = "Senha ou usuário inválido." }); 
            }

            return Ok(new { message = "Login realizado com sucesso." });
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromBody] SignupRequest request)
        {

            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            // Cria um novo usuário
            var user = new User
            {   
                Name = request.Name,
                Cpf = request.Cpf,
                Role = request.Role,
                IsConsumer = request.IsConsumer
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Usuário cadastrado com sucesso." });
        }
    }

    // Modelos para as requisições de login e signup
    public class LoginRequest
    {
        public long CPF { get; set; }
        public string Password { get; set; }
    }

    public class SignupRequest
    {
        public string Name { get; set; }
        public long Cpf { get; set; }
        public string Role { get; set; }
        public bool IsConsumer { get; set; }
    }
}
