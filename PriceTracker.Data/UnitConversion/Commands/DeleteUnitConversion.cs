using PriceTracker.Data.Results;

namespace PriceTracker.Data.UnitConversion.Commands;

public class DeleteUnitConversion : IRequest<Result<Unit>>
{
    public int Id { get; set; }
}

internal class DeleteUnitConversionHandler : IRequestHandler<DeleteUnitConversion, Result<Unit>>
{
    private readonly ApplicationDBContext _dbContext;

    private readonly ILogger<DeleteUnitConversionHandler> _logger;

    public DeleteUnitConversionHandler(ApplicationDBContext dbContext, ILogger<DeleteUnitConversionHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<Unit>> Handle(DeleteUnitConversion request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await _dbContext.UnitConversions.FindAsync(request.Id);

            if (entity == null)
            {
                return Result.NotFound<Unit>();
            }

            _dbContext.UnitConversions.Remove(entity);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Ok<Unit>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error.");

            return Result.ServerError<Unit>();
        }
    }
}