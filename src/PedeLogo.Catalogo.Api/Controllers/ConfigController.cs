using Microsoft.AspNetCore.Mvc;
using PedeLogo.Catalogo.Api.Config;

namespace PedeLogo.Catalogo.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ConfigController : ControllerBase
    {
        [HttpPut("unreadfor/{seconds}")]
        public IActionResult UnreadFor([FromRoute] int seconds)
        {
            ConfigManager.SetUnread(seconds);
            return Ok();
        }

        [HttpPut("unhealth")]
        public IActionResult UnHealth()
        {
            ConfigManager.SetUnHealth();
            return Ok();
        }
    }
}
