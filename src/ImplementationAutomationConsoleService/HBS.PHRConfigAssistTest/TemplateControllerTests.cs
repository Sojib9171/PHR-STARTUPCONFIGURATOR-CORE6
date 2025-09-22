using HBS.ImplementationAutomationConsole.API.Controllers;
using HBS.ImplementationAutomationConsole.Core.BLL.IServices;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ILogger = Serilog.ILogger;

namespace HBS.PHRConfigAssistTest
{
    public class ValidationControllerTests
    {
        private Mock<IValidationServices> _ivalidationServiceMock;
        private Mock<ILogger> _loggerMock;
        private ValidationController _validationController;

        [SetUp]
        public void Setup()
        {
            _ivalidationServiceMock = new Mock<IValidationServices>();
            _loggerMock = new Mock<ILogger>();
            _validationController = new ValidationController(_ivalidationServiceMock.Object, _loggerMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _ivalidationServiceMock.Reset();
            _loggerMock.Reset();
        }

        [Test]
        public async Task GetValidationOverview_ValidString_ReturnsOkObjectResult()
        {
            // Arrange
            var subsectionName = "Demo Subsection";
            var expectedResult = new List<int> { 1, 2, 3 };

            _ivalidationServiceMock.Setup(x => x.ValidateUploadedData(subsectionName)).Verifiable();
            _ivalidationServiceMock.Setup(x => x.GetValidationCounts(subsectionName)).ReturnsAsync(expectedResult).Verifiable();

            // Act
            var result = await _validationController.GetValidationOverview(subsectionName);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(expectedResult));

            //Verify
            _ivalidationServiceMock.Verify(x => x.ValidateUploadedData(subsectionName), Times.Once);
            _ivalidationServiceMock.Verify(x => x.GetValidationCounts(subsectionName), Times.Once);
        }

        [Test]
        public async Task GetValidationOverview_ValidString_ReturnsBadRequestObjectResult()
        {
            // Arrange
            var subsectionName = "Demo Subsection";
            var errorMesssage = "Some Error Occured";

            _ivalidationServiceMock.Setup(x => x.ValidateUploadedData(subsectionName)).Throws(new Exception(errorMesssage)).Verifiable();

            // Act
            var result = await _validationController.GetValidationOverview(subsectionName);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult.Value, Is.EqualTo(errorMesssage));

            //Verify
            _ivalidationServiceMock.Verify(x => x.ValidateUploadedData(subsectionName), Times.Once);
            _loggerMock.Verify(x => x.Error(errorMesssage), Times.Once);
        }


        [Test]
        public async Task DownloadValidatedExcel_ValidString_ReturnsOkObjectResult()
        {
            // Arrange
            var subsectionName = "Demo Subsection";
            var expectedResult = new List<string> { "base64String", "templateName" };

            _ivalidationServiceMock.Setup(x => x.DownloadValidatedExcel(subsectionName)).ReturnsAsync(expectedResult).Verifiable();

            // Act
            var result = await _validationController.DownloadValidatedExcel(subsectionName);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(expectedResult));

            //Verify
            _ivalidationServiceMock.Verify(x => x.DownloadValidatedExcel(subsectionName), Times.Once);
        }

        [Test]
        public async Task DownloadValidatedExcel_ValidString_ReturnsBadRequestObjectResult()
        {
            // Arrange
            var subsectionName = "Demo Subsection";
            var errorMesssage = "Some Error Occured";

            _ivalidationServiceMock.Setup(x => x.DownloadValidatedExcel(subsectionName)).Throws(new Exception(errorMesssage)).Verifiable();

            // Act
            var result = await _validationController.DownloadValidatedExcel(subsectionName);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult.Value, Is.EqualTo(errorMesssage));

            //Verify
            _ivalidationServiceMock.Verify(x => x.DownloadValidatedExcel(subsectionName), Times.Once);
            _loggerMock.Verify(x => x.Error(errorMesssage), Times.Once);
        }
    }
}