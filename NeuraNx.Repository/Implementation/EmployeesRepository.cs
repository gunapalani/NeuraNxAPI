using Microsoft.EntityFrameworkCore;
using NeuraNx.Models.ViewModels;
using NeuraNx.Repository.Entities;
using NeuraNx.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuraNx.Repository.Implementation
{
    public class EmployeesRepository : IEmployeesRepository
    {
        private readonly AppDbContext _dbContext;
        
        public EmployeesRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Employee>> GetEmployees()
        {
            return await _dbContext.Employees.ToListAsync();
        }

        public async Task<Employee?> GetEmployeeByIdAsync(Guid id)
        {
            return await _dbContext.Employees
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Employee> AddEmployee(EmployeeViewModel model)
        {
            var newEmployee = new Employee
            {
                Name = model.Name,
                Designation = model.Designation,
                Email = model.Email,
                Phone = model.Phone,
                CreatedOn = DateTimeOffset.UtcNow,
                UpdatedOn = DateTimeOffset.UtcNow
            };
            
            _dbContext.Employees.Add(newEmployee);
            await _dbContext.SaveChangesAsync();
            return newEmployee;
        }

        public async Task<Employee> UpdateEmployee(Guid id, EmployeeViewModel model)
        {
            var existingEmployee = await _dbContext.Employees
                .FirstOrDefaultAsync(e => e.Id == id);

            if (existingEmployee == null)
            {
                throw new KeyNotFoundException($"Employee with ID {id} not found.");
            }

            existingEmployee.Name = model.Name;
            existingEmployee.Designation = model.Designation;
            existingEmployee.Email = model.Email;
            existingEmployee.Phone = model.Phone;
            existingEmployee.UpdatedOn = DateTimeOffset.UtcNow;

            await _dbContext.SaveChangesAsync();
            return existingEmployee;
        }

        public async Task<bool> DeleteEmployee(Guid id)
        {
            var employee = await _dbContext.Employees
                .FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null)
                return false;

            _dbContext.Employees.Remove(employee);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _dbContext.Employees.AnyAsync(e => e.Id == id);
        }
    }
}
