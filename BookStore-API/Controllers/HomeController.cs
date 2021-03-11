using BookStore_API.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace BookStore_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly ILoggerService _logger;

        public HomeController(ILoggerService logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// This is the documentation
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            _logger.LogInfo("Test calling LogInfo");
            return Ok("Hello World!");
        }
    }
}
