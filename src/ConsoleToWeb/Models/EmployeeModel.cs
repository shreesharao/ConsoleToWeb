using Microsoft.EntityFrameworkCore;
using ConsoleToWeb.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using JetBrains.Annotations;

namespace ConsoleToWeb.Models
{
    public class EmployeeModel : DbContext
    {
        private readonly ILogger _logger = null;
        private readonly IConfiguration _configuration = null;
        public EmployeeModel(DbContextOptions<EmployeeModel> options,ILogger<EmployeeModel> logger,IConfiguration configuration) : base(options)
        {
            _logger = logger;
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder dbContextOptionsBuilder)
        {
            _logger.LogInformation("DbContext OnConfiguring executing");
          //  dbContextOptionsBuilder.UseSqlServer(_configuration.GetConnectionString("SQLServerConnection"));    //i can not get connectionstring here
        }

        public DbSet<Employee> Employees { get; set; }
    }
}
