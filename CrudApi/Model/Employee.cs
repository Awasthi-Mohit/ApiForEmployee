using System.ComponentModel.DataAnnotations;

namespace CrudApi.Model
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Department { get; set; }
        public decimal Salary { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsActive { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }

    }
}
