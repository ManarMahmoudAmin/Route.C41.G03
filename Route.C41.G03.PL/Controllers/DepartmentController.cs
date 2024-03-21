using Microsoft.AspNetCore.Mvc;
using Route.C41.G03.BLL.Interfaces;
using Route.C41.G03.BLL.Repositories;

namespace Route.C41.G03.PL.Controllers
{
    public class DepartmentController : Controller
    {
        private IDepartmentRepository _departmentsRepo { get; }

        public DepartmentController(IDepartmentRepository departmentsRepo)
        {
            _departmentsRepo = departmentsRepo ;
        }

        public IActionResult Index(IDepartmentRepository departmentRepo)
        {
            return View();
        }
    }
}
