using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using BuildingCompanyApp.Models;
using BuildingCompanyApp.Services;

namespace BuildingCompanyApp.Controllers
{
    public class DocumentController : Controller
    {
        private readonly IDocumentService _documentService;
        private readonly IProjectService _projectService;
        private readonly IWorkerService _workerService;
        private readonly IUserContextService _userContextService;

        public DocumentController(
            IDocumentService documentService,
            IProjectService projectService,
            IWorkerService workerService,
            IUserContextService userContextService)
        {
            _documentService = documentService;
            _projectService = projectService;
            _workerService = workerService;
            _userContextService = userContextService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (!_userContextService.IsAuthenticated())
                return RedirectToAction("Login", "Auth");

            if (!_userContextService.CanManageCompanyData())
            {
                return RedirectToAction("Index", "Dashboard");
            }

            var documents = await _documentService.GetAllDocumentsAsync();
            return View(new DocumentHubViewModel
            {
                Documents = documents,
                Projects = await _projectService.GetAllProjectsAsync(),
                Workers = await _workerService.GetAllWorkersAsync()
            });
        }

        [HttpGet]
        public async Task<IActionResult> Upload(int? projectId, int? workerId)
        {
            if (!_userContextService.IsAuthenticated())
                return RedirectToAction("Login", "Auth");

            if (!_userContextService.CanManageCompanyData())
            {
                return RedirectToAction(nameof(Index));
            }

            await LoadRelatedDataAsync(projectId, workerId);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Upload(Document document, IFormFile file)
        {
            if (!_userContextService.IsAuthenticated())
                return RedirectToAction("Login", "Auth");

            if (!_userContextService.CanManageCompanyData())
            {
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid && file != null)
            {
                await _documentService.UploadDocumentAsync(document, file, int.Parse(HttpContext.Session.GetString("UserId")!));
                return RedirectToAction(nameof(Index));
            }
            await LoadRelatedDataAsync(document.ProjectId, document.WorkerId);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (!_userContextService.IsAuthenticated())
                return RedirectToAction("Login", "Auth");

            if (!_userContextService.CanManageCompanyData())
            {
                return RedirectToAction(nameof(Index));
            }

            await _documentService.DeleteDocumentAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task LoadRelatedDataAsync(int? projectId, int? workerId)
        {
            ViewBag.Projects = await _projectService.GetAllProjectsAsync();
            ViewBag.Workers = await _workerService.GetAllWorkersAsync();
            ViewBag.SelectedProjectId = projectId;
            ViewBag.SelectedWorkerId = workerId;
        }
    }
}
