﻿using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Office2010.Excel;
using employee.Data;
using employee.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace employee.Controllers
{
    //[Authorize]
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;
        public EmployeeController(ApplicationDbContext context)
        {
            _context = context;
        }


      
        [HttpGet]
        public IActionResult GetEmployee(int? id)
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
                model.Experiences = new List<Experience>();
            }

            return Ok(model);
            //return View(model);
           //.return PartialView("Create" ,model);
            //return Json(new
            //{
            //    employee = model.Employee,
            //    experiences = model.Experiences
            //});
        }


        //js checking 
        [HttpPost]
        public IActionResult Save([FromBody] Employee employee)
        {
            if (ModelState.IsValid)
            {
                if (employee.EmployeeID > 0)
                {
                    _context.Employees.Update(employee);
                }
                else
                {
                    _context.Employees.Add(employee);
                }

                _context.SaveChanges();
                return Ok(new { message = "Employee saved successfully!" });
            }

            return BadRequest(ModelState);
        }


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
            }

            //return View(model);
            return Ok(model);
        }

        [HttpPost]
        public IActionResult AddEmployee([FromBody] EmployeeFormViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (model == null) return BadRequest("Model is null");
            if (model.Employee.EmployeeID > 0)
            {
                _context.Employees.Update(model.Employee);
            }
            else
            {
                _context.Employees.Add(model.Employee);
            }

            _context.SaveChanges();

            if (model.Experiences != null)
            {
                foreach (var experience in model.Experiences)
                {
                    experience.EmployeeID = model.Employee.EmployeeID;
                    if (experience.ExperienceID > 0)
                    {
                        var existingexp = _context.Experiences.FirstOrDefault(e => e.ExperienceID == experience.ExperienceID);
                        if (existingexp != null)
                        {
                            existingexp.Company = experience.Company;
                            existingexp.Department = experience.Department;
                            existingexp.Years = experience.Years;

                        }
                    }
                    else
                    {
                        _context.Experiences.Add(experience);
                    }
                }
                _context.SaveChanges();
            }
            return Ok(model);
        }

        //[HttpPost]
        //public IActionResult AddEmployee([FromBody] EmployeeFormViewModel model)
        //{
        //    if (!ModelState.IsValid) return BadRequest(ModelState);
        //    if (model == null) return BadRequest("Model is null");

        //    //var trackedEmployee = _context.ChangeTracker.Entries<Employee>().FirstOrDefault(e => e.Entity.EmployeeID == model.Employee.EmployeeID);
        //    //if (trackedEmployee != null) trackedEmployee.State = EntityState.Detached;

        //    var localEmployee = _context.ChangeTracker.Entries<Employee>().FirstOrDefault(e => e.Entity.EmployeeID == model.Employee.EmployeeID);
        //    if (localEmployee != null)
        //        localEmployee.State = EntityState.Detached;

        //    if (model.Employee.EmployeeID > 0)
        //    {
        //        //var existingEmployee = _context.Employees
        //        //    .AsNoTracking()
        //        //    .FirstOrDefault(e => e.EmployeeID == model.Employee.EmployeeID);

        //        //if (existingEmployee == null)
        //        //    return NotFound($"Employee with ID {model.Employee.EmployeeID} not found.");

        //        //_context.Employees.Update(model.Employee);
        //        _context.Attach(model.Employee).State = EntityState.Modified;

        //    }
        //    else
        //    {
        //        _context.Employees.Add(model.Employee);
        //    }
        //    _context.SaveChanges();


        //    if (model.Experiences != null)
        //    {
        //        foreach (var experience in model.Experiences)
        //        {
        //            experience.EmployeeID = model.Employee.EmployeeID;

        //            //var trackedExp = _context.ChangeTracker.Entries<Experience>().FirstOrDefault(e => e.Entity.ExperienceID == experience.ExperienceID);
        //            //if (trackedExp != null) trackedExp.State = EntityState.Detached;

        //            var localExp = _context.ChangeTracker.Entries<Experience>().FirstOrDefault(e => e.Entity.ExperienceID == experience.ExperienceID);

        //            if (localExp != null)
        //                localExp.State = EntityState.Detached;

        //            if (experience.ExperienceID > 0)
        //            {
        //                // var existingExperience = _context.Experiences
        //                //    .AsNoTracking()
        //                //    .FirstOrDefault(e => e.ExperienceID == experience.ExperienceID);

        //                //if (existingExperience != null)

        //                // _context.Experiences.Update(experience);
        //                _context.Attach(experience).State = EntityState.Modified;

        //            }
        //            else
        //            {
        //                _context.Experiences.Add(experience);
        //            }
        //        }
        //    }

        //    _context.SaveChanges();
        //    return Ok(model);
        //}


        public IActionResult List()
        {
            return View();
        }
        public IActionResult ListNew()
        {
            return View();
        }


        [HttpGet]
        public IActionResult ListEmployees(string query)
        {
            var employeesQuery = _context.Employees
                .Where(e => string.IsNullOrEmpty(query) || e.Name.Contains(query))
                .OrderBy(e => e.EmployeeID)
                .ToList();

            var employeeIds = employeesQuery.Select(e => e.EmployeeID).ToList();
            var experiences = _context.Experiences
                .Where(exp => employeeIds.Contains(exp.EmployeeID))
                .ToList();

            var result = employeesQuery.Select(emp => new EmployeeFormViewModel
            {
                Employee = emp,
                Experiences = experiences
                    .Where(e => e.EmployeeID == emp.EmployeeID)
                    .ToList()
            }).ToList();
            //var result = employeesQuery;
            return Ok(result);
            //ViewBag.Query = query;
        }



        [HttpPost]
        public IActionResult Delete(int id)
        {
            var employee = _context.Employees.Find(id);
            if (employee == null) return NotFound();

            var experience = _context.Experiences.FirstOrDefault(e => e.EmployeeID == id);

            if (employee != null)
                _context.Employees.Remove(employee);

            _context.SaveChanges();

            return Ok();
        }
    }
}
