using Microsoft.EntityFrameworkCore;
using Route.C41.G03.BLL.Interfaces;
using Route.C41.G03.DAL.Data;
using Route.C41.G03.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Route.C41.G03.BLL.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(ApplicationDbContext dbContext):base(dbContext)
        {
        }
        public IQueryable GetEmployeesByAddress(string address)
        {
            return _dbContext.Employees.Where(E => E.Address == address);
        }

        public IEnumerable<Employee> SearchByName(string name)
            => _dbContext.Employees.Where(E => E.Name.ToLower().Contains(name.ToLower()));

    }
}
