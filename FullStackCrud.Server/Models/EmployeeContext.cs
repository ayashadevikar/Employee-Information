﻿using Microsoft.EntityFrameworkCore;

namespace FullStackCrud.Server.Models
{
    public class EmployeeContext: DbContext
    {
        public EmployeeContext(DbContextOptions<EmployeeContext> options) : base(options)
        {
            
        }

        public DbSet<Employee> Employees { get; set; } 
    }
}
