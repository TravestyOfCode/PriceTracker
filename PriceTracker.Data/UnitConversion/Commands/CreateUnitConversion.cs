using PriceTracker.Data.Entity;
using PriceTracker.Data.Results;

namespace PriceTracker.Data.UnitConversion.Commands;

public class CreateUnitConversion : IRequest<Result<UnitConversionModel>>
{
    public int SourceUnitOfMeasureId { get; set; }

    public int DestinationUnitOfMeasureId { get; set; }

    public decimal ConversionRatio { get; set; }
}

internal class CreateUnitConversionHandler : IRequestHandler<CreateUnitConversion, Result<UnitConversionModel>>
{
    private readonly ApplicationDBContext _dbContext;

    private readonly ILogger<CreateUnitConversionHandler> _logger;

    public CreateUnitConversionHandler(ApplicationDBContext dbContext, ILogger<CreateUnitConversionHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<UnitConversionModel>> Handle(CreateUnitConversion request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = _dbContext.UnitConversions.Add(new Entity.UnitConversion()
            {
                SourceUnitOfMeasureId = request.SourceUnitOfMeasureId,
                DestinationUnitOfMeasureId = request.DestinationUnitOfMeasureId,
                ConversionRatio = request.ConversionRatio
            });

            await _dbContext.SaveChangesAsync(cancellationToken);

            // Load the referenced uoms.
            await entity.Reference(p => p.SourceUnitOfMeasure).LoadAsync();
            await entity.Reference(p => p.DestinationUnitOfMeasure).LoadAsync();

            return Result.Ok(entity.Entity.AsModel());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error.");

            return Result.ServerError<UnitConversionModel>();
        }
    }
}