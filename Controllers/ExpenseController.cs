using System;
using Microsoft.AspNetCore.Mvc;
using ExpenseTracker.Models;
using ExpenseTracker.Services;
using ExpenseTracker.ViewModels;

namespace ExpenseTracker.Controllers
{
    public class ExpenseController : Controller
    {
        private readonly IExpenseService _expenseService;

        public ExpenseController(IExpenseService expenseService)
        {
            _expenseService = expenseService;
        }

        public IActionResult Index(ExpenseFilterViewModel filter)
        {
            // Initialize filter if null
            if (filter == null)
            {
                filter = new ExpenseFilterViewModel();
            }

            var expenses = _expenseService.GetFilteredExpenses(
                filter.StartDate, 
                filter.EndDate, 
                filter.Category);
                
            var viewModel = new ExpenseViewModel
            {
                Expenses = expenses,
                Filter = filter,
                Categories = ExpenseCategories.Categories
            };
            
            return View(viewModel);
        }

        public IActionResult Create()
        {
            ViewBag.Categories = ExpenseCategories.Categories;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Expense expense)
        {
            if (ModelState.IsValid)
            {
                _expenseService.AddExpense(expense);
                return RedirectToAction(nameof(Index));
            }
            
            ViewBag.Categories = ExpenseCategories.Categories;
            return View(expense);
        }

        public IActionResult Edit(Guid id)
        {
            var expense = _expenseService.GetExpenseById(id);
            if (expense == null)
            {
                return NotFound();
            }
            
            ViewBag.Categories = ExpenseCategories.Categories;
            return View(expense);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, Expense expense)
        {
            if (id != expense.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _expenseService.UpdateExpense(expense);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "An error occurred while saving the expense. Please try again.");
                }
            }
            
            ViewBag.Categories = ExpenseCategories.Categories;
            return View(expense);
        }

        public IActionResult Delete(Guid id)
        {
            var expense = _expenseService.GetExpenseById(id);
            if (expense == null)
            {
                return NotFound();
            }

            return View(expense);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            _expenseService.DeleteExpense(id);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Report()
        {
            var monthlyChart = _expenseService.GetMonthlyExpenseChart(12);
            var categoryChart = _expenseService.GetCategoryExpenseChart();
            var summary = _expenseService.GetExpenseSummary(
                DateTime.Today.AddMonths(-12),
                DateTime.Today);
                
            var viewModel = new ExpenseReportViewModel
            {
                MonthlyChart = monthlyChart,
                CategoryChart = categoryChart,
                Summary = summary
            };
            
            return View(viewModel);
        }
    }
}
