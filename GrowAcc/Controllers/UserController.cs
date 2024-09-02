using GrowAcc.RequestCommands;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GrowAcc.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context) 
        {
            _context = context;
        }
        [HttpPost]
        public IActionResult Registration([FromBody] UserAccountRegistrationCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Повертаємо помилки валідації
            }
            return Ok("User created successfully.");
        }
    }
}
