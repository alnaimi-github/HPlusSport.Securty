using HPlusSport.Security.Web.Models;
using System.Text.Json;

namespace HPlusSport.Security.Web.Classes;

public static class ShopManager
{
    private const string sessionName = "cart";

    public static List<Product> GetCart()
    {
        var productsAsJson = StaticHttpContext.Current?.Session.GetString(sessionName);
        var products = new List<Product>();

        if (!string.IsNullOrEmpty(productsAsJson))
        {
            products = JsonSerializer.Deserialize<List<Product>>(productsAsJson) ?? new List<Product>();
        }

        return products;
    }

    public static void SetCart(List<Product> products)
    {
        var productsAsJson = JsonSerializer.Serialize<List<Product>>(products);
        StaticHttpContext.Current?.Session.SetString(sessionName, productsAsJson);
    }

    public static bool AddToCart(Product product)
    {
        var products = GetCart();
        if (products.Contains(product))
        {
            return false;
        }
        else
        {
            products.Add(product);
            SetCart(products);
            return true;
        }
    }

    public static bool RemoveFromCart(Product product)
    {
        var products = GetCart();
        if (!products.Contains(product))
        {
            return false;
        }
        else
        {
            products.Remove(product);
            SetCart(products);
            return true;
        }
    }

    public static Order CreateOrder(ShopContext _context)
    {
        var products = GetCart();
        if (products.Count() == 0)
        {
            throw new InvalidOperationException("Shopping cart is empty");
        }
        var email = StaticHttpContext.Current?.User?.Identity?.Name;
        if (string.IsNullOrEmpty(email))
        {
            throw new Exception("User name is empty");
        }

        var order = new Order()
        {   OrderDate = DateTime.Now,
            User = _context.Users.Single(u => u.Email == email)
        };

        order.Products = new List<Product>();
        foreach (var product in products)
        {
            order.Products.Add(_context.Products.Find(product.Id)!);
        }

        _context.Orders.Add(order);
        _context.SaveChanges();
        EmptyCart();
        return order;
    }

    private static bool EmptyCart()
    {
        StaticHttpContext.Current?.Session.Remove(sessionName);
        return true;
    }
}
