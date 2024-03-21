using MediatR;
using Microsoft.AspNetCore.Mvc;
using PriceTracker.Data.Results;
using PriceTracker.Data.UnitOfMeasure;
using PriceTracker.Data.UnitOfMeasure.Commands;
using PriceTracker.Data.UnitOfMeasure.Queries;
using PriceTracker.Web.Utilities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PriceTracker.Web.Controllers;

public class UnitOfMeasureController : Controller
{
    private readonly IMediator _mediator;

    private readonly ILogger<UnitOfMeasureController> _logger;

    public UnitOfMeasureController(IMediator mediator, ILogger<UnitOfMeasureController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Index(GetUnitOfMeasures request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(request, cancellationToken);

            if (result.WasSuccess)
            {
                var model = new Models.UnitOfMeasure.IndexViewModel()
                {
                    UnitOfMeasureList = result.Value
                };

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
    public async Task<IActionResult> Index(CreateUnitOfMeasure request, CancellationToken cancellationToken)
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
                return PartialView("CreateModal", request);
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
    public async Task<IActionResult> EditRow(GetUnitOfMeasureById request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(request, cancellationToken);

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
    public async Task<IActionResult> EditRow(UpdateUnitOfMeasure request, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return PartialView("EditingRow", new UnitOfMeasureModel());
            }

            var result = await _mediator.Send(request, cancellationToken);

            if (result.WasSuccess)
            {
                return PartialView("EditableRow", result.Value);
            }

            if (result.HasErrors)
            {
                ModelState.AddErrors(result.Errors);

                return PartialView("EditingRow", new UnitOfMeasureModel());
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
    public async Task<IActionResult> CancelEditRow(GetUnitOfMeasureById request, CancellationToken cancellationToken)
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
    public async Task<IActionResult> DeleteRow(DeleteUnitOfMeasure request, CancellationToken cancellationToken)
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
}
