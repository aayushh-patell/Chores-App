using System;
using FinalProject.Models;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models
{
	public enum Category
	{
		Cleaning,
		Errand,
		Finance,
		Kids,
		Shopping,
		Bills
	}

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
        public int? UserId { get; set; }
        public string Name { get; set; }
		public DateTime DueDate { get; set; }

        public Category? Category { get; set; }
		public Recurrence Recurrence { get; set; }
        public bool? Completed { get; set; }

		public virtual User? User { get; set; }
	}
}