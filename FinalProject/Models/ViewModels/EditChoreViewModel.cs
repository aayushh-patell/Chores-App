using System;
using FinalProject.Areas.Identity.Data;
using FinalProject.Controllers;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Models.ViewModels
{

    public class EditChoreViewModel
	{
        public int? Id { get; set; }

        public string? UserId { get; set; }
        public string Name { get; set; }
        public DateTime DueDate { get; set; }
        public int? CategoryId { get; set; }
        public Recurrence Recurrence { get; set; }
        public bool Completed { get; set; }
        public List<string>? SelectedMonths { get; set; }

        public List<Category>? CategoryOptions { get; set; }
        public List<User>? UserOptions { get; set; }

        public List<string> MonthOptions = new() {
            "January",
            "February",
            "March",
            "April",
            "May",
            "June",
            "July",
            "August",
            "September",
            "October",
            "November",
            "December"
        };

        public List<Recurrence> RecurrenceOptions = Enum.GetValues(typeof(Recurrence))
                                                        .Cast<Recurrence>()
                                                        .ToList();

        public List<bool> CompletedOptions = new() {
            true,
            false
        };
    }
}