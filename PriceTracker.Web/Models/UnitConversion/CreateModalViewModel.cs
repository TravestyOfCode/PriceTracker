using PriceTracker.Data.UnitConversion.Commands;
using PriceTracker.Data.UnitOfMeasure;
using System.Collections.Generic;

namespace PriceTracker.Web.Models.UnitConversion;

public class CreateModalViewModel
{
    public CreateUnitConversion Request { get; set; } = new CreateUnitConversion();

    public Dictionary<int, UnitOfMeasureModel> UnitOfMeasureDict { get; set; }

}
