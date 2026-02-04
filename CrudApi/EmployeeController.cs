using CrudApi.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CrudApi
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
        [HttpGet]
        public async Task<IActionResult>GetAll()
        {
            var employee=_employeeRepository.GetAllAsync(); 
            return Ok (employee);
        }

        [HttpPost]
        public async Task<IActionResult>Create(Employee employee)
        {
            if (ModelState == null)
            {
                return BadRequest();    
            }
            await _employeeRepository.AddAsync(employee);
            _employeeRepository.SaveChangesAsync();
            return Ok();    

        }
        public async Task<IActionResult>Update(int Id,Employee employee)
        {
            var employees = _employeeRepository.GetByIdAsync(Id);
            if(Id==null)
            {
                return BadRequest();

            }
            await _employeeRepository.UpdateAsync(employee);
            _employeeRepository.SaveChangesAsync();
            return Ok();    
        }

        public async Task<IActionResult>Delete(int Id)
        {

            var employee = _employeeRepository.GetByIdAsync(Id);
            if (Id == null)
            {
                return BadRequest();
            }
            await _employeeRepository.DeleteAsync(Id);
            _employeeRepository.SaveChangesAsync();
            return Ok();
        }
    }
}
