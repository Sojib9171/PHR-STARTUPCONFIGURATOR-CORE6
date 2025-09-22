using HBS.ImplementationAutomationConsole.API.Controllers;
using HBS.ImplementationAutomationConsole.Core.BLL.IServices;
using HBS.ImplementationAutomationConsole.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using SelectPdf;
using ILogger = Serilog.ILogger;

namespace HBS.PHRConfigAssistTest
{
    public class SignOffControllerTests
    {
        private Mock<ISignOffService> _isignOffServiceMock;
        private Mock<ILogger> _loggerMock;
        private SignOffController _signOffController;

        [SetUp]
        public void Setup()
        {
            _isignOffServiceMock = new Mock<ISignOffService>();
            _loggerMock = new Mock<ILogger>();
            _signOffController = new SignOffController(_isignOffServiceMock.Object, _loggerMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _isignOffServiceMock.Reset();
            _loggerMock.Reset();
        }

        [Test]
        public async Task GetSignOffDocument_DashboardPostDto_ReturnsOkObjectResult()
        {
            // Arrange
            var dashboardDto = new DashboardPostDto
            {
                Name = "DemoName",
                ApprovalDate = new DateTime(2021, 11, 25),
                ApprovalStatus = "Approved",
                SubsectionName = "DemoSubsection",
                Comment = "DemoComment",
                UserName = "DemoUser"
            };
            var moduleName = "DemoModule";
            var htmlString = "DemoHtmlString";
            //var pdfBytes = new byte[] { 1, 2, 3, 4, 5 };


            _isignOffServiceMock.Setup(x => x.GetModuleNameFromSubsection(dashboardDto.SubsectionName)).ReturnsAsync(moduleName).Verifiable();
            _isignOffServiceMock.Setup(x => x.GetSignOffHtmlString(dashboardDto.Name, dashboardDto.ApprovalDate, dashboardDto.ApprovalStatus
                               , moduleName, dashboardDto.SubsectionName, dashboardDto.Comment, dashboardDto.UserName)).ReturnsAsync(htmlString).Verifiable();


            var converter = new HtmlToPdf();
            PdfDocument doc = converter.ConvertHtmlString(htmlString);
            byte[] pdfBytes = doc.Save();
            doc.Close();

            //Act
            var result = await _signOffController.GetSignOffDocument(dashboardDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okResult = (OkObjectResult)result;
            Assert.IsNotNull(okResult.Value);
            Assert.IsInstanceOf<byte[]>(okResult.Value);

            //Verify
            _isignOffServiceMock.Verify(x => x.GetModuleNameFromSubsection(dashboardDto.SubsectionName), Times.Once);
            _isignOffServiceMock.Verify(x => x.GetSignOffHtmlString(dashboardDto.Name, dashboardDto.ApprovalDate, dashboardDto.ApprovalStatus
                                              , moduleName, dashboardDto.SubsectionName, dashboardDto.Comment, dashboardDto.UserName), Times.Once);
        }

        [Test]
        public async Task GetSignOffDocument_DashboardPostDto_ReturnsBadRequestbjectResult()
        {
            // Arrange
            var dashboardDto = new DashboardPostDto
            {
                Name = "DemoName",
                ApprovalDate = new DateTime(2021, 11, 25),
                ApprovalStatus = "Approved",
                SubsectionName = "DemoSubsection",
                Comment = "DemoComment",
                UserName = "DemoUser"
            };
            var moduleName = "DemoModule";
            var htmlString = "DemoHtmlString";
            var exception = new Exception("DemoException");

            _isignOffServiceMock.Setup(x => x.GetModuleNameFromSubsection(dashboardDto.SubsectionName)).ReturnsAsync(moduleName).Verifiable();
            _isignOffServiceMock.Setup(x => x.GetSignOffHtmlString(dashboardDto.Name, dashboardDto.ApprovalDate, dashboardDto.ApprovalStatus
                                              , moduleName, dashboardDto.SubsectionName, dashboardDto.Comment, dashboardDto.UserName)).ThrowsAsync(exception).Verifiable();

            //Act
            var result = await _signOffController.GetSignOffDocument(dashboardDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);

            var badRequestResult = (BadRequestObjectResult)result;
            Assert.IsNotNull(badRequestResult.Value);
            Assert.IsInstanceOf<string>(badRequestResult.Value);
            Assert.AreEqual(exception.Message, badRequestResult.Value);

            //Verify
            _isignOffServiceMock.Verify(x => x.GetModuleNameFromSubsection(dashboardDto.SubsectionName), Times.Once);
            _isignOffServiceMock.Verify(x => x.GetSignOffHtmlString(dashboardDto.Name, dashboardDto.ApprovalDate, dashboardDto.ApprovalStatus
                                                             , moduleName, dashboardDto.SubsectionName, dashboardDto.Comment, dashboardDto.UserName), Times.Once);
            _loggerMock.Verify(x => x.Error(exception.Message), Times.Once);
        }

        [Test]
        public async Task GenerateSignOffDocumentForCommonConfig_CommonConfigPostDto_ReturnsOkObjectResult()
        {
            // Arrange
            var commonConfigPostDto = new CommonConfigPostDto
            {
                Name = "DemoName",
                ApprovalDate = new DateTime(2021, 11, 25),
                ApprovalStatus = "Approved",
                Comment = "DemoComment",
                UserName = "DemoUser"
            };
            var htmlString = "DemoHtmlString";

            _isignOffServiceMock.Setup(x => x.GetSignOffHtmlStringForConfiguration(commonConfigPostDto.Name, commonConfigPostDto.ApprovalDate, commonConfigPostDto.ApprovalStatus
                                                             , commonConfigPostDto.Comment, commonConfigPostDto.UserName, "Common Configuration")).ReturnsAsync(htmlString).Verifiable();

            var converter = new HtmlToPdf();
            PdfDocument doc = converter.ConvertHtmlString(htmlString);
            byte[] pdfBytes = doc.Save();
            doc.Close();

            //Act
            var result = await _signOffController.GenerateSignOffDocumentForCommonConfig(commonConfigPostDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okResult = (OkObjectResult)result;
            Assert.IsNotNull(okResult.Value);
            Assert.IsInstanceOf<byte[]>(okResult.Value);

            //Verify
            _isignOffServiceMock.Verify(x => x.GetSignOffHtmlStringForConfiguration(commonConfigPostDto.Name, commonConfigPostDto.ApprovalDate, commonConfigPostDto.ApprovalStatus
                                                             , commonConfigPostDto.Comment, commonConfigPostDto.UserName, "Common Configuration"), Times.Once);
        }

        [Test]
        public async Task GenerateSignOffDocumentForCommonConfig_CommonConfigPostDto_ReturnsBadRequestbjectResult()
        {
            // Arrange
            var commonConfigPostDto = new CommonConfigPostDto
            {
                Name = "DemoName",
                ApprovalDate = new DateTime(2021, 11, 25),
                ApprovalStatus = "Approved",
                Comment = "DemoComment",
                UserName = "DemoUser"
            };
            var htmlString = "DemoHtmlString";
            var exception = new Exception("DemoException");

            _isignOffServiceMock.Setup(x => x.GetSignOffHtmlStringForConfiguration(commonConfigPostDto.Name, commonConfigPostDto.ApprovalDate, commonConfigPostDto.ApprovalStatus
                                                                            , commonConfigPostDto.Comment, commonConfigPostDto.UserName, "Common Configuration")).ThrowsAsync(exception).Verifiable();

            //Act
            var result = await _signOffController.GenerateSignOffDocumentForCommonConfig(commonConfigPostDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);

            var badRequestResult = (BadRequestObjectResult)result;
            Assert.IsNotNull(badRequestResult.Value);
            Assert.IsInstanceOf<string>(badRequestResult.Value);
            Assert.AreEqual(exception.Message, badRequestResult.Value);

            //Verify
            _isignOffServiceMock.Verify(x => x.GetSignOffHtmlStringForConfiguration(commonConfigPostDto.Name, commonConfigPostDto.ApprovalDate, commonConfigPostDto.ApprovalStatus
                                                                            , commonConfigPostDto.Comment, commonConfigPostDto.UserName, "Common Configuration"), Times.Once);
            _loggerMock.Verify(x => x.Error(exception.Message), Times.Once);
        }

        [Test]
        public async Task GenerateSignOffDocumentForConfigControl_ConfigControlPostDto_ReturnsOkObjectResult()
        {
            // Arrange
            var configControlPostDto = new ConfigControlPostDto
            {
                Name = "DemoName",
                ApprovalDate = new DateTime(2021, 11, 25),
                ApprovalStatus = "Approved",
                Comment = "DemoComment",
                UserName = "DemoUser"
            };
            var htmlString = "DemoHtmlString";

            _isignOffServiceMock.Setup(x => x.GetSignOffHtmlStringForConfiguration(configControlPostDto.Name, configControlPostDto.ApprovalDate, configControlPostDto.ApprovalStatus
                                                                                           , configControlPostDto.Comment, configControlPostDto.UserName, "Configuration Control")).ReturnsAsync(htmlString).Verifiable();

            var converter = new HtmlToPdf();
            PdfDocument doc = converter.ConvertHtmlString(htmlString);
            byte[] pdfBytes = doc.Save();
            doc.Close();

            //Act
            var result = await _signOffController.GenerateSignOffDocumentForConfigControl(configControlPostDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okResult = (OkObjectResult)result;
            Assert.IsNotNull(okResult.Value);
            Assert.IsInstanceOf<byte[]>(okResult.Value);

            //Verify
            _isignOffServiceMock.Verify(x => x.GetSignOffHtmlStringForConfiguration(configControlPostDto.Name, configControlPostDto.ApprovalDate, configControlPostDto.ApprovalStatus
                                                                                           , configControlPostDto.Comment, configControlPostDto.UserName, "Configuration Control"), Times.Once);
        }

        [Test]
        public async Task GenerateSignOffDocumentForConfigControl_ConfigControlPostDto_ReturnsBadRequestbjectResult()
        {
            // Arrange
            var configControlPostDto = new ConfigControlPostDto
            {
                Name = "DemoName",
                ApprovalDate = new DateTime(2021, 11, 25),
                ApprovalStatus = "Approved",
                Comment = "DemoComment",
                UserName = "DemoUser"
            };
            var htmlString = "DemoHtmlString";
            var exception = new Exception("DemoException");

            _isignOffServiceMock.Setup(x => x.GetSignOffHtmlStringForConfiguration(configControlPostDto.Name, configControlPostDto.ApprovalDate, configControlPostDto.ApprovalStatus
                                                                                                          , configControlPostDto.Comment, configControlPostDto.UserName, "Configuration Control")).ThrowsAsync(exception).Verifiable();

            //Act
            var result = await _signOffController.GenerateSignOffDocumentForConfigControl(configControlPostDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);

            var badRequestResult = (BadRequestObjectResult)result;
            Assert.IsNotNull(badRequestResult.Value);
            Assert.IsInstanceOf<string>(badRequestResult.Value);
            Assert.AreEqual(exception.Message, badRequestResult.Value);

            //Verify
            _isignOffServiceMock.Verify(x => x.GetSignOffHtmlStringForConfiguration(configControlPostDto.Name, configControlPostDto.ApprovalDate, configControlPostDto.ApprovalStatus
                                                                                                          , configControlPostDto.Comment, configControlPostDto.UserName, "Configuration Control"), Times.Once);
            _loggerMock.Verify(x => x.Error(exception.Message), Times.Once);
        }

        [Test]
        public async Task DownloadSignOffPdfWithRecordID_ValidInteger_ReturnsOkObjectResult()
        {
            // Arrange
            var recordId = 1;
            var pdfBytes = new byte[] { 1, 2, 3, 4, 5 };

            _isignOffServiceMock.Setup(x => x.GetSignOffPdfAsByteArray(recordId)).ReturnsAsync(pdfBytes).Verifiable();

            //Act
            var result = await _signOffController.DownloadSignOffPdfWithRecordID(recordId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okResult = (OkObjectResult)result;
            Assert.IsNotNull(okResult.Value);
            Assert.IsInstanceOf<byte[]>(okResult.Value);
            Assert.That(okResult.Value, Is.EqualTo(pdfBytes));

            //Verify
            _isignOffServiceMock.Verify(x => x.GetSignOffPdfAsByteArray(recordId), Times.Once);
        }

        [Test]
        public async Task DownloadSignOffPdfWithRecordID_ValidInteger_ReturnsBadRequestObjectResult()
        {
            // Arrange
            var recordId = 1;
            var exception = new Exception("DemoException");

            _isignOffServiceMock.Setup(x => x.GetSignOffPdfAsByteArray(recordId)).ThrowsAsync(exception).Verifiable();

            //Act
            var result = await _signOffController.DownloadSignOffPdfWithRecordID(recordId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);

            var badRequestResult = (BadRequestObjectResult)result;
            Assert.IsNotNull(badRequestResult.Value);
            Assert.IsInstanceOf<string>(badRequestResult.Value);
            Assert.That(badRequestResult.Value, Is.EqualTo(exception.Message));

            //Verify
            _isignOffServiceMock.Verify(x => x.GetSignOffPdfAsByteArray(recordId), Times.Once);
            _loggerMock.Verify(x => x.Error(exception.Message), Times.Once);
        }
    }
}