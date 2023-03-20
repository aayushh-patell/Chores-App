using System;
namespace FinalProject.Models.ViewModels
{
    public class IndexChoreViewModel
    {
        public bool? CompleteSelection { get; set; }
        public string? UserSelection { get; set; }
        public int? CategorySelection { get; set; }
        public Recurrence? RecurrenceSelection { get; set; }

        public List<Chore>? Chores { get; set; }
        public List<Category>? CategoryOptions { get; set; }
        public List<User>? UserOptions { get; set; }
        public List<Recurrence> RecurrenceOptions = Enum.GetValues(typeof(Recurrence))
                                                        .Cast<Recurrence>()
                                                        .ToList();
    }
}