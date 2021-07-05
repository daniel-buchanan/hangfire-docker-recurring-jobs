using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Hangfire;
using Hangfire.Server;
using Hangfire.Console;

namespace hfplay.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JobsController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<JobsController> _logger;

        public JobsController(ILogger<JobsController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public StatusCodeResult Post(AddJobRequest request)
        {
            RecurringJob.AddOrUpdate(request.Name, () => Messager.Message(null, request.Message), Cron.MinuteInterval(request.NumberOfMinutes));
            return StatusCode(201);
        }

        [HttpDelete]
        public StatusCodeResult Delete(string name)
        {
            RecurringJob.RemoveIfExists(name);
            return StatusCode(204);
        }

        public class AddJobRequest
        {
            public string Name { get; set; }
            public int NumberOfMinutes { get; set; }
            public string Message { get; set; }
        }
    }

    public static class Messager
    {
        public static void Message(PerformContext context, string message)
        {
            context.WriteLine(message);
        }
    }
}
