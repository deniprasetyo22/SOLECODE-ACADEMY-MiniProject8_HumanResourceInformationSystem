using Microsoft.EntityFrameworkCore;
using MiniProject5.Application.DTOs;
using MiniProject5.Application.Interfaces.IRepositories;
using MiniProject5.Persistence.Context;
using MiniProject5.Persistence.Models;
using MiniProject8.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProject5.Persistence.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly HrisContext _context;

        public DepartmentRepository(HrisContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Department>> GetAllDepartmentsAsync(paginationDto pagination)
        {
            var skipNumber = (pagination.pageNumber - 1) * pagination.pageSize;
            return await _context.Departments
                .Skip(skipNumber)
                .Take(pagination.pageSize)
                .ToListAsync();
        }

        public async Task<Department> GetDepartmentByIdAsync(int deptId)
        {
            return await _context.Departments.FindAsync(deptId);
        }

        public async Task<Department> AddDepartmentAsync(Department department)
        {
            _context.Departments.Add(department);
            await _context.SaveChangesAsync();
            return department;
        }

        public async Task UpdateDepartmentAsync(int deptId, Department department)
        {
            // Fetch the existing department from the database including locations
            var existingDept = await _context.Departments
                .Include(d => d.Locations)
                .FirstOrDefaultAsync(d => d.Deptid == deptId);

            if (existingDept == null)
            {
                throw new KeyNotFoundException($"Department with ID {deptId} not found.");
            }

            // Update department properties
            existingDept.Deptname = department.Deptname;
            existingDept.Mgrempid = department.Mgrempid;

            // Handle locations
            if (department.Locations != null)
            {
                // Find existing locations by their ID in the database
                var existingLocations = existingDept.Locations.ToList();

                // Remove locations that are not in the updated list
                var locationsToRemove = existingLocations
                    .Where(l => !department.Locations.Any(ul => ul.Locationid == l.Locationid))
                    .ToList();

                foreach (var location in locationsToRemove)
                {
                    existingDept.Locations.Remove(location);
                    _context.Locations.Remove(location);
                }

                // Add or update locations
                foreach (var updatedLocation in department.Locations)
                {
                    var existingLocation = existingLocations
                        .FirstOrDefault(l => l.Locationid == updatedLocation.Locationid);

                    if (existingLocation != null)
                    {
                        // Update existing location
                        existingLocation.Address = updatedLocation.Address;
                        // Update other properties if needed
                    }
                    else
                    {
                        // Add new location
                        existingDept.Locations.Add(new Location
                        {
                            Address = updatedLocation.Address,
                            // Set other properties as needed
                        });
                    }
                }
            }
            else
            {
                // Remove all locations if none are provided
                var locationsToRemove = existingDept.Locations.ToList();
                foreach (var location in locationsToRemove)
                {
                    _context.Locations.Remove(location);
                }
            }

            // Save changes to the database
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDepartmentAsync(int deptId)
        {
            var department = await _context.Departments.FindAsync(deptId);
            if (department != null)
            {
                _context.Departments.Remove(department);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IList<AverageSalaryDto>> GetAverageSalaryByDepartmentAsync()
        {
            return await _context.Employees
                .GroupBy(e => e.Deptid)
                .Select(g => new AverageSalaryDto
                {
                    DeptName = _context.Departments
                                .Where(d => d.Deptid == g.Key)
                                .Select(d => d.Deptname)
                                .FirstOrDefault() ?? "Unknown", // Default to "Unknown" if department is not found
                    AverageSalary = g.Average(e => e.Salary)
                })
                .ToListAsync();
        }
    }
}
