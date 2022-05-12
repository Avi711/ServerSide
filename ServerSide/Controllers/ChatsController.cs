using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ServerSide.Data;
using ServerSide.Models;

namespace ServerSide.Controllers
{
    public class ChatsController : Controller
    {
        private readonly ServerSideContext _context;

        public ChatsController(ServerSideContext context)
        {
            _context = context;
        }

        // GET: Chats
        public async Task<IActionResult> Index()
        {
              return View(await _context.Chat.ToListAsync());
        }

        // GET: Chats/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Chat == null)
            {
                return NotFound();
            }

            var chat = await _context.Chat
                .FirstOrDefaultAsync(m => m.id == id);
            if (chat == null)
            {
                return NotFound();
            }

            return View(chat);
        }

        // GET: Chats/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Chats/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,name,displayname,server")] Chat chat)
        {
            if (ModelState.IsValid)
            {
                _context.Add(chat);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(chat);
        }

        // GET: Chats/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Chat == null)
            {
                return NotFound();
            }

            var chat = await _context.Chat.FindAsync(id);
            if (chat == null)
            {
                return NotFound();
            }
            return View(chat);
        }

        // POST: Chats/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,name,displayname,server")] Chat chat)
        {
            if (id != chat.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chat);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChatExists(chat.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(chat);
        }

        // GET: Chats/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Chat == null)
            {
                return NotFound();
            }

            var chat = await _context.Chat
                .FirstOrDefaultAsync(m => m.id == id);
            if (chat == null)
            {
                return NotFound();
            }

            return View(chat);
        }

        // POST: Chats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Chat == null)
            {
                return Problem("Entity set 'ServerSideContext.Chat'  is null.");
            }
            var chat = await _context.Chat.FindAsync(id);
            if (chat != null)
            {
                _context.Chat.Remove(chat);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChatExists(int id)
        {
          return _context.Chat.Any(e => e.id == id);
        }
    }
}
