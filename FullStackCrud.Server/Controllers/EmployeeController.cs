using FullStackCrud.Server.Models;
using FullStackCrud.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace FullStackCrud.Server.Controllers
{
    [ApiController]
    [Route("api/employee")]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeService _employeeService;

        public EmployeeController(EmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Employee>>> Get() =>
            await _employeeService.GetAsync();

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Employee>> Get(string id)
        {
            var employee = await _employeeService.GetAsync(id);
            if (employee == null)
                return NotFound();

            return employee;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Employee employee)
        {
            await _employeeService.CreateAsync(employee);
            return CreatedAtAction(nameof(Get), new { id = employee.Id }, employee);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, Employee employee)
        {
            var existing = await _employeeService.GetAsync(id);
            if (existing == null)
                return NotFound();

            employee.Id = existing.Id;
            await _employeeService.UpdateAsync(id, employee);
            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var existing = await _employeeService.GetAsync(id);
            if (existing == null)
                return NotFound();

            await _employeeService.DeleteAsync(id);
            return NoContent();
        }
    }

    }

