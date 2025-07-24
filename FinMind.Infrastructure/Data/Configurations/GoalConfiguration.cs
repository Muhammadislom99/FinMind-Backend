namespace FinMind.Infrastructure.Data.Configurations;

public class GoalConfiguration : BaseEntityConfiguration<Goal>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Goal> builder)
    {
        builder.Property(g => g.TargetAmount)
            .IsRequired();

        builder.Property(g => g.Status)
            .HasDefaultValue(GoalStatus.Active);

        builder.HasOne(g => g.Account)
            .WithMany()
            .HasForeignKey(g => g.AccountId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(g => g.AccountId)
            .IsUnique();
    }
}