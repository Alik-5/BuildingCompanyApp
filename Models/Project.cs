using System;
using System.Collections.Generic;
using System.Linq;

namespace BuildingCompanyApp.Models
{
    public class Project
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = "Planning";
        public DateTime StartDate { get; set; }
        public DateTime Deadline { get; set; }
        public DateTime? CompletionDate { get; set; }
        public decimal Budget { get; set; }
        public string Location { get; set; } = string.Empty;
        public string ClientName { get; set; } = string.Empty;
        public string ClientEmail { get; set; } = string.Empty;
        public string ClientPhone { get; set; } = string.Empty;
        public string CurrentStage { get; set; } = "Foundation";
        public int StageCompletionPercent { get; set; }
        public string SiteManager { get; set; } = string.Empty;
        public string Priority { get; set; } = "Medium";
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public ICollection<ProjectWorker> ProjectWorkers { get; set; } = new List<ProjectWorker>();
        public ICollection<ProjectImage> ProjectImages { get; set; } = new List<ProjectImage>();
        public ICollection<ProjectStage> Stages { get; set; } = new List<ProjectStage>();
        public ICollection<Document> Documents { get; set; } = new List<Document>();

        public int AttachmentCount => (ProjectImages?.Count ?? 0) + (Documents?.Count ?? 0);
        public int CompletedStageCount => Stages?.Count(stage => stage.IsCompleted) ?? 0;
    }

    public class ProjectWorker
    {
        public int ProjectWorkerId { get; set; }
        public int ProjectId { get; set; }
        public int WorkerId { get; set; }
        public string Role { get; set; } = string.Empty;
        public DateTime AssignedDate { get; set; }

        public Project? Project { get; set; }
        public Worker? Worker { get; set; }
    }

    public class ProjectImage
    {
        public int ImageId { get; set; }
        public int ProjectId { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public string ImageName { get; set; } = string.Empty;
        public DateTime UploadedDate { get; set; }

        public Project? Project { get; set; }
    }

    public class ProjectStage
    {
        public int ProjectStageId { get; set; }
        public int ProjectId { get; set; }
        public string StageName { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
        public int DisplayOrder { get; set; }
    }
}
