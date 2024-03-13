using PriceTracker.Data.UnitOfMeasure;
using System.ComponentModel.DataAnnotations;

namespace PriceTracker.Data.Product;

public class ProductModel
{
    [Display(Name = "Id")]
    public int Id { get; set; }

    [Display(Name = "Name")]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(128)]
    public string Name { get; set; }

    [Display(Name = "Default Unit of Measure")]
    public int? DefaultUnitOfMeasureId { get; set; }

    [Display(Name = "Default Unit of Measure")]
    public UnitOfMeasureModel DefaultUnitOfMeasure { get; set; }
}
