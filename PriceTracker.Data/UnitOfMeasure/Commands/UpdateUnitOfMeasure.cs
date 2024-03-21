using PriceTracker.Data.Entity;
using PriceTracker.Data.Results;

namespace PriceTracker.Data.UnitOfMeasure.Commands;

public class UpdateUnitOfMeasure : IRequest<Result<UnitOfMeasureModel>>
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Abbreviation { get; set; }
}

internal class UpdateUnitOfMeasureHandler : IRequestHandler<UpdateUnitOfMeasure, Result<UnitOfMeasureModel>>
{
    private readonly ApplicationDBContext _dbContext;

    private readonly ILogger<UpdateUnitOfMeasureHandler> _logger;

    public UpdateUnitOfMeasureHandler(ApplicationDBContext dbContext, ILogger<UpdateUnitOfMeasureHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<UnitOfMeasureModel>> Handle(UpdateUnitOfMeasure request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await _dbContext.UnitOfMeasures.AsTracking().SingleOrDefaultAsync(p => p.Id.Equals(request.Id), cancellationToken);

            if (entity == null)
            {
                return Result.NotFound<UnitOfMeasureModel>();
            }

            entity.Name = request.Name;
            entity.Abbreviation = request.Abbreviation;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Ok(entity.AsModel());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error.");

            return Result.ServerError<UnitOfMeasureModel>();
        }
    }
}