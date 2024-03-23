using System.Collections.Generic;
using System.Net;

namespace PriceTracker.Data.Results;

public class PagedResult<T> : Result<List<T>>, IPagedQuery
{
    public new List<T> Value { get; internal set; }

    public int Page { get; internal set; }

    public int PerPage { get; internal set; }

    public string SortBy { get; internal set; }

    public SortOrder SortOrder { get; set; }

    public int TotalPages { get; set; }

    public PagedResult(HttpStatusCode statusCode, List<T> value = default) : base(statusCode, value)
    {
    }

    public static PagedResult<T> Ok(List<T> value, IPagedQuery query, double totalCount)
    {
        return new PagedResult<T>(HttpStatusCode.OK, value)
        {
            Page = query.Page,
            PerPage = query.PerPage,
            SortBy = query.SortBy,
            SortOrder = query.SortOrder,
            TotalPages = (int)Math.Ceiling(totalCount / query.PerPage)
        };
    }

    public static PagedResult<T> Created(IPagedQuery query) => new(HttpStatusCode.Created, null)
    {
        Page = query.Page,
        PerPage = query.PerPage,
        SortBy = query.SortBy,
        SortOrder = query.SortOrder
    };

    public static PagedResult<T> BadRequest(IPagedQuery query)
    {
        return new PagedResult<T>(HttpStatusCode.BadRequest, default)
        {
            Page = query.Page,
            PerPage = query.PerPage,
            SortBy = query.SortBy,
            SortOrder = query.SortOrder
        };
    }

    public static PagedResult<T> BadRequest(string property, string errorMessage, IPagedQuery query)
    {
        var result = new PagedResult<T>(HttpStatusCode.BadRequest, default)
        {
            Page = query.Page,
            PerPage = query.PerPage,
            SortBy = query.SortBy,
            SortOrder = query.SortOrder
        };

        result.AddError(property, errorMessage);

        return result;
    }

    public static PagedResult<T> Unauthorized(IPagedQuery query) => new(HttpStatusCode.Unauthorized, null)
    {
        Page = query.Page,
        PerPage = query.PerPage,
        SortBy = query.SortBy,
        SortOrder = query.SortOrder
    };

    public static PagedResult<T> Forbidden(IPagedQuery query) => new(HttpStatusCode.Forbidden, null)
    {
        Page = query.Page,
        PerPage = query.PerPage,
        SortBy = query.SortBy,
        SortOrder = query.SortOrder
    };

    public static PagedResult<T> NotFound(IPagedQuery query) => new(HttpStatusCode.NotFound, null)
    {
        Page = query.Page,
        PerPage = query.PerPage,
        SortBy = query.SortBy,
        SortOrder = query.SortOrder
    };

    public static PagedResult<T> ServerError(IPagedQuery query)
    {
        return new PagedResult<T>(HttpStatusCode.InternalServerError)
        {
            Page = query.Page,
            PerPage = query.PerPage,
            SortBy = query.SortBy,
            SortOrder = query.SortOrder
        };
    }
}
