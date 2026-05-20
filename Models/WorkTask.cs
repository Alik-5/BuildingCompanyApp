using System;

namespace BuildingCompanyApp.Models
{
    public class WorkTask
    {
        public int WorkTaskId { get; set; }
        public int ProjectId { get; set; }
        public int AssignedWorkerId { get; set; }
        public int CreatedByUserId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ProjectName { get; set; } = string.Empty;
        public string AssignedTo { get; set; } = string.Empty;
        public string Priority { get; set; } = "Medium";
        public string Status { get; set; } = "Open";
        public int ProgressPercent { get; set; }
        public int CommentCount { get; set; }
        public string LatestComment { get; set; } = string.Empty;
        public bool NotificationsEnabled { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime DueDate { get; set; }
    }
}
