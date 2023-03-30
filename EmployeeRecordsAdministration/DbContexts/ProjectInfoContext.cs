using EmployeeRecordsAdministration.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeRecordsAdministration.DbContexts
{
    public class ProjectInfoContext : IdentityDbContext //DbContext
    {
        public DbSet<ProjectData> Projects { get; set; } = null!;
        public DbSet<TaskData> Tasks { get; set; } = null!;

        public ProjectInfoContext(DbContextOptions<ProjectInfoContext> options)
            : base(options)
        {

        }
    }
}
