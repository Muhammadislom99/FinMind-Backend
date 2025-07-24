namespace FinMind.Infrastructure.Data.Configurations;

public class CategoryConfiguration : BaseEntityConfiguration<Category>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Category> builder)
    {
        builder.Property(c => c.Type)
            .IsRequired()
            .HasDefaultValue(CategoryType.Expense);

        builder.Property(c => c.Icon)
            .IsRequired(false)
            .HasMaxLength(100);

        builder.Property(c => c.Color)
            .IsRequired(false)
            .HasMaxLength(7);

        builder.Property(c => c.IsSystem)
            .HasDefaultValue(false);
        builder.Property(c => c.IsActive)
            .HasDefaultValue(true);

        builder.HasOne(c => c.ParentCategory)
            .WithMany(c => c.Categories)
            .HasForeignKey(c => c.ParentCategoryId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(c => c.BudgetAmount)
            .IsRequired(false);

        builder.HasOne(c => c.Account)
            .WithMany()
            .HasForeignKey(c => c.AccountId)
            .IsRequired(false);

        builder.HasIndex(c => c.AccountId).IsUnique();
    }
}