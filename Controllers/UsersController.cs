using API.DbDataContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Data;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly ILogger<UsersController> _logger;
        public DataContext _context { get; }

        public UsersController(ILogger<UsersController> logger, DataContext context)
        {
            _context = context;

            _logger = logger;
        }

        [AllowAnonymous]
        [HttpGet("")]
        public async Task<IActionResult> GetUsers()
        {
            DbSet<AppUser> userData = _context.Users;
            List<AppUser> allUsers = new List<AppUser>();

            if (userData != null)
            {
                allUsers = await userData.ToListAsync();
            }


            return Ok(allUsers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            DbSet<AppUser> userData = _context.Users;
            AppUser user = null;

            if (userData != null)
            {
                user = await userData.FindAsync(id);
            }

            return Ok(user);
        }
    }
}