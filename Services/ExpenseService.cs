using System;
using System.Collections.Generic;
using System.Linq;
using ExpenseTracker.Data;
using ExpenseTracker.Models;

namespace ExpenseTracker.Services
{
    public interface IExpenseService
    {
        IEnumerable<Expense> GetAllExpenses();
        IEnumerable<Expense> GetFilteredExpenses(DateTime? startDate, DateTime? endDate, string category);
        Expense GetExpenseById(Guid id);
        void AddExpense(Expense expense);
        void UpdateExpense(Expense expense);
        void DeleteExpense(Guid id);
        ExpenseSummary GetExpenseSummary(DateTime? startDate = null, DateTime? endDate = null);
        ChartData GetMonthlyExpenseChart(int months = 6);
        ChartData GetCategoryExpenseChart();
    }

    public class ExpenseService : IExpenseService
    {
        private readonly IExpenseRepository _repository;

        public ExpenseService(IExpenseRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Expense> GetAllExpenses()
        {
            return _repository.GetAll();
        }

        public IEnumerable<Expense> GetFilteredExpenses(DateTime? startDate, DateTime? endDate, string category)
        {
            var expenses = _repository.GetAll();

            if (startDate.HasValue)
            {
                expenses = expenses.Where(e => e.Date >= startDate.Value.Date);
            }

            if (endDate.HasValue)
            {
                expenses = expenses.Where(e => e.Date <= endDate.Value.Date);
            }

            if (!string.IsNullOrWhiteSpace(category) && category != "All")
            {
                expenses = expenses.Where(e => e.Category == category);
            }

            return expenses.OrderByDescending(e => e.Date);
        }

        public Expense GetExpenseById(Guid id)
        {
            return _repository.GetById(id);
        }

        public void AddExpense(Expense expense)
        {
            _repository.Add(expense);
        }

        public void UpdateExpense(Expense expense)
        {
            _repository.Update(expense);
        }

        public void DeleteExpense(Guid id)
        {
            _repository.Delete(id);
        }

        public ExpenseSummary GetExpenseSummary(DateTime? startDate = null, DateTime? endDate = null)
        {
            var expenses = GetFilteredExpenses(startDate, endDate, null);
            var summary = new ExpenseSummary
            {
                TotalAmount = expenses.Sum(e => e.Amount)
            };

            // Calculate amount by category
            var categoryGroups = expenses.GroupBy(e => e.Category);
            foreach (var group in categoryGroups)
            {
                summary.AmountByCategory[group.Key] = group.Sum(e => e.Amount);
            }

            // Calculate amount by month
            var monthGroups = expenses
                .GroupBy(e => new { e.Date.Year, e.Date.Month })
                .OrderBy(g => g.Key.Year)
                .ThenBy(g => g.Key.Month);

            foreach (var group in monthGroups)
            {
                var monthName = new DateTime(group.Key.Year, group.Key.Month, 1).ToString("MMM yyyy");
                summary.AmountByMonth[monthName] = group.Sum(e => e.Amount);
            }

            return summary;
        }

        public ChartData GetMonthlyExpenseChart(int months = 6)
        {
            var endDate = DateTime.Today;
            var startDate = endDate.AddMonths(-months + 1).Date;
            
            var expenses = GetFilteredExpenses(startDate, endDate, null);
            
            var monthlyData = new ChartData();
            
            // Create a list of all months in the range
            var allMonths = Enumerable.Range(0, months)
                .Select(i => endDate.AddMonths(-i))
                .OrderBy(d => d)
                .Select(d => new { Year = d.Year, Month = d.Month, MonthName = d.ToString("MMM yyyy") })
                .ToList();
                
            foreach (var month in allMonths)
            {
                var monthlyTotal = expenses
                    .Where(e => e.Date.Year == month.Year && e.Date.Month == month.Month)
                    .Sum(e => e.Amount);
                    
                monthlyData.Labels.Add(month.MonthName);
                monthlyData.Values.Add(monthlyTotal);
            }
            
            return monthlyData;
        }

        public ChartData GetCategoryExpenseChart()
        {
            var expenses = GetAllExpenses();
            var categoryData = new ChartData();
            
            var categoryGroups = expenses
                .GroupBy(e => e.Category)
                .Select(g => new { Category = g.Key, Total = g.Sum(e => e.Amount) })
                .OrderByDescending(g => g.Total);
                
            foreach (var category in categoryGroups)
            {
                categoryData.Labels.Add(category.Category);
                categoryData.Values.Add(category.Total);
            }
            
            return categoryData;
        }
    }
}
