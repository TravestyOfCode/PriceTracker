using PriceTracker.Data.Entity;
using PriceTracker.Data.Results;
using System.Collections.Generic;
using System.Linq;

namespace PriceTracker.Data.UnitConversion.Queries;

public class GetUnitConversionsBySourceUoM : IRequest<Result<List<UnitConversionModel>>>
{
    public int SourceUnitOfMeasureId { get; set; }
}

internal class GetUnitConversionsBySourceUoMHandler : IRequestHandler<GetUnitConversionsBySourceUoM, Result<List<UnitConversionModel>>>
{
    private readonly ApplicationDBContext _dbContext;

    private readonly ILogger<GetUnitConversionsBySourceUoMHandler> _logger;

    public GetUnitConversionsBySourceUoMHandler(ApplicationDBContext dbContext, ILogger<GetUnitConversionsBySourceUoMHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<List<UnitConversionModel>>> Handle(GetUnitConversionsBySourceUoM request, CancellationToken cancellationToken)
    {
        try
        {
            var entities = await _dbContext.UnitConversions
                .Where(p => p.SourceUnitOfMeasureId.Equals(request.SourceUnitOfMeasureId))
                .ProjectToModel()
                .ToListAsync(cancellationToken);

            return Result.Ok(entities);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error.");

            return Result.ServerError<List<UnitConversionModel>>();
        }
    }
}