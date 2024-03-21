using PriceTracker.Data.UnitOfMeasure;

namespace PriceTracker.Data.UnitConversion;

public class UnitConversionModel
{
    public int Id { get; set; }

    public int SourceUnitOfMeasureId { get; set; }

    public UnitOfMeasureModel SourceUnitOfMeasure { get; set; }

    public int DestinationUnitOfMeasureId { get; set; }

    public UnitOfMeasureModel DestinationUnitOfMeasure { get; set; }

    public decimal ConversionRatio { get; set; }
}
