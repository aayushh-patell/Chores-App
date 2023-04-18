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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;

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
			IndexChoreViewModel vm = new IndexChoreViewModel();

			// Create a template, unrefined, list of chores, including related elements
			var chores = _context.Chores
				.Include(c => c.Category)
				.Include(c => c.User)
				.Include(c => c.ChoreMonths)
				.OrderBy(c => c.DueDate)
				.ToList();

			// Check for URL parameters
			chores = FilterByURL(chores, item1, item2, item3);

			vm.Chores = chores;

			// Populate lists for user filtering options
            vm.CategoryOptions = _context.Categories.ToList();
            vm.UserOptions = _context.Users.ToList();

            return View(vm);
		}

		[HttpPost]
		public async Task<IActionResult> Index(string? item1, string? item2, string? item3, IndexChoreViewModel vm)
		{
			// Create a template, unrefined, list of chores, including related elements
			var chores = _context.Chores
				.Include(c => c.Category)
				.Include(c => c.User)
				.Include(c => c.ChoreMonths)
                .OrderBy(c => c.DueDate)
                .ToList();

			// Filter chores by their complete status upon user selection
			chores = FilterByCompleteSelection(chores, vm.CompleteSelection);

			// Filter chores by a specified user if a user was selected
			if (vm.UserSelection != null)
			{
				chores = FilterByUserSelection(chores, vm.UserSelection);
			}

            // Filter chores by a specified category if a category was selected
            if (vm.CategorySelection != null)
            {
                chores = FilterByCategorySelection(chores, vm.CategorySelection);
            }

            // Filter chores by a specified recurrence valye if a recurrence value was selected
            if (vm.RecurrenceSelection != null)
			{
				chores = FilterByRecurrenceSelection(chores, vm.RecurrenceSelection);
			}

            // Check for URL parameters
            chores = FilterByURL(chores, item1, item2, item3);

			vm.Chores = chores;

			// Populate selection fields for category and user
            vm.CategoryOptions = _context.Categories.ToList();
            vm.UserOptions = _context.Users.ToList();

            return View(vm);
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

		[Authorize]
		public IActionResult Create()
		{
			CreateChoreViewModel vm = new();

            // Populate selection fields for category and user
            vm.CategoryOptions = _context.Categories.ToList();
			vm.UserOptions = _context.Users.ToList();

			return View(vm);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize]
        public async Task<IActionResult> Create(CreateChoreViewModel vm)
		{
			// Initialise values of a newly created chore based on user input saved in the ViewModel
			Chore ch = InitialiseValues(vm);

			if (ModelState.IsValid)
			{
				_context.Add(ch);
				await _context.SaveChangesAsync();
			}

			return RedirectToAction(nameof(Index));
		}

		[Authorize]
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null || _context.Chores == null)
			{
				return NotFound();
			}

            var vm = new EditChoreViewModel();

			vm.Id = id;

            // Populate selection fields for category and user
            vm.CategoryOptions = _context.Categories.ToList();
            vm.UserOptions = _context.Users.ToList();

			return View(vm);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize]
		public async Task<IActionResult> Edit(EditChoreViewModel vm)
		{
			var ch = _context.Chores.Find(vm.Id);

			// Resets ChoreMonths associated with the Chore the user wishes to edit
			if (ch.Recurrence == Recurrence.SemiMonthly && ch.ChoreMonths != null)
			{
                foreach (var cm in _context.ChoreMonths)
                {
                    if (cm.ChoreId == vm.Id)
                    {
                        _context.ChoreMonths.Remove(cm);
                    }
                }
            }

			ch = EditValues(ch, vm);

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

		[Authorize]
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
		[Authorize]
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

		// Returns a list of chores that meets the user's URL selection
		private List<Chore> FilterByURL(List<Chore> chores, string? item1, string? item2, string? item3)
		{
            if (item1 != null)
            {
                item1 = item1.ToLower();
                chores = chores.Where(c => c.DueDate.ToString("MMMM").ToLower() == item1 || c.Category?.Title.ToLower() == item1 || c.User?.FirstName.ToLower() == item1).ToList();

                if (item2 != null)
                {
                    item2 = item2.ToLower();
                    chores = chores.Where(c => c.Category?.Title.ToLower() == item2 || c.User?.FirstName.ToLower() == item2).ToList();

                    if (item3 != null)
                    {
                        item3 = item3.ToLower();
                        chores = chores.Where(c => c.User?.FirstName.ToLower() == item3).ToList();
                    }
                }
            }

			return chores;
        }

		// Returns a list of chores based on the selected complete status filter
		private List<Chore> FilterByCompleteSelection(List<Chore> chores, bool? complete)
		{
            if (complete == true)
            {
                chores = chores.Where(c => c.Completed == true).ToList();
            }
            else if (complete == false)
            {
                chores = chores.Where(c => c.Completed == false).ToList();
            }

			return chores;
        }

        // Returns a list of chores based on the selected user filter
        private List<Chore> FilterByUserSelection(List<Chore> chores, string? userId)
		{
			return chores.Where(c => c.UserId == userId).ToList();
		}

        // Returns a list of chores based on the selected category filter
        private List<Chore> FilterByCategorySelection(List<Chore> chores, int? categoryId)
        {
			return chores.Where(c => c.CategoryId == categoryId).ToList();
        }

        // Returns a list of chores based on the selected recurrence filter
        private List<Chore> FilterByRecurrenceSelection(List<Chore> chores, Recurrence? recurrenceSelection)
        {
			return chores.Where(c => c.Recurrence == recurrenceSelection).ToList();
        }

		// Initialise values of a newly created chore with values from the ViewModel
        public Chore InitialiseValues(CreateChoreViewModel vm)
		{
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
            if (ch.Recurrence == Recurrence.SemiMonthly && vm.SelectedMonths != null)
            {
                ch.ChoreMonths = new List<ChoreMonth>();
                foreach (var month in vm.SelectedMonths)
                {
                    ChoreMonth cm = new(ch.Id, month);
                    ch.ChoreMonths.Add(cm);
                }
            }

            return ch;
        }

		// Edit values of an existing chore based on values from the ViewModel 
		private Chore EditValues(Chore ch, EditChoreViewModel vm)
		{
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
            if (ch.Recurrence == Recurrence.SemiMonthly && vm.SelectedMonths != null)
            {
                ch.ChoreMonths = new List<ChoreMonth>();
                foreach (var month in vm.SelectedMonths)
                {
                    ChoreMonth cm = new(ch.Id, month);
                    ch.ChoreMonths.Add(cm);
                }
            }

            return ch;
        }

		// Alters Complete status of a Chore when checked by the user +
		// Creates a new Chore with an updated DueDate based on the Recurrence value of the initial Chore
		// No functionality for SemiMonthly recurrence
		[Authorize]
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
        private DateTime NextIteration(Chore completedChore)
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