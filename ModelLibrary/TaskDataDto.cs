using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeRecordsAdministration.Models
{
    public class TaskDataDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = String.Empty;
        public string Description { get; set; }
        public bool Completed { get; set; }
    }
}
