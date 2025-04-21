using System;
using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.ViewModels
{
    public class ExpenseFilterViewModel
    {
        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateTime? StartDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        public DateTime? EndDate { get; set; }

        [Display(Name = "Category")]
        public string Category { get; set; }

        public ExpenseFilterViewModel()
        {
            // Default to current month
            StartDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            EndDate = DateTime.Today;
            Category = "All";
        }
    }
}
