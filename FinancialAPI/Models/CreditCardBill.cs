using System;

namespace FinancialAPI.Models
{
    public class CreditCardBill
    {
        public ulong Id { get; set; }
        public ulong UserId { get; set; }
        public ulong CreditCardId { get; set; }
        public ushort ReferenceYear { get; set; }
        public byte ReferenceMonth { get; set; }
        public DateTime? ClosingDate { get; set; }
        public DateTime? DueDate { get; set; }
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
