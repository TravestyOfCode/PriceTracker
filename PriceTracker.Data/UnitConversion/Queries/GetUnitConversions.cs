using PriceTracker.Data.Entity;
using PriceTracker.Data.Results;
using System.Collections.Generic;

namespace PriceTracker.Data.UnitConversion.Queries;

public class GetUnitConversions : IRequest<Result<List<UnitConversionModel>>>
{

}

internal class GetUnitConversionsHandler : IRequestHandler<GetUnitConversions, Result<List<UnitConversionModel>>>
{
    private readonly ApplicationDBContext _dbContext;

    private readonly ILogger<GetUnitConversionsHandler> _logger;

    public GetUnitConversionsHandler(ApplicationDBContext dbContext, ILogger<GetUnitConversionsHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<List<UnitConversionModel>>> Handle(GetUnitConversions request, CancellationToken cancellationToken)
    {
        try
        {
            var entities = await _dbContext.UnitConversions
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