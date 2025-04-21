using System;
using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Models
{
    public class Expense
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        [DataType(DataType.Currency)]
        [Display(Name = "Amount")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Date is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Date")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Category is required")]
        [Display(Name = "Category")]
        public string Category { get; set; }

        [Display(Name = "Notes")]
        [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters")]
        public string Notes { get; set; }

        public Expense()
        {
            Id = Guid.NewGuid();
            Date = DateTime.Today;
        }
    }

    public static class ExpenseCategories
    {
        public static readonly string[] Categories = new string[]
        {
            "Food & Groceries",
            "Transportation",
            "Housing",
            "Utilities",
            "Healthcare",
            "Entertainment",
            "Personal Care",
            "Education",
            "Clothing",
            "Travel",
            "Gifts & Donations",
            "Other"
        };
    }
}
