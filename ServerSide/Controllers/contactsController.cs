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

    public class contactsController : Controller
    {
        private readonly ServerSideContext _context;

        public contactsController(ServerSideContext context)
        {
            _context = context;
        }

        // GET: contacts
        [HttpGet]
        public async Task<IActionResult> IndexAsync()
        {
            User curUser = await getCurrentUserAsync();
            if (curUser != null)
                return Json(curUser.contacts.ToList());
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
            var contacts = curUser.contacts;
            var contact = contacts.FirstOrDefault(m => m.id == id);
            if (contact == null)
                return NotFound("No such contact");
            return Json(contact);

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
        public async Task<IActionResult> Create([Bind("id,name,server")] contact contact)
        {

            User curUser = await getCurrentUserAsync();
            if (curUser == null)
                return BadRequest("You are not logged in");
            contact temp = _context.contact.FirstOrDefault(m => m.id == contact.id);
            if (temp != null)
                curUser.contacts.Add(temp);
            else
                curUser.contacts.Add(contact);
            //_context.Update(curUser);
            //_context.User.Update(curUser);
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
            var contact = curUser.contacts.FirstOrDefault(m => m.id == id);
            if (contact == null)
                return NotFound("No such contact");
            if(name != null)
                contact.name = name;
            if(server != null)
                contact.server = server;
            await _context.SaveChangesAsync();
            return Ok();
        }

        // POST: contacts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("id,name,server,last,lastdate")] contact contact)
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
            if (id == null || _context.contact == null)
            {
                return NotFound();
            }

            var contact = await _context.contact
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
            if (_context.contact == null)
            {
                return Problem("Entity set 'ServerSideContext.contact'  is null.");
            }
            var contact = await _context.contact.FindAsync(id);
            if (contact != null)
            {
                _context.contact.Remove(contact);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(IndexAsync));
        }

        private bool contactExists(string id)
        {
            return _context.contact.Any(e => e.id == id);
        }


        private async Task<User> getCurrentUserAsync()
        {
            var user = HttpContext.User.Identity as ClaimsIdentity;
            if (user.IsAuthenticated == true)
            {
                var userClaims = user.Claims;
                string name = userClaims.FirstOrDefault(o => o.Type == "UserId").Value;
                return await _context.User.Where(u => u.Username == name).Include(x => x.contacts).FirstOrDefaultAsync();
            }
            return null;
        }
    }
}
