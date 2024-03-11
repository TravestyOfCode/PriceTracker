using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Linq;
using System.Net;

namespace PriceTracker.Data.Results;

public class Result : IResult
{
    public HttpStatusCode StatusCode { get; internal set; }

    public bool WasSuccess => (int)StatusCode >= 200 && (int)StatusCode <= 299;

    public bool WasFailure => !WasSuccess;

    internal object _Value;
    public object Value
    {
        get => WasSuccess ? _Value : throw new InvalidOperationException();
    }

    public ModelStateDictionary Errors { get; internal set; }

    public bool HasErrors => Errors.Any();

    public Result(HttpStatusCode statusCode, object value = default)
    {
        StatusCode = statusCode;
        _Value = value;
        Errors = new ModelStateDictionary();
    }

    public void AddError(string propertyName, string errorMessage)
    {
        Errors.TryAddModelError(propertyName, errorMessage);
    }

    public static Result Ok() => new(HttpStatusCode.OK, null);
    public static Result<T> Ok<T>() => new(HttpStatusCode.OK, default);
    public static Result<T> Ok<T>(T value) => new(HttpStatusCode.OK, value);

    public static Result Created() => new(HttpStatusCode.Created, null);
    public static Result<T> Created<T>() => new(HttpStatusCode.Created, default);
    public static Result<T> Created<T>(T value) => new(HttpStatusCode.Created, value);

    public static Result BadRequest() => new(HttpStatusCode.BadRequest, null);
    public static Result<T> BadRequest<T>() => new(HttpStatusCode.BadRequest, default);

    public static Result Unauthorized() => new(HttpStatusCode.Unauthorized, null);
    public static Result<T> Unauthorized<T>() => new(HttpStatusCode.Unauthorized, default);

    public static Result Forbidden() => new(HttpStatusCode.Forbidden, null);
    public static Result<T> Forbidden<T>() => new(HttpStatusCode.Forbidden, default);

    public static Result NotFound() => new(HttpStatusCode.NotFound, null);
    public static Result<T> NotFound<T>() => new(HttpStatusCode.NotFound, default);

    public static Result ServerError() => new(HttpStatusCode.InternalServerError, null);
    public static Result<T> ServerError<T>() => new(HttpStatusCode.InternalServerError, default);
}
