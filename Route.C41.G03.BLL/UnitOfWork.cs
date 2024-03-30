using Route.C41.G03.BLL.Interfaces;
using Route.C41.G03.BLL.Repositories;
using Route.C41.G03.DAL.Data;
using Route.C41.G03.DAL.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Route.C41.G03.BLL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;
        private Hashtable _repositories;
        public IGenericRepository<T> Repository<T>() where T : class
        {
            var key = typeof(T).Name;

            if(!_repositories.ContainsKey(key))
            {
                if(key == nameof(Employee))
                {
                    var repository = new EmployeeRepository(_dbContext);
                    _repositories.Add(key, repository);
                }
                else
                {
                    var repository = new GenericRepository<T>(_dbContext);
                    _repositories.Add(key, repository);
                }
            }
            return _repositories[key] as IGenericRepository<T>;
        }

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;

        }
        public int Complete()
        {
           return _dbContext.SaveChanges();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
