using employee.Data;
using employee.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;
using X.PagedList.Extensions;

namespace employee.Controllers
{

    [Authorize]
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
                var experiences = _context.Experiences
                                          .Where(e => e.EmployeeID == id.Value)
                                          .ToList();
                if (employee != null)
                {
                    model.Employee = employee;
                    model.Experiences = experiences;
                }
            }
            else
            {
                model.Employee = new Employee();
                model.Experiences = new List<Experience> ();
            }

            return View(model);
        }


        //[HttpPost]
        //public IActionResult Create(EmployeeFormViewModel model)
        //{
        //    if (!ModelState.IsValid) return BadRequest(ModelState);

        //    if (model.Employee.EmployeeID > 0)
        //    {
        //        _context.Employees.Update(model.Employee);

        //        var existingExperience = _context.Experiences.FirstOrDefault(e => e.EmployeeID == model.Employee.EmployeeID);

        //        //if (model.Experience.ExperienceID > 0)
        //        if (existingExperience != null)
        //        {
        //            //_context.Experiences.Update(model.Experience);

        //            existingExperience.Department = model.Experience.Department;
        //            existingExperience.Years = model.Experience.Years;
        //            _context.Experiences.Update(existingExperience);

        //        }
        //        else
        //        {
        //            model.Experience.EmployeeID = model.Employee.EmployeeID;
        //            _context.Experiences.Add(model.Experience);
        //        }
        //    }
        //    else
        //    {
        //        _context.Employees.Add(model.Employee);
        //        _context.SaveChanges();

        //        model.Experience.EmployeeID = model.Employee.EmployeeID;
        //        _context.Experiences.Add(model.Experience);
        //    }

        //    _context.SaveChanges();
        //    return RedirectToAction("List");

        //}

        [HttpPost]
        public IActionResult Create(EmployeeFormViewModel model, string? deletedExperienceIds)
        {
            if (ModelState.IsValid)
            {
                if (model.Employee.EmployeeID > 0)
                {
                    _context.Employees.Update(model.Employee);
                }
                else
                {
                    _context.Employees.Add(model.Employee);
                }
                _context.SaveChanges();


                if (model.Employee.EmployeeID > 0 && !string.IsNullOrEmpty(deletedExperienceIds))
                {
                    var idsToDelete = deletedExperienceIds.Split(',')
                        .Select(id => int.Parse(id))
                        .ToList();

                    var experiencesToDelete = _context.Experiences
                        .Where(e => e.EmployeeID == model.Employee.EmployeeID &&
                                    idsToDelete.Contains(e.ExperienceID))
                        .ToList();

                    _context.Experiences.RemoveRange(experiencesToDelete);
                }


                // Add or update experiences
                if (model.Experiences != null)
                {
                    foreach (var experience in model.Experiences)
                    {
                        experience.EmployeeID = model.Employee.EmployeeID;
                        if (experience.ExperienceID > 0)
                        {
                            _context.Experiences.Update(experience);
                        }
                        else
                        {
                            _context.Experiences.Add(experience);
                        }
                    }
                }

                _context.SaveChanges();
                return RedirectToAction("List");
            }

            return View(model);
        }






        public IActionResult List(string query, int page = 1, int pageSize = 50)
        {
            var employeesQuery = _context.Employees
                .Where(e => string.IsNullOrEmpty(query) || e.Name.Contains(query))
                .OrderBy(e => e.EmployeeID);

            var pagedEmployees = employeesQuery.ToPagedList(page, pageSize);
            var employeeIds = pagedEmployees.Select(e => e.EmployeeID).ToList();

            var experiences = _context.Experiences
                .Where(exp => employeeIds.Contains(exp.EmployeeID))
                .ToList();

            var result = pagedEmployees.Select(emp => new EmployeeFormViewModel
            {
                Employee = emp,
                Experiences = experiences
                    .Where(e => e.EmployeeID == emp.EmployeeID)
                    .ToList()
            }).ToList();

            // Create an IPagedList from the result list
            var pagedResult = result.ToPagedList(page, pageSize);

            // Pass pagedResult to the view
            ViewBag.Query = query;
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;

            return View(pagedResult);  // Pass the pagedResult (IPagedList<EmployeeFormViewModel>) to the view
        }



        ////list and search
        //public IActionResult List(string query, int page = 1, int pageSize = 50)
        //{
        //    //var employees = _context.Employees
        //    //    .Where(e => string.IsNullOrEmpty(query)|| e.Name.Contains(query))
        //    //    .ToList();

        //    var employeesQuery = _context.Employees
        //     .Where(e => string.IsNullOrEmpty(query) || e.Name.Contains(query))
        //     .OrderBy(e => e.EmployeeID);

        //    var pagedEmployees = employeesQuery.ToPagedList(page, pageSize);
        //    var employeeIds = pagedEmployees.Select(e => e.EmployeeID).ToList();
        //    var experiences = _context.Experiences
        //        .Where(exp => employeeIds.Contains(exp.EmployeeID))
        //        .ToList();


        //    var result = pagedEmployees.Select(emp => new EmployeeFormViewModel
        //    {
        //        Employee = emp,
        //        Experience = experiences.FirstOrDefault(e => e.EmployeeID == emp.EmployeeID)
        //    }).ToList();

        //    ViewBag.Query = query;
        //    ViewBag.Page = page;
        //    ViewBag.PageSize = pageSize;
        //    ViewBag.PagedEmployees = pagedEmployees;

        //    return View(result);
        //}


        [HttpPost]
        public IActionResult Delete(int id)
        {
            var employee = _context.Employees.Find(id);
            if (employee == null) return NotFound();

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
