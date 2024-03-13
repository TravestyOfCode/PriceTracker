using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PriceTracker.Data.Store;
using System.Linq;

namespace PriceTracker.Data.Entity;

public class Store
{
    public int Id { get; set; }

    public string Name { get; set; }
}

public class StoreConfiguration : IEntityTypeConfiguration<Store>
{
    public void Configure(EntityTypeBuilder<Store> builder)
    {
        builder.ToTable(nameof(Store));

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .IsRequired(true)
            .HasMaxLength(128);

        builder.HasIndex(p => p.Name)
            .IsUnique(true)
            .IsClustered(false);
    }
}

internal static class StoreExtensions
{
    public static StoreModel AsModel(this Store entity)
    {
        return entity == null ? null : new StoreModel()
        {
            Id = entity.Id,
            Name = entity.Name
        };
    }

    public static IQueryable<StoreModel> ProjectToModel(this IQueryable<Store> query)
    {
        return query?.Select(p => new StoreModel()
        {
            Id = p.Id,
            Name = p.Name
        });
    }
}