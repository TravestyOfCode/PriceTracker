using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PriceTracker.Data.Entity;

internal class UnitOfMeasure
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Abbreviation { get; set; }

    public decimal ConversionToGramsRatio { get; set; }
}

internal class UnitOfMeasureConfiguration : IEntityTypeConfiguration<UnitOfMeasure>
{
    public void Configure(EntityTypeBuilder<UnitOfMeasure> builder)
    {
        builder.ToTable(nameof(UnitOfMeasure));

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .IsRequired(true)
            .HasMaxLength(16);

        builder.Property(p => p.Abbreviation)
            .IsRequired(true)
            .HasMaxLength(8);

        builder.Property(p => p.ConversionToGramsRatio)
            .HasColumnType("decimal(9,5)");
    }
}
