using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace employee.Models

{
    public class Employee
    {
        [Key]
        public int EmployeeID { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = string.Empty;

        [Range(18, 100, ErrorMessage = "Age must be between 18 and 100.")]
        public int? Age { get; set; }
        [Required(ErrorMessage = "Gender is required.")]
        public string? Gender { get; set; }

        [Phone(ErrorMessage = "Invalid phone number.")]
        public string? Contact { get; set; }
    }
}
