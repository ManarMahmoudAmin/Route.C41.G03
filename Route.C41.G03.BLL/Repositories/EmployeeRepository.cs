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
    internal class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public EmployeeRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public int Add(Employee employee)
        {
            _dbContext.Add(employee);
            return _dbContext.SaveChanges();
        }

        public int Delete(Employee employee)
        {
            _dbContext.Remove(employee);
            return _dbContext.SaveChanges();
        }
        public int Update(Employee employee)
        {
            _dbContext.Update(employee);
            return _dbContext.SaveChanges();
        }

        public Employee Get(int id)
        {
            ///var employee = _dbContext.Employees.Local.Where(E => E.Id == id).FirstOrDefault();
            ///if(employee == null)
            ///{
            ///    employee = _dbContext.Employees.Where(E => E.Id == id).FirstOrDefault();
            ///}
            ///return employee;

            return _dbContext.Find<Employee>(id);
        }

        public IEnumerable<Employee> GetAll()
        =>   _dbContext.Employees.AsNoTracking().ToList();
        

    }
}
