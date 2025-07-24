namespace FinMind.Infrastructure.Data.Configurations;

public class LoanConfiguration : BaseEntityConfiguration<Loan>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Loan> builder)
    {
        builder.Property(l => l.Status)
            .HasDefaultValue(LoanStatus.Active);
        
        builder.HasOne(l => l.Account)
            .WithMany()
            .HasForeignKey(l => l.AccountId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(l => l.AccountId)
            .IsUnique();
    }
}