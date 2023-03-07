using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FinalProject.Areas.Identity.Data;
using FinalProject.Models;

namespace FinalProject.Controllers
{
    public class ChoreController : Controller
    {
        private readonly FinalProjectIdentityDbContext _context;

        public ChoreController(FinalProjectIdentityDbContext context)
        {
            _context = context;
        }

        // GET: Chore
        public async Task<IActionResult> Index()
        {
            var finalProjectIdentityDbContext = _context.Chores.Include(c => c.User);
            return View(await finalProjectIdentityDbContext.ToListAsync());
        }

        // GET: Chore/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Chores == null)
            {
                return NotFound();
            }

            var chore = await _context.Chores
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chore == null)
            {
                return NotFound();
            }

            return View(chore);
        }

        // GET: Chore/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Chore/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,Name,DueDate,Category,Recurrence,Completed")] Chore chore)
        {
            if (ModelState.IsValid)
            {
                _context.Add(chore);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", chore.UserId);
            return View(chore);
        }

        // GET: Chore/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Chores == null)
            {
                return NotFound();
            }

            var chore = await _context.Chores.FindAsync(id);
            if (chore == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", chore.UserId);
            return View(chore);
        }

        // POST: Chore/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,Name,DueDate,Category,Recurrence,Completed")] Chore chore)
        {
            if (id != chore.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chore);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChoreExists(chore.Id))
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
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", chore.UserId);
            return View(chore);
        }

        // GET: Chore/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Chores == null)
            {
                return NotFound();
            }

            var chore = await _context.Chores
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chore == null)
            {
                return NotFound();
            }

            return View(chore);
        }

        // POST: Chore/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Chores == null)
            {
                return Problem("Entity set 'FinalProjectIdentityDbContext.Chores'  is null.");
            }
            var chore = await _context.Chores.FindAsync(id);
            if (chore != null)
            {
                _context.Chores.Remove(chore);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChoreExists(int id)
        {
          return (_context.Chores?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
