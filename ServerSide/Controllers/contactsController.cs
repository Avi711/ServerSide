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
using ServerSide.Services;

namespace ServerSide.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class ContactsController : Controller
    {
        private readonly UserList _context;
        private readonly IContactService _service;
        private readonly ServerSideContext _context2;

        public ContactsController(ServerSideContext context)
        {
            //_context = context;
            _context = UserList.GetInstance();
            _service = new ContactService(context);
            _context2 = context;

        }

        [HttpGet]
        public async Task<IActionResult> GetAllContacts()
        {
            User curUser = await getCurrentUserAsync();
            if (curUser != null)
            {
                return Json(_service.GetAllContacts(curUser));
            }
            return BadRequest("You are not logged in");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ContactDetails(string id)
        {
            User curUser = await getCurrentUserAsync();
            if (curUser == null)
                return NotFound("You are not logged in");
            Contact contact = _service.ContactDetails(curUser, id);
            if(contact == null)
                return NotFound("No such contact");
            return Json(contact);
        }


        [HttpPost]
        public async Task<IActionResult> CreateContact([Bind("id,name,server")] Contact contact)
        {

            User curUser = await getCurrentUserAsync();
            if (curUser == null)
                return BadRequest("You are not logged in");


            // Chat c = curUser.Chats.Where(c => c.name == contact.id).FirstOrDefault();
            // if (c != null)
            //     return BadRequest("Contact Already exists");
            // Chat chat = new Chat();
            // int nextId = _context.ChatId++;
            //if (_context.Chat.CountAsync().Result > 0)
            //    nextId = _context.Chat.MaxAsync(x => x.id).Id + 1;

            // chat.id = nextId;
            // chat.name = contact.id;
            // chat.displayname = contact.name;
            // chat.server = contact.server;
            //  curUser.Chats.Add(chat);
            //await _context.Chat.AddAsync(chat);
            //await _context.SaveChangesAsync();

            Contact contact1 = _service.CreateContact(curUser, contact);
            if (contact1 != null)
                return Created("", contact);
            return BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditContact(string id, [Bind("name,server")] Contact contact)
        {
          //  if (contact.id == null)
          //  {
          //      return NotFound();
          //  }
            User curUser = await getCurrentUserAsync();
            if (curUser == null)
                return NotFound("You are not logged in");

            //  Chat chat = curUser.Chats.Where(c => c.name == contact.id).FirstOrDefault();
            //  if (chat == null)
            //      return NotFound("No such contact");
            //  if (contact.name != null)
            //      chat.displayname = contact.name;
            //  if (contact.server != null)
            //      chat.server = contact.server;
            //  return NoContent();

            Contact contact1 = _service.EditContact(curUser,id, contact);
            if (contact1 != null)
                return NoContent();
            return NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContact(string id)
        {

         //   if (id == null)
         //   {
         //       return NotFound("id is null");
         //   }
            User curUser = await getCurrentUserAsync();
            if (curUser == null)
                 return BadRequest("You are not logged in");
            //Chat chat = curUser.Chats.Where(u => u.name == id).FirstOrDefault();
            // if(chat == null)
            //     return NotFound("There is no such user");
            // curUser.Chats.Remove(chat);

            if (_service.DeleteContact(curUser, id) == -1)
                return NotFound("There is no such user");
            return NoContent();
        }


        [HttpGet("{id}/messages")]
        public async Task<IActionResult> GetAllMessages(string id)
        {
           //  if (id == null)
           //  {
          //       return NotFound("id is null");
           //  }
             User curUser = await getCurrentUserAsync();
             if (curUser == null)
                return BadRequest("You are not logged in");

           //  Chat chat = curUser.Chats.Where(c => c.name == id).FirstOrDefault();
           //  if (chat == null)
            //     return NotFound("No such contact");
           //  if (chat.messages == null)
           //     return Json(new List<Message>());
           //  return Json(chat.messages);

            List<Message> messages = _service.GetAllMessages(curUser, id);
            if (messages != null)
                return Json(messages);
            return BadRequest();

        }

        [HttpPost("{id}/messages")]
        public async Task<IActionResult> CreateMessage(string id, [Bind("content")] Message message)
        {
           // if (id == null)
           // {
           //     return NotFound("id is null");
           // }
            User curUser = await getCurrentUserAsync();
            if (curUser == null)
                return BadRequest("You are not logged in");

           // Chat chat = curUser.Chats.Where(c => c.name == id).FirstOrDefault();
           // if (chat == null)
           //     return NotFound("No such contact");
           // message.id = _context.MessageId++;
           // message.sent = true;
           // message.created = DateTime.Now;
           // if (chat.messages == null)
           //     chat.messages = new List<Message>();
           // chat.messages.Add(message);

            if(_service.CreateMessage(curUser, id ,message) != null)
                return Created("", message);
            return BadRequest();
        }

        [HttpGet("{id}/messages/{id2}")]
        public async Task<IActionResult> ViewSpecificMessage(string id, int id2)
        {
         //   if (id == null)
         //   {
         //       return NotFound("id is null");
         //   }
            User curUser = await getCurrentUserAsync();
            if (curUser == null)
                return BadRequest("You are not logged in");

            //Chat chat = curUser.Chats.Where(c => c.name == id).FirstOrDefault();
            // if (chat.messages == null)
            //     return NotFound("No message with that id");
            // return Json(chat.messages.Where(m => m.id == id2));
            Message message = _service.ViewSpecificMessage(curUser, id, id2);
            if (message != null)
                return Json(message);
            return NotFound();
        }

        [HttpPut("{id}/messages/{id2}")]
        public async Task<IActionResult> UpdateSpecificMessage(string id, int id2, [Bind("content")] Message message)
        {

          //  if (id == null)
          //  {
          //      return NotFound("id is null");
          //  }
            User curUser = await getCurrentUserAsync();
            if (curUser == null)
                return BadRequest("You are not logged in");
            //   Chat chat = curUser.Chats.Where(c => c.name == id).FirstOrDefault();
            //  if (chat.messages == null)
            //      return NotFound("No User with that id");
            //  Message msg = chat.messages.Where(m => m.id == message.id).FirstOrDefault();
            //  if (msg == null)
            //      return NotFound("No Message with that id");
            //  msg.content = message.content;
            // return NoContent();


            Message message2 = _service.UpdateSpecificMessage(curUser,id,id2, message);
            if (message2 != null)
                return NoContent();
            return NotFound();


        }
        [HttpDelete("{id}/messages/{id2}")]
        public async Task<IActionResult> DeleteSpecificMwssage(string id, int id2)
        {
           // if (id == null)
           // {
          //      return NotFound("id is null");
          //  }
            User curUser = await getCurrentUserAsync();
            if (curUser == null)
                return BadRequest("You are not logged in");

            //   Chat chat = curUser.Chats.Where(c => c.name == id).FirstOrDefault();
            //  Message msg = chat.messages.Where(m => m.id == id2).FirstOrDefault();
            //  chat.messages.Remove(msg);
            // return NoContent();

            Message message = _service.DeleteSpecificMwssage(curUser, id, id2);
            if (message != null)
                return NoContent();
            return BadRequest();

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
    }
}