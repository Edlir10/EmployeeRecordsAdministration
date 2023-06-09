﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeRecordsAdministration.Entities
{
    public class ProjectData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Title { get; set; }
        [MaxLength(200)]
        public string Description { get; set; }
        //public bool Completed { get; set; }
        public ICollection<TaskData> Tasks { get; set; }
           = new List<TaskData>();

        public ProjectData(string title)
        {
            Title = title;
        }
    }
}
