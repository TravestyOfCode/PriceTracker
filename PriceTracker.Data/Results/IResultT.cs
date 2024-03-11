namespace PriceTracker.Data.Results;

public interface IResult<T> : IResult
{
    public new T Value { get; }
}
