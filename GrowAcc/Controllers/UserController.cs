using GrowAcc.BusinessFlow;
using GrowAcc.Core;
using GrowAcc.Database;
using GrowAcc.Requests;
using Microsoft.AspNetCore.Mvc;
using GrowAcc.Culture;
using Microsoft.EntityFrameworkCore;

namespace GrowAcc.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserAccountService _userService;
        public UserController(IUserAccountService userAccountService) 
        {
            _userService = userAccountService;
        }
        [HttpPost("registration")]
        public async Task<IActionResult> Registration([FromBody] UserAccountRegistrationRequest request)
        {
            var culture = CultureConfiguration.DefineCulture(Request.Headers.AcceptLanguage);

            var result = await _userService.Registration(request, culture);
            if(result.IsSuccess)
            {
                return Ok(new Success(true, CultureConfiguration.Get("UserAccountRegistrate", culture)));
            }
            else if (result.Error.ErrorType == ErrorType.NotValid)
            {
                return StatusCode(500, new SuccessData(false, (DomainValidationError)result.Error));
            }
            return StatusCode(500, new Success(false, result.Error.ErrorMessage));    
        }
        [HttpGet("confirm")]
        public async Task<IActionResult> ConfirmEmail(string token)
        {
            var culture = CultureConfiguration.DefineCulture(Request.Headers.AcceptLanguage);

            var result = await _userService.ConfirmEmailByToken(token, culture);

            if (result.IsSuccess)
            {
                return Ok(new SuccessData(true, new MessageUser(CultureConfiguration.Get("UserAccountConfirmed", culture), result.Value)));
            }
            return StatusCode(500, new Success(false, result.Error.ErrorMessage));
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserAccountLoginRequest request)
        {
            var culture = CultureConfiguration.DefineCulture(Request.Headers.AcceptLanguage);

            var result = await _userService.Login(request, culture);

            if (result.IsSuccess)
            {
                return Ok(new SuccessData(true, result.Value));
            }
            return StatusCode(500, new Success(false, result.Error.ErrorMessage));
        }
    }
}
