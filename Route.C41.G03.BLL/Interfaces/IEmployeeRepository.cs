using Route.C41.G03.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Route.C41.G03.BLL.Interfaces
{
    internal interface IEmployeeRepository
    {
        int Add(Employee employee);
        int Update(Employee employee);
        int Delete(Employee employee);
        Employee Get(int id);
        IEnumerable<Employee> GetAll(); 

    }
}
