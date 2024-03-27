﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Route.C41.G03.BLL.Interfaces;
using Route.C41.G03.DAL.Models;
using System.Linq;

namespace Route.C41.G03.PL.Controllers
{
    public class EmployeeController : Controller
    {

        private readonly IEmployeeRepository _EmployeesRepo;
        //private readonly IDepartmentRepository _departmentRepos;
        private readonly IWebHostEnvironment _env;

        public EmployeeController(IEmployeeRepository EmployeesRepo, /*IDepartmentRepository departmentRepos,*/ IWebHostEnvironment env)
        {
            _EmployeesRepo = EmployeesRepo;
            //_departmentRepos = departmentRepos;
            _env = env;
        }

        public IActionResult Index(string searchInput)
        {
            var employees = Enumerable.Empty<Employee>();

            if (string.IsNullOrEmpty(searchInput))
                employees = _EmployeesRepo.GetAll();
            else
                employees = _EmployeesRepo.SearchByName(searchInput.ToLower());
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
                var count = _EmployeesRepo.Add(employee);
                if (count > 0)
                    TempData["Message"] = "Employee Is Created Successfully";
                else  
                    TempData["Message"] = "An Error Has Occurred, Employee Is Not Created ";
                
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        public IActionResult Details(int? id, string viewName = "Details")
        {
            if (!id.HasValue)
                return BadRequest();

            var employee = _EmployeesRepo.Get(id.Value);

            if (employee is null)
                return NotFound();

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
                _EmployeesRepo.Update(employee);
                return RedirectToAction(nameof(Index));
            }
            catch (System.Exception ex)
            {
                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, "An Error Has Occured during Updating the employee");
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
                _EmployeesRepo.Delete(employee);
                return RedirectToAction(nameof(Index));
            }
            catch (System.Exception ex)
            {
                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, "An Error Has Occured during Updating the employee");
                return View(employee);
            }
        }
    }
}
