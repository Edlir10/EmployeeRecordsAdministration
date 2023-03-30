using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeRecordsAdministration.Models
{
    public class ProjectDataDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        //public bool Completed { get; set; }
        public ICollection<TaskDataDto> Tasks { get; set; }
           = new List<TaskDataDto>();
    }
}
