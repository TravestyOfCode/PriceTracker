using PriceTracker.Data.Entity;
using PriceTracker.Data.Results;
using System.Linq;

namespace PriceTracker.Data.UnitConversion.Queries;

public class GetUnitConversionById : IRequest<Result<UnitConversionModel>>
{
    public int Id { get; set; }
}

internal class GetUnitConversionByIdHandler : IRequestHandler<GetUnitConversionById, Result<UnitConversionModel>>
{
    private readonly ApplicationDBContext _dbContext;

    private readonly ILogger<GetUnitConversionByIdHandler> _logger;

    public GetUnitConversionByIdHandler(ApplicationDBContext dbContext, ILogger<GetUnitConversionByIdHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<UnitConversionModel>> Handle(GetUnitConversionById request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await _dbContext.UnitConversions
                .Where(p => p.Id.Equals(request.Id))
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