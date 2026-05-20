using System;

namespace BuildingCompanyApp.Models
{
    public class Document
    {
        public int DocumentId { get; set; }
        public int? ProjectId { get; set; }
        public int? WorkerId { get; set; }
        public string DocumentName { get; set; } = string.Empty;
        public string DocumentPath { get; set; } = string.Empty;
        public string DocumentType { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Tags { get; set; } = string.Empty;
        public long FileSizeInBytes { get; set; }
        public int UploadedBy { get; set; }
        public DateTime UploadedDate { get; set; }
        public DateTime? ExpiryDate { get; set; }

        public Project? Project { get; set; }
        public Worker? Worker { get; set; }
        public User? User { get; set; }
    }
}
