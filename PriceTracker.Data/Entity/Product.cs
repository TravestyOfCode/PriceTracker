using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PriceTracker.Data.Product;
using PriceTracker.Data.UnitOfMeasure;
using System.Collections.Generic;
using System.Linq;

namespace PriceTracker.Data.Entity;

internal class Product
{
    public int Id { get; set; }

    public string Name { get; set; }

    public int? DefaultUnitOfMeasureId { get; set; }

    public UnitOfMeasure DefaultUnitOfMeasure { get; set; }

    public IList<PriceHistory> PriceHistories { get; set; }
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

internal static class ProductExtensions
{
    public static ProductModel AsModel(this Product entity)
    {
        return entity == null ? null : new ProductModel()
        {
            Id = entity.Id,
            Name = entity.Name,
            DefaultUnitOfMeasureId = entity.DefaultUnitOfMeasureId,
            DefaultUnitOfMeasure = entity.DefaultUnitOfMeasure.AsModel()
        };
    }

    public static IQueryable<ProductModel> ProjectToModel(this IQueryable<Product> query, bool includeDefaultUnitOfMeasure = true)
    {
        if (includeDefaultUnitOfMeasure)
        {
            return query?.Select(p => new ProductModel()
            {
                Id = p.Id,
                Name = p.Name,
                DefaultUnitOfMeasureId = p.DefaultUnitOfMeasureId,
                DefaultUnitOfMeasure = new UnitOfMeasureModel()
                {
                    Id = p.DefaultUnitOfMeasure.Id,
                    Name = p.DefaultUnitOfMeasure.Name,
                    Abbreviation = p.DefaultUnitOfMeasure.Abbreviation
                }
            });
        }

        return query?.Select(p => new ProductModel()
        {
            Id = p.Id,
            Name = p.Name,
            DefaultUnitOfMeasureId = p.DefaultUnitOfMeasureId
        });

    }
}
