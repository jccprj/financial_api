using System;

namespace FinancialAPI.Dtos
{
    public record BillDto(ulong Id, ulong CreditCardId, ushort ReferenceYear, byte ReferenceMonth, DateTime? ClosingDate, DateTime? DueDate, string Status, decimal TotalAmount, decimal PaidAmount);
}
