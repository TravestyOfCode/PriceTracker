using MediatR;
using Microsoft.AspNetCore.Mvc;
using PriceTracker.Data.Product;
using PriceTracker.Data.Product.Commands;
using PriceTracker.Data.Product.Queries;
using PriceTracker.Data.Results;
using PriceTracker.Data.UnitOfMeasure.Queries;
using PriceTracker.Web.Models.Product;
using PriceTracker.Web.Utilities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PriceTracker.Web.Controllers;

public class ProductController : Controller
{
    private readonly IMediator _mediator;

    private readonly ILogger<ProductController> _logger;

    public ProductController(IMediator mediator, ILogger<ProductController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Index(GetProductsPaged request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(request, cancellationToken);

            if (result.WasSuccess)
            {
                var model = new Models.Product.IndexViewModel(result);

                var uoms = await _mediator.Send(new GetUnitOfMeasuresAsDict(), cancellationToken);

                if (uoms.WasSuccess)
                {
                    model.Request.UnitOfMeasures = uoms.Value;
                }

                return View(model);
            }

            return StatusCode((int)result.StatusCode);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error.");

            return StatusCode(500);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Index(CreateProduct request, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                Response.Headers.HXRetarget("#create-modal");
                return PartialView("CreateModal", request);
            }

            var result = await _mediator.Send(request, cancellationToken);

            if (result.WasSuccess)
            {
                Response.Headers.HXRefresh();
                return Ok();
            }

            if (result.HasErrors)
            {
                ModelState.AddErrors(result.Errors);
                Response.Headers.HXRetarget("#create-modal");

                var uoms = await _mediator.Send(new GetUnitOfMeasuresAsDict(), cancellationToken);

                if (uoms.WasSuccess)
                {
                    return PartialView("CreateModal", new CreateModalViewModel(request, uoms.Value));
                }
            }

            return StatusCode((int)result.StatusCode);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error.");

            return StatusCode(500, "Unexpected error");
        }
    }

    [HttpGet]
    public async Task<IActionResult> EditRow(int id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await GetProductAsEditable(id, cancellationToken);

            if (result.WasSuccess)
            {
                return PartialView("EditingRow", result.Value);
            }

            return StatusCode((int)result.StatusCode);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error.");

            return StatusCode(500);
        }
    }

    [HttpPost]
    public async Task<IActionResult> EditRow(UpdateProduct request, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return PartialView("EditingRow", new ProductModel());
            }

            var result = await _mediator.Send(request, cancellationToken);

            if (result.WasSuccess)
            {
                return PartialView("EditableRow", result.Value);
            }

            if (result.HasErrors)
            {
                ModelState.AddErrors(result.Errors);

                return PartialView("EditingRow", new ProductModel());
            }

            return StatusCode((int)result.StatusCode);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error.");

            return StatusCode(500);
        }
    }

    [HttpGet]
    public async Task<IActionResult> CancelEditRow(GetProductById request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(request, cancellationToken);

            if (result.WasSuccess)
            {
                return PartialView("EditableRow", result.Value);
            }

            return StatusCode((int)result.StatusCode);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error.");

            return StatusCode(500);
        }
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteRow(DeleteProduct request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(request, cancellationToken);

            if (result.WasSuccess)
            {
                return Ok();
            }

            return StatusCode((int)result.StatusCode);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error.");

            return StatusCode(500);
        }
    }

    public async Task<Result<EditingViewModel>> GetProductAsEditable(int id, CancellationToken cancellationToken)
    {
        try
        {
            var product = await _mediator.Send(new GetProductById() { Id = id }, cancellationToken);

            if (product.WasFailure)
            {
                return new Result<EditingViewModel>(product.StatusCode);
            }

            var unitOfMeasures = await _mediator.Send(new GetUnitOfMeasures(), cancellationToken);

            if (unitOfMeasures.WasFailure)
            {
                return new Result<EditingViewModel>(unitOfMeasures.StatusCode);
            }

            return Result.Ok(new EditingViewModel()
            {
                Id = product.Value.Id,
                Name = product.Value.Name,
                DefaultUnitOfMeasureId = product.Value.DefaultUnitOfMeasureId,
                UnitOfMeasures = unitOfMeasures.Value
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error.");

            return Result.ServerError<EditingViewModel>();
        }
    }
}
