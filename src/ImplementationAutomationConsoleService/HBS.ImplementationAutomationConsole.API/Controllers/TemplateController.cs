using HBS.ImplementationAutomationConsole.Core.BLL.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;

namespace HBS.ImplementationAutomationConsole.API.Controllers
{
    [ApiController]
    public class TemplateController : ControllerBase
    {
        private readonly ITemplateServices _itemplateServices;
        private readonly ILogger _logger;
        public TemplateController(
            ITemplateServices itemplateServices, ILogger logger)
        {
            _itemplateServices = itemplateServices;
            _logger = logger;
        }

        [Route("/api/UploadTemplate")]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UploadTemplate([FromForm] IFormFile file, [FromForm] string template_id, [FromForm] int module_id, [FromForm] string subsection_id)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return BadRequest("Please select a file.");

                var folderDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Uploaded Excels");
                var filePath = Path.Combine(folderDirectory, file.FileName);


                if (!Directory.Exists(folderDirectory))
                    Directory.CreateDirectory(folderDirectory);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
                var templateId = template_id;
                var moduleId = module_id;
                var subsectionID = subsection_id;
                var templateName = file.FileName;
                var isActive = true;

                await _itemplateServices.UploadTemplate(templateId, moduleId, subsectionID, templateName, fileBytes, isActive);

                return Ok("Template Successfully Uploaded");
            }
            catch(Exception ex)
            {
                _logger.Error(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Authorize]
        [Route("/api/DownloadTemplate")]
        public async Task<IActionResult> DownloadTemplate(string subsectionName)
        {
            try
            {
                var response=new List<string>();
                if(subsectionName=="Roster Information")
                {
                    response= await _itemplateServices.DownloadTemplateWithDynamicData(subsectionName);
                }
                //else if (subsectionName == "Statutory Leave")
                //{
                //    response = await _itemplateServices.DownloadTemplateWithDynamicDataForStatutoryLeave(subsectionName);
                //}
                else
                {
                    response = await _itemplateServices.DownloadTemplate(subsectionName);
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}