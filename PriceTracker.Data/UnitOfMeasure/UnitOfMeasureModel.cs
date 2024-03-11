using System.ComponentModel.DataAnnotations;

namespace PriceTracker.Data.UnitOfMeasure;

public class UnitOfMeasureModel
{
    [Display(Name = "Id")]
    public int Id { get; set; }

    [Display(Name = "Name")]
    [Required]
    [MaxLength(16)]
    public string Name { get; set; }

    [Display(Name = "Abbreviation")]
    [Required]
    [MaxLength(8)]
    public string Abbreviation { get; set; }

    [Display(Name = "Conversion to Grams Ratio")]
    [Required]
    public decimal ConversionToGramsRatio { get; set; }
}
