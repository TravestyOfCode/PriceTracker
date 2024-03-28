namespace PriceTracker.Data.Results;

public enum SortOrder { NONE, ASC, DESC }

public interface IPagedResult : IPagedQuery
{
    public int TotalPages { get; set; }
}
