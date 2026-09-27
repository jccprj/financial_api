using System;

namespace FinancialAPI.Dtos
{
    public record TransactionDto(ulong Id, decimal Amount, string Currency, DateTime Date, string? Description, string ClassificationStatus, ulong CreditCardId, ulong? MerchantId);

    public record TransactionFilterParams(DateTime? DateFrom, DateTime? DateTo, decimal? MinAmount, decimal? MaxAmount, ulong? MerchantId, ulong? CategoryId, string? Status);

    public record ClassifyTransactionDto(ulong? MerchantId, ulong? CategoryId, ulong? SubcategoryId, ulong? ContextId);
}
