using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ServerSide.Data;
using ServerSide.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

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
        public IActionResult Login([Bind("Username,Password")] User user)
        {
            if (Authenricate(user.Username, user.Password) != null)
            {
                var claims = new[]
                {
                new Claim(JwtRegisteredClaimNames.Sub,_configuration["JWTParams:Subject"]),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat,DateTime.UtcNow.ToString()),
                new Claim("UserId",user.Username)
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


        [HttpGet("user")]
        public async Task<IActionResult> GetUser()
        {
            User curUser = await getCurrentUserAsync();
            if (curUser != null)
            {
                User user = new User{ Username = curUser.Username, DisplayName = curUser.DisplayName, Image = curUser.Image, Password = curUser.Password };
                return Json(user);
            }
            return BadRequest("you are not logged in");

        }

        public async Task<User> getCurrentUserAsync()
        {
            var user = HttpContext.User.Identity as ClaimsIdentity;
            if (user.IsAuthenticated == true)
            {
                var userClaims = user.Claims;
                string name = userClaims.FirstOrDefault(o => o.Type == "UserId").Value;
                // return await _context.User.Where(u => u.Username == name).Include(x => x.Chats).FirstOrDefaultAsync();
                return _context.Items.Where(u => u.Username == name).FirstOrDefault();

            }
            return null;
        }

        private User Authenricate(string username, string password)
        {
            if (username == null || password == null) return null;
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
