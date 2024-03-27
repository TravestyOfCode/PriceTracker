using PriceTracker.Data.Product;
using PriceTracker.Data.Results;
using System.Collections.Generic;

namespace PriceTracker.Web.Models.Product;

public class IndexViewModel : IPagedResult
{
    public List<ProductModel> ProductList { get; set; }

    public int Page { get; set; }

    public int PerPage { get; set; }

    public string SortBy { get; set; }

    public SortOrder SortOrder { get; set; }

    public int TotalPages { get; set; }

    public CreateModalViewModel Request { get; set; }

    public IndexViewModel()
    {
        ProductList = new List<ProductModel>();
        Request = new CreateModalViewModel();
    }

    public IndexViewModel(PagedResult<ProductModel> pagedResult) : this()
    {
        ProductList = pagedResult.Value ?? new List<ProductModel>();
        Page = pagedResult.Page;
        PerPage = pagedResult.PerPage;
        SortOrder = pagedResult.SortOrder;
        SortBy = pagedResult.SortBy;
        TotalPages = pagedResult.TotalPages;
    }
}
