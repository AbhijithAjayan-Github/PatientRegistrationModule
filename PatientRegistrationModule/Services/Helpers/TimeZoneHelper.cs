namespace PatientRegistrationModule.Services.Helpers
{
    public class TimeZoneHelper
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<TimeZoneHelper> logger;
        public TimeZoneHelper(IConfiguration configuration, ILogger<TimeZoneHelper> logger)
        {
            this.logger = logger;
            this.configuration = configuration;
        }
        public DateTime ConvertToLocalTime(DateTime utcDateTime)
        {
            var timeZoneId = configuration["TimeZone"];
            if (string.IsNullOrWhiteSpace(timeZoneId))
            {
                logger.LogError("TimeZone configuration is missing.");
                throw new InvalidOperationException("TimeZone configuration is missing.");
            }
            logger.LogInformation($"TimeZone set in configuration {timeZoneId} ");
            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, timeZoneInfo);
        }
    }
}
