using System.ComponentModel.DataAnnotations;
namespace ConsoleToWeb.Entities
{
    public class Employee
    {
        
        public string ID { get; set; }
        [Required]
        public string Name { get; set; }
        public string Department { get; set; }
        public double Salary { get; set; }
    }
}
