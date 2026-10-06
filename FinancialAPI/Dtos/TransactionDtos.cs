using System;
using System.ComponentModel;

namespace FinancialAPI.Dtos
{
    public record TransactionDto(
        [property: Description("Identificador da transação"), DefaultValue(101UL)] ulong Id,
        [property: Description("Valor da transação"), DefaultValue(typeof(decimal), "89.90")] decimal Amount,
        [property: Description("Moeda da transação"), DefaultValue("BRL")] string Currency,
        [property: Description("Data da transação")] DateTime Date,
        [property: Description("Descrição da transação"), DefaultValue("Compra no mercado")] string? Description,
        [property: Description("Status de classificação (PENDING, SUGGESTED, CONFIRMED)"), DefaultValue("PENDING")] string ClassificationStatus,
        [property: Description("Identificador do cartão"), DefaultValue(5UL)] ulong CreditCardId,
        [property: Description("Identificador do estabelecimento")] ulong? MerchantId);

    public record TransactionFilterParams(
        [property: Description("Data inicial do filtro")] DateTime? DateFrom,
        [property: Description("Data final do filtro")] DateTime? DateTo,
        [property: Description("Valor mínimo")] decimal? MinAmount,
        [property: Description("Valor máximo")] decimal? MaxAmount,
        [property: Description("Filtrar por estabelecimento")] ulong? MerchantId,
        [property: Description("Filtrar por categoria")] ulong? CategoryId,
        [property: Description("Filtrar por status de classificação")] string? Status);

    public record ClassifyTransactionDto(
        [property: Description("Identificador do estabelecimento")] ulong? MerchantId,
        [property: Description("Identificador da categoria")] ulong? CategoryId,
        [property: Description("Identificador da subcategoria")] ulong? SubcategoryId,
        [property: Description("Identificador do contexto")] ulong? ContextId);
}
