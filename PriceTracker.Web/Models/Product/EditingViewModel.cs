using PriceTracker.Data.UnitOfMeasure;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PriceTracker.Web.Models.Product;

public class EditingViewModel
{
    [Display(Name = "Id")]
    public int Id { get; set; }

    [Display(Name = "Name")]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(128)]
    public string Name { get; set; }

    [Display(Name = "Default Unit of Measure")]
    public int? DefaultUnitOfMeasureId { get; set; }

    public IEnumerable<UnitOfMeasureModel> UnitOfMeasures { get; set; }
}
