using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace employee.Models
{
    public class EmployeeFormViewModel
    {
        public Employee Employee { get; set; } = new Employee();
        public List<Experience> Experiences { get; set; } = new List<Experience>();
    }
}
