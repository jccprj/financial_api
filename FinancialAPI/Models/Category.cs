using System;

namespace FinancialAPI.Models
{
    public class Category
    {
        public ulong Id { get; set; }
        public ulong UserId { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
