namespace FinMind.Infrastructure.Data.Configurations;

public class TagConfiguration : BaseEntityConfiguration<Tag>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Tag> builder)
    {
        builder.Property(t => t.Color)
            .IsRequired(false)
            .HasMaxLength(7);
    }
}