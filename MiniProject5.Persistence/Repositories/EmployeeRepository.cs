using Microsoft.EntityFrameworkCore;
using MiniProject8.Application.Interfaces.IRepositories;
using MiniProject8.Domain.Models;
using MiniProject8.Persistence.Context;

namespace MiniProject8.Persistence.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly HrisContext _context;

        public EmployeeRepository(HrisContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            return await _context.Employees
                .Include(b=>b.Dept)
                .Include(b=>b.Leaverequests)
                .ToListAsync();
        }

        public async Task<Employee> GetEmployeeByIdAsync(int empId)
        {
            return await _context.Employees.Include(e => e.Dependents).FirstOrDefaultAsync(e => e.Empid == empId);
        }

        public async Task<Employee> AddEmployeeAsync(Employee employee)
        {
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return employee;
        }

        public async Task UpdateEmployeeAsync(int empId, Employee employee)
        {
            var existingEmployee = await _context.Employees
                .Include(e => e.Dependents)
                .FirstOrDefaultAsync(e => e.Empid == empId);

            if (existingEmployee == null)
            {
                throw new KeyNotFoundException($"Employee with ID {empId} not found.");
            }

            // Update properties from the provided employee object
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

            // Update the employee in the context
            _context.Employees.Update(existingEmployee);
            await _context.SaveChangesAsync();
        }

        public async Task DeactivateEmployeeAsync(Employee employee)
        {
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
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

        public async Task<Employee> GetEmployeeByUserIdAsync(string userId)
        {
            return await _context.Employees
                .Include(e => e.Dependents) // Include dependents if needed
                .FirstOrDefaultAsync(e => e.userId == userId);
        }

        public async Task DeleteDependentAsync(int dependentId)
        {
            var dependent = await _context.Dependents.FindAsync(dependentId);
            if (dependent != null)
            {
                _context.Dependents.Remove(dependent);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Employee>> GetSupervisedEmployeesAsync(int supervisorId)
        {
            return await _context.Employees
                .Where(e => e.SupervisorId == supervisorId)
                .ToListAsync();
        }
    }
}
