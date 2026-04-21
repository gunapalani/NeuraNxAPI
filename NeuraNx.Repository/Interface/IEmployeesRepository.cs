using NeuraNx.Models.ViewModels;
using NeuraNx.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuraNx.Repository.Interface
{
    public interface IEmployeesRepository
    {
        Task<List<Employee>> GetEmployees();
        Task<Employee?> GetEmployeeByIdAsync(Guid id);
        Task<Employee> AddEmployee(EmployeeViewModel model);
        Task<Employee> UpdateEmployee(Guid id, EmployeeViewModel model);
        Task<bool> DeleteEmployee(Guid Id);
        Task<bool> ExistsAsync(Guid id);
    }
}
