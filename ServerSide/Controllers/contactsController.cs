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
        private readonly ServerSideContext _context;

        public ContactsController(ServerSideContext context)
        {
            _context = context;
        }

        // GET: contacts
        [HttpGet]
        public async Task<IActionResult> IndexAsync()
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

        // GET: contacts/Details/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            User curUser = await getCurrentUserAsync();
            if (curUser == null)
                return BadRequest("You are not logged in");
            Chat chat = curUser.Chats.Where(chat => chat.name == id).FirstOrDefault();
            if (chat == null)
                return NotFound("No such contact");
            return Json(buildContact(chat));

        }

        // GET: contacts/Create
        //[HttpPost]
        public IActionResult Create()
        {
            return View();
        }

        // POST: contacts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,name,server")] Contact contact)
        {

            User curUser = await getCurrentUserAsync();
            if (curUser == null)
                return BadRequest("You are not logged in");
            Chat c = curUser.Chats.Where(c => c.name == contact.id).FirstOrDefault();
            if (c != null)
                return BadRequest("Contact Already exists");
            Chat chat = new Chat();
            int nextId = 2;
            //if (_context.Chat.CountAsync().Result > 0)
            //    nextId = _context.Chat.MaxAsync(x => x.id).Id + 1;
            chat.id = nextId;
            chat.name = contact.id;
            chat.displayname = contact.name;
            chat.server = contact.server;
            curUser.Chats.Add(chat);
            //await _context.Chat.AddAsync(chat);
            await _context.SaveChangesAsync();
            return Ok();
        }

        // GET: contacts/Edit/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(string id, string name, string server)
        {
            if (id == null)
            {
                return NotFound();
            }
            User curUser = await getCurrentUserAsync();
            if (curUser == null)
                return BadRequest("You are not logged in");

            Chat chat = curUser.Chats.Where(c => c.name == id).FirstOrDefault();
            if (chat == null)
                return NotFound("No such contact");
            if (name != null)
                chat.displayname = name;
            if (server != null)
                chat.server = server;
            await _context.SaveChangesAsync();
            return Ok();
        }

        // POST: contacts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("id,name,server,last,lastdate")] Contact contact)
        {
            if (id != contact.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contact);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!contactExists(contact.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(IndexAsync));
            }
            return View(contact);
        }

        // GET: contacts/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Contact == null)
            {
                return NotFound();
            }

            var contact = await _context.Contact
                .FirstOrDefaultAsync(m => m.id == id);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        // POST: contacts/Delete/5
        [HttpDelete("{id}")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Contact == null)
            {
                return Problem("Entity set 'ServerSideContext.contact'  is null.");
            }
            var contact = await _context.Contact.FindAsync(id);
            if (contact != null)
            {
                _context.Contact.Remove(contact);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(IndexAsync));
        }

        private bool contactExists(string id)
        {
            return _context.Contact.Any(e => e.id == id);
        }


        private async Task<User> getCurrentUserAsync()
        {
            var user = HttpContext.User.Identity as ClaimsIdentity;
            if (user.IsAuthenticated == true)
            {
                var userClaims = user.Claims;
                string name = userClaims.FirstOrDefault(o => o.Type == "UserId").Value;
                return await _context.User.Where(u => u.Username == name).Include(x => x.Chats).FirstOrDefaultAsync();
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
                if (chat.messages.Count > 0)
                {
                    temp.last = chat.messages.LastOrDefault().content;
                    temp.lastdate = chat.messages.FirstOrDefault().created;
                }
            }
            return temp;
        }
    }
}