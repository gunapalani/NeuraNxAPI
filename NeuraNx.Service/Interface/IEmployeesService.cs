using NeuraNx.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuraNx.Service.Interface
{
    public interface IEmployeesService
    {
        Task<List<EmployeeViewModel>> GetEmployees();
        Task<EmployeeViewModel?> GetEmployeeByIdAsync(Guid id);
        Task<EmployeeViewModel> AddEmployee(EmployeeViewModel model);
        Task<EmployeeViewModel> UpdateEmployee(Guid id, EmployeeViewModel model);
        Task DeleteEmployee(Guid Id);
    }
}
