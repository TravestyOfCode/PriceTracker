using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PriceTracker.Data.UnitConversion;
using System.Linq;

namespace PriceTracker.Data.Entity;

internal class UnitConversion
{
    public int Id { get; set; }

    public int SourceUnitOfMeasureId { get; set; }

    public UnitOfMeasure SourceUnitOfMeasure { get; set; }

    public int DestinationUnitOfMeasureId { get; set; }

    public UnitOfMeasure DestinationUnitOfMeasure { get; set; }

    public decimal ConversionRatio { get; set; }
}

internal class UnitConversionConfiguration : IEntityTypeConfiguration<UnitConversion>
{
    public void Configure(EntityTypeBuilder<UnitConversion> builder)
    {
        builder.ToTable(nameof(UnitConversion));

        builder.HasKey(p => p.Id);

        builder.Property(p => p.ConversionRatio)
            .HasColumnType("decimal(9,5)");

        builder.HasOne(p => p.SourceUnitOfMeasure)
            .WithMany()
            .HasPrincipalKey(p => p.Id)
            .HasForeignKey(p => p.SourceUnitOfMeasureId)
            .OnDelete(DeleteBehavior.ClientCascade);

        builder.HasOne(p => p.DestinationUnitOfMeasure)
            .WithMany()
            .HasPrincipalKey(p => p.Id)
            .HasForeignKey(p => p.DestinationUnitOfMeasureId)
            .OnDelete(DeleteBehavior.ClientCascade);

        builder.HasIndex(p => new { p.SourceUnitOfMeasureId, p.DestinationUnitOfMeasureId })
            .IsUnique(true)
            .IsClustered(false);
    }
}

internal static class UnitConversionExtensions
{
    public static UnitConversionModel AsModel(this UnitConversion entity)
    {
        return entity == null ? null : new UnitConversionModel()
        {
            SourceUnitOfMeasureId = entity.SourceUnitOfMeasureId,
            SourceUnitOfMeasure = entity.SourceUnitOfMeasure.AsModel(),
            DestinationUnitOfMeasureId = entity.DestinationUnitOfMeasureId,
            DestinationUnitOfMeasure = entity.DestinationUnitOfMeasure.AsModel(),
            ConversionRatio = entity.ConversionRatio
        };
    }

    public static IQueryable<UnitConversionModel> ProjectToModel(this IQueryable<UnitConversion> query)
    {
        return query?.Select(p => new UnitConversionModel()
        {
            SourceUnitOfMeasureId = p.SourceUnitOfMeasureId,
            SourceUnitOfMeasure = new Data.UnitOfMeasure.UnitOfMeasureModel()
            {
                Id = p.SourceUnitOfMeasure.Id,
                Abbreviation = p.SourceUnitOfMeasure.Abbreviation,
                ConversionToGramsRatio = p.SourceUnitOfMeasure.ConversionToGramsRatio,
                Name = p.SourceUnitOfMeasure.Name
            },
            DestinationUnitOfMeasureId = p.DestinationUnitOfMeasureId,
            DestinationUnitOfMeasure = new Data.UnitOfMeasure.UnitOfMeasureModel()
            {
                Id = p.DestinationUnitOfMeasure.Id,
                Abbreviation = p.DestinationUnitOfMeasure.Abbreviation,
                ConversionToGramsRatio = p.DestinationUnitOfMeasure.ConversionToGramsRatio,
                Name = p.DestinationUnitOfMeasure.Name
            },
            ConversionRatio = p.ConversionRatio
        });
    }
}
