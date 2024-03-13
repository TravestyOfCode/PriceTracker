using PriceTracker.Data.Entity;
using PriceTracker.Data.Results;
using PriceTracker.Data.UnitOfMeasure;
using System.ComponentModel.DataAnnotations;

namespace PriceTracker.Data.Product.Commands;

public class UpdateProduct : IRequest<Result<ProductModel>>
{
    [Display(Name = "Id")]
    public int Id { get; set; }

    [Display(Name = "Name")]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(128)]
    public string Name { get; set; }

    [Display(Name = "Default Unit of Measure")]
    public int? DefaultUnitOfMeasureId { get; set; }

    [Display(Name = "Default Unit of Measure")]
    public UnitOfMeasureModel DefaultUnitOfMeasure { get; set; }
}

internal class UpdateProductHandler : IRequestHandler<UpdateProduct, Result<ProductModel>>
{
    private readonly ApplicationDBContext _dbContext;

    private readonly ILogger<UpdateProductHandler> _logger;

    public UpdateProductHandler(ApplicationDBContext dbContext, ILogger<UpdateProductHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<ProductModel>> Handle(UpdateProduct request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await _dbContext.Products.SingleOrDefaultAsync(p => p.Id.Equals(request.Id), cancellationToken);

            if (entity == null)
            {
                return Result.NotFound<ProductModel>();
            }

            entity.Name = request.Name;
            entity.DefaultUnitOfMeasureId = request.DefaultUnitOfMeasureId;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Ok(entity.AsModel());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error.");

            return Result.ServerError<ProductModel>();
        }
    }
}
