﻿using CompanyWeb.Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Infrastructure
{
    public partial class CompanyDbContext : DbContext
    {
        public CompanyDbContext(DbContextOptions<CompanyDbContext> options) : base(options) { }

        public virtual DbSet<Departement> Departements { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<Workson> Worksons { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<DepartementLocation> DepartementLocations { get; set; }
        public virtual DbSet<EmployeeDependent> EmployeeDependents { get; set; }

    }
}
