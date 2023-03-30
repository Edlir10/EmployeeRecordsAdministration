using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeRecordsAdministration.Entities
{
    public class TaskData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Title { get; set; }

        [MaxLength(200)]
        public string Description { get; set; }
        public bool Completed { get; set; }
        [ForeignKey("ProjectDataId")]
        public ProjectData ProjectData { get; set; }
        public int ProjectDataId { get; set; }
        public TaskData(string title)
        {
            Title = title;
        }
    }
}
