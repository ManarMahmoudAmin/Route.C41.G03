using Microsoft.AspNetCore.Mvc;
using Route.C41.G03.BLL.Interfaces;
using Route.C41.G03.BLL.Repositories;
using Route.C41.G03.DAL.Models;

namespace Route.C41.G03.PL.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentRepository _departmentsRepo;

        public DepartmentController(IDepartmentRepository departmentsRepo)
        {
            _departmentsRepo = departmentsRepo ;
        }

        public IActionResult Index(IDepartmentRepository departmentRepo)
        {
            var departments = _departmentsRepo.GetAll();
            return View(departments);
        }
        public IActionResult Create() {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Department department)
        {
            if(ModelState.IsValid) 
            {
                var count = _departmentsRepo.Add(department);
                if (count > 0)
                    return RedirectToAction(nameof(Index));
            }
            return View(department);
        }
    }
}
