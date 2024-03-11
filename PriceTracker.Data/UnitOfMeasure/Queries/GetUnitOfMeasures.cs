using PriceTracker.Data.Entity;
using PriceTracker.Data.Results;
using System.Collections.Generic;

namespace PriceTracker.Data.UnitOfMeasure.Queries;

public class GetUnitOfMeasures : IRequest<Result<List<UnitOfMeasureModel>>>
{

}

internal class GetUnitOfMeasuresHandler : IRequestHandler<GetUnitOfMeasures, Result<List<UnitOfMeasureModel>>>
{
    private readonly ApplicationDBContext _dbContext;

    private readonly ILogger<GetUnitOfMeasuresHandler> _logger;

    public GetUnitOfMeasuresHandler(ApplicationDBContext dbContext, ILogger<GetUnitOfMeasuresHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<List<UnitOfMeasureModel>>> Handle(GetUnitOfMeasures request, CancellationToken cancellationToken)
    {
        try
        {
            var entities = await _dbContext.UnitOfMeasures
                .ProjectToModel()
                .ToListAsync(cancellationToken);

            return Result.Ok(entities);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error.");

            return Result.ServerError<List<UnitOfMeasureModel>>();
        }
    }
}