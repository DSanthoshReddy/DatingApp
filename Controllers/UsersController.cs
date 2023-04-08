using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using API.DbDataContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using API.Data;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        public DataContext _context { get; }

        public UsersController(ILogger<UsersController> logger,DataContext context)
        {
            _context = context;
            
            _logger = logger;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetUsers()
        {
            DbSet<AppUser> userData = _context.Users;
            List<AppUser>  allUsers = new List<AppUser>();

            if(userData != null)
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