﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Route.C41.G03.DAL.Models
{
    public class Department
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Code Is Required!")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Name Is Required!")]

        public string Name { get; set; }
        //[Display(Name= "Date Of Creation")]
        public DateTime DateOfCreation { get; set; }

        public ICollection<Employee> Employees { get; set; }
    }
}
