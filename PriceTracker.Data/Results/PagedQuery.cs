namespace PriceTracker.Data.Results;

public abstract class PagedQuery<T> : IRequest<PagedResult<T>>, IPagedQuery
{
    private int _Page = 1;
    public int Page
    {
        get => _Page;
        set => _Page = value > 0 ? value : throw new ArgumentOutOfRangeException(nameof(Page));
    }

    private int _PerPage = 10;
    public int PerPage
    {
        get => _PerPage;
        set => _PerPage = value > 0 ? value : throw new ArgumentOutOfRangeException(nameof(PerPage));

    }

    public string SortBy { get; set; }

    public SortOrder SortOrder { get; set; }

}
