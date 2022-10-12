using Microsoft.AspNetCore.Mvc;
using MM.Mwnz.Services;
using Mwnz.Client.Services;

namespace MM.Mwnz.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompanyController : ControllerBase
    {
        private readonly ILogger<CompanyController> _logger;
        private readonly IMwnzClientService _mwnzClientService;

        public CompanyController(ILogger<CompanyController> logger, IMwnzClientService mwnzClientService)
        {
            _logger = logger;
            _mwnzClientService = mwnzClientService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCompany(double id)
        {
            try
            {
                return Ok(await _mwnzClientService.GetCompany(id));
            }
            catch (ApiException e)
            {
                _logger.LogWarning($"Could not make successful call. Error: {e.Message}");
                return BadRequest();
            }
        }
    }
}