using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace ServerSide.Controllers
{
    [Route("api/controller")]
    [ApiController]
    public class UsersController : Controller 
    {
        public IConfiguration _configuration;

        public UsersController(IConfiguration config)
        {
            _configuration = config;
        }
        [HttpPost]
        public IActionResult Post(string username, string password)
        {
            //if true
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,_configuration["JWTParams:Subject"]),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat,DateTime.UtcNow.ToString()),
                new Claim("UserId",username)
           };
            var secretkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTParams:SecretKey"]));
            var mac = new SigningCredentials(secretkey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _configuration["JWTParams:Issuer"],
                                _configuration["JWTParams:Audience"],
                                claims,
                                expires: DateTime.UtcNow.AddMinutes(20),
                                signingCredentials: mac);


            return Ok(new JwtSecurityTokenHandler().WriteToken(token));

        }
    }

}
