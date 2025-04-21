// Main site JavaScript file

// Wait for DOM to be loaded
document.addEventListener('DOMContentLoaded', () => {
    // Initialize Bootstrap tooltips
    const tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });

    // Initialize date filters to today's date if not set
    const dateInputs = document.querySelectorAll('input[type="date"]');
    dateInputs.forEach(input => {
        if (!input.value && !input.getAttribute('data-no-default')) {
            // Default to today for empty date inputs
            const today = new Date();
            const year = today.getFullYear();
            const month = String(today.getMonth() + 1).padStart(2, '0');
            const day = String(today.getDate()).padStart(2, '0');
            input.value = `${year}-${month}-${day}`;
        }
    });

    // Format currency inputs
    const formatCurrency = (input) => {
        input.addEventListener('blur', function () {
            const value = this.value.replace(/[^\d.-]/g, '');
            if (value && !isNaN(value)) {
                this.value = parseFloat(value).toFixed(2);
            }
        });
    };

    document.querySelectorAll('input[data-type="currency"]').forEach(formatCurrency);
    
    // Expense filter form submission
    const filterForm = document.getElementById('filterForm');
    if (filterForm) {
        filterForm.addEventListener('submit', (e) => {
            // Prevent submission if dates are invalid
            const startDate = new Date(document.getElementById('Filter_StartDate').value);
            const endDate = new Date(document.getElementById('Filter_EndDate').value);
            
            if (startDate > endDate) {
                e.preventDefault();
                alert('Start date cannot be later than end date.');
                return false;
            }
        });
    }
});
