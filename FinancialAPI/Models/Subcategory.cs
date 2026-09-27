using System;

namespace FinancialAPI.Models
{
    public class Subcategory
    {
        public ulong Id { get; set; }
        public ulong CategoryId { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
