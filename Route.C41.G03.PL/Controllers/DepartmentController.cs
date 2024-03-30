using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Route.C41.G03.BLL;
using Route.C41.G03.BLL.Interfaces;
using Route.C41.G03.BLL.Repositories;
using Route.C41.G03.DAL.Models;

namespace Route.C41.G03.PL.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        //private readonly IDepartmentRepository _departmentsRepo;
        private readonly IWebHostEnvironment _env;

        public DepartmentController(IUnitOfWork unitOfWork/*IDepartmentRepository departmentsRepo*/, IWebHostEnvironment env)
        {
            _unitOfWork = unitOfWork;
            //_departmentsRepo = departmentsRepo ;
            _env = env;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var departments = _unitOfWork.Repository<Department>().GetAll();
            _unitOfWork.Complete();
            return View(departments);
        }
        [HttpGet]

        public IActionResult Create() {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Department department)
        {
            if(ModelState.IsValid)
            {
                _unitOfWork.Repository<Department>().Add(department);
                var count = _unitOfWork.Complete();
                if (count > 0)
                    TempData["Message"] = "Department Is Created Successfully";
                else
                    TempData["Message"] = "An Error Has Occurred, Department Is Not Created ";
                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }

        public IActionResult Details(int? id, string viewName = "Details")
        {
            if (!id.HasValue)
                return BadRequest();

            var department = _unitOfWork.Repository<Department>().Get(id.Value);

            if(department is null)
                return NotFound();
            _unitOfWork.Complete();


            return View(viewName, department);
        }
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            return Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, Department department)
        {
            if(id != department.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(department);

            try 
            {
                _unitOfWork.Repository<Department>().Update(department);
                return RedirectToAction(nameof(Index));
            }
            catch(System.Exception ex) 
            {
                if(_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, "An Error Has Occured during Updating the Department");
                _unitOfWork.Complete();

                return View(department);
            }
        }

        public IActionResult Delete(int? id)
        {
            return Details(id, "Delete");
        }

        [HttpPost]
        public IActionResult Delete(Department department)
        {
            try
            {
                _unitOfWork.Repository<Department>().Delete(department);
                return RedirectToAction(nameof(Index));
            }
            catch(System.Exception ex)
            {
                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, "An Error Has Occured during Updating the Department");
                _unitOfWork.Complete();

                return View(department);
            }
        }
    }
}
