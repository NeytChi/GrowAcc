using GrowAcc.BusinessFlow;
using GrowAcc.Core;
using GrowAcc.Database;
using GrowAcc.Requests;
using Microsoft.AspNetCore.Mvc;
using CSharpFunctionalExtensions;

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
            if(result.I)
            if (result.IsCompletedSuccessfully)
            {
                return Ok(new Success(true,"User was created successfully. Check your email to confirm your registration.");
            }
            return BadRequest(ModelState);
        }
    }
}
