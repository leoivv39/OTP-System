namespace OtpServer.Otp
{
    public class ConfigurationOtpContext : IOtpContext
    {
        private readonly IConfiguration _configuration;

        public ConfigurationOtpContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public int OtpSecondsToExpire => int.Parse(_configuration["Otp:SecondsToExpire"]);

        public int OtpLength => int.Parse(_configuration["Otp:Length"]);
    }
}
