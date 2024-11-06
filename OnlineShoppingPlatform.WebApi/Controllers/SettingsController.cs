using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineShoppingPlatform.Business.Operations.Setting;

namespace OnlineShoppingPlatform.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        // Dependency injection for the setting service
        private readonly ISettingService _settingService;
        // Constructor to initialize SettingsController with the setting service
        public SettingsController(ISettingService settingService)
        {
            _settingService = settingService;
        }
        // Endpoint to toggle the maintenance mode of the application
        [HttpPatch]
        public async Task<IActionResult> ToggleMaintenence()
        {
            // Calls the setting service to toggle maintenance mode
            await _settingService.ToggleMaintenence();

            return Ok();
        }
    }
}
