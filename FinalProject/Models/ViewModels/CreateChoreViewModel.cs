using System;
namespace FinalProject.Models.ViewModels
{
	public class CreateChoreViewModel
	{
        public string? UserId { get; set; }
        public string Name { get; set; }
        public DateTime DueDate { get; set; }
        public int? CategoryId { get; set; }
        public Recurrence Recurrence { get; set; }
        public bool? Completed { get; set; }
    }
}