using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniProject5.Application.DTOs;
using MiniProject5.Application.Interfaces.IRepositories;
using MiniProject5.Persistence.Context;
using MiniProject5.Persistence.Models;
using MiniProject6.Application.DTOs;
using MiniProject6.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MiniProject5.Persistence.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly HrisContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<AppUser> _userManager;

        public EmployeeRepository(HrisContext context, IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }
        
        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync(paginationDto pagination)
        {
            var skipNumber = (pagination.pageNumber - 1) * pagination.pageSize;
            return await _context.Employees
                .Skip(skipNumber)
                .Take(pagination.pageSize)
                .ToListAsync();
        }

        public async Task<Employee> GetEmployeeByIdAsync(int empId)
        {
            var employee = await _context.Employees
                .Where(e => e.Empid == empId)
                .Include(e => e.Dependents)
                .FirstOrDefaultAsync();
            return employee;
        }

        public async Task<EmployeeDto> GetOwnProfile()
        {
            var user = _httpContextAccessor.HttpContext?.User.Identity!.Name;

            var currentUser = await _userManager.FindByNameAsync(user!);

            var userId = currentUser?.Id;

            if (string.IsNullOrEmpty(userId))
            {
                // Log jika userId tidak ditemukan
                Console.WriteLine("User ID tidak ditemukan di HttpContext.");
                return null;
            }

            var employee = await _context.Employees
                .Include(e => e.Dependents)
                .FirstOrDefaultAsync(e => e.userId == userId);

            if (employee == null)
            {
                // Log jika employee tidak ditemukan
                Console.WriteLine($"Karyawan dengan User ID {userId} tidak ditemukan.");
                return null;
            }

            return new EmployeeDto
            {
                Empid = employee.Empid,
                Fname = employee.Fname,
                Lname = employee.Lname,
                Email = employee.Email,
                Address = employee.Address,
                Position = employee.Position,
                Sex = employee.Sex,
                Dob = employee.Dob,
                Phoneno = employee.Phoneno,
                Emptype = employee.Emptype,
                Level = employee.Level,
                Deptid = employee.Deptid,
                SupervisorId = employee.SupervisorId,
                Status = employee.Status,
                Reason = employee.Reason,
                Lastupdateddate = employee.Lastupdateddate,
                Dependents = employee.Dependents?.Select(d => new DependentDto
                {
                    Dependentid = d.Dependentid,
                    fName = d.fName,
                    lName = d.lName,
                    Sex = d.Sex,
                    Dob = d.Dob,
                    Relationship = d.Relationship
                }).ToList() ?? new List<DependentDto>()
            };
        }

        public async Task<bool> UpdateOwnProfile(EmployeeDto employeeDto)
        {
            var user = _httpContextAccessor.HttpContext?.User.Identity!.Name;

            var currentUser = await _userManager.FindByNameAsync(user!);

            var userId = currentUser?.Id;

            if (string.IsNullOrEmpty(userId))
            {
                // Log jika userId tidak ditemukan
                Console.WriteLine("User ID tidak ditemukan di HttpContext.");
                return false;
            }

            var existingEmployee = await _context.Employees
                .Include(e => e.Dependents)
                .FirstOrDefaultAsync(e => e.userId == userId);

            if (existingEmployee == null)
            {
                // Log jika employee tidak ditemukan
                Console.WriteLine($"Karyawan dengan User ID {userId} tidak ditemukan.");
                return false;
            }

            // Update the existing employee's properties
            existingEmployee.Fname = employeeDto.Fname;
            existingEmployee.Lname = employeeDto.Lname;
            existingEmployee.Email = employeeDto.Email;
            existingEmployee.Address = employeeDto.Address;
            existingEmployee.Position = employeeDto.Position;
            existingEmployee.Sex = employeeDto.Sex;
            existingEmployee.Dob = employeeDto.Dob;
            existingEmployee.Phoneno = employeeDto.Phoneno;
            existingEmployee.Emptype = employeeDto.Emptype;
            existingEmployee.Level = employeeDto.Level;
            existingEmployee.Deptid = employeeDto.Deptid;
            existingEmployee.SupervisorId = employeeDto.SupervisorId;
            existingEmployee.Status = employeeDto.Status;
            existingEmployee.Reason = employeeDto.Reason;
            existingEmployee.Lastupdateddate = DateTime.Now;

            // Handle dependents
            if (employeeDto.Dependents != null)
            {
                // Find existing dependents by their ID
                var existingDependents = existingEmployee.Dependents.ToList();

                // Remove dependents that are not in the updated list
                var dependentsToRemove = existingDependents
                    .Where(d => !employeeDto.Dependents.Any(ud => ud.Dependentid == d.Dependentid))
                    .ToList();

                foreach (var dependent in dependentsToRemove)
                {
                    _context.Dependents.Remove(dependent);
                }

                // Add or update dependents
                foreach (var updatedDependentDto in employeeDto.Dependents)
                {
                    var existingDependent = existingDependents
                        .FirstOrDefault(d => d.Dependentid == updatedDependentDto.Dependentid);

                    if (existingDependent != null)
                    {
                        // Update existing dependent
                        existingDependent.fName = updatedDependentDto.fName;
                        existingDependent.lName = updatedDependentDto.lName;
                        existingDependent.Sex = updatedDependentDto.Sex;
                        existingDependent.Dob = updatedDependentDto.Dob;
                        existingDependent.Relationship = updatedDependentDto.Relationship;
                    }
                    else
                    {
                        // Add new dependent
                        existingEmployee.Dependents.Add(new Dependent
                        {
                            fName = updatedDependentDto.fName,
                            lName = updatedDependentDto.lName,
                            Sex = updatedDependentDto.Sex,
                            Dob = updatedDependentDto.Dob,
                            Relationship = updatedDependentDto.Relationship
                        });
                    }
                }
            }
            else
            {
                _context.Dependents.RemoveRange(existingEmployee.Dependents);
            }

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saat update profile: {ex.Message}");
                return false;
            }
        }


        public async Task<Employee> AddEmployeeAsync(Employee employee)
        {
            employee.Status = "Active";
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return employee;
        }

        public async Task UpdateEmployeeAsync(int empId, Employee employee)
        {
            var existingEmployee = await _context.Employees
                .Include(e => e.Dependents) // Include dependents if needed
                .FirstOrDefaultAsync(e => e.Empid == empId);

            // Update properties
            existingEmployee.Fname = employee.Fname;
            existingEmployee.Lname = employee.Lname;
            existingEmployee.Ssn = employee.Ssn;
            existingEmployee.Email = employee.Email;
            existingEmployee.Address = employee.Address;
            existingEmployee.Position = employee.Position;
            existingEmployee.Salary = employee.Salary;
            existingEmployee.Sex = employee.Sex;
            existingEmployee.Dob = employee.Dob;
            existingEmployee.Phoneno = employee.Phoneno;
            existingEmployee.Emptype = employee.Emptype;
            existingEmployee.Level = employee.Level;
            existingEmployee.Deptid = employee.Deptid;
            existingEmployee.Lastupdateddate = DateTime.Now;
            existingEmployee.SupervisorId = employee.SupervisorId;

            // Handle dependents
            if (employee.Dependents != null)
            {
                // Find existing dependents by their ID in the database
                var existingDependents = existingEmployee.Dependents.ToList();

                // Remove dependents that are not in the updated list
                var dependentsToRemove = existingDependents
                    .Where(d => !employee.Dependents.Any(ud => ud.Dependentid == d.Dependentid))
                    .ToList();

                foreach (var dependent in dependentsToRemove)
                {
                    _context.Dependents.Remove(dependent);
                }

                // Add or update dependents
                foreach (var updatedDependent in employee.Dependents)
                {
                    var existingDependent = existingDependents
                        .FirstOrDefault(d => d.Dependentid == updatedDependent.Dependentid);

                    if (existingDependent != null)
                    {
                        // Update existing dependent
                        existingDependent.fName = updatedDependent.fName;
                        existingDependent.lName = updatedDependent.lName;
                        existingDependent.Sex = updatedDependent.Sex;
                        existingDependent.Dob = updatedDependent.Dob;
                        existingDependent.Relationship = updatedDependent.Relationship;
                    }
                    else
                    {
                        // Add new dependent
                        existingEmployee.Dependents.Add(new Dependent
                        {
                            fName = updatedDependent.fName,
                            lName = updatedDependent.lName,
                            Sex = updatedDependent.Sex,
                            Dob = updatedDependent.Dob,
                            Relationship = updatedDependent.Relationship
                        });
                    }
                }
            }
            else
            {
                // Remove all dependents if none are provided
                _context.Dependents.RemoveRange(existingEmployee.Dependents);
            }

            // Save changes to the database
            await _context.SaveChangesAsync();
        }


        public async Task DeactivateEmployeeAsync(int empId, string reason)
        {
            var employee = await _context.Employees.FindAsync(empId);
            if (employee != null)
            {
                employee.Status = "Not Active";
                employee.Reason = reason;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteEmployeeAsync(int empId)
        {
            var employee = await _context.Employees.FindAsync(empId);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Employee>> SearchEmployee(searchDto search, paginationDto pagination)
        {
            var emp = _context.Employees.AsQueryable();

            if (search.empId.HasValue)
            {
                emp = emp.Where(cek => cek.Empid == search.empId.Value);
            }

            if (!string.IsNullOrEmpty(search.fName))
            {
                emp = emp.Where(cek => cek.Fname.ToLower().Contains(search.fName.ToLower()));
            }

            if (!string.IsNullOrEmpty(search.lName))
            {
                emp = emp.Where(cek => cek.Lname.ToLower().Contains(search.lName.ToLower()));
            }

            if (!string.IsNullOrEmpty(search.ssn))
            {
                emp = emp.Where(cek => cek.Ssn.ToLower().Contains(search.ssn.ToLower()));
            }

            if (!string.IsNullOrEmpty(search.address))
            {
                emp = emp.Where(cek => cek.Address.ToLower().Contains(search.address.ToLower()));
            }

            if (!string.IsNullOrEmpty(search.position))
            {
                emp = emp.Where(cek => cek.Position.ToLower().Contains(search.position.ToLower()));
            }

            if (!string.IsNullOrEmpty(search.sex))
            {
                emp = emp.Where(cek => cek.Sex.ToLower().Contains(search.sex.ToLower()));
            }

            if (!string.IsNullOrEmpty(search.empType))
            {
                emp = emp.Where(cek => cek.Emptype.ToLower().Contains(search.empType.ToLower()));
            }

            if (search.level.HasValue)
            {
                emp = emp.Where(cek => cek.Level == search.level.Value);
            }

            if (search.deptId.HasValue)
            {
                emp = emp.Where(cek => cek.Deptid == search.deptId.Value);
            }

            if (!string.IsNullOrEmpty(search.status))
            {
                emp = emp.Where(cek => cek.Status.ToLower().Contains(search.status.ToLower()));
            }

            // Apply sorting
            if (!string.IsNullOrEmpty(pagination.orderBy))
            {
                switch (pagination.orderBy.ToLower())
                {
                    case "empid":
                        emp = emp.OrderBy(e => e.Empid);
                        break;
                    case "fname":
                        emp = emp.OrderBy(e => e.Fname);
                        break;
                    case "lname":
                        emp = emp.OrderBy(e => e.Lname);
                        break;
                    case "ssn":
                        emp = emp.OrderBy(e => e.Ssn);
                        break;
                    case "address":
                        emp = emp.OrderBy(e => e.Address);
                        break;
                    case "position":
                        emp = emp.OrderBy(e => e.Position);
                        break;
                    case "sex":
                        emp = emp.OrderBy(e => e.Sex);
                        break;
                    case "emptype":
                        emp = emp.OrderBy(e => e.Emptype);
                        break;
                    case "level":
                        emp = emp.OrderBy(e => e.Level);
                        break;
                    case "deptid":
                        emp = emp.OrderBy(e => e.Deptid);
                        break;
                    case "status":
                        emp = emp.OrderBy(e => e.Status);
                        break;
                    default:
                        throw new ArgumentException($"Invalid order by property: {pagination.orderBy}");
                }
            }

            var skipNumber = (pagination.pageNumber - 1) * pagination.pageSize;

            return await emp
                .Skip(skipNumber)
                .Take(pagination.pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<Employee>> GetSupervisedEmployeesAsync(int supervisorId)
        {
            return await _context.Employees
                .Where(e => e.SupervisorId == supervisorId)
                .ToListAsync();
        }



    }
}
