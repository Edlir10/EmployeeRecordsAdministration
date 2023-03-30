using AutoMapper;
using EmployeeRecordsAdministration.DbContexts;
using EmployeeRecordsAdministration.Models;
using EmployeeRecordsAdministration.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeRecordsAdministration.Controllers
{
    [ApiController]
    [Route("api/projects/{projectId}/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly IProjectInfoRepository _projectInfoRepository;
        private readonly IMapper _mapper;
        public TasksController(IProjectInfoRepository projectInfoRepository, IMapper mapper)
        {
            _projectInfoRepository = projectInfoRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskDataDto>>> GetTasks(int projectId)
        {

            if (!await _projectInfoRepository.ProjectExistsAsync(projectId))
            {
                return NotFound();
            }

            var tasksForProject = await _projectInfoRepository
                .GetTasksForProjectAsync(projectId);

            return Ok(_mapper.Map<IEnumerable<TaskDataDto>>(tasksForProject));
        }

        [HttpGet("{taskId}", Name = "GetTask")]
        public async Task<IActionResult> GetTask(int projectId, int taskId)
        {
            if (!await _projectInfoRepository.ProjectExistsAsync(projectId))
            {
                return NotFound();
            }

            var task = await _projectInfoRepository
                .GetTaskForProjectAsync(projectId, taskId);

            if (task == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<TaskDataDto>(task));
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask(int projectId, TaskDataForCreationDto task)
        {

            if (!await _projectInfoRepository.ProjectExistsAsync(projectId))
            {
                return NotFound();
            }

                var finalTaskData = _mapper.Map<Entities.TaskData>(task);

                await _projectInfoRepository.AddTaskForProjectAsync(projectId, finalTaskData);

                await _projectInfoRepository.SaveChangesAsync();

            var createdTaskToReturn = _mapper.Map<Models.TaskDataDto>(finalTaskData);

            return CreatedAtRoute("GetTask",
                new
                {
                    projectId = projectId,
                    taskId = createdTaskToReturn.Id
                },
                createdTaskToReturn);


        }

        [HttpPut("{taskId}")]
        public async Task<ActionResult> UpdateTask(int projectId, int taskId,
            TaskDataForUpdateDto task)
        {
            if (!await _projectInfoRepository.ProjectExistsAsync(projectId))
            {
                return NotFound();
            }

            var taskEntity = await _projectInfoRepository
                .GetTaskForProjectAsync(projectId, taskId);
            if (taskEntity == null)
            {
                return NotFound();
            }

            //automapper overrides the values in the destination with those from the source object
            _mapper.Map(task, taskEntity);

            await _projectInfoRepository.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{taskId}")]
        public async Task<ActionResult> DeleteTask(int projectId, int taskId)
        {
            if (!await _projectInfoRepository.ProjectExistsAsync(projectId))
            {
                return NotFound();
            }

            var taskEntity = await _projectInfoRepository
                .GetTaskForProjectAsync(projectId, taskId);
            if (taskEntity == null)
            {
                return NotFound();
            }

            _projectInfoRepository.DeleteTask(taskEntity);
            await _projectInfoRepository.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{taskId}")]
        public async Task<ActionResult> PartiallyUpdateTask(
            int projectId, int taskId,
            JsonPatchDocument<TaskDataForUpdateDto> patchDocument)
        {
            if (!await _projectInfoRepository.ProjectExistsAsync(projectId))
            {
                return NotFound();
            }

            var taskEntity = await _projectInfoRepository
                .GetTaskForProjectAsync(projectId, taskId);
            if (taskEntity == null)
            {
                return NotFound();
            }

            var taskToPatch = _mapper.Map<TaskDataForUpdateDto>(taskEntity);

            patchDocument.ApplyTo(taskToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!TryValidateModel(taskToPatch))
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(taskToPatch, taskEntity);
            await _projectInfoRepository.SaveChangesAsync();

            return NoContent();
        }
    }
}
