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
        /// <summary>
        /// Треба визначитися, яким саме чином опрацьовувати помилки при запиті та при успішному виконанні методу. В якому форматі повертати відповідь.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Registration([FromBody] UserAccountRegistrationRequest request)
        {
            var result = _userService.SingUp(request);
            if (result.IsCompleted)
            {
                return Ok("User was created successfully.");
            }
            return BadRequest(ModelState);
        }
    }
}
