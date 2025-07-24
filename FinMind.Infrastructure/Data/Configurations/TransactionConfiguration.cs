namespace FinMind.Infrastructure.Data.Configurations;

public class TransactionConfiguration : BaseEntityConfiguration<Transaction>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Transaction> builder)
    {
        builder.Property(t => t.DateTime)
            .HasDefaultValueSql("GETUTCDATE()")
            .ValueGeneratedOnAdd();
    }
}