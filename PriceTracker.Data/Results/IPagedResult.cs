namespace PriceTracker.Data.Results;

public enum SortOrder { ASC, DESC }

public interface IPagedResult : IPagedQuery
{
    public int TotalPages { get; set; }
}
