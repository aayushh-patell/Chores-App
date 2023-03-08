using System;
using FinalProject.Models;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models
{
	public enum Recurrence
	{
		Once,
		Daily,
		Weekly,
		Monthly,
		SemiMonthly,
		Annualy
	}

	public class Chore
	{
		public int Id { get; set; }
        public string? UserId { get; set; }
        public string Name { get; set; }
		public DateTime DueDate { get; set; }
        public int? CategoryId { get; set; }
		public Recurrence Recurrence { get; set; }
        public bool? Completed { get; set; }

		public virtual Category Category { get; set; }
		public virtual User User { get; set; }
	}

    public class Category
    {
        public int Id { get; set; }
		public string Title { get; set; }

		public virtual ICollection<Chore> Chores { get; set; }
    }
}