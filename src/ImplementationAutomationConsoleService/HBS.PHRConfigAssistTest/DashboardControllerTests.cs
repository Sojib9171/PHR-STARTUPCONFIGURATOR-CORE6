using HBS.ImplementationAutomationConsole.API.Controllers;
using HBS.ImplementationAutomationConsole.Core.BLL.IServices;
using HBS.ImplementationAutomationConsole.Core.Extensions;
using HBS.ImplementationAutomationConsole.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using System.Text;
using ILogger = Serilog.ILogger;

namespace HBS.PHRConfigAssistTest
{
    public class DashboardControllerTests
    {
        private Mock<IDashboardServices> _idashboardServicesMock;
        private Mock<ISignOffService> _isignOffServicesMock;
        private Mock<IWizardDataServices> _iWizardDataServicesMock;
        private Mock<ILogger> _loggerMock;
        private DashboardController _dashboardController;

        [SetUp]
        public void Setup()
        {
            _idashboardServicesMock = new Mock<IDashboardServices>();
            _isignOffServicesMock = new Mock<ISignOffService>();
            _iWizardDataServicesMock = new Mock<IWizardDataServices>();
            _loggerMock = new Mock<ILogger>();
            _dashboardController = new DashboardController(_idashboardServicesMock.Object, _isignOffServicesMock.Object, _iWizardDataServicesMock.Object, _loggerMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _idashboardServicesMock.Reset();
            _isignOffServicesMock.Reset();
            _iWizardDataServicesMock.Reset();
            _loggerMock.Reset();
        }

        [Test]
        public async Task UploadDashboardData_DashboardPostDto_ReturnsOkObjectResult()
        {
            // Arrange
            var dashboardDto = new DashboardPostDto()
            {
                SubsectionName = "demoSubsectionName",
                UserName = "demoUser",
                Name = "demoUserName",
                Comment = "demoComment",
                ModuleID= 1,
                SubsectionID="2",
                ApprovalStatus = "Approved",
                ApprovalSignature = "demoSignature",
                ApprovalDate = new DateTime(2021, 1, 1),
                ApprovalData = new byte[1] { 0 },
                SignOffPdfData = new byte[1] { 0 },
            };

            var htmlString = "htmlString";
            var expectedString = "Data successfully inserted";

            // Mock setup
            _isignOffServicesMock.Setup(x => x.GetModuleNameFromSubsection(dashboardDto.SubsectionName)).Returns(Task.FromResult("demoModuleName")).Verifiable();

            _isignOffServicesMock.Setup(x => x.GetSignOffHtmlString(dashboardDto.Name, dashboardDto.ApprovalDate, dashboardDto.ApprovalStatus, "demoModuleName", dashboardDto.SubsectionName, dashboardDto.Comment, dashboardDto.UserName))
                .Returns(Task.FromResult(htmlString)).Verifiable();

            _idashboardServicesMock.Setup(x => x.InsertDashboardInfo(dashboardDto)).Verifiable();


            // Act
            var result = await _dashboardController.UploadDashboardData(dashboardDto);

            // Assert
            _isignOffServicesMock.Verify(x => x.GetModuleNameFromSubsection(dashboardDto.SubsectionName), Times.Once);
            _isignOffServicesMock.Verify(x => x.GetSignOffHtmlString(dashboardDto.Name, dashboardDto.ApprovalDate, dashboardDto.ApprovalStatus, "demoModuleName", dashboardDto.SubsectionName, dashboardDto.Comment, dashboardDto.UserName), Times.Once);
            _idashboardServicesMock.Verify(x => x.InsertDashboardInfo(dashboardDto), Times.Once);

            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(expectedString, okResult.Value);
        }

        [Test]
        public async Task UploadDashboardData_DashboardPostDto_ReturnsBadRequestObjectResult()
        {
            // Arrange
            var dashboardDto = new DashboardPostDto()
            {
                SubsectionName = "demoSubsectionName",
                UserName = "demoUser",
                Name = "demoUserName",
                Comment = "demoComment",
                ModuleID = 1,
                SubsectionID = "2",
                ApprovalStatus = "Approved",
                ApprovalSignature = "demoSignature",
                ApprovalDate = new DateTime(2021, 1, 1),
                ApprovalData = new byte[1] { 0 },
                SignOffPdfData = new byte[1] { 0 },
            };

            var htmlString = "htmlString";
            var expectedString = "Data successfully inserted";
            var errorMessage = "Some Error Occured";

            // Mock setup
            _isignOffServicesMock.Setup(x => x.GetModuleNameFromSubsection(dashboardDto.SubsectionName)).Returns(Task.FromResult("demoModuleName")).Verifiable();

            _isignOffServicesMock.Setup(x => x.GetSignOffHtmlString(dashboardDto.Name, dashboardDto.ApprovalDate, dashboardDto.ApprovalStatus, "demoModuleName", dashboardDto.SubsectionName, dashboardDto.Comment, dashboardDto.UserName))
                .Returns(Task.FromResult(htmlString)).Verifiable();

            _idashboardServicesMock.Setup(x => x.InsertDashboardInfo(dashboardDto)).Throws(new Exception(errorMessage)).Verifiable();


            // Act
            var result = await _dashboardController.UploadDashboardData(dashboardDto);

            // Assert
            _isignOffServicesMock.Verify(x => x.GetModuleNameFromSubsection(dashboardDto.SubsectionName), Times.Once);
            _isignOffServicesMock.Verify(x => x.GetSignOffHtmlString(dashboardDto.Name, dashboardDto.ApprovalDate, dashboardDto.ApprovalStatus, "demoModuleName", dashboardDto.SubsectionName, dashboardDto.Comment, dashboardDto.UserName), Times.Once);
            _idashboardServicesMock.Verify(x => x.InsertDashboardInfo(dashboardDto), Times.Once);


            _loggerMock.Verify(x => x.Error(It.Is<string>(msg => msg == errorMessage)), Times.Once);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult.Value, Is.EqualTo(errorMessage));
        }

