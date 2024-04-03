using AutoMapper;
using Route.C41.G03.DAL.Models;
using Route.C41.G03.PL.ViewModels;

namespace Route.C41.G03.PL.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Employee, EmployeeViewModel>().ReverseMap();
        }
    }
}
