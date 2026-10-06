using System;
using System.ComponentModel;

namespace FinancialAPI.Dtos
{
    public record BillDto(
        [property: Description("Identificador da fatura"), DefaultValue(10UL)] ulong Id,
        [property: Description("Identificador do cartão de crédito"), DefaultValue(5UL)] ulong CreditCardId,
        [property: Description("Ano de referência da fatura"), DefaultValue((ushort)2026)] ushort ReferenceYear,
        [property: Description("Mês de referência da fatura"), DefaultValue((byte)9)] byte ReferenceMonth,
        [property: Description("Data de fechamento da fatura")] DateTime? ClosingDate,
        [property: Description("Data de vencimento da fatura")] DateTime? DueDate,
        [property: Description("Status da fatura (OPEN, CLOSED, PAID, OVERDUE)"), DefaultValue("OPEN")] string Status,
        [property: Description("Valor total da fatura"), DefaultValue(typeof(decimal), "1500.75")] decimal TotalAmount,
        [property: Description("Valor pago da fatura"), DefaultValue(typeof(decimal), "300.00")] decimal PaidAmount);
}
