using BuildingCompanyApp.Models;
using BuildingCompanyApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace BuildingCompanyApp.Controllers
{
    public class TaskController : Controller
    {
        private readonly ITaskService _taskService;
        private readonly IProjectService _projectService;
        private readonly IWorkerService _workerService;
        private readonly IUserContextService _userContextService;

        public TaskController(
            ITaskService taskService,
            IProjectService projectService,
            IWorkerService workerService,
            IUserContextService userContextService)
        {
            _taskService = taskService;
            _projectService = projectService;
            _workerService = workerService;
            _userContextService = userContextService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (!_userContextService.IsAuthenticated())
                return RedirectToAction("Login", "Auth");

            var tasks = await _taskService.GetAllTasksAsync();
            var canManage = _userContextService.CanManageCompanyData();
            var currentWorker = await _userContextService.GetCurrentWorkerAsync();

            if (!canManage && currentWorker != null)
            {
                tasks = tasks.Where(task => task.AssignedWorkerId == currentWorker.WorkerId).ToList();
            }

            return View(new TaskBoardViewModel
            {
                CanManage = canManage,
                CurrentEmployeeName = currentWorker?.FullName ?? string.Empty,
                Tasks = tasks
            });
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            if (!_userContextService.IsAuthenticated())
                return RedirectToAction("Login", "Auth");

            if (!_userContextService.CanManageCompanyData())
            {
                return RedirectToAction(nameof(Index));
            }

            return View(await BuildTaskUpsertViewModelAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Create(TaskUpsertViewModel model)
        {
            if (!_userContextService.IsAuthenticated())
                return RedirectToAction("Login", "Auth");

            if (!_userContextService.CanManageCompanyData())
            {
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                var worker = await _workerService.GetWorkerByIdAsync(model.AssignedWorkerId);
                var project = await _projectService.GetProjectByIdAsync(model.ProjectId);
                var task = new WorkTask
                {
                    ProjectId = model.ProjectId,
                    AssignedWorkerId = model.AssignedWorkerId,
                    CreatedByUserId = _userContextService.GetUserId() ?? 0,
                    Title = model.Title,
                    Description = model.Description,
                    ProjectName = project?.ProjectName ?? string.Empty,
                    AssignedTo = worker?.FullName ?? string.Empty,
                    Priority = model.Priority,
                    Status = string.IsNullOrWhiteSpace(model.Status) ? "Open" : model.Status,
                    ProgressPercent = model.ProgressPercent,
                    CommentCount = model.CommentCount,
                    LatestComment = model.LatestComment,
                    NotificationsEnabled = model.NotificationsEnabled,
                    CreatedDate = DateTime.Today,
                    DueDate = model.DueDate == default ? DateTime.Today.AddDays(3) : model.DueDate
                };
                await _taskService.AddTaskAsync(task);
                return RedirectToAction(nameof(Index));
            }

            model.AvailableProjects = await _projectService.GetAllProjectsAsync();
            model.AvailableWorkers = await _workerService.GetAllWorkersAsync();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (!_userContextService.IsAuthenticated())
                return RedirectToAction("Login", "Auth");

            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            var currentWorker = await _userContextService.GetCurrentWorkerAsync();
            var canManage = _userContextService.CanManageCompanyData();
            if (!canManage && (currentWorker == null || task.AssignedWorkerId != currentWorker.WorkerId))
            {
                return RedirectToAction(nameof(Index));
            }

            var model = await BuildTaskUpsertViewModelAsync(task);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, TaskUpsertViewModel model)
        {
            if (!_userContextService.IsAuthenticated())
                return RedirectToAction("Login", "Auth");

            var existingTask = await _taskService.GetTaskByIdAsync(id);
            if (existingTask == null)
            {
                return NotFound();
            }

            var currentWorker = await _userContextService.GetCurrentWorkerAsync();
            var canManage = _userContextService.CanManageCompanyData();
            if (!canManage && (currentWorker == null || existingTask.AssignedWorkerId != currentWorker.WorkerId))
            {
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                if (canManage)
                {
                    var worker = await _workerService.GetWorkerByIdAsync(model.AssignedWorkerId);
                    var project = await _projectService.GetProjectByIdAsync(model.ProjectId);
                    existingTask.ProjectId = model.ProjectId;
                    existingTask.AssignedWorkerId = model.AssignedWorkerId;
                    existingTask.ProjectName = project?.ProjectName ?? existingTask.ProjectName;
                    existingTask.AssignedTo = worker?.FullName ?? existingTask.AssignedTo;
                    existingTask.Title = model.Title;
                    existingTask.Description = model.Description;
                    existingTask.Priority = model.Priority;
                    existingTask.NotificationsEnabled = model.NotificationsEnabled;
                    existingTask.DueDate = model.DueDate;
                }

                existingTask.Status = model.Status;
                existingTask.ProgressPercent = model.ProgressPercent;
                existingTask.CommentCount = model.CommentCount;
                existingTask.LatestComment = model.LatestComment;
                await _taskService.UpdateTaskAsync(existingTask);
                return RedirectToAction(nameof(Index));
            }

            model.AvailableProjects = await _projectService.GetAllProjectsAsync();
            model.AvailableWorkers = await _workerService.GetAllWorkersAsync();
            return View(model);
        }

        private async Task<TaskUpsertViewModel> BuildTaskUpsertViewModelAsync(WorkTask? task = null)
        {
            return new TaskUpsertViewModel
            {
                WorkTaskId = task?.WorkTaskId ?? 0,
                ProjectId = task?.ProjectId ?? 0,
                AssignedWorkerId = task?.AssignedWorkerId ?? 0,
                Title = task?.Title ?? string.Empty,
                Description = task?.Description ?? string.Empty,
                Priority = task?.Priority ?? "Medium",
                Status = task?.Status ?? "Open",
                ProgressPercent = task?.ProgressPercent ?? 0,
                CommentCount = task?.CommentCount ?? 0,
                LatestComment = task?.LatestComment ?? string.Empty,
                NotificationsEnabled = task?.NotificationsEnabled ?? true,
                DueDate = task?.DueDate ?? DateTime.Today.AddDays(3),
                AvailableProjects = await _projectService.GetAllProjectsAsync(),
                AvailableWorkers = await _workerService.GetAllWorkersAsync()
            };
        }
    }
}
