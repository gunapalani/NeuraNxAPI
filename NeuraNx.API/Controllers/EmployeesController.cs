using Microsoft.AspNetCore.Mvc;
using NeuraNx.Models.ViewModels;
using NeuraNx.Service.Interface;

namespace NeuraNx.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly ILogger<EmployeesController> _logger;
        private readonly IEmployeesService _employeesService;

        public EmployeesController(ILogger<EmployeesController> logger, IEmployeesService employeesService)
        {
            _logger = logger;
            _employeesService = employeesService;
        }

        /// <summary>
        /// Get all employees
        /// </summary>
        /// <returns>List of employees</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeViewModel>>> GetAllEmployees()
        {
            try
            {
                var employees = await _employeesService.GetEmployees();
                return Ok(employees);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all employees");
                return StatusCode(500, new { message = "An error occurred while retrieving employees.", error = ex.Message });
            }
        }

        /// <summary>
        /// Get an employee by ID
        /// </summary>
        /// <param name="id">Employee ID</param>
        /// <returns>Employee details</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeViewModel>> GetEmployeeById(Guid id)
        {
            try
            {
                var employee = await _employeesService.GetEmployeeByIdAsync(id);
                if (employee == null)
                {
                    return NotFound(new { message = $"Employee with ID {id} not found." });
                }

                return Ok(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting employee with ID: {id}", id);
                return StatusCode(500, new { message = "An error occurred while retrieving the employee.", error = ex.Message });
            }
        }

        /// <summary>
        /// Create a new employee
        /// </summary>
        /// <param name="model">Employee creation data</param>
        /// <returns>Created employee</returns>
        [HttpPost]
        public async Task<ActionResult<EmployeeViewModel>> CreateEmployee([FromBody] EmployeeViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var createdEmployee = await _employeesService.AddEmployee(model);
                return CreatedAtAction(nameof(GetEmployeeById), new { id = createdEmployee.Id }, createdEmployee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating employee");
                return StatusCode(500, new { message = "An error occurred while creating the employee.", error = ex.Message });
            }
        }

        /// <summary>
        /// Update an existing employee
        /// </summary>
        /// <param name="id">Employee ID</param>
        /// <param name="model">Employee update data</param>
        /// <returns>Updated employee</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<EmployeeViewModel>> UpdateEmployee(Guid id, [FromBody] EmployeeViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var updatedEmployee = await _employeesService.UpdateEmployee(id, model);
                return Ok(updatedEmployee);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating employee with ID: {id}", id);
                return StatusCode(500, new { message = "An error occurred while updating the employee.", error = ex.Message });
            }
        }

        /// <summary>
        /// Delete an employee
        /// </summary>
        /// <param name="id">Employee ID</param>
        /// <returns>No content</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEmployee(Guid id)
        {
            try
            {
                await _employeesService.DeleteEmployee(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting employee with ID: {id}", id);
                return StatusCode(500, new { message = "An error occurred while deleting the employee.", error = ex.Message });
            }
        }
    }
}
