using PriceTracker.Data.Store;
using PriceTracker.Data.Store.Commands;
using System.Collections.Generic;

namespace PriceTracker.Web.Models.Store;

public class IndexViewModel
{
    public List<StoreModel> Stores { get; set; }

    public CreateStore Request { get; set; } = new CreateStore();
}
