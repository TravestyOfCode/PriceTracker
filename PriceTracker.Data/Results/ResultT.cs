using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Net;

namespace PriceTracker.Data.Results;

public class Result<T> : Result, IResult<T>
{
    public Result(HttpStatusCode statusCode, T value = default) : base(statusCode, value)
    {
    }

    public new T Value => (T)base.Value;
}

public static class ResultExtenions
{
    public static void AddErrors(this ModelStateDictionary modelState, ModelStateDictionary errors)
    {
        if (modelState == null || errors == null)
        {
            return;
        }

        foreach (var entry in errors)
        {
            foreach (var error in entry.Value.Errors)
            {
                if (error.Exception != null)
                {
                    modelState.TryAddModelException(entry.Key, error.Exception);
                }
                if (!string.IsNullOrWhiteSpace(error.ErrorMessage))
                {
                    modelState.TryAddModelError(entry.Key, error.ErrorMessage);
                }
            }
        }
    }
}
