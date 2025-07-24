namespace FinMind.Infrastructure.Data.Configurations;

public abstract class BaseEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : BaseEntity
{
    public void Configure(EntityTypeBuilder<TEntity> builder)
    {
        ConfigureBase(builder);
        ConfigureNameAndDescriptionIfExists(builder);
        //   ConfigureEnumPropertiesAsString(builder);
        ConfigureEntity(builder);
    }

    private void ConfigureBase(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("NEWID()");

        builder.Property(x => x.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(x => x.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        builder.HasQueryFilter(x => !x.IsDeleted);
    }

    private void ConfigureNameAndDescriptionIfExists(EntityTypeBuilder<TEntity> builder)
    {
        var entityType = typeof(TEntity);

        var nameProp = entityType.GetProperty("Name") ?? entityType.GetProperty("Title");
        if (nameProp != null && nameProp.PropertyType == typeof(string))
        {
            builder.Property(nameProp.Name)
                .IsRequired()
                .HasMaxLength(100);
        }

        var descProp = entityType.GetProperty("Description");
        if (descProp != null && descProp.PropertyType == typeof(string))
        {
            builder.Property("Description")
                .HasMaxLength(2000);
        }
    }

    protected abstract void ConfigureEntity(EntityTypeBuilder<TEntity> builder);

    private void ConfigureEnumPropertiesAsString(EntityTypeBuilder<TEntity> builder)
    {
        var entityType = typeof(TEntity);
        var props = entityType.GetProperties()
            .Where(p =>
                (p.Name.EndsWith("Type") || p.Name == "Status") &&
                p.PropertyType != typeof(bool));

        foreach (var prop in props)
        {
            var propertyBuilder = builder.Property(prop.Name);

            var propType = prop.PropertyType;
            var underlyingType = Nullable.GetUnderlyingType(propType) ?? propType;
            if (underlyingType.IsEnum)
            {
                propertyBuilder.HasConversion<string>();
            }
        }
    }
}