using OnlineShoppingPlatform.Business.Operations.Setting;

namespace OnlineShoppingPlatform.WebApi.Middlewares
{
    public class MaintenanceMiddleware   // Middleware class to handle maintenance mode for the application
    {
        private readonly RequestDelegate _next; // Delegate for the next middleware in the pipeline

        public MaintenanceMiddleware(RequestDelegate next)
        {
            _next = next;  
        }

        // Method that is called for each HTTP request
        public async Task Invoke(HttpContext context)
        {
            // Retrieve the setting service from the request services
            var settingService = context.RequestServices.GetRequiredService<ISettingService>();
            bool maintenanceMode = settingService.GetMaintenanceState();
            // Allow access to login and settings endpoints even in maintenance mode
            if (context.Request.Path.StartsWithSegments("/api/aut/login") || context.Request.Path.StartsWithSegments("/api/settings"))
            {
                await _next(context);
                return;
            }

            if (maintenanceMode)
            {
                await context.Response.WriteAsync("We are currently unable to provide service.");
            }
            else
            {
                await _next(context);
            }
        }
    }
}
