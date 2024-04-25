using Microsoft.Extensions.DependencyInjection;
using Route.C41.G03.BLL;
using Route.C41.G03.BLL.Interfaces;
using Route.C41.G03.BLL.Repositories;
using Route.C41.G03.PL.Helpers;
using Route.C41.G03.PL.Services;

namespace Route.C41.G03.PL.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
			services.AddTransient<IEmailSender, EmailSender>();
			services.AddScoped<IUnitOfWork, UnitOfWork>();
			//services.AddScoped<IDepartmentRepository, DepartmentRepository>();
			//services.AddScoped<IEmployeeRepository, EmployeeRepository>();
			services.AddAutoMapper(M => M.AddProfile(new MappingProfiles()));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
