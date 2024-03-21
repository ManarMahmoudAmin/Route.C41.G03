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
    internal class DepartmentRepository : IDepartmentRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public DepartmentRepository(ApplicationDbContext dbContext)
        {
            dbContext = _dbContext;
        }
        public int Add(Department entity)
        {
            _dbContext.Departments.Add(entity);
            return _dbContext.SaveChanges();
        }

        public int Update(Department entity)
        {
            _dbContext.Departments.Update(entity);
            return _dbContext.SaveChanges();
        }
        public int Delete(Department entity)
        {
            _dbContext.Departments.Remove(entity);
            return _dbContext.SaveChanges();
        }

        public IEnumerable<Department> GetAll()
            => _dbContext.Departments.AsNoTracking().ToList();
        

        public Department GetById(int id)
        {
            //return _dbContext.Departments.Find(id);
            return _dbContext.Find<Department>(id);
            ///var department = _dbContext.Departments.Local.Where(D => D.Id == id).FirstOrDefault();
            ///if (department == null) 
            ///    department = _dbContext.Departments.Where(D => D.Id == id).FirstOrDefault();
            ///return department;
        }

    }
}
