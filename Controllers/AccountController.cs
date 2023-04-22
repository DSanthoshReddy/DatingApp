using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DbDataContext;
using API.Dto;
using API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    
    public class AccountController : BaseApiController
    {
        public DataContext _context {get; set;}
        public ITokenService _token {get; set;}
        public AccountController(DataContext context,ITokenService token)
        {
            _context = context;
            _token = token;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto _registerDto)
        {
            bool userAvailable = await IsUserExists(_registerDto.UserName);

            if(userAvailable)
            {
                return BadRequest("User already exists");
            }

            using var hmac = new HMACSHA512();
            var user = new AppUser
            {
                UserName = _registerDto.UserName,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(_registerDto.Password)),
                PasswordSalt = hmac.Key
            };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            UserDto newUser = new UserDto{UserName =_registerDto.UserName,Token = _token.CreateToken(user) };
            return newUser;

        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto _loginDto)
        {
            AppUser user = await _context.Users.SingleOrDefaultAsync(x => x.UserName ==_loginDto.UserName);

            if(user == null)
            {
                return Unauthorized("Invalid Username");
            }

            var hmac = new HMACSHA512(user.PasswordSalt);
            byte[] hashPassword = hmac.ComputeHash(Encoding.UTF8.GetBytes(_loginDto.Password));

            for(int i = 0; i<hashPassword.Length; i++)
            {
                if(hashPassword[i] != user.PasswordHash[i])
                {
                    return Unauthorized("invalid password");
                }
            }

            return new UserDto
            {
                UserName = user.UserName,
                Token = _token.CreateToken(user)
            };

        }
       
       
        private async Task<bool> IsUserExists(string userName)
        {
            return await _context.Users.AnyAsync(x => x.UserName == userName);
        }
    }
}