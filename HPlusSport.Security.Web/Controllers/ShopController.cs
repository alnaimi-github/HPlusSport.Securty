using System.Web.Mvc;
using HPlusSport.Security.Web.Classes;
using HPlusSport.Security.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ActionResult = Microsoft.AspNetCore.Mvc.ActionResult;
using Controller = Microsoft.AspNetCore.Mvc.Controller;

namespace HPlusSport.Security.Web.Controllers;

[Microsoft.AspNetCore.Authorization.Authorize]
[IgnoreAntiforgeryToken(Order = 1001)]
public class ShopController : Controller
{
    private readonly ShopContext _context;

    public ShopController(ShopContext context)
    {
        _context = context;
        _context.Database.EnsureCreated();
    }

    // GET: Shop
    public ActionResult Index()
    {
        var categories = _context.Categories.Include("Products").ToList();
        return View(categories);
    }

    // GET: Category
    public ActionResult Category(int id)
    {
        var category = _context.Categories.Include("Products").FirstOrDefault(c => c.Id == id);
        if (category == null)
        {
            return NotFound();
        }
        return View(category);
    }

    // POST: AddToCart
    [Microsoft.AspNetCore.Mvc.HttpPost]
    public ActionResult AddToCart(int id)
    {
        var product = _context.Products.Find(id);
        if (product != null)
        {
            ShopManager.AddToCart(product);
        }
        return Redirect(HttpContext.Request.GetTypedHeaders()?.Referer?.ToString() ?? "~/Shop");
    }

    // GET: Cart
    public ActionResult Cart()
    {
        var products = ShopManager.GetCart();
        return View(products);
    }

    // POST: Order
    [Microsoft.AspNetCore.Mvc.HttpPost]
    [Microsoft.AspNetCore.Mvc.ValidateAntiForgeryToken]
    public ActionResult Order()
    {
        var order = ShopManager.CreateOrder(_context);
        return View("ThankYou", order);
    }

    // GET: Shop/Search
    [ValidateInput(false)]
    public ActionResult Search(string q)
    {
        ViewData["SearchTerm"] = q;
        return View();
    }

    // GET: Shop/AdminOrders
    public ActionResult AdminOrders()
    {
        var orders = _context.Orders.ToList();
        return View(orders);
    }

    // GET: Shop/AdminOrder/1
    public ActionResult AdminOrder(string id)
    {
        int idAsInt = Int32.Parse(new string(id.Trim().TakeWhile(c => char.IsDigit(c)).ToArray()));
        var order = _context.Orders.Include("Products").Where(o => o.Id == idAsInt).FirstOrDefault();
        if (order == null)
        {
            return NotFound();
        }

        var totalAmount = _context.Database.SqlQueryRaw<decimal>(
            $"SELECT SUM(Product.Price) AS Value FROM Product WHERE Product.Id IN (SELECT ProductsId FROM [OrderProduct (Dictionary<string, object>)] WHERE OrdersId={id})"
            );
        ViewData["TotalAmount"] = totalAmount.First();

        return View(order);
    }
}
