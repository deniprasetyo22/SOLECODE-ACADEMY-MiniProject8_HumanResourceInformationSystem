using MiniProject8.Application.DTOs;
using MiniProject8.Application.Interfaces.IRepositories;
using MiniProject8.Application.Interfaces.IServices;
using MiniProject8.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProject8.Application.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentService(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public async Task<IEnumerable<Department>> GetAllDepartmentsNoPagesAsync()
        {
            return await _departmentRepository.GetAllDepartmentsAsync();
        }

        public async Task<object> GetAllDepartmentsAsync(QueryObjectDepartment query)
        {
            var department = await _departmentRepository.GetAllDepartmentsAsync();
            var temp = department.AsQueryable();

            if (!string.IsNullOrEmpty(query.Keyword))
            {
                string keywordLower = query.Keyword.ToLower();
                temp = temp.Where(b => b.Deptname.ToLower().Contains(keywordLower));
            }

            var total = temp.Count();

            if (!string.IsNullOrWhiteSpace(query.SortBy))
            {
                switch (query.SortBy.ToLower())
                {
                    case "deptname":
                        temp = query.SortOrder.Equals("asc", StringComparison.OrdinalIgnoreCase)
                            ? temp.OrderBy(s => s.Deptname)
                            : temp.OrderByDescending(s => s.Deptname);
                        break;
                    default:
                        temp = query.SortOrder.Equals("asc")
                            ? temp.OrderBy(s => s.Deptid)
                            : temp.OrderByDescending(s => s.Deptid);
                        break;
                }
            }

            var skipNumber = (query.PageNumber - 1) * query.PageSize;

            var deptList = temp.Skip(skipNumber).Take(query.PageSize).ToList();

            return new { total = total, data = deptList };
        }

        public async Task<Department> GetDepartmentByIdAsync(int deptId)
        {
            var department = await _departmentRepository.GetDepartmentByIdAsync(deptId);

            if (department == null)
            {
                throw new KeyNotFoundException($"Department with ID {deptId} not found.");
            }

            return department;
        }

        public async Task<Department> AddDepartmentAsync(Department department)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(department.Deptname))
                throw new ArgumentException("Department name is required.");

            if (department.Locations != null && department.Locations.Any(loc => string.IsNullOrWhiteSpace(loc.Address)))
                throw new ArgumentException("All locations must have a valid address.");

            // Add department
            return await _departmentRepository.AddDepartmentAsync(department);
        }

        public async Task UpdateDepartmentAsync(int deptId, Department department)
        {
            var existingDept = await _departmentRepository.GetDepartmentByIdAsync(deptId);

            if (existingDept == null)
                throw new KeyNotFoundException($"Department with ID {deptId} not found.");

            // Update fields
            existingDept.Deptname = department.Deptname ?? existingDept.Deptname;
            existingDept.Mgrempid = department.Mgrempid ?? existingDept.Mgrempid;

            // Handle locations
            if (department.Locations != null)
            {
                var existingLocations = existingDept.Locations.ToList();

                // Remove old locations not in the updated list
                var locationsToRemove = existingLocations
                    .Where(l => !department.Locations.Any(ul => ul.Locationid == l.Locationid))
                    .ToList();

                foreach (var location in locationsToRemove)
                {
                    existingDept.Locations.Remove(location);
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
                    }
                    else
                    {
                        // Add new location
                        existingDept.Locations.Add(new Location
                        {
                            Address = updatedLocation.Address
                        });
                    }
                }
            }
            else
            {
                // Remove all locations if none are provided
                existingDept.Locations.Clear();
            }

            // Save changes
            await _departmentRepository.UpdateDepartmentAsync(existingDept);
        }

        public async Task DeleteDepartmentAsync(int deptId)
        {
            var department = await _departmentRepository.GetDepartmentByIdAsync(deptId);

            if (department == null)
                throw new KeyNotFoundException($"Department with ID {deptId} not found.");

            await _departmentRepository.DeleteDepartmentAsync(deptId);
        }
    }
}
