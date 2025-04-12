using EquipmentLoanApi.Data;
using EquipmentLoanApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EquipmentLoanApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;
        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        // DELETE /delete-users
        [HttpDelete("delete-users")]
        public async Task<IActionResult> DeleteUsers([FromBody] List<DeleteUsersRequest> requests)
        {
            foreach (var request in requests)
            {
                var user = await _context.Users.FindAsync(request.Cpf);
                if (user == null)
                {
                    return BadRequest(new { message = "CPF não informado ou inválido"});
                }

                    _context.Users.Remove(user);
            }
            await _context.SaveChangesAsync();
            return Ok(new { message = "Usuários deletados com sucesso." });
        }

        // PUT /update-users
        [HttpPut("update-users")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequest request)
        {
            var user = await _context.Users.FindAsync(request.Cpf);
            if (user == null)
            {
                return BadRequest(new { message = "CPF não informado ou inválido. " });
            }

            if (request.Name != null){
                user.Name = request.Name;
            }
            if (request.Role != null){
                user.Role = request.Role;
            }
            if (request.IsConsumer != null){
                user.IsConsumer = request.IsConsumer;
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Usuário atualizado com sucesso." });
        }

        // POST /search-user
        [HttpPost("search-user")]
        public async Task<IActionResult> SearchUser([FromBody] SearchUserRequest request)
        {
            List<User> users = new List<User>();

            if (request.Cpf == null)
            {
                return BadRequest(new { message = "Forneça CPF para pesquisa." });                
            }

            var user = await _context.Users.FindAsync(request.Cpf);
                if (user == null)
                    return BadRequest(new { message = "CPF inválido. "});
                users.Add(user);    

            return Ok(users);
        }
    }

    public class DeleteUsersRequest
    {
        public long Cpf { get; set; }
    }

    public class UpdateUserRequest
    {
        public long Cpf { get; set; }
        public string? Name { get; set; }
        public string? Role { get; set; }
        public bool IsConsumer { get; set; }
    }

    public class SearchUserRequest
    {
        public long? Cpf { get; set; }
    }
}
