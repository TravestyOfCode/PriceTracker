using MediatR;
using Microsoft.AspNetCore.Mvc;
using PriceTracker.Data.Results;
using PriceTracker.Data.UnitConversion.Commands;
using PriceTracker.Data.UnitConversion.Queries;
using PriceTracker.Data.UnitOfMeasure.Queries;
using PriceTracker.Web.Models.UnitConversion;
using PriceTracker.Web.Utilities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PriceTracker.Web.Controllers;

public class UnitConversionController : Controller
{
    private readonly IMediator _mediator;

    private readonly ILogger<UnitConversionController> _logger;

    public UnitConversionController(IMediator mediator, ILogger<UnitConversionController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        try
        {
            var result = await GenerateIndexViewModel(cancellationToken);

            if (result.WasSuccess)
            {
                return View(result.Value);
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
    public async Task<IActionResult> Create(CreateUnitConversion request, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                var uomResult = await _mediator.Send(new GetUnitOfMeasuresAsDict(), cancellationToken);

                if (uomResult.WasFailure)
                {
                    return StatusCode(500);
                }
                return PartialView("CreateModal", new CreateModalViewModel() { UnitOfMeasureDict = uomResult.Value });
            }

            var result = await _mediator.Send(request, cancellationToken);

            if (result.WasSuccess)
            {
                Response.Headers.HXRefresh();
                return Ok();
            }

            if (result.HasErrors)
            {
                var uomResults = await _mediator.Send(new GetUnitOfMeasuresAsDict(), cancellationToken);

                if (uomResults.WasFailure)
                {
                    return StatusCode(500);
                }

                ModelState.AddErrors(result.Errors);

                return PartialView("CreateModal", new CreateModalViewModel() { UnitOfMeasureDict = uomResults.Value });
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
    public async Task<IActionResult> EditRow(GetUnitConversionById request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(request, cancellationToken);

            if (result.WasFailure)
            {
                return StatusCode((int)result.StatusCode);
            }

            var uoms = await _mediator.Send(new GetUnitOfMeasuresAsDict(), cancellationToken);

            if (uoms.WasFailure)
            {
                return StatusCode((int)uoms.StatusCode);
            }

            return PartialView("EditingRow", new EditRowViewModel() { UnitConversion = result.Value, UnitOfMeasureDict = uoms.Value });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error.");

            return StatusCode(500);
        }
    }

    [HttpPost]
    public async Task<IActionResult> EditRow(UpdateUnitConversion request, CancellationToken cancellationToken)
    {
        try
        {
            if (ModelState.IsValid)
            {
                var result = await _mediator.Send(request, cancellationToken);

                if (result.WasSuccess)
                {
                    return PartialView("EditableRow", result.Value);
                }
                if (result.StatusCode != System.Net.HttpStatusCode.BadRequest)
                {
                    return StatusCode((int)result.StatusCode);
                }

                ModelState.AddErrors(result.Errors);
            }

            var units = await _mediator.Send(new GetUnitOfMeasuresAsDict(), cancellationToken);

            if (units.WasFailure)
            {
                return StatusCode((int)units.StatusCode);
            }

            return PartialView("EditingRow", new EditRowViewModel(request) { UnitOfMeasureDict = units.Value });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error.");

            return StatusCode(500);
        }
    }

    [HttpGet]
    public async Task<IActionResult> CancelEditRow(GetUnitConversionById request, CancellationToken cancellationToken)
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
    public async Task<IActionResult> DeleteRow(DeleteUnitConversion request, CancellationToken cancellationToken)
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

    private async Task<Result<IndexViewModel>> GenerateIndexViewModel(CancellationToken cancellationToken)
    {
        var conversions = await _mediator.Send(new GetUnitConversions(), cancellationToken);
        if (conversions.WasFailure)
        {
            return Result.ServerError<IndexViewModel>();
        }

        var units = await _mediator.Send(new GetUnitOfMeasuresAsDict(), cancellationToken);
        if (units.WasFailure)
        {
            return Result.ServerError<IndexViewModel>();
        }

        return Result.Ok(new IndexViewModel()
        {
            UnitConversionList = conversions.Value,
            Request = new CreateModalViewModel()
            {
                UnitOfMeasureDict = units.Value
            }
        });

    }
}
