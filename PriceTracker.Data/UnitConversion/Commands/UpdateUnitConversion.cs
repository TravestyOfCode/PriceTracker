using PriceTracker.Data.Entity;
using PriceTracker.Data.Results;

namespace PriceTracker.Data.UnitConversion.Commands;

public class UpdateUnitConversion : IRequest<Result<UnitConversionModel>>
{
    public int Id { get; set; }

    public int SourceUnitOfMeasureId { get; set; }

    public int DestinationUnitOfMeasureId { get; set; }

    public decimal ConversionRatio { get; set; }
}

internal class UpdateUnitConversionHandler : IRequestHandler<UpdateUnitConversion, Result<UnitConversionModel>>
{
    private readonly ApplicationDBContext _dbContext;

    private readonly ILogger<UpdateUnitConversionHandler> _logger;

    public UpdateUnitConversionHandler(ApplicationDBContext dbContext, ILogger<UpdateUnitConversionHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<UnitConversionModel>> Handle(UpdateUnitConversion request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await _dbContext.UnitConversions
                .Include(p => p.SourceUnitOfMeasure)
                .Include(p => p.DestinationUnitOfMeasure)
                .AsTracking()
                .SingleOrDefaultAsync(p => p.Id.Equals(request.Id), cancellationToken);

            if (entity == null)
            {
                return Result.NotFound<UnitConversionModel>();
            }

            entity.SourceUnitOfMeasureId = request.SourceUnitOfMeasureId;
            entity.DestinationUnitOfMeasureId = request.DestinationUnitOfMeasureId;
            entity.ConversionRatio = request.ConversionRatio;

            await _dbContext.SaveChangesAsync(cancellationToken);

            // Do we need to refresh the entity details?

            return Result.Ok(entity.AsModel());

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error.");

            return Result.ServerError<UnitConversionModel>();
        }
    }
}