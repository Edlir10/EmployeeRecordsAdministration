using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeRecordsAdministration.Models
{
    public class TaskDataForUpdateDto
    {
        [Required]
        [MaxLength(50)]
        public string Title { get; set; }
        [MaxLength(200)]
        public string Description { get; set; }
        public bool Completed { get; set; }
    }
}
