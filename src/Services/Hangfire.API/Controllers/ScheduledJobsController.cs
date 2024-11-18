using Hangfire.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos.ScheduledJob;

namespace Hangfire.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduledJobsController : ControllerBase
    {
        private readonly IBackgroundJobService _backgroundJobService;

        public ScheduledJobsController(IBackgroundJobService backgroundJobService)
        {
            _backgroundJobService = backgroundJobService;
        }

        [HttpPost]
        [Route("send-email-reminder-checkout-order")]
        public IActionResult SendEmailReminderCheckoutOrderEmail([FromBody] ReminderCheckoutOrderDto request)
        {
            var jobId = _backgroundJobService.SendEmailContent(request.email, request.subject, request.emailContent, request.enqueueAt);
            return Ok(jobId);
        }

        [HttpDelete]
        [Route("delete/jobId/{id}")]
        public IActionResult DeleteJob(string id)
        {
            var result = _backgroundJobService.ScheduledJobService.Delete(id);
            return Ok(result);
        }
    }
}
