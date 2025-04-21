using System;
using System.Collections.Generic;

namespace ExpenseTracker.Models
{
    public class ExpenseSummary
    {
        public decimal TotalAmount { get; set; }
        public Dictionary<string, decimal> AmountByCategory { get; set; }
        public Dictionary<string, decimal> AmountByMonth { get; set; }
        
        public ExpenseSummary()
        {
            AmountByCategory = new Dictionary<string, decimal>();
            AmountByMonth = new Dictionary<string, decimal>();
        }
    }

    public class ChartData
    {
        public List<string> Labels { get; set; }
        public List<decimal> Values { get; set; }
        
        public ChartData()
        {
            Labels = new List<string>();
            Values = new List<decimal>();
        }
    }
}
