using Microsoft.EntityFrameworkCore;
using FinancialAPI.Models;
using System.Text.RegularExpressions;

namespace FinancialAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<CreditCard> CreditCards { get; set; }
        public DbSet<CreditCardBill> CreditCardBills { get; set; }
        public DbSet<FinancialTransaction> FinancialTransactions { get; set; }
        public DbSet<Merchant> Merchants { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Subcategory> Subcategories { get; set; }
        public DbSet<ContextEntity> Contexts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AppUser>().ToTable("app_user");
            modelBuilder.Entity<Account>().ToTable("account");
            modelBuilder.Entity<CreditCard>().ToTable("credit_card");
            modelBuilder.Entity<CreditCardBill>().ToTable("credit_card_bill");
            modelBuilder.Entity<FinancialTransaction>().ToTable("financial_transaction");
            modelBuilder.Entity<Merchant>().ToTable("merchant");
            modelBuilder.Entity<Category>().ToTable("category");
            modelBuilder.Entity<Subcategory>().ToTable("subcategory");
            modelBuilder.Entity<ContextEntity>().ToTable("context");

            // Keys and simple mappings
            modelBuilder.Entity<AppUser>().HasKey(u => u.Id);
            modelBuilder.Entity<FinancialTransaction>().HasKey(t => t.Id);
            modelBuilder.Entity<CreditCard>().HasKey(c => c.Id);
            modelBuilder.Entity<CreditCardBill>().HasKey(b => b.Id);
            modelBuilder.Entity<Merchant>().HasKey(m => m.Id);
            modelBuilder.Entity<Category>().HasKey(c => c.Id);
            modelBuilder.Entity<Subcategory>().HasKey(s => s.Id);
            modelBuilder.Entity<ContextEntity>().HasKey(c => c.Id);

            // Explicit snake_case column mappings for all entity properties
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    property.SetColumnName(ToSnakeCase(property.Name));
                }
            }
        }

        private static string ToSnakeCase(string value)
        {
            return Regex.Replace(value, "([a-z0-9])([A-Z])", "$1_$2").ToLowerInvariant();
        }
    }
}
