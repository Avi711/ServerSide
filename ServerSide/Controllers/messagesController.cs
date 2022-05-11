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
    public class messagesController : Controller
    {
        private readonly ServerSideContext _context;

        public messagesController(ServerSideContext context)
        {
            _context = context;
        }

        // GET: messages
        public async Task<IActionResult> Index()
        {
              return View(await _context.message.ToListAsync());
        }

        // GET: messages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.message == null)
            {
                return NotFound();
            }

            var message = await _context.message
                .FirstOrDefaultAsync(m => m.id == id);
            if (message == null)
            {
                return NotFound();
            }

            return View(message);
        }

        // GET: messages/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: messages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,content,created,sent")] message message)
        {
            if (ModelState.IsValid)
            {
                _context.Add(message);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(message);
        }

        // GET: messages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.message == null)
            {
                return NotFound();
            }

            var message = await _context.message.FindAsync(id);
            if (message == null)
            {
                return NotFound();
            }
            return View(message);
        }

        // POST: messages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,content,created,sent")] message message)
        {
            if (id != message.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(message);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!messageExists(message.id))
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
            return View(message);
        }

        // GET: messages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.message == null)
            {
                return NotFound();
            }

            var message = await _context.message
                .FirstOrDefaultAsync(m => m.id == id);
            if (message == null)
            {
                return NotFound();
            }

            return View(message);
        }

        // POST: messages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.message == null)
            {
                return Problem("Entity set 'ServerSideContext.message'  is null.");
            }
            var message = await _context.message.FindAsync(id);
            if (message != null)
            {
                _context.message.Remove(message);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool messageExists(int id)
        {
          return _context.message.Any(e => e.id == id);
        }
    }
}
