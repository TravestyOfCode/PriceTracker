using MediatR;
using Microsoft.AspNetCore.Mvc;
using PriceTracker.Data.Results;
using PriceTracker.Data.Store;
using PriceTracker.Data.Store.Commands;
using PriceTracker.Data.Store.Queries;
using PriceTracker.Web.Utilities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PriceTracker.Web.Controllers;

public class StoreController : Controller
{
    private readonly IMediator _mediator;

    private readonly ILogger<StoreController> _logger;

    public StoreController(IMediator mediator, ILogger<StoreController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Index(GetStores request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(request, cancellationToken);

            if (result.WasSuccess)
            {
                var model = new Models.Store.IndexViewModel()
                {
                    Stores = result.Value
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
    public async Task<IActionResult> Index(CreateStore request, CancellationToken cancellationToken)
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
    public async Task<IActionResult> EditRow(GetStoreById request, CancellationToken cancellationToken)
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
    public async Task<IActionResult> EditRow(UpdateStore request, CancellationToken cancellationToken)
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

                if (result.HasErrors)
                {
                    ModelState.AddErrors(result.Errors);
                }
            }

            return PartialView("EditingRow", new StoreModel() { Id = request.Id });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error.");

            return StatusCode(500);
        }
    }

    [HttpGet]
    public async Task<IActionResult> CancelEditRow(GetStoreById request, CancellationToken cancellationToken)
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
    public async Task<IActionResult> DeleteRow(DeleteStore request, CancellationToken cancellationToken)
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
