using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ExpenseTracker.Models;
using ExpenseTracker.Services;

namespace ExpenseTracker.Controllers
{
    public class HomeController : Controller
    {
        private readonly IExpenseService _expenseService;

        public HomeController(IExpenseService expenseService)
        {
            _expenseService = expenseService;
        }

        public IActionResult Index()
        {
            var recentExpenses = _expenseService.GetFilteredExpenses(
                DateTime.Today.AddDays(-30), 
                DateTime.Today, 
                null).Take(5);
                
            var summary = _expenseService.GetExpenseSummary(
                DateTime.Today.AddMonths(-1),
                DateTime.Today);
                
            var monthlyChart = _expenseService.GetMonthlyExpenseChart(6);
            var categoryChart = _expenseService.GetCategoryExpenseChart();

            ViewBag.RecentExpenses = recentExpenses;
            ViewBag.Summary = summary;
            ViewBag.MonthlyChart = monthlyChart;
            ViewBag.CategoryChart = categoryChart;
            
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
