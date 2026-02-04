using CrudApi.Data;
using CrudApi.Model;
using Microsoft.EntityFrameworkCore;

namespace CrudApi
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _context;
        public EmployeeRepository(AppDbContext context)
        {
                 _context=context;   
        }
        public async Task<Employee> AddAsync(Employee employee)
        {
           _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return employee;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var emp = await _context.Employees.FindAsync(id);
            if (emp == null) return false;

            _context.Employees.Remove(emp);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Employee>> GetAllAsync()
        {
            return await _context.Employees.ToListAsync();
        }

        public async Task<Employee?> GetByIdAsync(int id)
        {
            return await _context.Employees.FindAsync(id);
        }

        public void SaveChangesAsync()
        {
           _context.SaveChanges();
        }

        public async Task<Employee?> UpdateAsync(Employee employee)
        {
            var existing = await _context.Employees.FindAsync(employee.Id);
            if (existing == null) return null;

            existing.Name = employee.Name;
            existing.Department = employee.Department;
            existing.Salary = employee.Salary;

            await _context.SaveChangesAsync();
            return existing;
        }
    }
}
