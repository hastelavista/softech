using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace employee.Models
{
    public class EmployeeFormViewModel
    {
        public Employee Employee { get; set; } = new Employee();
        public Experience Experience { get; set; } = new Experience();
    }
}
