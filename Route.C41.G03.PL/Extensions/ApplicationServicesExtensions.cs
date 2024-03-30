﻿using Microsoft.Extensions.DependencyInjection;
using Route.C41.G03.BLL;
using Route.C41.G03.BLL.Interfaces;
using Route.C41.G03.BLL.Repositories;

namespace Route.C41.G03.PL.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            //services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            //services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}
