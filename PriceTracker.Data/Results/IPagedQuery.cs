using Microsoft.IdentityModel.Tokens;
using System.Linq;

namespace PriceTracker.Data.Results;

public interface IPagedQuery
{
    public int Page { get; }

    public int PerPage { get; }

    public string SortBy { get; }

    public SortOrder SortOrder { get; }
}

public static class IPagedQueryExtensions
{
    public static IQueryable<T> AsPagedQuery<T>(this IQueryable<T> query, IPagedQuery pagedQuery)
    {
        if (query == null || pagedQuery == null)
        {
            return query;
        }

        if (!pagedQuery.SortBy.IsNullOrEmpty())
        {
            switch (pagedQuery.SortOrder)
            {
                case SortOrder.DESC: query = query.OrderByDescending(p => EF.Property<object>(p, pagedQuery.SortBy)); break;
                default: query = query.OrderBy(p => EF.Property<object>(p, pagedQuery.SortBy)); break;
            }
        }

        return query.Skip((pagedQuery.Page - 1) * pagedQuery.PerPage).Take(pagedQuery.PerPage);
    }
}