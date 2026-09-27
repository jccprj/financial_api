using System;

namespace FinancialAPI.Models
{
    public class Account
    {
        public ulong Id { get; set; }
        public ulong UserId { get; set; }
        public ulong BankId { get; set; }
        public string Name { get; set; }
        public string AccountType { get; set; }
        public string Currency { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
