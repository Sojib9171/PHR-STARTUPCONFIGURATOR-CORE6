using HBS.ImplementationAutomationConsole.API.Controllers;
using HBS.ImplementationAutomationConsole.Core.BLL.IServices;
using HBS.ImplementationAutomationConsole.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using ILogger = Serilog.ILogger;

namespace HBS.PHRConfigAssistTest
{
    public class AdminControllerTests
    {
        private Mock<IAdminServices> _adminServicesMock;
        private Mock<ILogger> _loggerMock;
        private AdminController _adminController;

        [SetUp]
        public void Setup()
        {
            _adminServicesMock = new Mock<IAdminServices>();
            _loggerMock = new Mock<ILogger>();
            _adminController = new AdminController(_adminServicesMock.Object, _loggerMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _adminServicesMock.Reset();
            _loggerMock.Reset();
        }


        [Test]
        public async Task UploadAdminData_VendorPostDto_ReturnsOkObjectResult()
        {
            // Arrange
            var vendorDto = new VendorPostDto()
            {
                VendorName = "demoVendorName",
                UserRecordId = 1,
                VendorApprovalComment="demoComment",
                VendorApprovalStatus="demoStatus",
                VendorApprovalDate=DateTime.Now,
                SubsectionName="demoSubsectionName"
            };

            // Mock setup
            _adminServicesMock.Setup(x => x.InsertVendorInfo(It.IsAny<VendorPostDto>()))
                .Returns(Task.FromResult(true)).Verifiable();
            // Act
            var result = await _adminController.UploadAdminData(vendorDto);
            _adminServicesMock.Verify(x => x.InsertVendorInfo(vendorDto), Times.Once);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual("Data successfully inserted", okResult.Value);
        }

        [Test]
        public async Task UploadAdminData_VendorPostDto_ReturnsBadRequestObjectResult()
        {
            // Arrange
            var vendorDto = new VendorPostDto()
            {
                VendorName = "demoVendorName",
                UserRecordId = 1,
                VendorApprovalComment = "demoComment",
                VendorApprovalStatus = "demoStatus",
                VendorApprovalDate = DateTime.Now,
                SubsectionName = "demoSubsectionName"
            };
            var errorMessage = "Some error occurred";

            // Mock setup
            _adminServicesMock.Setup(x => x.InsertVendorInfo(vendorDto))
                .Throws(new Exception(errorMessage)).Verifiable();

            // Act
            var result = await _adminController.UploadAdminData(vendorDto);


            // Assert
            _adminServicesMock.Verify(x => x.InsertVendorInfo(vendorDto), Times.Once);
            _loggerMock.Verify(x => x.Error(It.Is<string>(msg => msg == errorMessage)), Times.Once);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult.Value, Is.EqualTo(errorMessage));
        }

        [Test]
        public async Task GetPendingApprovalListForVendor_ValidString_ReturnsOkObjectResult()
        {
            // Arrange
            var serverParams = "{\"sortField\":\"module_name\",\"sortType\":\"asc\",\"page\":1,\"perPage\":10,\"searchText\":\"\"}";
            var parameters = JsonConvert.DeserializeObject<ServerParamsDto>(serverParams);
            var expectedResult = new object();

            // Mock setup
            _adminServicesMock.Setup(x => x.GetApprovalFromVendor(It.IsAny<ServerParamsDto>()))
                .Returns(Task.FromResult(expectedResult)).Verifiable();

            // Act
            var result = await _adminController.GetPendingApprovalListForVendor(serverParams);

            // Assert
            _adminServicesMock.Verify(x => x.GetApprovalFromVendor(It.IsAny<ServerParamsDto>()), Times.Once);
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(expectedResult));
        }

