using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Product { get; set; }
    public DbSet<Customer> Customer { get; set; }
    public DbSet<Sale> Sale { get; set; }
    public DbSet<SaleItem> SaleItem { get; set; }
    public DbSet<Category> Category { get; set; }
    public DbSet<Expense> Expense { get; set; }
    public DbSet<User> User { get; set; }
    public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}