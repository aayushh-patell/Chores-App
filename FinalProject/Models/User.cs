﻿using System;
using Microsoft.AspNetCore.Identity;
using FinalProject.Models;

namespace FinalProject.Models
{
	public class User : IdentityUser
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }

		public virtual ICollection<Chore> Chores { get; set; }
	}
}