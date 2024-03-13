using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PriceTracker.Data.PriceHistory;
using System.Linq;

namespace PriceTracker.Data.Entity;

internal class PriceHistory
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public Product Product { get; set; }

    public int UnitOfMeasureId { get; set; }

    public UnitOfMeasure UnitOfMeasure { get; set; }

    public decimal Quantity { get; set; }

    public DateOnly Date { get; set; }

    public int? StoreId { get; set; }

    public Store Store { get; set; }
}

internal class PriceHistoryConfiguration : IEntityTypeConfiguration<PriceHistory>
{
    public void Configure(EntityTypeBuilder<PriceHistory> builder)
    {
        builder.ToTable(nameof(PriceHistory));

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Quantity)
            .HasColumnType("decimal(9,5)");

        builder.Property(p => p.Date)
            .HasColumnType("date");

        builder.HasOne(p => p.Product)
            .WithMany(p => p.PriceHistories)
            .HasPrincipalKey(p => p.Id)
            .HasForeignKey(p => p.ProductId)
            .OnDelete(DeleteBehavior.ClientCascade);

        builder.HasOne(p => p.UnitOfMeasure)
            .WithMany()
            .HasPrincipalKey(p => p.Id)
            .HasForeignKey(p => p.UnitOfMeasureId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.Store)
            .WithMany()
            .HasPrincipalKey(p => p.Id)
            .HasForeignKey(p => p.StoreId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}

internal static class PriceHistoryExtensions
{
    public static PriceHistoryModel AsModel(this PriceHistory entity)
    {
        return entity == null ? null : new PriceHistoryModel()
        {
            Id = entity.Id,
            ProductId = entity.ProductId,
            Product = entity.Product.AsModel(),
            UnitOfMeasureId = entity.UnitOfMeasureId,
            UnitOfMeasure = entity.UnitOfMeasure.AsModel(),
            Quantity = entity.Quantity,
            Date = entity.Date,
            StoreId = entity.StoreId,
            Store = entity.Store.AsModel()
        };
    }

    public static IQueryable<PriceHistoryModel> ProjectToModel(this IQueryable<PriceHistory> query)
    {
        return query?.Select(p => new PriceHistoryModel()
        {
            Id = p.Id,
            ProductId = p.ProductId,
            UnitOfMeasureId = p.UnitOfMeasureId,
            StoreId = p.StoreId,
            Quantity = p.Quantity,
            Date = p.Date,
            Product = p.Product == null ? null : new Data.Product.ProductModel()
            {
                Id = p.Product.Id,
                Name = p.Product.Name,
                DefaultUnitOfMeasureId = p.Product.DefaultUnitOfMeasureId
            },
            UnitOfMeasure = p.UnitOfMeasure == null ? null : new Data.UnitOfMeasure.UnitOfMeasureModel()
            {
                Id = p.UnitOfMeasure.Id,
                Name = p.UnitOfMeasure.Name,
                Abbreviation = p.UnitOfMeasure.Abbreviation,
                ConversionToGramsRatio = p.UnitOfMeasure.ConversionToGramsRatio
            },
            Store = p.Store == null ? null : new Data.Store.StoreModel()
            {
                Id = p.Store.Id,
                Name = p.Store.Name
            }
        });
    }
}