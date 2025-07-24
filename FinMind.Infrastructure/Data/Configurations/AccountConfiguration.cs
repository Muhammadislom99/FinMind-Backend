namespace FinMind.Infrastructure.Data.Configurations;

public class AccountConfiguration : BaseEntityConfiguration<Account>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Account> builder)
    {
        builder.Property(b => b.Description).HasMaxLength(1000).IsRequired();
        builder.Property(a => a.Balance)
            .HasDefaultValue(0.0);

        builder.Property(a => a.Currency)
            .IsRequired()
            .HasMaxLength(3)
            .HasDefaultValue("TJK");

        builder.Property(a => a.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(a => a.CheckingAccount)
            .HasDefaultValue(false);

        builder.HasOne(a => a.User)
            .WithMany(u => u.Accounts)
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}