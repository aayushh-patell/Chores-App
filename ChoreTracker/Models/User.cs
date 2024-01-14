using System;
using Microsoft.AspNetCore.Identity;
using ChoreTracker.Models;

namespace ChoreTracker.Models
{
	public class User : IdentityUser
	{
		// Self Properties
		public string FirstName { get; set; }
		public string LastName { get; set; }

		// Navigation Properties
		public virtual ICollection<Chore> Chores { get; set; }
	}
}