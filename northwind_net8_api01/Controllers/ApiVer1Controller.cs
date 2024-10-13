using Microsoft.AspNetCore.Mvc;

namespace northwind_net8_api01.Controllers
{
    [ApiController]
    [Route("v1")]
    public class ApiVer1Controller : ControllerBase
    {
        private readonly ILogger<ApiVer1Controller> _logger;

        public ApiVer1Controller(ILogger<ApiVer1Controller> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("test")]
        public IActionResult Get()
        {
            return Ok("success");
        }
    }
}
