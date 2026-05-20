using System;
using System.Collections.Generic;

namespace BuildingCompanyApp.Models
{
    public class MaterialItem
    {
        public int MaterialItemId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Unit { get; set; } = string.Empty;
        public decimal QuantityInStock { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal ReorderLevel { get; set; }
        public string SupplierName { get; set; } = string.Empty;
        public string StorageLocation { get; set; } = string.Empty;
        public DateTime LastUpdated { get; set; }
        public ICollection<MaterialTransaction> Transactions { get; set; } = new List<MaterialTransaction>();
    }

    public class MaterialTransaction
    {
        public int MaterialTransactionId { get; set; }
        public int MaterialItemId { get; set; }
        public string TransactionType { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string SupplierName { get; set; } = string.Empty;
        public string ReferenceProject { get; set; } = string.Empty;
        public DateTime TransactionDate { get; set; }
    }
}
