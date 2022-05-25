using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ServerSide.Data;
using ServerSide.Models;

namespace ServerSide.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class ContactsController : Controller
    {
        private readonly UserList _context;

        public ContactsController(ServerSideContext context)
        {
            //_context = context;
            _context = UserList.GetInstance();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllContacts()
        {
            User curUser = await getCurrentUserAsync();
            if (curUser != null)
            {
                List <Contact> contacts = new List<Contact>();

                curUser.Chats.ForEach(c =>
                {
                    contacts.Add(buildContact(c));
                });

                return Json(contacts);
            }
            return BadRequest("You are not logged in");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ContactDetails(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            User curUser = await getCurrentUserAsync();
            if (curUser == null)
                return NotFound("You are not logged in");
            Chat chat = curUser.Chats.Where(chat => chat.name == id).FirstOrDefault();
            if (chat == null)
                return NotFound("No such contact");
            return Json(buildContact(chat));

        }


        [HttpPost]
        public async Task<IActionResult> CreateContact([Bind("id,name,server")] Contact contact)
        {

            User curUser = await getCurrentUserAsync();
            if (curUser == null)
                return BadRequest("You are not logged in");
            Chat c = curUser.Chats.Where(c => c.name == contact.id).FirstOrDefault();
            if (c != null)
                return BadRequest("Contact Already exists");
            Chat chat = new Chat();
            int nextId = _context.ChatId++;
            //if (_context.Chat.CountAsync().Result > 0)
            //    nextId = _context.Chat.MaxAsync(x => x.id).Id + 1;

            chat.id = nextId;
            chat.name = contact.id;
            chat.displayname = contact.name;
            chat.server = contact.server;
            curUser.Chats.Add(chat);
            //await _context.Chat.AddAsync(chat);
            //await _context.SaveChangesAsync();
            return Created("", contact);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditContact([Bind("id,name,server")] Contact contact)
        {
            if (contact.id == null)
            {
                return NotFound();
            }
            User curUser = await getCurrentUserAsync();
            if (curUser == null)
                return NotFound("You are not logged in");

            Chat chat = curUser.Chats.Where(c => c.name == contact.id).FirstOrDefault();
            if (chat == null)
                return NotFound("No such contact");
            if (contact.name != null)
                chat.displayname = contact.name;
            if (contact.server != null)
                chat.server = contact.server;
            //await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContact(string id)
        {

            if (id == null)
            {
                return NotFound("id is null");
            }
            User curUser = await getCurrentUserAsync();
            if (curUser == null)
                return BadRequest("You are not logged in");
            Chat chat = curUser.Chats.Where(u => u.name == id).FirstOrDefault();
            if(chat == null)
                return NotFound("There is no such user");
            curUser.Chats.Remove(chat);
            return NoContent();
        }


        [HttpGet("{id}/messages")]
        public async Task<IActionResult> GetAllMessages(string id)
        {
            if (id == null)
            {
                return NotFound("id is null");
            }
            User curUser = await getCurrentUserAsync();
            if (curUser == null)
                return BadRequest("You are not logged in");

            Chat chat = curUser.Chats.Where(c => c.name == id).FirstOrDefault();
            if (chat == null)
                return NotFound("No such contact");
            if (chat.messages == null)
                return Json(new List<Message>());
            return Json(chat.messages);
        }

        [HttpPost("{id}/messages")]
        public async Task<IActionResult> CreateMessage(string id, [Bind("content")] Message message)
        {
            if (id == null)
            {
                return NotFound("id is null");
            }
            User curUser = await getCurrentUserAsync();
            if (curUser == null)
                return BadRequest("You are not logged in");

            Chat chat = curUser.Chats.Where(c => c.name == id).FirstOrDefault();
            if (chat == null)
                return NotFound("No such contact");
            message.id = _context.MessageId++;
            message.sent = true;
            message.created = DateTime.Now;
            if (chat.messages == null)
                chat.messages = new List<Message>();
            chat.messages.Add(message);
            return Created("", message);
        }

        [HttpGet("{id}/messages/{id2}")]
        public async Task<IActionResult> ViewSpecificMessage(string id, int id2)
        {
            if (id == null)
            {
                return NotFound("id is null");
            }
            User curUser = await getCurrentUserAsync();
            if (curUser == null)
                return BadRequest("You are not logged in");

            Chat chat = curUser.Chats.Where(c => c.name == id).FirstOrDefault();
            //if (chat == null)
            //  return NotFound("No such contact");
            if (chat.messages == null)
                return NotFound("No message with that id");
            return Json(chat.messages.Where(m => m.id == id2));
        }

        [HttpPut("{id}/messages/{id2}")]
        public async Task<IActionResult> UpdateSpecificMessage(string id, [Bind("id2,content")] Message message)
        {

            if (id == null)
            {
                return NotFound("id is null");
            }
            User curUser = await getCurrentUserAsync();
            if (curUser == null)
                return BadRequest("You are not logged in");

            Chat chat = curUser.Chats.Where(c => c.name == id).FirstOrDefault();
            if (chat.messages == null)
                return NotFound("No User with that id");

            Message msg = chat.messages.Where(m => m.id == message.id).FirstOrDefault();
            if (msg == null)
                return NotFound("No Message with that id");

            msg.content = message.content;
            return NoContent();


        }
        [HttpDelete("{id}/messages/{id2}")]
        public async Task<IActionResult> DeleteSpecificMwssage(string id, int id2)
        {
            if (id == null)
            {
                return NotFound("id is null");
            }
            User curUser = await getCurrentUserAsync();
            if (curUser == null)
                return BadRequest("You are not logged in");

            Chat chat = curUser.Chats.Where(c => c.name == id).FirstOrDefault();
            Message msg = chat.messages.Where(m => m.id == id2).FirstOrDefault();

            chat.messages.Remove(msg);
            return NoContent();

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

        private Contact buildContact(Chat chat)
        {
            Contact temp = new Contact();
            if (chat != null)
            {
                temp.id = chat.name;
                temp.name = chat.displayname;
                temp.server = chat.server;
                if (chat.messages != null && chat.messages.Count > 0)
                {
                    temp.last = chat.messages.LastOrDefault().content;
                    temp.lastdate = chat.messages.LastOrDefault().created;
                    
                }
            }
            return temp;
        }
    }
}