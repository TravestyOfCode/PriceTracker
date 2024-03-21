using PriceTracker.Data.UnitConversion;
using PriceTracker.Data.UnitConversion.Commands;
using PriceTracker.Data.UnitOfMeasure;
using System.Collections.Generic;

namespace PriceTracker.Web.Models.UnitConversion;

public class EditRowViewModel
{
    public UnitConversionModel UnitConversion { get; set; } = new UnitConversionModel();

    public Dictionary<int, UnitOfMeasureModel> UnitOfMeasureDict { get; set; }

    public EditRowViewModel()
    {

    }

    public EditRowViewModel(UpdateUnitConversion request)
    {
        if (request == null)
        {
            return;
        }

        UnitConversion = new UnitConversionModel()
        {
            Id = request.Id,
            ConversionRatio = request.ConversionRatio,
            DestinationUnitOfMeasureId = request.DestinationUnitOfMeasureId,
            SourceUnitOfMeasureId = request.SourceUnitOfMeasureId
        };
    }

}
