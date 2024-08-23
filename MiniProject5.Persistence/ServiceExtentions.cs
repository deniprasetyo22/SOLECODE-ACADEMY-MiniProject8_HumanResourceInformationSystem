using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MiniProject5.Application.Interfaces.IRepositories;
using MiniProject5.Application.Interfaces.IServices;
using MiniProject5.Application.Services;
using MiniProject5.Persistence.Context;
using MiniProject5.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProject5.Persistence
{
    public static class ServiceExtentions
    {
        public static void ConfigurePersistence(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<HrisContext>(options => options.UseNpgsql(connectionString));
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<IWorksOnRepository, WorksOnRepository>();
            services.AddScoped<IWorksOnService, WorksOnService>();
        }
    }
}
