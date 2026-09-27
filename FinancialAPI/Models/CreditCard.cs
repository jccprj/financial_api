using System;

namespace FinancialAPI.Models
{
    public class CreditCard
    {
        public ulong Id { get; set; }
        public ulong UserId { get; set; }
        public ulong AccountId { get; set; }
        public string? Name { get; set; }
        public string LastFourDigits { get; set; }
        public byte? ClosingDay { get; set; }
        public byte? DueDay { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
