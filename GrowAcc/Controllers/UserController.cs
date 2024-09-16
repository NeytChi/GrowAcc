using GrowAcc.BusinessFlow;
using GrowAcc.Database;
using GrowAcc.Requests;
using Microsoft.AspNetCore.Mvc;

namespace GrowAcc.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private UserAccountService _userService;
        private readonly IUserRepository _userRepository;
        public UserController(UserAccountService userAccountService, IUserRepository repository) 
        {
            _userService = userAccountService;
            _userRepository = repository;
        }
        [HttpPost]
        public IActionResult Registration([FromBody] UserAccountRegistrationRequest request)
        {
            var result = _userService.Registration(request);
            if (result.IsCompleted)
            {
                return Ok("User created successfully.");
            }
            return BadRequest(ModelState);
        }
    }
}
