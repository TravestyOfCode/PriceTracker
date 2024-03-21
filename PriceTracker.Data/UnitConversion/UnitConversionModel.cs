using PriceTracker.Data.UnitOfMeasure;
using System.ComponentModel.DataAnnotations;

namespace PriceTracker.Data.UnitConversion;

public class UnitConversionModel
{
    public int Id { get; set; }

    [Display(Name = "Source Unit")]
    public int SourceUnitOfMeasureId { get; set; }

    [Display(Name = "Source Unit")]
    public UnitOfMeasureModel SourceUnitOfMeasure { get; set; }

    [Display(Name = "Destination Unit")]
    public int DestinationUnitOfMeasureId { get; set; }

    [Display(Name = "Destination Unit")]
    public UnitOfMeasureModel DestinationUnitOfMeasure { get; set; }

    [Display(Name = "Conversion Ratio")]
    [DisplayFormat(DataFormatString = "{0:N5}", ApplyFormatInEditMode = true)]
    public decimal ConversionRatio { get; set; }
}
