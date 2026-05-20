using System;
using System.Collections.Generic;

namespace BuildingCompanyApp.Models
{
    public class Worker
    {
        public int WorkerId { get; set; }
        public int UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;
        public DateTime HireDate { get; set; }
        public decimal Salary { get; set; }
        public string AttendanceStatus { get; set; } = "Present";
        public decimal WorkHours { get; set; }
        public string ProfileImageUrl { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }

        public User? User { get; set; }
        public ICollection<ProjectWorker> ProjectWorkers { get; set; } = new List<ProjectWorker>();
    }
}
