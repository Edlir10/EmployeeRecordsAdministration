using AutoMapper;
using EmployeeRecordsAdministration.DbContexts;
using EmployeeRecordsAdministration.Models;
using EmployeeRecordsAdministration.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace EmployeeRecordsAdministration.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectInfoRepository _projectInfoRepository;
        private readonly IMapper _mapper;
        public ProjectsController(IProjectInfoRepository projectInfoRepository, IMapper mapper)
        {
            _projectInfoRepository = projectInfoRepository;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectDataWithoutTasksDto>>> GetProjects()
        {
            var projectEntities = await _projectInfoRepository.GetProjectsAsync();
            return Ok(_mapper.Map<IEnumerable<ProjectDataWithoutTasksDto>>(projectEntities));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProject(int id)
        {
            var project = await _projectInfoRepository.GetProjectAsync(id);

            if (project == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<ProjectDataDto>(project));
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject(ProjectDataForCreationDto data)
        {

            var finalProjectData = _mapper.Map<Entities.ProjectData>(data);

            await _projectInfoRepository.AddProjectAsync(finalProjectData);

            await _projectInfoRepository.SaveChangesAsync();

            return CreatedAtAction("GetProject", new { finalProjectData.Id }, data);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject(int id, ProjectDataForUpdateDto project)
        {
            if (!await _projectInfoRepository.ProjectExistsAsync(id))
            {
                return NotFound();
            }
            var projectEntity = await _projectInfoRepository.GetProjectAsync(id);

            if (projectEntity == null)
            {
                return NotFound();
            }

            _mapper.Map(project, projectEntity);

            await _projectInfoRepository.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProject(int id)
        {
            if (!await _projectInfoRepository.ProjectExistsAsync(id))
            {
                return NotFound();
            }

            var projectEntity = await _projectInfoRepository
                .GetProjectAsync(id);
            if (projectEntity == null)
            {
                return NotFound();
            }

            _projectInfoRepository.DeleteProject(projectEntity);
            await _projectInfoRepository.SaveChangesAsync();

            return NoContent();
        }
    }
}
