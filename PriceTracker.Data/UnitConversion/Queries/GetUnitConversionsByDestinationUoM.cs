using PriceTracker.Data.Entity;
using PriceTracker.Data.Results;
using System.Collections.Generic;
using System.Linq;

namespace PriceTracker.Data.UnitConversion.Queries;

public class GetUnitConversionsByDestinationUoM : IRequest<Result<List<UnitConversionModel>>>
{
    public int DestinationUnitOfMeasureId { get; set; }
}

internal class GetUnitConversionsByDestinationUoMHandler : IRequestHandler<GetUnitConversionsByDestinationUoM, Result<List<UnitConversionModel>>>
{
    private readonly ApplicationDBContext _dbContext;

    private readonly ILogger<GetUnitConversionsByDestinationUoMHandler> _logger;

    public GetUnitConversionsByDestinationUoMHandler(ApplicationDBContext dbContext, ILogger<GetUnitConversionsByDestinationUoMHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<List<UnitConversionModel>>> Handle(GetUnitConversionsByDestinationUoM request, CancellationToken cancellationToken)
    {
        try
        {
            var entities = await _dbContext.UnitConversions
                .Where(p => p.DestinationUnitOfMeasureId.Equals(request.DestinationUnitOfMeasureId))
                .ProjectToModel()
                .ToListAsync(cancellationToken);

            return Result.Ok(entities);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error.");

            return Result.ServerError<List<UnitConversionModel>>();
        }
    }
}
