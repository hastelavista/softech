using employee.Data;
using employee.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace employee.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;
        public EmployeeController(ApplicationDbContext context)
        {
            _context = context;
        }



        [HttpGet]
        public IActionResult Create(int? id)
        {
            var model = new EmployeeFormViewModel();

            if (id.HasValue)
            {
                var employee = _context.Employees.FirstOrDefault(e => e.EmployeeID == id.Value);
                var experience = _context.Experiences.FirstOrDefault(e => e.EmployeeID == id.Value);

                if (employee != null)
                {
                    model.Employee = employee;
                    model.Experience = experience;
                }
            }
            else
            {
                model.Employee = new Employee();
                model.Experience = new Experience();
            }

            return View(model);
        }


        [HttpPost]
        public IActionResult Create(EmployeeFormViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (model.Employee.EmployeeID > 0)
            {
                _context.Employees.Update(model.Employee);

                //var existingExperience = _context.Experiences.FirstOrDefault(e => e.EmployeeID == model.Employee.EmployeeID);

                if (model.Experience.ExperienceID > 0)
                {
                    _context.Experiences.Update(model.Experience);

                    //existingExperience.Department = model.Experience.Department;
                    //existingExperience.Years = model.Experience.Years;
                    //_context.Experiences.Update(existingExperience);

                }
                else
                {
                    model.Experience.EmployeeID = model.Employee.EmployeeID;
                    _context.Experiences.Add(model.Experience);
                }
            }
            else
            {
                _context.Employees.Add(model.Employee);
                _context.SaveChanges();

                model.Experience.EmployeeID = model.Employee.EmployeeID;
                _context.Experiences.Add(model.Experience);
            }

            _context.SaveChanges();
            return RedirectToAction("List");

        }


        //list and search
        public IActionResult List(string query)
        {
            var employees = _context.Employees
                .Where(e => string.IsNullOrEmpty(query)|| e.Name.Contains(query))
                .ToList();

            var result = (from emp in employees
                          join exp in _context.Experiences
                          on emp.EmployeeID equals exp.EmployeeID into joined
                          from exp in joined.DefaultIfEmpty()
                          select new EmployeeFormViewModel
                          {
                              Employee = emp,
                              Experience = exp
                          }).ToList();

            return View(result);
        }


        [HttpPost]
        public IActionResult Delete(int id)
        {
            var employee = _context.Employees.FirstOrDefault(e => e.EmployeeID == id);
            var experience = _context.Experiences.FirstOrDefault(e => e.EmployeeID == id);

            if (employee != null)
            {
                _context.Employees.Remove(employee);
            }

            if (experience != null)
            {
                _context.Experiences.Remove(experience);
            }

            _context.SaveChanges();

            return RedirectToAction("List");
        }

    }
}
