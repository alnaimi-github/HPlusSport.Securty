using System.ComponentModel.DataAnnotations;

namespace HPlusSport.Security.Web.Models;

public class User
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public string Salt { get; set; } = string.Empty;
    public string Hash { get; set; } = string.Empty;

    public virtual List<Order> Orders { get; set; } = default!;
}
