using System.Net;

namespace PriceTracker.Data.Results;

public interface IResult
{
    public HttpStatusCode StatusCode { get; }

    public bool WasSuccess { get; }

    public bool WasFailure { get; }

    public object Value { get; }
}
