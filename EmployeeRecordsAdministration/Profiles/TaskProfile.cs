using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace EmployeeRecordsAdministration.Profiles
{
    public class TaskProfile : Profile
    {
        public TaskProfile()
        {
            CreateMap<Entities.TaskData, Models.TaskDataDto>();
            CreateMap<Models.TaskDataForCreationDto, Entities.TaskData>();
            CreateMap<Models.TaskDataForUpdateDto, Entities.TaskData>();
            CreateMap<Entities.TaskData, Models.TaskDataForUpdateDto>();
        }

    }
}