        [Test]
        public async Task UploadDashboardDataForWizard_DashboardWizardPostDto_ReturnsOkObjectResult()
        {
            // Arrange
            var dashboardWizardDto = new DashboardWizardPostDto()
            {
                SubsectionName = "demoSubsectionName",
                UserName = "demoUser",
                Name = "demoUserName",
                Comment = "demoComment",
                ModuleID = 1,
                SubsectionID = "2",
                ApprovalStatus = "Approved",
                ApprovalSignature = "demoSignature",
                ApprovalDate = new DateTime(2021, 1, 1),
                ApprovalData = new byte[1] { 0 },
                SignOffPdfData = new byte[1] { 0 },
                RowIDs= new int[] { 1, 2, 3 }
            };

            var htmlString = "htmlString";
            var expectedString = "Data successfully inserted";

            var dashboardDto = new DashboardPostDto()
            {
                SubsectionName = dashboardWizardDto.SubsectionName,
                UserName = dashboardWizardDto.UserName,
                Name = dashboardWizardDto.Name,
                Comment = dashboardWizardDto.Comment,
                ModuleID = dashboardWizardDto.ModuleID,
                SubsectionID = dashboardWizardDto.SubsectionID,
                ApprovalStatus = dashboardWizardDto.ApprovalStatus,
                ApprovalSignature = dashboardWizardDto.ApprovalSignature,
                ApprovalDate = dashboardWizardDto.ApprovalDate,
                ApprovalData = dashboardWizardDto.ApprovalData,
                SignOffPdfData = dashboardWizardDto.SignOffPdfData
            };

            // Mock setup
            _isignOffServicesMock.Setup(x => x.GetModuleNameFromSubsection(dashboardWizardDto.SubsectionName)).Returns(Task.FromResult("demoModuleName")).Verifiable();

            _isignOffServicesMock.Setup(x => x.GetSignOffHtmlString(dashboardWizardDto.Name, dashboardWizardDto.ApprovalDate, dashboardWizardDto.ApprovalStatus, "demoModuleName", dashboardWizardDto.SubsectionName, dashboardWizardDto.Comment, dashboardWizardDto.UserName))
                .Returns(Task.FromResult(htmlString)).Verifiable();

            _idashboardServicesMock.Setup(x => x.InsertDashboardInfo(It.IsAny<DashboardPostDto>())).Verifiable();
            _iWizardDataServicesMock.Setup(x => x.UpdateDraftAndApprovalStatusForAbsence(dashboardWizardDto.RowIDs, dashboardDto.SubsectionName)).Verifiable();


            // Act
            var result = await _dashboardController.UploadDashboardDataForWizard(dashboardWizardDto);

            // Assert
            _isignOffServicesMock.Verify(x => x.GetModuleNameFromSubsection(dashboardDto.SubsectionName), Times.Once);
            _isignOffServicesMock.Verify(x => x.GetSignOffHtmlString(dashboardDto.Name, dashboardDto.ApprovalDate, dashboardDto.ApprovalStatus, "demoModuleName", dashboardDto.SubsectionName, dashboardDto.Comment, dashboardDto.UserName), Times.Once);
            _idashboardServicesMock.Verify(x => x.InsertDashboardInfo(It.IsAny<DashboardPostDto>()), Times.Once);
            _iWizardDataServicesMock.Verify(x => x.UpdateDraftAndApprovalStatusForAbsence(dashboardWizardDto.RowIDs, dashboardDto.SubsectionName), Times.Once);

            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(expectedString, okResult.Value);
        }

