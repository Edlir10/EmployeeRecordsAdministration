using EmployeeRecordsAdministration.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace EmployeeRecordsAdministration.Services
{
    public interface IProjectInfoRepository
    {
            Task<IEnumerable<ProjectData>> GetProjectsAsync();
            Task<IEnumerable<ProjectData>> GetProjectsAsync(string? name, string? searchQuery);
            Task<ProjectData?> GetProjectAsync(int projectId);
        Task AddProjectAsync(ProjectData data);
        void DeleteProject(ProjectData data);
        Task<bool> ProjectExistsAsync(int projectId);
            Task<IEnumerable<TaskData>> GetTasksForProjectAsync(int projectId);
            Task<TaskData?> GetTaskForProjectAsync(int projectId, int taskId);
            Task AddTaskForProjectAsync(int projectId, TaskData task);
            void DeleteTask(TaskData task);
            Task<bool> SaveChangesAsync();
        

    }
}
