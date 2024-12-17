using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Shared.Services.Email
{
    public class MailRequest
    {
        [EmailAddress]
        public string From { get; set; } = string.Empty;

        [EmailAddress]
        public string ToAddress { get; set; } = string.Empty;

        public IEnumerable<string> ToAddresses { get; set; } = new List<string>();

        [Required]
        public string Subject { get; set; } = string.Empty;

        [Required]
        public string Body { get; set; } = string.Empty;

        public IFormFileCollection? Attachments { get; set; }
    }
}
