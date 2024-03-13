using PriceTracker.Data.Entity;
using PriceTracker.Data.Results;
using System.ComponentModel.DataAnnotations;

namespace PriceTracker.Data.Product.Commands;

public class CreateProduct : IRequest<Result<ProductModel>>
{
    [Display(Name = "Name")]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(128)]
    public string Name { get; set; }

    [Display(Name = "Default Unit of Measure")]
    public int? DefaultUnitOfMeasureId { get; set; }
}

internal class CreateProductHandler : IRequestHandler<CreateProduct, Result<ProductModel>>
{
    private readonly ApplicationDBContext _dbContext;

    private readonly ILogger<CreateProductHandler> _logger;

    public CreateProductHandler(ApplicationDBContext dbContext, ILogger<CreateProductHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<ProductModel>> Handle(CreateProduct request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = _dbContext.Products.Add(new Entity.Product()
            {
                Name = request.Name,
                DefaultUnitOfMeasureId = request.DefaultUnitOfMeasureId
            });

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Ok(entity.Entity.AsModel());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error.");

            return Result.ServerError<ProductModel>();
        }
    }
}
