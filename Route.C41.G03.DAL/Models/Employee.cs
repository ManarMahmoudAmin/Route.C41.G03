﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Route.C41.G03.DAL.Models
{
    public enum Gender
    {
        [EnumMember(Value = "Male")]
        Male =1, 
        [EnumMember(Value = "Female")]
        Female = 2
    }
    public enum EmpType
    {
        FullTime =1,
        PartTime = 2
    }
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Age { get; set; }
        public string Address { get; set; }
        public decimal Salary { get; set; }
        public bool IsActive { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public Gender Gender { get; set; }
        public EmpType EmployeeType { get; set; }
        public DateTime HiringDate { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsDeleted { get; set; }
        public string ImageName { get; set; }   
        //[ForeignKey("Department")]
        public int? DepartmentId {  get; set; }
        //[InverseProperty(nameof(Models.Department.Employees))]
        public Department Department { get; set; }
    }
}
