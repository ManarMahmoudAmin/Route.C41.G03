using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;
using Route.C41.G03.BLL;
using Route.C41.G03.BLL.Interfaces;
using Route.C41.G03.BLL.Repositories;
using Route.C41.G03.DAL.Models;
using Route.C41.G03.PL.Helpers;
using Route.C41.G03.PL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Route.C41.G03.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        //private readonly IEmployeeRepository _EmployeesRepo;
        //private readonly IDepartmentRepository _departmentRepos;
        private readonly IWebHostEnvironment _env;

        public EmployeeController(IUnitOfWork unitOfWork, IMapper mapper/*,IDepartmentRepository departmentRepos,*/, IWebHostEnvironment env)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

            //_EmployeesRepo = EmployeesRepo;
            //_departmentRepos = departmentRepos;
            _env = env;
        }

        ///{
        ///    var employees = Enumerable.Empty<Employee>();
        ///
        ///    if (string.IsNullOrEmpty(searchInput))
        ///        employees = _unitOfWork.Repository<Employee>().GetAll();
        ///    else
        ///        employees = _unitOfWork.Repository<Employee>().SearchByName(searchInput);
        ///
        ///    var mappedEmployee = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);
        ///    return View(mappedEmployee);
        ///}

        public async Task<IActionResult> Index(string searchInput)
        {
            var employees = Enumerable.Empty<Employee>();
            var employeeRepo = _unitOfWork.Repository<Employee>() as EmployeeRepository;

            if (string.IsNullOrEmpty(searchInput))
                employees = await employeeRepo.GetAllAsync();
            else
                employees = employeeRepo.SearchByName(searchInput);

            var mappedEmployee = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);
            return View(mappedEmployee);

        }

        [HttpGet]

        public IActionResult Create()
        {
            //ViewBag.Departments = _departmentRepos.GetAll();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(EmployeeViewModel employeeVM)
        {
            if (ModelState.IsValid)
            {
                var fileName = await DocumentSettings.UploadFile(employeeVM.Image, "images");
                var mappedEmployee = _mapper.Map<EmployeeViewModel,Employee>(employeeVM);
                mappedEmployee.ImageName = fileName;
                _unitOfWork.Repository<Employee>().Add(mappedEmployee);
                var count = await _unitOfWork.Complete();
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(employeeVM);
        }

        public async Task<IActionResult> Details(int? id, string viewName = "Details")
        {
            if (!id.HasValue)
                return BadRequest();

            var employee = await _unitOfWork.Repository<Employee>().GetAsync(id.Value);
            var mappedEmployee = _mapper.Map<Employee, EmployeeViewModel>(employee);

            if (employee is null)
                return NotFound();
            if(viewName.Equals("Delete", StringComparison.OrdinalIgnoreCase) || viewName.Equals("Edit", StringComparison.OrdinalIgnoreCase));
            TempData["ImageName"] = employee.ImageName;

            await _unitOfWork.Complete();

            return View(viewName, mappedEmployee);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            //ViewBag.Departments = _departmentRepos.GetAll();
            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, EmployeeViewModel employeeVM)
        {
            if (id != employeeVM.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(employeeVM);

            try
            {
                if (employeeVM.Image == null)
                {
                    if (TempData["ImageName"] != null)
                        employeeVM.ImageName = TempData["ImageName"] as string;
                }
                else
                {
                    DocumentSettings.DeleteFile(TempData["ImageName"] as string, "Images");
                    employeeVM.ImageName = await DocumentSettings.UploadFile(employeeVM.Image, "Images");
                }
                var mappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                _unitOfWork.Repository<Employee>().Update(mappedEmployee);
                return RedirectToAction(nameof(Index));
            }
            catch (System.Exception ex)
            {
                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, "An Error Has Occured during Updating the employee");
                await _unitOfWork.Complete();

                return View(employeeVM);
            }
        }

        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");
        }

        [HttpPost]
        public IActionResult Delete(EmployeeViewModel employeeVM)
        {
            try
            {
                employeeVM.ImageName = TempData["ImageName"] as string;
                var mappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);

                _unitOfWork.Repository<Employee>().Delete(mappedEmployee);
                return RedirectToAction(nameof(Index));
            }
            catch (System.Exception ex)
            {
                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, "An Error Has Occured during Updating the employee");
                _unitOfWork.Complete();

                return View(employeeVM);
            }
        }
    }
}
