using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    [Route("api")]
    public class ServerController : Controller 
    {
        public IConfiguration _configuration;
        private readonly ServerSideContext _context2;
        private readonly UserList _context;

        public ServerController(IConfiguration config, ServerSideContext context)
        {
            //_context = context;
            _configuration = config;
            _context = UserList.GetInstance();
            _context2 = context;
        }
        [AllowAnonymous]
        [HttpPost("Login")]
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

        [HttpPost("Register")]
        public IActionResult Register([Bind("Username,Password,DisplayName,Image")] User user)
        {
            //User temp = _context.Items.Where(u => u.Username == user.Username).FirstOrDefault();
            User temp = _context2.User.FirstOrDefault(u => u.Username == user.Username);
            if (temp != null) return BadRequest("Username Already exists");
            user.Chats = new List<Chat>();
            //_context.Add(user);
            _context2.Add(user);
            _context2.SaveChanges();
            return Ok();
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

        [HttpPost("invitations")]
        public async Task<IActionResult> Invitations([Bind("from,to,server")] ContactTransfer contact)
        {
            if (contact.from == null || contact.server == null || contact.to == null)
                return BadRequest("One or more parameters missing");
            //User user = _context.Items.Where(u => u.Username == contact.to).FirstOrDefault();
            User user = _context2.User.Where(u => u.Username == contact.to).Include(x => x.Chats).FirstOrDefault();
            if (user == null)
                return BadRequest("user don't exist");
            Chat chat = new Chat { name = contact.from, displayname = contact.from, server = contact.server, messages = new List<Message>() };
            user.Chats.Add(chat);
            _context2.SaveChanges();
            return Created("", chat);
        }

        [HttpPost("transfer")]
        public async Task<IActionResult> transfer([Bind("from,to,content")] ContactTransfer contact)
        {
            if (contact.from == null || contact.content == null || contact.to == null)
                return BadRequest("One or more parameters missing");
            //User user = _context.Items.Where(u => u.Username == contact.to).FirstOrDefault();
            User user = _context2.User.Where(u => u.Username == contact.to).Include(x => x.Chats).FirstOrDefault();
            if (user == null)
                return BadRequest("user don't exist");
            Chat chat = user.Chats.Where(u => u.name == contact.from).FirstOrDefault();
            if (chat == null)
                return BadRequest("There is no chat with:" + contact.from);
            Chat chat2 = _context2.Chat.Where(c => c.id == chat.id).Include(x => x.messages).FirstOrDefault();
            if (chat2.messages == null)
                chat2.messages = new List<Message>();
            Message message = new Message { sent = false, content = contact.content, created = DateTime.Now};
            chat2.messages.Add(message);
            _context2.SaveChanges();
            return Created("", message);
        }


        public async Task<User> getCurrentUserAsync()
        {
            var user = HttpContext.User.Identity as ClaimsIdentity;
            if (user.IsAuthenticated == true)
            {
                var userClaims = user.Claims;
                string name = userClaims.FirstOrDefault(o => o.Type == "UserId").Value;
                 return _context2.User.Where(u => u.Username == name).Include(x => x.Chats).FirstOrDefault();
                //return _context.Items.Where(u => u.Username == name).FirstOrDefault();
            }
            return null;
        }

        private User Authenricate(string username, string password)
        {
            if (username == null || password == null) return null;
            //var curUser = _context.Items.FirstOrDefault(u => u.Username.ToLower() == username.ToLower() &&
            //                                                u.Password == password);
            var curUser = _context2.User.FirstOrDefault(u => u.Username.ToLower() == username.ToLower() &&
                                                             (u.Password == password));      
            if (curUser != null)
                return curUser;
            return null;
        }
    }

}
