using System.Net;

namespace PriceTracker.Data.Results;

public class Result<T> : Result, IResult<T>
{
    public Result(HttpStatusCode statusCode, T value = default) : base(statusCode, value)
    {
    }

    public new T Value => (T)base.Value;
}
