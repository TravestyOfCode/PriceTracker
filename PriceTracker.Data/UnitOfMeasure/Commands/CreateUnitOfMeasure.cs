using PriceTracker.Data.Entity;
using PriceTracker.Data.Results;

namespace PriceTracker.Data.UnitOfMeasure.Commands;

public class CreateUnitOfMeasure : IRequest<Result<UnitOfMeasureModel>>
{
    public string Name { get; set; }

    public string Abbreviation { get; set; }

    public decimal ConversionToGramsRatio { get; set; }
}

internal class CreateUnitOfMeasureHandler : IRequestHandler<CreateUnitOfMeasure, Result<UnitOfMeasureModel>>
{
    private readonly ApplicationDBContext _dbContext;

    private readonly ILogger<CreateUnitOfMeasureHandler> _logger;

    public CreateUnitOfMeasureHandler(ApplicationDBContext dbContext, ILogger<CreateUnitOfMeasureHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<UnitOfMeasureModel>> Handle(CreateUnitOfMeasure request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = _dbContext.UnitOfMeasures.Add(new Entity.UnitOfMeasure()
            {
                Name = request.Name,
                Abbreviation = request.Abbreviation,
                ConversionToGramsRatio = request.ConversionToGramsRatio
            });

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Ok(entity.Entity.AsModel());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error.");

            return Result.ServerError<UnitOfMeasureModel>();
        }
    }
}