        [Test]
        public async Task UploadDashboardDataForWizard_DashboardWizardPostDto_ReturnsBadRequestObjectResult()
        {
            // Arrange
            var dashboardWizardDto = new DashboardWizardPostDto()
            {
                SubsectionName = "demoSubsectionName",
                UserName = "demoUser",
                Name = "demoUserName",
                Comment = "demoComment",
                ModuleID = 1,
                SubsectionID = "2",
                ApprovalStatus = "Approved",
                ApprovalSignature = "demoSignature",
                ApprovalDate = new DateTime(2021, 1, 1),
                ApprovalData = new byte[1] { 0 },
                SignOffPdfData = new byte[1] { 0 },
                RowIDs = new int[] { 1, 2, 3 }
            };

            var dashboardDto = new DashboardPostDto()
            {
                SubsectionName = dashboardWizardDto.SubsectionName,
                UserName = dashboardWizardDto.UserName,
                Name = dashboardWizardDto.Name,
                Comment = dashboardWizardDto.Comment,
                ModuleID = dashboardWizardDto.ModuleID,
                SubsectionID = dashboardWizardDto.SubsectionID,
                ApprovalStatus = dashboardWizardDto.ApprovalStatus,
                ApprovalSignature = dashboardWizardDto.ApprovalSignature,
                ApprovalDate = dashboardWizardDto.ApprovalDate,
                ApprovalData = dashboardWizardDto.ApprovalData,
                SignOffPdfData = dashboardWizardDto.SignOffPdfData
            };

            var htmlString = "htmlString";
            var expectedString = "Data successfully inserted";
            var errorMessage = "Some Error Occured";

            // Mock setup
            _isignOffServicesMock.Setup(x => x.GetModuleNameFromSubsection(dashboardDto.SubsectionName)).Returns(Task.FromResult("demoModuleName")).Verifiable();

            _isignOffServicesMock.Setup(x => x.GetSignOffHtmlString(dashboardDto.Name, dashboardDto.ApprovalDate, dashboardDto.ApprovalStatus, "demoModuleName", dashboardDto.SubsectionName, dashboardDto.Comment, dashboardDto.UserName))
                .Returns(Task.FromResult(htmlString)).Verifiable();

            _idashboardServicesMock.Setup(x => x.InsertDashboardInfo(It.IsAny<DashboardPostDto>())).Throws(new Exception(errorMessage)).Verifiable();
            _iWizardDataServicesMock.Setup(x => x.UpdateDraftAndApprovalStatusForAbsence(dashboardWizardDto.RowIDs, dashboardDto.SubsectionName));

            // Act
            var result = await _dashboardController.UploadDashboardDataForWizard(dashboardWizardDto);

            // Assert
            _isignOffServicesMock.Verify(x => x.GetModuleNameFromSubsection(dashboardDto.SubsectionName), Times.Once);
            _isignOffServicesMock.Verify(x => x.GetSignOffHtmlString(dashboardDto.Name, dashboardDto.ApprovalDate, dashboardDto.ApprovalStatus, "demoModuleName", dashboardDto.SubsectionName, dashboardDto.Comment, dashboardDto.UserName), Times.Once);
            _idashboardServicesMock.Verify(x => x.InsertDashboardInfo(It.IsAny<DashboardPostDto>()), Times.Once);
      


            _loggerMock.Verify(x => x.Error(It.Is<string>(msg => msg == errorMessage)), Times.Once);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult.Value, Is.EqualTo(errorMessage));
        }

