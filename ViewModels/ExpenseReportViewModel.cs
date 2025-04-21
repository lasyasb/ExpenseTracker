using ExpenseTracker.Models;

namespace ExpenseTracker.ViewModels
{
    public class ExpenseReportViewModel
    {
        public ChartData MonthlyChart { get; set; }
        public ChartData CategoryChart { get; set; }
        public ExpenseSummary Summary { get; set; }
    }
}
