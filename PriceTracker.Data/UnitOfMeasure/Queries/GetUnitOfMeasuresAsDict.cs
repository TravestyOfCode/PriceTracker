using PriceTracker.Data.Results;
using System.Collections.Generic;

namespace PriceTracker.Data.UnitOfMeasure.Queries;

public class GetUnitOfMeasuresAsDict : IRequest<Result<Dictionary<int, UnitOfMeasureModel>>>
{

}

internal class GetUnitOfMeasuresAsDictHandler : IRequestHandler<GetUnitOfMeasuresAsDict, Result<Dictionary<int, UnitOfMeasureModel>>>
{
    private readonly ApplicationDBContext _dbContext;

    private readonly ILogger<GetUnitOfMeasuresAsDictHandler> _logger;

    public GetUnitOfMeasuresAsDictHandler(ApplicationDBContext dbContext, ILogger<GetUnitOfMeasuresAsDictHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<Dictionary<int, UnitOfMeasureModel>>> Handle(GetUnitOfMeasuresAsDict request, CancellationToken cancellationToken)
    {
        try
        {
            var entities = await _dbContext.UnitOfMeasures
                .ToDictionaryAsync(p => p.Id,
                                   p => new UnitOfMeasureModel()
                                   {
                                       Id = p.Id,
                                       Name = p.Name,
                                       Abbreviation = p.Abbreviation,
                                       ConversionToGramsRatio = p.ConversionToGramsRatio
                                   }, cancellationToken);

            return Result.Ok(entities);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error.");

            return Result.ServerError<Dictionary<int, string>>();
        }
    }
}
