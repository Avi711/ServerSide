using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ServerSide.Data;
using ServerSide.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
namespace ServerSide.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : Controller 
    {
        public IConfiguration _configuration;
        //private readonly ServerSideContext _context;
        private readonly UserList _context;



        public LoginController(IConfiguration config, ServerSideContext context)
        {
            //_context = context;
            _configuration = config;
            _context = UserList.GetInstance();
        }
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            if (Authenricate(username, password) != null)
            {
                var claims = new[]
                {
                new Claim(JwtRegisteredClaimNames.Sub,_configuration["JWTParams:Subject"]),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat,DateTime.UtcNow.ToString()),
                new Claim("UserId",username)
                };
                var secretkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTParams:SecretKey"]));
                var mac = new SigningCredentials(secretkey, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(_configuration["JWTParams:Issuer"],
                                    _configuration["JWTParams:Audience"],
                                    claims,
                                    expires: DateTime.UtcNow.AddMinutes(20),
                                    signingCredentials: mac);


                return Ok(new JwtSecurityTokenHandler().WriteToken(token));
            }
            return BadRequest("Username and/or password are incorrect");

        }

        private User Authenricate(string username, string password)
        {
            //var curUser = _context.User.FirstOrDefault(u => u.Username.ToLower() == username.ToLower() &&
            //                                                u.Password == password);
            var curUser = _context.Items.FirstOrDefault(u => u.Username.ToLower() == username.ToLower() &&
                                                            u.Password == password);
            if (curUser != null)
                return curUser;
            return null;
        }
    }

}
