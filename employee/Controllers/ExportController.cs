//using employee.Data;
//using employee.Models;
//using Microsoft.AspNetCore.Mvc;
//using ClosedXML;
//using Excel = Microsoft.Office.Interop.Excel;



//namespace employee.Controllers
//{
//    public class ExportController : Controller
//    {
//        private readonly ApplicationDbContext _context;
//        public ExportController(ApplicationDbContext context)
//        {
//            _context = context;
//        }
//        [HttpGet]
//        public IActionResult Employees(string format = "excel", string? query = null)
//        {
//            var employees = _context.Employees
//                .Where(e => string.IsNullOrEmpty(query) || e.Name.Contains(query))
//                .ToList();

//            var employeeIds = employees.Select(e => e.EmployeeID).ToList();

//            var experiences = _context.Experiences
//                .Where(e => employeeIds.Contains(e.EmployeeID))
//                .ToList();

//            var exportData = employees.Select(emp => new EmployeeFormViewModel
//            {
//                Employee = emp,
//                Experience = experiences.FirstOrDefault(e => e.EmployeeID == emp.EmployeeID)
//            }).ToList();

//            if (format.ToLower() == "excel")
//            {
//                return ExportAsExcel(exportData);
//            }
//            else if (format.ToLower() == "pdf")
//            {
//                return View("ExportPdf", exportData); // This will return a PDF using Rotativa or DinkToPdf
//            }

//            return BadRequest("Unsupported format.");
//        }

//        private FileResult ExportAsExcel(List<EmployeeFormViewModel> data)
//        {
//            using (var workbook = new XLWorkbook())
//            {
//                var ws = workbook.Worksheets.Add("Employees");
//                ws.Cell(1, 1).Value = "S.N.";
//                ws.Cell(1, 2).Value = "Name";
//                ws.Cell(1, 3).Value = "Age";
//                ws.Cell(1, 4).Value = "Gender";
//                ws.Cell(1, 5).Value = "Contact";
//                ws.Cell(1, 6).Value = "Department";
//                ws.Cell(1, 7).Value = "Experience Years";

//                int row = 2, sn = 1;
//                foreach (var item in data)
//                {
//                    ws.Cell(row, 1).Value = sn++;
//                    ws.Cell(row, 2).Value = item.Employee.Name;
//                    ws.Cell(row, 3).Value = item.Employee.Age;
//                    ws.Cell(row, 4).Value = item.Employee.Gender;
//                    ws.Cell(row, 5).Value = item.Employee.Contact;
//                    ws.Cell(row, 6).Value = item.Experience?.Department ?? "";
//                    ws.Cell(row, 7).Value = item.Experience?.Years ?? 0;
//                    row++;
//                }

//                using (var stream = new MemoryStream())
//                {
//                    workbook.SaveAs(stream);
//                    stream.Position = 0;
//                    return File(stream.ToArray(),
//                                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
//                                "Employee_List.xlsx");
//                }
//            }
//        }
//    }

//}