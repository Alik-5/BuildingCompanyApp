using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using BuildingCompanyApp.Models;
using BuildingCompanyApp.Services;

namespace BuildingCompanyApp.Controllers
{
    public class ProjectController : Controller
    {
        private readonly IProjectService _projectService;
        private readonly IWorkerService _workerService;
        private readonly IUserContextService _userContextService;

        public ProjectController(
            IProjectService projectService,
            IWorkerService workerService,
            IUserContextService userContextService)
        {
            _projectService = projectService;
            _workerService = workerService;
            _userContextService = userContextService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (!_userContextService.IsAuthenticated())
                return RedirectToAction("Login", "Auth");

            var projects = await _projectService.GetAllProjectsAsync();
            var currentWorker = await _userContextService.GetCurrentWorkerAsync();
            var canManage = _userContextService.CanManageCompanyData();
            if (!canManage && currentWorker != null)
            {
                projects = projects.Where(project => project.ProjectWorkers.Any(link => link.WorkerId == currentWorker.WorkerId)).ToList();
            }

            ViewBag.CanManage = canManage;
            return View(projects);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            if (!_userContextService.IsAuthenticated())
                return RedirectToAction("Login", "Auth");

            var project = await _projectService.GetProjectByIdAsync(id);
            if (project == null)
                return NotFound();

            if (!await CanAccessProjectAsync(project))
            {
                return RedirectToAction("Index", "Dashboard");
            }

            ViewBag.CanManage = _userContextService.CanManageCompanyData();
            return View(project);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            if (!_userContextService.IsAuthenticated())
                return RedirectToAction("Login", "Auth");

            if (!_userContextService.CanManageCompanyData())
            {
                return RedirectToAction("Index");
            }

            return View(await BuildProjectUpsertViewModelAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProjectUpsertViewModel model)
        {
            if (!_userContextService.IsAuthenticated())
                return RedirectToAction("Login", "Auth");

            if (!_userContextService.CanManageCompanyData())
            {
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                var now = DateTime.UtcNow;
                model.SelectedWorkerIds ??= new List<int>();
                var project = MapProject(model);
                project.StartDate = project.StartDate == default ? DateTime.Today : project.StartDate;
                project.CreatedDate = now;
                project.UpdatedDate = now;
                project.Priority = string.IsNullOrWhiteSpace(project.Priority) ? "Medium" : project.Priority;
                project.CurrentStage = string.IsNullOrWhiteSpace(project.CurrentStage) ? "Foundation" : project.CurrentStage;
                await _projectService.AddProjectAsync(project);
                await _projectService.ReplaceProjectAssignmentsAsync(project.ProjectId, model.SelectedWorkerIds);
                return RedirectToAction(nameof(Index));
            }

            model.AvailableWorkers = await BuildWorkerOptionsAsync(model.SelectedWorkerIds);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (!_userContextService.IsAuthenticated())
                return RedirectToAction("Login", "Auth");

            if (!_userContextService.CanManageCompanyData())
            {
                return RedirectToAction("Index");
            }

            var project = await _projectService.GetProjectByIdAsync(id);
            if (project == null)
                return NotFound();

            return View(await BuildProjectUpsertViewModelAsync(project));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, ProjectUpsertViewModel model)
        {
            if (!_userContextService.IsAuthenticated())
                return RedirectToAction("Login", "Auth");

            if (!_userContextService.CanManageCompanyData())
            {
                return RedirectToAction("Index");
            }

            if (id != model.ProjectId)
                return BadRequest();

            if (ModelState.IsValid)
            {
                var existingProject = await _projectService.GetProjectByIdAsync(id);
                model.SelectedWorkerIds ??= new List<int>();
                var project = MapProject(model);
                project.CreatedDate = existingProject?.CreatedDate ?? DateTime.UtcNow;
                project.UpdatedDate = DateTime.UtcNow;
                await _projectService.UpdateProjectAsync(project);
                await _projectService.ReplaceProjectAssignmentsAsync(project.ProjectId, model.SelectedWorkerIds);
                return RedirectToAction(nameof(Index));
            }

            model.AvailableWorkers = await BuildWorkerOptionsAsync(model.SelectedWorkerIds);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (!_userContextService.IsAuthenticated())
                return RedirectToAction("Login", "Auth");

            if (!_userContextService.CanManageCompanyData())
            {
                return RedirectToAction("Index");
            }

            await _projectService.DeleteProjectAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> CanAccessProjectAsync(Project project)
        {
            if (_userContextService.CanManageCompanyData())
            {
                return true;
            }

            var currentWorker = await _userContextService.GetCurrentWorkerAsync();
            return currentWorker != null && project.ProjectWorkers.Any(link => link.WorkerId == currentWorker.WorkerId);
        }

        private async Task<ProjectUpsertViewModel> BuildProjectUpsertViewModelAsync(Project? project = null)
        {
            var selectedWorkerIds = project?.ProjectWorkers.Select(link => link.WorkerId).ToList() ?? new List<int>();
            return new ProjectUpsertViewModel
            {
                ProjectId = project?.ProjectId ?? 0,
                ProjectName = project?.ProjectName ?? string.Empty,
                Description = project?.Description ?? string.Empty,
                Status = project?.Status ?? "Planning",
                StartDate = project?.StartDate ?? DateTime.Today,
                Deadline = project?.Deadline ?? DateTime.Today.AddMonths(1),
                Budget = project?.Budget ?? 0,
                Location = project?.Location ?? string.Empty,
                ClientName = project?.ClientName ?? string.Empty,
                ClientEmail = project?.ClientEmail ?? string.Empty,
                ClientPhone = project?.ClientPhone ?? string.Empty,
                CurrentStage = project?.CurrentStage ?? "Foundation",
                StageCompletionPercent = project?.StageCompletionPercent ?? 0,
                SiteManager = project?.SiteManager ?? string.Empty,
                Priority = project?.Priority ?? "Medium",
                SelectedWorkerIds = selectedWorkerIds,
                AvailableWorkers = await BuildWorkerOptionsAsync(selectedWorkerIds)
            };
        }

        private async Task<List<WorkerOptionViewModel>> BuildWorkerOptionsAsync(IEnumerable<int>? selectedIds = null)
        {
            var selected = selectedIds?.ToHashSet() ?? new HashSet<int>();
            var workers = await _workerService.GetAllWorkersAsync();
            return workers.Select(worker => new WorkerOptionViewModel
            {
                WorkerId = worker.WorkerId,
                Label = $"{worker.FullName} - {worker.Role}",
                Selected = selected.Contains(worker.WorkerId)
            }).ToList();
        }

        private static Project MapProject(ProjectUpsertViewModel model)
        {
            return new Project
            {
                ProjectId = model.ProjectId,
                ProjectName = model.ProjectName,
                Description = model.Description,
                Status = model.Status,
                StartDate = model.StartDate,
                Deadline = model.Deadline,
                Budget = model.Budget,
                Location = model.Location,
                ClientName = model.ClientName,
                ClientEmail = model.ClientEmail,
                ClientPhone = model.ClientPhone,
                CurrentStage = model.CurrentStage,
                StageCompletionPercent = model.StageCompletionPercent,
                SiteManager = model.SiteManager,
                Priority = model.Priority
            };
        }
    }
}