        [Test]
        public async Task GetPendingApprovalListForVendor_ValidString_ReturnsBadRequestObjectResult()
        {
            // Arrange
            var serverParams = "{\"sortField\":\"module_name\",\"sortType\":\"asc\",\"page\":1,\"perPage\":10,\"searchText\":\"\"}";
            var parameters = JsonConvert.DeserializeObject<ServerParamsDto>(serverParams);
            var errorMessage = "Some error occurred";

            // Mock setup
            _adminServicesMock.Setup(x => x.GetApprovalFromVendor(It.IsAny<ServerParamsDto>()))
                .Throws(new Exception(errorMessage)).Verifiable();

            // Act
            var result = await _adminController.GetPendingApprovalListForVendor(serverParams);

            // Assert
            _adminServicesMock.Verify(x => x.GetApprovalFromVendor(It.IsAny<ServerParamsDto>()), Times.Once);
            _loggerMock.Verify(x => x.Error(It.Is<string>(msg => msg == errorMessage)), Times.Once);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult.Value, Is.EqualTo(errorMessage));
        }

        [Test]
        public async Task GetApprovalSummaryListForVendor_ValidString_ReturnsOkObjectResult()
        {
            // Arrange
            var serverParams = "{\"sortField\":\"module_name\",\"sortType\":\"asc\",\"page\":1,\"perPage\":10,\"searchText\":\"\"}";
            var parameters = JsonConvert.DeserializeObject<ServerParamsDto>(serverParams);
            var expectedResult = new object();

            // Mock setup
            _adminServicesMock.Setup(x => x.GetApprovedDataListForVendor(It.IsAny<ServerParamsDto>()))
                .Returns(Task.FromResult(expectedResult)).Verifiable();

            // Act
            var result = await _adminController.GetApprovalSummaryListForVendor(serverParams);

            // Assert
            _adminServicesMock.Verify(x => x.GetApprovedDataListForVendor(It.IsAny<ServerParamsDto>()), Times.Once);
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
            _adminServicesMock.Setup(x => x.GetApprovedDataListForVendor(It.IsAny<ServerParamsDto>()))
                .Throws(new Exception(errorMessage)).Verifiable();

            // Act
            var result = await _adminController.GetApprovalSummaryListForVendor(serverParams);

            // Assert
            _adminServicesMock.Verify(x => x.GetApprovedDataListForVendor(It.IsAny<ServerParamsDto>()), Times.Once);
            _loggerMock.Verify(x => x.Error(It.Is<string>(msg => msg == errorMessage)), Times.Once);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult.Value, Is.EqualTo(errorMessage));
        }

        [Test]
        public async Task DownloadExcelWithData_ValidInteger_ReturnsOkObjectResult()
        {
            // Arrange
            var recordId = 1;
            var expectedResult = "Success";

            // Mock setup
            _adminServicesMock.Setup(x => x.GetExcelDataAsByteArray(recordId))
                .Returns(Task.FromResult(expectedResult)).Verifiable();

            // Act
            var result = await _adminController.DownloadExcelWithData(recordId);

            // Assert
            _adminServicesMock.Verify(x => x.GetExcelDataAsByteArray(recordId), Times.Once);
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(expectedResult));
        }

        [Test]
        public async Task DownloadExcelWithData_ValidInteger_ReturnsBadRequestObjectResult()
        {
            // Arrange
            var recordId = 1;
            var errorMessage = "Some error occurred";

            // Mock setup
            _adminServicesMock.Setup(x => x.GetExcelDataAsByteArray(recordId))
                .Throws(new Exception(errorMessage)).Verifiable();

            // Act
            var result = await _adminController.DownloadExcelWithData(recordId);

            // Assert
            _adminServicesMock.Verify(x => x.GetExcelDataAsByteArray(recordId), Times.Once);
            _loggerMock.Verify(x => x.Error(It.Is<string>(msg => msg == errorMessage)), Times.Once);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult.Value, Is.EqualTo(errorMessage));
        }


        [Test]
        public async Task GetVendorDashboardData_ValidString_ReturnsOkObjectResult()
        {
            // Arrange
            var serverParams = "{\"subsection\":\"Bank Details\",\"sortField\":\"vendor_approval_date\",\"sortType\":\"desc\",\"page\":1,\"perPage\":10,\"searchText\":\"\"}";
            var parameters = JsonConvert.DeserializeObject<HistoryServerParamsDto>(serverParams);
            var expectedResult = new object();

            // Mock setup
            _adminServicesMock.Setup(x => x.GetVendorDashboardHistoryInfo(It.IsAny<HistoryServerParamsDto>()))
                .Returns(Task.FromResult(expectedResult)).Verifiable();

            // Act
            var result = await _adminController.GetVendorDashboardData(serverParams);

            // Assert
            _adminServicesMock.Verify(x => x.GetVendorDashboardHistoryInfo(It.IsAny<HistoryServerParamsDto>()), Times.Once);
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(expectedResult));
        }

        [Test]
        public async Task GetVendorDashboardData_ValidString_ReturnsBadRequestObjectResult()
        {
            // Arrange
            var serverParams = "{\"subsection\":\"Bank Details\",\"sortField\":\"vendor_approval_date\",\"sortType\":\"desc\",\"page\":1,\"perPage\":10,\"searchText\":\"\"}";
            var parameters = JsonConvert.DeserializeObject<HistoryServerParamsDto>(serverParams);
            var errorMessage = "Some error occurred";

            // Mock setup
            _adminServicesMock.Setup(x => x.GetVendorDashboardHistoryInfo(It.IsAny<HistoryServerParamsDto>()))
                .Throws(new Exception(errorMessage)).Verifiable();

            // Act
            var result = await _adminController.GetVendorDashboardData(serverParams);

            // Assert
            _adminServicesMock.Verify(x => x.GetVendorDashboardHistoryInfo(It.IsAny<HistoryServerParamsDto>()), Times.Once);
            _loggerMock.Verify(x => x.Error(It.Is<string>(msg => msg == errorMessage)), Times.Once);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult.Value, Is.EqualTo(errorMessage));
        }

        [Test]
        public async Task GetRemainingApprovalCount_NoParameter_ReturnsOkObjectResult()
        {
            // Arrange
            var expectedResult = 1;

            // Mock setup
            _adminServicesMock.Setup(x => x.GetRemainingApprovalCount())
                .Returns(Task.FromResult(expectedResult)).Verifiable();

            // Act
            var result = await _adminController.GetRemainingApprovalCount();

            // Assert
            _adminServicesMock.Verify(x => x.GetRemainingApprovalCount(), Times.Once);
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(expectedResult));
        }

        [Test]
        public async Task GetRemainingApprovalCount_NoParameter_ReturnsBadRequestObjectResult()
        {
            // Arrange
            var expectedResult = 1;
            var errorMessage = "Some error occured";

            // Mock setup
            _adminServicesMock.Setup(x => x.GetRemainingApprovalCount())
                .Throws(new Exception(errorMessage)).Verifiable();

            // Act
            var result = await _adminController.GetRemainingApprovalCount();

            // Assert
            _adminServicesMock.Verify(x => x.GetRemainingApprovalCount(), Times.Once);
            _loggerMock.Verify(x => x.Error(It.Is<string>(msg => msg == errorMessage)), Times.Once);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult.Value, Is.EqualTo(errorMessage));
        }

        [Test]
        public async Task GetVendorApprovalSummaryHistory_ValidString_ReturnsOkObjectResult()
        {
            // Arrange
            var serverParams = "{\"subsection\":\"Bank Details\",\"sortField\":\"vendor_approval_date\",\"sortType\":\"desc\",\"page\":1,\"perPage\":10,\"searchText\":\"\"}";
            var parameters = JsonConvert.DeserializeObject<HistoryServerParamsDto>(serverParams);
            var expectedResult = new object();

            // Mock setup
            _adminServicesMock.Setup(x => x.GetVendorApprovalSummaryHistory(It.IsAny<HistoryServerParamsDto>()))
                .Returns(Task.FromResult(expectedResult)).Verifiable();

            // Act
            var result = await _adminController.GetVendorApprovalSummaryHistory(serverParams);

            // Assert
            _adminServicesMock.Verify(x => x.GetVendorApprovalSummaryHistory(It.IsAny<HistoryServerParamsDto>()), Times.Once);
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(expectedResult));
        }

        [Test]
        public async Task GetVendorApprovalSummaryHistory_ValidString_ReturnsBadRequestObjectResult()
        {
            // Arrange
            var serverParams = "{\"subsection\":\"Bank Details\",\"sortField\":\"vendor_approval_date\",\"sortType\":\"desc\",\"page\":1,\"perPage\":10,\"searchText\":\"\"}";
            var parameters = JsonConvert.DeserializeObject<HistoryServerParamsDto>(serverParams);
            var errorMessage = "Some error occurred";

            // Mock setup
            _adminServicesMock.Setup(x => x.GetVendorApprovalSummaryHistory(It.IsAny<HistoryServerParamsDto>()))
                .Throws(new Exception(errorMessage)).Verifiable();

            // Act
            var result = await _adminController.GetVendorApprovalSummaryHistory(serverParams);

            // Assert
            _adminServicesMock.Verify(x => x.GetVendorApprovalSummaryHistory(It.IsAny<HistoryServerParamsDto>()), Times.Once);
            _loggerMock.Verify(x => x.Error(It.Is<string>(msg => msg == errorMessage)), Times.Once);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult.Value, Is.EqualTo(errorMessage));
        }
    }
}