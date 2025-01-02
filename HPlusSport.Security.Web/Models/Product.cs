using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HPlusSport.Security.Web.Models;

public class Product
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public string Sku { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    [Precision(14, 2)]
    public decimal Price { get; set; }
    public bool IsAvailable { get; set; }

    public virtual Category Category { get; set; } = default!;
    public virtual List<Order> Orders { get; set; } = default!;
}
