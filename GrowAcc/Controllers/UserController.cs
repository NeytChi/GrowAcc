using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GrowAcc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context) 
        {
            _context = context;
        }

        public bool Registration()
        {
            return true;

        }
        public ActionResult Login()
        {
            return null;
        }
        public ActionResult Logout()
        {
            return null;
        }

    }
}
