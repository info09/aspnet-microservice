using Contracts.Configurations;

namespace Infrastructure.Configurations
{
    public class SMTPEmailSetting : IEmailSMTPSettings
    {
        public string DisplayName { get; set; } = string.Empty;
        public bool EnableVerification { get; set; }
        public string From { get; set; } = string.Empty;
        public string SMTPServer { get; set; } = string.Empty;
        public bool UseSsl { get; set; }
        public int Port { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
