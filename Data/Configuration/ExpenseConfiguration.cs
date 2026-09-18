using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurations;

public class ExpenseConfiguration : IEntityTypeConfiguration<Expense>
{
    public void Configure(EntityTypeBuilder<Expense> builder)
    {
        builder.HasKey(e => e.ExpenseId);

        builder.Property(e => e.Description)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(e => e.Value)
            .IsRequired()
            .HasPrecision(10, 2);

        builder.Property(e => e.Date)
            .IsRequired();
    }
}