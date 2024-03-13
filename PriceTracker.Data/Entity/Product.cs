using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PriceTracker.Data.Entity;

internal class Product
{
    public int Id { get; set; }

    public string Name { get; set; }

    public int? DefaultUnitOfMeasureId { get; set; }

    public UnitOfMeasure DefaultUnitOfMeasure { get; set; }
}

internal class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable(nameof(Product));

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .IsRequired(true)
            .HasMaxLength(128);

        builder.HasOne(p => p.DefaultUnitOfMeasure)
            .WithMany()
            .HasPrincipalKey(p => p.Id)
            .HasForeignKey(p => p.DefaultUnitOfMeasureId)
            .OnDelete(DeleteBehavior.ClientSetNull);

        builder.HasIndex(p => p.Name)
            .IsUnique(true)
            .IsClustered(false);
    }
}