        [Test]
        public async Task GetDashboardData_ValidString_ReturnsOkObjectResult()
        {
            // Arrange
            var serverParams = "{\"sortField\":\"module_name\",\"sortType\":\"asc\",\"page\":1,\"perPage\":10,\"searchText\":\"\"}";
            var parameters = JsonConvert.DeserializeObject<ServerParamsDto>(serverParams);
            var expectedResult = new object();

            // Mock setup
            _idashboardServicesMock.Setup(x => x.GetDashboardInfo(It.IsAny<ServerParamsDto>()))
                .Returns(Task.FromResult(expectedResult)).Verifiable();

            // Act
            var result = await _dashboardController.GetDashboardData(serverParams);

            // Assert
            _idashboardServicesMock.Verify(x => x.GetDashboardInfo(It.IsAny<ServerParamsDto>()), Times.Once);
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(expectedResult));
        }

        [Test]
        public async Task GetApprovalSummaryListForVendor_ValidString_ReturnsBadRequestObjectResult()
        {
            // Arrange
            var serverParams = "{\"sortField\":\"module_name\",\"sortType\":\"asc\",\"page\":1,\"perPage\":10,\"searchText\":\"\"}";
            var parameters = JsonConvert.DeserializeObject<ServerParamsDto>(serverParams);
            var errorMessage = "Some error occurred";

            // Mock setup
            _idashboardServicesMock.Setup(x => x.GetDashboardInfo(It.IsAny<ServerParamsDto>()))
                .Throws(new Exception(errorMessage)).Verifiable();

            // Act
            var result = await _dashboardController.GetDashboardData(serverParams);

            // Assert
            _idashboardServicesMock.Verify(x => x.GetDashboardInfo(It.IsAny<ServerParamsDto>()), Times.Once);
            _loggerMock.Verify(x => x.Error(It.Is<string>(msg => msg == errorMessage)), Times.Once);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult.Value, Is.EqualTo(errorMessage));
        }

        [Test]
        public async Task GetDashboardHistoryData_ValidString_ReturnsOkObjectResult()
        {
            // Arrange
            var serverParams = "{\"subsection\":\"Bank Details\",\"sortField\":\"vendor_approval_date\",\"sortType\":\"desc\",\"page\":1,\"perPage\":10,\"searchText\":\"\"}";
            var parameters = JsonConvert.DeserializeObject<HistoryServerParamsDto>(serverParams);
            var expectedResult = new object();

            // Mock setup
            _idashboardServicesMock.Setup(x => x.GetDashboardHistoryInfo(It.IsAny<HistoryServerParamsDto>()))
                .Returns(Task.FromResult(expectedResult)).Verifiable();

            // Act
            var result = await _dashboardController.GetDashboardHistoryData(serverParams);

            // Assert
            _idashboardServicesMock.Verify(x => x.GetDashboardHistoryInfo(It.IsAny<HistoryServerParamsDto>()), Times.Once);
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(expectedResult));
        }

        [Test]
        public async Task GetDashboardHistoryData_ValidString_ReturnsBadRequestObjectResult()
        {
            // Arrange
            var serverParams = "{\"subsection\":\"Bank Details\",\"sortField\":\"vendor_approval_date\",\"sortType\":\"desc\",\"page\":1,\"perPage\":10,\"searchText\":\"\"}";
            var parameters = JsonConvert.DeserializeObject<HistoryServerParamsDto>(serverParams);
            var errorMessage = "Some error occurred";

            // Mock setup
            _idashboardServicesMock.Setup(x => x.GetDashboardHistoryInfo(It.IsAny<HistoryServerParamsDto>()))
                .Throws(new Exception(errorMessage)).Verifiable();

            // Act
            var result = await _dashboardController.GetDashboardHistoryData(serverParams);

            // Assert
            _idashboardServicesMock.Verify(x => x.GetDashboardHistoryInfo(It.IsAny<HistoryServerParamsDto>()), Times.Once);
            _loggerMock.Verify(x => x.Error(It.Is<string>(msg => msg == errorMessage)), Times.Once);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult.Value, Is.EqualTo(errorMessage));
        }
    }
}
