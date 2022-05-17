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
namespace ServerSide.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegisterController : Controller
    {
        public IConfiguration _configuration;
        //private readonly ServerSideContext _context;
        private readonly UserList _context;

        public RegisterController(IConfiguration config, ServerSideContext context)
        {
            //_context = context;
            _configuration = config;
            _context = UserList.GetInstance();
        }
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Register([Bind("Username,Password,DisplayName,Image")] User user)
        {
            User temp = _context.Items.Where(u => u.Username == user.Username).FirstOrDefault();
            if (temp != null) return BadRequest("Username Already exists");
            user.Chats = new List<Chat>();
            _context.Add(user);
            return Ok();
        }

    }

}
