using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PriceTracker.Data.UnitOfMeasure;
using System.Linq;

namespace PriceTracker.Data.Entity;

internal class UnitOfMeasure
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Abbreviation { get; set; }
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
    }
}

internal static class UnitOfMeasureExtensions
{
    public static UnitOfMeasureModel AsModel(this UnitOfMeasure entity)
    {
        return entity == null ? null : new UnitOfMeasureModel()
        {
            Name = entity.Name,
            Abbreviation = entity.Abbreviation
        };
    }

    public static IQueryable<UnitOfMeasureModel> ProjectToModel(this IQueryable<UnitOfMeasure> query)
    {
        return query?.Select(p => new UnitOfMeasureModel()
        {
            Id = p.Id,
            Name = p.Name,
            Abbreviation = p.Abbreviation
        });
    }
}
