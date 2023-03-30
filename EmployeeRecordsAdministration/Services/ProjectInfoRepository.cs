using EmployeeRecordsAdministration.DbContexts;
using EmployeeRecordsAdministration.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace EmployeeRecordsAdministration.Services
{
    public class ProjectInfoRepository : IProjectInfoRepository
    {
            private readonly ProjectInfoContext _context;
            public ProjectInfoRepository(ProjectInfoContext context)
            {
                _context = context;
            }
            public async Task<IEnumerable<ProjectData>> GetProjectsAsync()
            {
                return await _context.Projects.OrderBy(c => c.Title).ToListAsync();
            }

            //filtering and searching
            public async Task<IEnumerable<ProjectData>> GetProjectsAsync(string? title, string? searchQuery)
            {

                //collection to start from
                var collection = _context.Projects as IQueryable<ProjectData>;

                if (!string.IsNullOrWhiteSpace(title))
                {
                    title = title.Trim();
                    collection = collection.Where(c => c.Title == title);
                }

                if (!string.IsNullOrWhiteSpace(searchQuery))
                {
                    searchQuery = searchQuery.Trim();
                    collection = collection.Where(a => a.Title.Contains(searchQuery) || (a.Description != null && a.Description.Contains(searchQuery))); //to avoid nulL reference exceptions
                }


                var collectionToReturn = await collection.OrderBy(c => c.Title).ToListAsync();

                return (collectionToReturn);
            }

            public async Task<ProjectData?> GetProjectAsync(int projectId)
            {
               
                    return await _context.Projects.Include(c => c.Tasks)
                        .Where(c => c.Id == projectId).FirstOrDefaultAsync();

            }


        public async Task AddProjectAsync(ProjectData data)
        {

            await _context.Projects.AddAsync(data);
        }

        public void DeleteProject(ProjectData data)
        {
            _context.Projects.Remove(data);
        }




        //returns true if the project with the given id exists
        public async Task<bool> ProjectExistsAsync(int projectId)
            {
                return await _context.Projects.AnyAsync(c => c.Id == projectId);
            }

        public async Task<TaskData?> GetTaskForProjectAsync(int projectId, int taskId)
        {
            return await _context.Tasks
                .Where(c => c.ProjectDataId == projectId && c.Id == taskId)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TaskData>> GetTasksForProjectAsync(int projectId)
        {
            return await _context.Tasks.Where(c => c.ProjectDataId == projectId).ToListAsync();
        }


        public async Task AddTaskForProjectAsync(int projectId, TaskData task)
        {
            var project = await GetProjectAsync(projectId);
            if (project != null)
            {
                project.Tasks.Add(task);
            }
        }

        public void DeleteTask(TaskData task)
        {
            _context.Tasks.Remove(task);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }

    }
}
