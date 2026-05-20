using System.Collections.Generic;
using System.Threading.Tasks;
using BuildingCompanyApp.Models;
using Microsoft.AspNetCore.Http;

namespace BuildingCompanyApp.Services
{
    public interface IDocumentService
    {
        Task<List<Document>> GetAllDocumentsAsync();
        Task<Document?> GetDocumentByIdAsync(int id);
        Task UploadDocumentAsync(Document document, IFormFile file, int userId);
        Task DeleteDocumentAsync(int id);
        Task<List<Document>> GetDocumentsByProjectAsync(int projectId);
    }

    public class DocumentService : IDocumentService
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IWebHostEnvironment _env;

        public DocumentService(IDocumentRepository documentRepository, IWebHostEnvironment env)
        {
            _documentRepository = documentRepository;
            _env = env;
        }

        public async Task<List<Document>> GetAllDocumentsAsync()
        {
            return await _documentRepository.GetAllDocumentsAsync();
        }

        public async Task<Document?> GetDocumentByIdAsync(int id)
        {
            return await _documentRepository.GetDocumentByIdAsync(id);
        }

        public async Task UploadDocumentAsync(Document document, IFormFile file, int userId)
        {
            if (file != null && file.Length > 0)
            {
                var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "documents");
                Directory.CreateDirectory(uploadsFolder);

                var fileName = Path.GetRandomFileName() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                document.DocumentPath = $"/uploads/documents/{fileName}";
                document.DocumentType = string.IsNullOrWhiteSpace(document.DocumentType) ? Path.GetExtension(file.FileName).Trim('.').ToUpperInvariant() : document.DocumentType;
                document.Category = string.IsNullOrWhiteSpace(document.Category) ? document.DocumentType : document.Category;
                document.FileSizeInBytes = file.Length;
                document.UploadedBy = userId;
                document.UploadedDate = DateTime.UtcNow;

                await _documentRepository.AddDocumentAsync(document);
            }
        }

        public async Task DeleteDocumentAsync(int id)
        {
            var document = await _documentRepository.GetDocumentByIdAsync(id);
            if (document != null)
            {
                // Delete physical file
                var relativePath = document.DocumentPath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
                var filePath = Path.Combine(_env.WebRootPath, relativePath.Replace($"uploads{Path.DirectorySeparatorChar}", string.Empty));
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                await _documentRepository.DeleteDocumentAsync(id);
            }
        }

        public async Task<List<Document>> GetDocumentsByProjectAsync(int projectId)
        {
            return await _documentRepository.GetDocumentsByProjectAsync(projectId);
        }
    }
}
