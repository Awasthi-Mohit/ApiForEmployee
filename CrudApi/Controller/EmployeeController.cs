using CrudApi.DTO;
using CrudApi.Model;
using CrudApi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CrudApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
       
        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        // GET: api/Employee
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var employees = await _employeeRepository.GetAllAsync();

            var employeeDTOs = employees.Select(e => new EmployeeDTO
            {
                Id = e.Id,
                Name = e.Name,
                Department = e.Department
            }).ToList();

            return Ok(employeeDTOs);
        }

        [Authorize(Roles = "Admin")]

        [HttpPost]
        public async Task<IActionResult> Create(EmployeeDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var employee = new Employee
            {
                Name = dto.Name,
                Department = dto.Department
            };

            await _employeeRepository.AddAsync(employee);

            dto.Id = employee.Id;

            return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null) return NotFound();

            var dto = new EmployeeDTO
            {
                Id = employee.Id,
                Name = employee.Name,
                Department = employee.Department
            };

            return Ok(dto);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, EmployeeDTO dto)
        {
            if (id != dto.Id) return BadRequest("Id mismatch");

            var existing = await _employeeRepository.GetByIdAsync(id);
            if (existing == null) return NotFound();

            // Map DTO → Model
            existing.Name = dto.Name;
            existing.Department = dto.Department;

            await _employeeRepository.UpdateAsync(existing);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _employeeRepository.DeleteAsync(id);
            if (!deleted) return NotFound();

            return NoContent();
        }
    }
}
