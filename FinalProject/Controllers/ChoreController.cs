using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FinalProject.Areas.Identity.Data;
using FinalProject.Models;
using FinalProject.Models.ViewModels;
using System.Numerics;

namespace FinalProject.Controllers
{
	public class ChoreController : Controller
	{
		private readonly FinalProjectIdentityDbContext _context;

		public ChoreController(FinalProjectIdentityDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<IActionResult> Index(string? item1, string? item2, string? item3)
		{
			// Create a template, unrefined, list of chores, including related elements
			var chores = _context.Chores
				.Include(c => c.Category)
				.Include(c => c.User)
				.Include(c => c.ChoreMonths)
				.ToList();

			// Check for URL parameters
			if (item1 != null)
			{
				// Refine the chores list by filtering for URL parameters
				chores = chores.Where(c => c.DueDate.ToString("MMMM") == item1 || c.Category?.Title == item1 || c.User?.FirstName == item1).ToList();

				if (item2 != null) {
					chores = chores.Where(c => c.Category.Title == item2 || c.User.FirstName == item2).ToList();

					if (item3 != null)
					{
						chores = chores.Where(c => c.User.FirstName == item3).ToList();
					}
				}
			}

			return View(chores);
		}

		[HttpPost]
		public async Task<IActionResult> Index(bool? complete)
		{
			// Create a template, unrefined, list of chores, including related elements
			var chores = _context.Chores
				.Include(c => c.Category)
				.Include(c => c.User)
				.Include(c => c.ChoreMonths)
				.ToList();

			// Filter chores by their complete status upon user selection
			if (complete == true)
			{
				chores = chores.Where(c => c.Completed == true).ToList();
			}
			else if (complete == false)
			{
				chores = chores.Where(c => c.Completed == false).ToList();
			}

			return View(chores);
		}

		public async Task<IActionResult> Details(int? id)
		{
			if (id == null || _context.Chores == null)
			{
				return NotFound();
			}

			var chore = await _context.Chores
				.Include(c => c.Category)
				.Include(c => c.User)
                .Include(c => c.ChoreMonths)
                .FirstOrDefaultAsync(m => m.Id == id);

			if (chore == null)
			{
				return NotFound();
			}

			return View(chore);
		}

		public IActionResult Create()
		{
			CreateChoreViewModel vm = new();
			vm.CategoryOptions = _context.Categories.ToList();
			vm.UserOptions = _context.Users.ToList();
			return View(vm);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CreateChoreViewModel vm)
		{
			// Initialise values of a newly created chore based on user input saved in the ViewModel
			Chore ch = new Chore();

			if (vm.UserId != null)
			{
				ch.UserId = vm.UserId;
			}

			ch.Name = vm.Name;
			ch.DueDate = vm.DueDate;

			if (vm.CategoryId != null)
			{
				ch.CategoryId = vm.CategoryId;
			}

			ch.Recurrence = vm.Recurrence;
			ch.Completed = vm.Completed;

			// Save the user-selected months for any chore with a semimonthly recurrence
			if (ch.Recurrence == Recurrence.SemiMonthly)
			{
                ch.ChoreMonths = new List<ChoreMonth>();
                foreach (var month in vm.SelectedMonths)
				{
					ChoreMonth cm = new(ch.Id, month);
					ch.ChoreMonths.Add(cm);
				}
			}

			if (ModelState.IsValid)
			{
				_context.Add(ch);
				await _context.SaveChangesAsync();
			}

			return RedirectToAction(nameof(Index));
		}

		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null || _context.Chores == null)
			{
				return NotFound();
			}

            var vm = new EditChoreViewModel();
			vm.Id = id;

			//if (ch == null)
			//{
			//	return NotFound();
			//}



            vm.CategoryOptions = _context.Categories.ToList();
            vm.UserOptions = _context.Users.ToList();

			return View(vm);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(EditChoreViewModel vm)
		{
			var ch = _context.Chores.Find(vm.Id);

			ch.UserId = vm.UserId;
			ch.Name = vm.Name;
			ch.DueDate = vm.DueDate;
			ch.CategoryId = vm.CategoryId;
			ch.Recurrence = vm.Recurrence;
			ch.Completed = vm.Completed;
            ch.ChoreMonths = null;

			foreach (var cm in _context.ChoreMonths)
			{
				if (cm.ChoreId == vm.Id)
				{
					_context.ChoreMonths.Remove(cm);
				}
			}

            // Save the user-selected months for any chore with a semimonthly recurrence
            if (ch.Recurrence == Recurrence.SemiMonthly && vm.SelectedMonths != null)
            {
                ch.ChoreMonths = new List<ChoreMonth>();
                foreach (var month in vm.SelectedMonths)
                {
                    ChoreMonth cm = new(ch.Id, month);
                    ch.ChoreMonths.Add(cm);
                }
            }

            if (ModelState.IsValid)
			{
				try
				{
					_context.Update(ch);
					_context.SaveChanges();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!ChoreExists(ch.Id))
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

			return View(vm);
		}

		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null || _context.Chores == null)
			{
				return NotFound();
			}

			var chore = await _context.Chores
				.Include(c => c.Category)
				.Include(c => c.User)
                .Include(c => c.ChoreMonths)
                .FirstOrDefaultAsync(m => m.Id == id);

			if (chore == null)
			{
				return NotFound();
			}

			return View(chore);
		}

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

        public async Task<IActionResult> Complete(int? id)
        {
            // Retrieve the chore that was marked as complete
            Chore completedChore = _context.Chores.Find(id);

            // Change the chore's completed attribute to true
            completedChore.Completed = true;

            // Create a new chore based on the recurrence value of the completed chore
            if (completedChore.Recurrence != Recurrence.Once && completedChore.Recurrence != Recurrence.SemiMonthly)
            {
                // Set values of the new chore
                Chore newChore = new Chore();
                newChore.UserId = completedChore.UserId;
                newChore.User = completedChore.User;
                newChore.Name = completedChore.Name;
                newChore.Recurrence = completedChore.Recurrence;
                newChore.CategoryId = completedChore.CategoryId;
                newChore.Category = completedChore.Category;
                newChore.DueDate = NextIteration(completedChore);

                _context.Chores.Add(newChore);
            }

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // Returns a datetime value for the next chore's due date based on its recurrence value
        public DateTime NextIteration(Chore completedChore)
        {
            var dueDate = DateTime.Today;

            switch (completedChore.Recurrence)
            {
                case Recurrence.Daily:
                    dueDate = completedChore.DueDate.AddDays(1);
                    break;
                case Recurrence.Weekly:
                    dueDate = completedChore.DueDate.AddDays(7);
                    break;
                case Recurrence.Monthly:
                    dueDate = completedChore.DueDate.AddMonths(1);
                    break;
                case Recurrence.Annualy:
                    dueDate = completedChore.DueDate.AddYears(1);
                    break;
                default:
                    break;
            }

            return dueDate;
        }
    }
}