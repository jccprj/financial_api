using System;

namespace FinancialAPI.Models
{
    public class FinancialTransaction
    {
        public ulong Id { get; set; }
        public ulong UserId { get; set; }
        public ulong? NotificationId { get; set; }
        public ulong CreditCardId { get; set; }
        public ulong? MerchantId { get; set; }
        public string FinancialTransactionType { get; set; }
        public string PaymentMethod { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public DateTime FinancialTransactionDate { get; set; }
        public string? Description { get; set; }
        public bool IsInstallment { get; set; }
        public int? InstallmentCount { get; set; }
        public int? InstallmentNumber { get; set; }
        public string ClassificationStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
