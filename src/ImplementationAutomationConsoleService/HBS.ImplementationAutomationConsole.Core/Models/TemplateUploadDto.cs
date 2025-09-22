using Microsoft.AspNetCore.Http;

namespace HBS.ImplementationAutomationConsole.Core.Models
{
    public class TemplateUploadDto
    {
        public IFormFile file { get; set; }
        public string myString { get; set; }
    }
}