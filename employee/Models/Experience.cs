using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace employee.Models
{
    public class Experience
    {
        [Key]
        public int ExperienceID { get; set; }

        public int EmployeeID { get; set; }

        [Required(ErrorMessage = "Select Department")]
        public string Department { get; set; }

        [ForeignKey(nameof(EmployeeID))]
        [ValidateNever]
        public Employee? Employee { get; set; }

        [Range(0, 50, ErrorMessage = "Years of experience must be between 0 and 50.")]
        public int? Years { get; set; }
    }
}
