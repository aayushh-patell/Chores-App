using System;
namespace FinalProject.Models
{
	public class ChoreMonth
	{
        // FK Properties
        public int ChoreId { get; set; }
		public string Month { get; set; }

		// Navigation Properties
		public virtual Chore Chore { get; set; }

		public ChoreMonth(int choreId, string month)
		{
			ChoreId = choreId;
			Month = month;
		}
    }
}