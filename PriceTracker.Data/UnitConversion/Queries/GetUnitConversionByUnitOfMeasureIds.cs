using PriceTracker.Data.Entity;
using PriceTracker.Data.Results;
using System.Linq;

namespace PriceTracker.Data.UnitConversion.Queries;

public class GetUnitConversionByUnitOfMeasureIds : IRequest<Result<UnitConversionModel>>
{
    public int SourceUnitOfMeasureId { get; set; }

    public int DestinationUnitOfMeasureId { get; set; }
}

internal class GetUnitConversionByUnitOfMeasureIdsHandler : IRequestHandler<GetUnitConversionByUnitOfMeasureIds, Result<UnitConversionModel>>
{
    private readonly ApplicationDBContext _dbContext;

    private readonly ILogger<GetUnitConversionByUnitOfMeasureIdsHandler> _logger;

    public GetUnitConversionByUnitOfMeasureIdsHandler(ApplicationDBContext dbContext, ILogger<GetUnitConversionByUnitOfMeasureIdsHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<UnitConversionModel>> Handle(GetUnitConversionByUnitOfMeasureIds request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await _dbContext.UnitConversions
                .Where(p => p.SourceUnitOfMeasureId.Equals(request.SourceUnitOfMeasureId) && p.DestinationUnitOfMeasureId.Equals(request.DestinationUnitOfMeasureId))
                .ProjectToModel()
                .SingleOrDefaultAsync(cancellationToken);

            if (entity == null)
            {
                return Result.NotFound<UnitConversionModel>();
            }

            return Result.Ok(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error.");

            return Result.ServerError<UnitConversionModel>();
        }
    }
}