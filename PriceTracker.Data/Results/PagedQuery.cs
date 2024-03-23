namespace PriceTracker.Data.Results;

public abstract class PagedQuery<T> : IRequest<PagedResult<T>>, IPagedQuery
{
    public int Page { get; set; }

    public int PerPage { get; set; }

    public string SortBy { get; set; }

    public SortOrder SortOrder { get; set; }

}
