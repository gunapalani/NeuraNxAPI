using AutoMapper;
using NeuraNx.Models.ViewModels;
using NeuraNx.Repository.Interface;
using NeuraNx.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuraNx.Service.Implementation
{
    public class EmployeesService : IEmployeesService
    {
        public readonly IEmployeesRepository _employeesRepository;
        private readonly IMapper _mapper;

        public EmployeesService(IEmployeesRepository employeesRepository, IMapper mapper)
        {
            _employeesRepository = employeesRepository;
            _mapper = mapper;
        }

        public async Task<List<EmployeeViewModel>> GetEmployees()
        {
            var employees = await _employeesRepository.GetEmployees();
            return _mapper.Map<List<EmployeeViewModel>>(employees);
        }

        public async Task<EmployeeViewModel?> GetEmployeeByIdAsync(Guid id)
        {
            var employee = await _employeesRepository.GetEmployeeByIdAsync(id);
            return employee != null ? _mapper.Map<EmployeeViewModel>(employee) : null;
        }

        public async Task<EmployeeViewModel> AddEmployee(EmployeeViewModel model)
        {
            var createdEmployee = await _employeesRepository.AddEmployee(model);
            return _mapper.Map<EmployeeViewModel>(createdEmployee);
        }

        public async Task<EmployeeViewModel> UpdateEmployee(Guid id, EmployeeViewModel model)
        {
            var updatedEmployee = await _employeesRepository.UpdateEmployee(id, model);
            return _mapper.Map<EmployeeViewModel>(updatedEmployee);
        }

        public async Task DeleteEmployee(Guid Id)
        {
            await _employeesRepository.DeleteEmployee(Id);
        }
    }
}
