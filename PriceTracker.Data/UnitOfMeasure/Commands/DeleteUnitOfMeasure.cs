using PriceTracker.Data.Results;

namespace PriceTracker.Data.UnitOfMeasure.Commands;

public class DeleteUnitOfMeasure : IRequest<Result>
{
    public int Id { get; set; }
}

internal class DeleteUnitOfMeasureHandler : IRequestHandler<DeleteUnitOfMeasure, Result>
{
    private readonly ApplicationDBContext _dbContext;

    private readonly ILogger<DeleteUnitOfMeasureHandler> _logger;

    public DeleteUnitOfMeasureHandler(ApplicationDBContext dbContext, ILogger<DeleteUnitOfMeasureHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result> Handle(DeleteUnitOfMeasure request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await _dbContext.UnitOfMeasures.SingleOrDefaultAsync(p => p.Id.Equals(request.Id), cancellationToken);

            if (entity == null)
            {
                return Result.NotFound();
            }

            _dbContext.UnitOfMeasures.Remove(entity);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error.");

            return Result.ServerError();
        }
    }
}
