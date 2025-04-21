using System.Collections.Generic;
using ExpenseTracker.Models;

namespace ExpenseTracker.ViewModels
{
    public class ExpenseViewModel
    {
        public IEnumerable<Expense> Expenses { get; set; }
        public ExpenseFilterViewModel Filter { get; set; }
        public string[] Categories { get; set; }
    }
}
