using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace EmployeeRecordsAdministration.Profiles
{
    public class ProjectProfile : Profile
    {
        public ProjectProfile()
        {
            CreateMap<Entities.ProjectData, Models.ProjectDataWithoutTasksDto>();
            CreateMap<Entities.ProjectData, Models.ProjectDataDto>();
            CreateMap<Models.ProjectDataForCreationDto, Entities.ProjectData>();
            CreateMap<Models.ProjectDataForUpdateDto, Entities.ProjectData>();
            CreateMap<Entities.ProjectData, Models.ProjectDataForUpdateDto>();
        }
        
    }
}
