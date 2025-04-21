using System;
using System.Collections.Generic;
using System.Linq;
using ExpenseTracker.Models;

namespace ExpenseTracker.Data
{
    public interface IExpenseRepository
    {
        IEnumerable<Expense> GetAll();
        Expense GetById(Guid id);
        void Add(Expense expense);
        void Update(Expense expense);
        void Delete(Guid id);
    }

    public class ExpenseRepository : IExpenseRepository
    {
        private readonly List<Expense> _expenses;

        public ExpenseRepository()
        {
            _expenses = new List<Expense>();
        }

        public IEnumerable<Expense> GetAll()
        {
            return _expenses.OrderByDescending(e => e.Date).ToList();
        }

        public Expense GetById(Guid id)
        {
            return _expenses.FirstOrDefault(e => e.Id == id);
        }

        public void Add(Expense expense)
        {
            if (expense == null)
            {
                throw new ArgumentNullException(nameof(expense));
            }

            if (expense.Id == Guid.Empty)
            {
                expense.Id = Guid.NewGuid();
            }

            _expenses.Add(expense);
        }

        public void Update(Expense expense)
        {
            if (expense == null)
            {
                throw new ArgumentNullException(nameof(expense));
            }

            var existingExpense = GetById(expense.Id);
            if (existingExpense == null)
            {
                throw new InvalidOperationException($"Expense with ID {expense.Id} not found");
            }

            _expenses.Remove(existingExpense);
            _expenses.Add(expense);
        }

        public void Delete(Guid id)
        {
            var expense = GetById(id);
            if (expense != null)
            {
                _expenses.Remove(expense);
            }
        }
    }
}
