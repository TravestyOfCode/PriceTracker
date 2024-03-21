using PriceTracker.Data.UnitOfMeasure;
using PriceTracker.Data.UnitOfMeasure.Commands;
using System.Collections.Generic;

namespace PriceTracker.Web.Models.UnitOfMeasure;

public class IndexViewModel
{
    public IEnumerable<UnitOfMeasureModel> UnitOfMeasureList { get; set; } = new List<UnitOfMeasureModel>();

    public CreateUnitOfMeasure Request { get; set; } = new CreateUnitOfMeasure();
}
