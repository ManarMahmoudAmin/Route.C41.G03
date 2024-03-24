using Microsoft.EntityFrameworkCore;
using Route.C41.G03.BLL.Interfaces;
using Route.C41.G03.DAL.Data;
using Route.C41.G03.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Route.C41.G03.BLL.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public EmployeeRepository(ApplicationDbContext dbContext):base(dbContext)
        {
            _dbContext = dbContext;
        }
        public IQueryable GetEmployeesByAddress(string address)
        {
            return _dbContext.Employees.Where(E => E.Address == address);
        }
    }
}
