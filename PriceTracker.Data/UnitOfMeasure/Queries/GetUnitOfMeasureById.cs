using PriceTracker.Data.Entity;
using PriceTracker.Data.Results;
using System.Linq;

namespace PriceTracker.Data.UnitOfMeasure.Queries;

public class GetUnitOfMeasureById : IRequest<Result<UnitOfMeasureModel>>
{
    public int Id { get; set; }
}

internal class GetUnitOfMeasureByIdHandler : IRequestHandler<GetUnitOfMeasureById, Result<UnitOfMeasureModel>>
{
    private readonly ApplicationDBContext _dbContext;

    private readonly ILogger<GetUnitOfMeasureByIdHandler> _logger;

    public GetUnitOfMeasureByIdHandler(ApplicationDBContext dbContext, ILogger<GetUnitOfMeasureByIdHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<UnitOfMeasureModel>> Handle(GetUnitOfMeasureById request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await _dbContext.UnitOfMeasures
                .Where(p => p.Id.Equals(request.Id))
                .ProjectToModel()
                .SingleOrDefaultAsync(cancellationToken);

            if (entity == null)
            {
                return Result.NotFound<UnitOfMeasureModel>();
            }

            return Result.Ok(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error.");

            return Result.ServerError<UnitOfMeasureModel>();
        }
    }
}