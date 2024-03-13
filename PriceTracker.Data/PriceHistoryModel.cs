using PriceTracker.Data.Product;
using PriceTracker.Data.Store;
using PriceTracker.Data.UnitOfMeasure;
using System.ComponentModel.DataAnnotations;

namespace PriceTracker.Data;

public class PriceHistoryModel
{
    [Display(Name = "Id")]
    public int Id { get; set; }

    [Display(Name = "Product Id")]
    public int ProductId { get; set; }

    [Display(Name = "Product")]
    public ProductModel Product { get; set; }

    [Display(Name = "Unit of Measure Id")]
    public int UnitOfMeasureId { get; set; }

    [Display(Name = "Unit of Measure")]
    public UnitOfMeasureModel UnitOfMeasure { get; set; }

    [Display(Name = "Quantity")]
    public decimal Quantity { get; set; }

    [Display(Name = "Date")]
    public DateOnly Date { get; set; }

    [Display(Name = "Store Id")]
    public int? StoreId { get; set; }

    [Display(Name = "Store")]
    public StoreModel Store { get; set; }
}
