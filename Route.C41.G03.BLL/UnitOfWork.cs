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
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ApplicationDbContext _dbContext;

        private Hashtable ObjectRepository;

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            //EmployeeRepository= new EmployeeRepository(dbContext);
            //DepartmentRepository= new DepartmentRepository(dbContext);
            _dbContext = dbContext;
            ObjectRepository = new Hashtable();
        }
        //public IEmployeeRepository EmployeeRepository { get; set ; }
        //public IDepartmentRepository DepartmentRepository { get ; set; }

        public int Complete()
        {
            return _dbContext.SaveChanges();
        }

        public void Dispose()
        => _dbContext.Dispose();

        public IGenericRepository<T> Repository<T>() where T : class
        {
            var Key = typeof(T).Name;

            if (!ObjectRepository.ContainsKey(Key))
            {
                if (Key == nameof(Employee))
                {
                    var repository = new EmployeeRepository(_dbContext);
                    ObjectRepository.Add(Key, repository);
                }
                else
                {
                    var repository = new GenericRepository<T>(_dbContext);
                    ObjectRepository.Add(Key, repository);
                }
            }
            return new GenericRepository<T>(_dbContext);
        }
    }
}