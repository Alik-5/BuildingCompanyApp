using System;

namespace BuildingCompanyApp.Models
{
    public class FinanceEntry
    {
        public int FinanceEntryId { get; set; }
        public string EntryType { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string ProjectName { get; set; } = string.Empty;
        public int? WorkerId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public DateTime EntryDate { get; set; }
    }

    public class Invoice
    {
        public int InvoiceId { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public string ClientName { get; set; } = string.Empty;
        public string ProjectName { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
        public DateTime? PaidDate { get; set; }
    }
}
