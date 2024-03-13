using System.ComponentModel.DataAnnotations;

namespace PriceTracker.Data.Store;

public class StoreModel
{
    [Display(Name = "Id")]
    public int Id { get; set; }

    [Display(Name = "Name")]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(128)]
    public string Name { get; set; }
}
