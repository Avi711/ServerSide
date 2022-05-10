using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ServerSide.Data;
using ServerSide.Models;
using ServerSide.Services;

namespace ServerSide.Controllers
{
    public class RatingsController : Controller
    {
        private readonly IRatingService _context;

        public RatingsController(ServerSideContext context)
        {
            _context = new RatingService();
            //_context = context;
        }

        // GET: Ratings
        public IActionResult Index()
        {
            if(_context.GetAll().Count > 0)
                ViewBag.average = _context.GetAll().Average(x => x.rate);
            else
                ViewBag.average = "There is no ratings yet.";
            return View(_context.GetAll());
        }

        public IActionResult Search(string query)
        {
            if(query == null) 
                return Json(_context.GetAll());
            var q = _context.GetAll().Where(r => r.Name.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                                                 r.Comment.Contains(query, StringComparison.OrdinalIgnoreCase));
            return Json(q.ToList());
        }


        // GET: Ratings/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rating = _context.Get((int)id);
            if (rating == null)
            {
                return NotFound();
            }

            return View(rating);
        }

        // GET: Ratings/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Ratings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Comment,PublishedDate,Name,rate")] Rating rating)
        {
            if (ModelState.IsValid)
            {
                _context.Create(rating.Comment, rating.Name, rating.rate);
                return RedirectToAction(nameof(Index));
            }
            return View(rating);
        }

        // GET: Ratings/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rating = _context.Get((int)id);
            if (rating == null)
            {
                return NotFound();
            }
            return View(rating);
        }

        // POST: Ratings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Comment,PublishedDate,Name,rate")] Rating rating)
        {
            if (id != rating.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Edit(id, rating.Comment, rating.rate);
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(rating);
        }

        // GET: Ratings/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rating = _context.Get((int)id);
            if (rating == null)
            {
                return NotFound();
            }

            return View(rating);
        }

        // POST: Ratings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var rating = _context.Get((int)id);
            _context.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
