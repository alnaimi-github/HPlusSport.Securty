namespace HPlusSport.Security.Web.Classes;

public class StaticHttpContext
{
    static IServiceProvider? services = null;

    public static IServiceProvider? Services
    {
        get { return services; }
        set
        {
            if (services != null)
            {
                throw new Exception("Services already set");
            }
            services = value;
        }
    }

    public static HttpContext? Current
    {
        get
        {
            IHttpContextAccessor? httpContextAccessor = services?.GetService(typeof(IHttpContextAccessor)) as IHttpContextAccessor;
            return httpContextAccessor?.HttpContext;
        }
    }
}
