using GrowAcc.BusinessFlow;
using GrowAcc.Core;
using GrowAcc.Database;
using GrowAcc.Requests;
using Microsoft.AspNetCore.Mvc;
using CSharpFunctionalExtensions;
using GrowAcc.Models;

namespace GrowAcc.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserAccountService _userService;
        private readonly IUserRepository _userRepository;
        public UserController(IUserAccountService userAccountService, IUserRepository repository) 
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
        public async Task<IActionResult> Registration([FromBody] UserAccountRegistrationRequest request)
        {
            var result = await _userService.Registration(request);
            if(result.IsFailure)
            {
                return StatusCode(500, new Success(false, result.Error.ErrorMessage));
            }
            if (result.IsSuccess)
            {
                return Ok(new Success(true,"User was created successfully. Check your email to confirm your registration."));
            }
            return BadRequest(ModelState);
        }
    }
}
