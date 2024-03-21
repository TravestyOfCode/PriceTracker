using PriceTracker.Data.UnitConversion;
using System.Collections.Generic;

namespace PriceTracker.Web.Models.UnitConversion;

public class IndexViewModel
{
    public List<UnitConversionModel> UnitConversionList { get; set; }

    public CreateModalViewModel Request { get; set; }
}
