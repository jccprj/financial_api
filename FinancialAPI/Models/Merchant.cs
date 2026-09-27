using System;

namespace FinancialAPI.Models
{
    public class Merchant
    {
        public ulong Id { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public string MerchantType { get; set; }
        public bool RequiresClassification { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
