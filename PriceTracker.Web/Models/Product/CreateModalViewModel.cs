using PriceTracker.Data.Product.Commands;
using PriceTracker.Data.UnitOfMeasure;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PriceTracker.Web.Models.Product;

public class CreateModalViewModel
{
    [Display(Name = "Name")]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(128)]
    public string Name { get; set; }

    [Display(Name = "Default Unit of Measure")]
    public int? DefaultUnitOfMeasureId { get; set; }

    public Dictionary<int, UnitOfMeasureModel> UnitOfMeasures { get; set; } = new Dictionary<int, UnitOfMeasureModel>();

    public CreateModalViewModel()
    {

    }

    public CreateModalViewModel(CreateProduct request, Dictionary<int, UnitOfMeasureModel> uoms)
    {
        Name = request.Name;
        DefaultUnitOfMeasureId = request.DefaultUnitOfMeasureId;
        UnitOfMeasures = uoms;
    }
}
