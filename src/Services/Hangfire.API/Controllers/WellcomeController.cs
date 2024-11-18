using Contracts.ScheduledJobs;
using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;

namespace Hangfire.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WellcomeController : ControllerBase
    {
        private readonly IScheduledJobService _scheduledJobService;
        private readonly ILogger _logger;

        public WellcomeController(IScheduledJobService scheduledJobService, ILogger logger)
        {
            _scheduledJobService = scheduledJobService;
            _logger = logger;
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult Wellcome()
        {
            string? jobId = _scheduledJobService.Enqueue(() => ResponseWellcome("Hello world."));
            return Ok($"Job Id: {jobId} - Enqueue Job");
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult DelayedWellcome()
        {
            int second = 5;
            string? jobId = _scheduledJobService.Schedule(() => ResponseWellcome("Hello world."), TimeSpan.FromSeconds(second));
            return Ok($"Job Id: {jobId} - Schedule Job");
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult WellcomeAt()
        {
            DateTimeOffset enqueueAt = DateTimeOffset.UtcNow.AddSeconds(10);
            string? jobId = _scheduledJobService.Schedule(() => ResponseWellcome("Hello world."), enqueueAt);
            return Ok($"Job Id: {jobId} - Schedule Job");
        }

        [NonAction]
        public void ResponseWellcome(string text) => _logger.Information(messageTemplate: text);
    }
}
