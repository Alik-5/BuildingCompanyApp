using System;

namespace BuildingCompanyApp.Models
{
    public class Client
    {
        public int ClientId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string ContractStatus { get; set; } = string.Empty;
        public string ProjectHistory { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public DateTime ReminderDate { get; set; }
        public DateTime LastContactDate { get; set; }
    }
}
