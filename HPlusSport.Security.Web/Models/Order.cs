using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HPlusSport.Security.Web.Models;

public class Order
{
    public int Id { get; set; }
    [DisplayName("Order Date")]
    public DateTime OrderDate { get; set; }

    public virtual User User { get; set; } = default!;
    public virtual List<Product> Products { get; set; } = default!;
}
