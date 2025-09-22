using HBS.ImplementationAutomationConsole.API.Controllers;
using HBS.ImplementationAutomationConsole.Core.BLL.IServices;
using HBS.ImplementationAutomationConsole.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SelectPdf;
using ILogger = Serilog.ILogger;

namespace HBS.PHRConfigAssistTest
{
    public class TemplateControllerTests
    {
        private Mock<ITemplateServices> _itemplateServiceMock;
        private Mock<ILogger> _loggerMock;
        private TemplateController _templateController;

        [SetUp]
        public void Setup()
        {
            _itemplateServiceMock = new Mock<ITemplateServices>();
            _loggerMock = new Mock<ILogger>();
            _templateController = new TemplateController(_itemplateServiceMock.Object, _loggerMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _itemplateServiceMock.Reset();
            _loggerMock.Reset();
        }

        [Test]
        public async Task DownloadTemplate_ValidStringRosterInformation_ReturnsOkObjectResult()
        {
            // Arrange
            var subsectionName = "Roster Information";
            var expectedResult = new List<string> { "String 1", "String 2" };

            _itemplateServiceMock.Setup(x => x.DownloadTemplateWithDynamicData(subsectionName)).ReturnsAsync(expectedResult).Verifiable();

            // Act
            var result = await _templateController.DownloadTemplate(subsectionName);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(expectedResult));

            //Verify
            _itemplateServiceMock.Verify(x => x.DownloadTemplateWithDynamicData(subsectionName), Times.Once);
        }

        [Test]
        public async Task DownloadTemplate_ValidString_ReturnsOkObjectResult()
        {
            // Arrange
            var subsectionName = "Demo Subsection";
            var expectedResult = new List<string> { "String 1", "String 2" };

            _itemplateServiceMock.Setup(x => x.DownloadTemplate(subsectionName)).ReturnsAsync(expectedResult).Verifiable();

            // Act
            var result = await _templateController.DownloadTemplate(subsectionName);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(expectedResult));

            //Verify
            _itemplateServiceMock.Verify(x => x.DownloadTemplate(subsectionName), Times.Once);
        }


        [Test]
        public async Task DownloadTemplate_ValidString_ReturnsBadRequestObjectResult()
        {
            // Arrange
            var subsectionName = "Demo Subsection";
            var errorMesssage = "Some Error Occured";

            _itemplateServiceMock.Setup(x => x.DownloadTemplate(subsectionName)).Throws(new Exception(errorMesssage)).Verifiable();

            // Act
            var result = await _templateController.DownloadTemplate(subsectionName);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult.Value, Is.EqualTo(errorMesssage));

            //Verify
            _itemplateServiceMock.Verify(x => x.DownloadTemplate(subsectionName), Times.Once);
            _loggerMock.Verify(x => x.Error(errorMesssage), Times.Once);
        }
    }
}