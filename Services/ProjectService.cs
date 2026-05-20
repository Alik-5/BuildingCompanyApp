using System.Collections.Generic;
using System.Threading.Tasks;
using BuildingCompanyApp.Models;

namespace BuildingCompanyApp.Services
{
    public interface IProjectService
    {
        Task<List<Project>> GetAllProjectsAsync();
        Task<Project?> GetProjectByIdAsync(int id);
        Task AddProjectAsync(Project project);
        Task UpdateProjectAsync(Project project);
        Task DeleteProjectAsync(int id);
        Task<List<Project>> GetProjectsByStatusAsync(string status);
        Task ReplaceProjectAssignmentsAsync(int projectId, IEnumerable<int> workerIds);
    }

    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectService(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<List<Project>> GetAllProjectsAsync()
        {
            return await _projectRepository.GetAllProjectsAsync();
        }

        public async Task<Project?> GetProjectByIdAsync(int id)
        {
            return await _projectRepository.GetProjectByIdAsync(id);
        }

        public async Task AddProjectAsync(Project project)
        {
            await _projectRepository.AddProjectAsync(project);
        }

        public async Task UpdateProjectAsync(Project project)
        {
            await _projectRepository.UpdateProjectAsync(project);
        }

        public async Task DeleteProjectAsync(int id)
        {
            await _projectRepository.DeleteProjectAsync(id);
        }

        public async Task<List<Project>> GetProjectsByStatusAsync(string status)
        {
            return await _projectRepository.GetProjectsByStatusAsync(status);
        }

        public async Task ReplaceProjectAssignmentsAsync(int projectId, IEnumerable<int> workerIds)
        {
            await _projectRepository.ReplaceProjectAssignmentsAsync(projectId, workerIds);
        }
    }
}
