using System.Text;

namespace Basket.API.Services
{
    public class EmailTemplateService
    {
        private static readonly string _baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        private static readonly string _templatePath = Path.Combine(_baseDirectory, "EmailTemplate");

        protected string ReadTemplate(string fileName, string format = "html")
        {
            var path = Path.Combine(_templatePath, fileName + "." + format);
            using var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var sr = new StreamReader(fs, Encoding.Default);
            var content = sr.ReadToEnd();
            sr.Close();
            return content;
        }
    }
}
