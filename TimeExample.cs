using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace FunctionP1
{
    public class TimeExample
    {
        private readonly ILogger _logger;

        public TimeExample(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<TimeExample>();
        }

        [Function("TimeExample")]
        public void Run([TimerTrigger("* * * * * *")] TimerInfo myTimer)
        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            if (myTimer.ScheduleStatus is not null)
            {
                _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
            }
        }
    }
}
