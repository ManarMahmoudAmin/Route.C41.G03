using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Route.C41.G03.BLL.Interfaces;
using Route.C41.G03.BLL.Repositories;
using Route.C41.G03.DAL.Models;
using System.Linq;

namespace Route.C41.G03.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        //private readonly IEmployeeRepository _EmployeesRepo;
        //private readonly IDepartmentRepository _departmentRepos;
        private readonly IWebHostEnvironment _env;

        public EmployeeController(IUnitOfWork unitOfWork, /*IDepartmentRepository departmentRepos,*/ IWebHostEnvironment env)
        {
            _unitOfWork = unitOfWork;

            //_EmployeesRepo = EmployeesRepo;
            //_departmentRepos = departmentRepos;
            _env = env;
        }

        public IActionResult Index(string searchInput)
        {
            var employees = Enumerable.Empty<Employee>();
            var employeeRepo = _unitOfWork.Repository<Employee>() as EmployeeRepository;
            if (string.IsNullOrEmpty(searchInput))
                employees = employeeRepo.GetAll();
            else
                employees = employeeRepo.SearchByName(searchInput.ToLower());
            _unitOfWork.Complete();

            return View(employees);

        }
        [HttpGet]

        public IActionResult Create()
        {
            //ViewBag.Departments = _departmentRepos.GetAll();
            return View();
        }
        [HttpPost]
        public IActionResult Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                 _unitOfWork.Repository<Employee>().Add(employee);
                var count = _unitOfWork.Complete();
                if (count > 0)
                    TempData["Message"] = "Employee Is Created Successfully";
                else  
                    TempData["Message"] = "An Error Has Occurred, Employee Is Not Created ";
                _unitOfWork.Complete();

                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        public IActionResult Details(int? id, string viewName = "Details")
        {
            if (!id.HasValue)
                return BadRequest();

            var employee = _unitOfWork.Repository<Employee>().Get(id.Value);

            if (employee is null)
                return NotFound();
            _unitOfWork.Complete();

            return View(viewName, employee);
        }
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            //ViewBag.Departments = _departmentRepos.GetAll();
            return Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, Employee employee)
        {
            if (id != employee.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(employee);

            try
            {
                _unitOfWork.Repository<Employee>().Update(employee);
                return RedirectToAction(nameof(Index));
            }
            catch (System.Exception ex)
            {
                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, "An Error Has Occured during Updating the employee");
                _unitOfWork.Complete();

                return View(employee);
            }
        }

        public IActionResult Delete(int? id)
        {
            return Details(id, "Delete");
        }

        [HttpPost]
        public IActionResult Delete(Employee employee)
        {
            try
            {
                _unitOfWork.Repository<Employee>().Delete(employee);
                return RedirectToAction(nameof(Index));
            }
            catch (System.Exception ex)
            {
                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, "An Error Has Occured during Updating the employee");
                _unitOfWork.Complete();

                return View(employee);
            }
        }
    }
}
