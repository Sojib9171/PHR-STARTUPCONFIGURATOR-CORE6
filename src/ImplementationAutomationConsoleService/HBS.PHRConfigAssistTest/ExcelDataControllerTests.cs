using HBS.ImplementationAutomationConsole.API.Controllers;
using HBS.ImplementationAutomationConsole.Core.BLL.IServices;
using HBS.ImplementationAutomationConsole.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ILogger = Serilog.ILogger;

namespace HBS.PHRConfigAssistTest
{
    public class ExcelDataControllerTests
    {
        private Mock<IExcelDataServices> _iexcelDataServicesMock;
        private Mock<ILogger> _loggerMock;
        private ExcelDataController _excelDataController;

        [SetUp]
        public void Setup()
        {
            _iexcelDataServicesMock = new Mock<IExcelDataServices>();
            _loggerMock = new Mock<ILogger>();
            _excelDataController = new ExcelDataController(_iexcelDataServicesMock.Object, _loggerMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _iexcelDataServicesMock.Reset();
            _loggerMock.Reset();
        }

        [Test]
        public async Task UploadEmployeeFile_ValidIFormFileAndValidString_ReturnsOkObjectResult()
        {
            // Arrange
            var formFileMock = new Mock<IFormFile>();
            formFileMock.Setup(x => x.Length).Returns(100); // Set the length to a non-zero value
            formFileMock.Setup(x => x.FileName).Returns("example.txt");
            string subsectionName = "demoSubsectionName";
            var expectedString = "Data inserted";


            // Mock setup
            _iexcelDataServicesMock.Setup(x => x.UploadData(It.IsAny<MemoryStream>(), It.IsAny<string>())).Verifiable();

            // Act
            var result = await _excelDataController.UploadEmployeeFile(formFileMock.Object, subsectionName);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(expectedString));

            //Verify
            _iexcelDataServicesMock.Verify(x => x.UploadData(It.IsAny<MemoryStream>(), subsectionName), Times.Once);
        }

        [Test]
        public async Task UploadEmployeeFile_ValidIFormFileAndValidString_ReturnsBadRequestObjectResult()
        {
            // Arrange
            var formFileMock = new Mock<IFormFile>();
            formFileMock.Setup(x => x.Length).Returns(100); // Set the length to a non-zero value
            formFileMock.Setup(x => x.FileName).Returns("example.txt");
            string subsectionName = "demoSubsectionName";
            var errorMessage = "Some Error Occured";


            // Mock setup
            _iexcelDataServicesMock.Setup(x => x.UploadData(It.IsAny<MemoryStream>(), It.IsAny<string>()))
                .Throws(new Exception(errorMessage)).Verifiable();

            // Act
            var result = await _excelDataController.UploadEmployeeFile(formFileMock.Object, subsectionName);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult.Value, Is.EqualTo(errorMessage));

            //Verify
            _iexcelDataServicesMock.Verify(x => x.UploadData(It.IsAny<MemoryStream>(), subsectionName), Times.Once);
            _loggerMock.Verify(x => x.Error(It.Is<string>(msg => msg == errorMessage)), Times.Once);
        }

        [Test]
        public async Task DeleteallFromTable_ValidDeleteBySubsectionDto_ReturnsOkObjectResult()
        {
            // Arrange
            var model = new DeleteBySubsectionDto
            {
                subsectionName = "demoSubsectionName"
            };


            // Mock setup
            _iexcelDataServicesMock.Setup(x => x.DeleteAllFromTable(It.IsAny<string>())).Verifiable();

            // Act
            var result = await _excelDataController.DeleteallFromTable(model);

            // Assert
            Assert.IsInstanceOf<OkResult>(result);

            //Verify
            _iexcelDataServicesMock.Verify(x => x.DeleteAllFromTable(model.subsectionName), Times.Once);
        }

        [Test]
        public async Task DeleteallFromTable_ValidDeleteBySubsectionDto_ReturnsBadRequestObjectResult()
        {
            // Arrange
            var model = new DeleteBySubsectionDto
            {
                subsectionName = "demoSubsectionName"
            };

            var errorMessage = "Some Error Occured";

            // Mock setup
            _iexcelDataServicesMock.Setup(x => x.DeleteAllFromTable(It.IsAny<string>())).Throws(new Exception(errorMessage)).Verifiable();

            // Act
            var result = await _excelDataController.DeleteallFromTable(model);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult.Value, Is.EqualTo(errorMessage));

            //Verify
            _iexcelDataServicesMock.Verify(x => x.DeleteAllFromTable(model.subsectionName), Times.Once);
            _loggerMock.Verify(x => x.Error(It.Is<string>(msg => msg == errorMessage)), Times.Once);
        }

        [Test]
        public async Task DownloadExcelWithData_ValidString_ReturnsOkObjectResult()
        {
            // Arrange

            var subsectionName = "demoSubsectionName";
            var expectedOutput = new List<string> { "data1", "data2" };


            // Mock setup
            _iexcelDataServicesMock.Setup(x => x.DownloadExcel(It.IsAny<string>())).ReturnsAsync(expectedOutput).Verifiable();

            // Act
            var result = await _excelDataController.DownloadExcelWithData(subsectionName);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(expectedOutput));

            //Verify
            _iexcelDataServicesMock.Verify(x => x.DownloadExcel(subsectionName), Times.Once);
        }

        [Test]
        public async Task DownloadExcelWithData_ValidString_ReturnsBadRequestObjectResult()
        {
            // Arrange

            var subsectionName = "demoSubsectionName";
            var errorMessage = "Some Error Occured";


            // Mock setup
            _iexcelDataServicesMock.Setup(x => x.DownloadExcel(It.IsAny<string>())).Throws(new Exception(errorMessage)).Verifiable();

            // Act
            var result = await _excelDataController.DownloadExcelWithData(subsectionName);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult.Value, Is.EqualTo(errorMessage));

            //Verify
            _iexcelDataServicesMock.Verify(x => x.DownloadExcel(subsectionName), Times.Once);
            _loggerMock.Verify(x => x.Error(It.Is<string>(msg => msg == errorMessage)), Times.Once);
        }

        [Test]
        public async Task UpdateDependentColumnsBySubsection_ValidString_ReturnsOkObjectResult()
        {
            // Arrange
            var subsectionName = "Short Leave";

            // Mock setup
            _iexcelDataServicesMock.Setup(x => x.UpdateDependentColumns(It.IsAny<string>())).Verifiable();

            // Act
            var result = await _excelDataController.UpdateDependentColumnsBySubsection(subsectionName);

            // Assert
            Assert.IsInstanceOf<OkResult>(result);

            //Verify
            _iexcelDataServicesMock.Verify(x => x.UpdateDependentColumns(subsectionName), Times.Once);
        }

        [Test]
        public async Task UpdateDependentColumnsBySubsection_ValidString_ReturnsBadRequestObjectResult()
        {
            // Arrange
            var subsectionName = "Short Leave";
            var errorMessage = "Some Error Occured";

            // Mock setup
            _iexcelDataServicesMock.Setup(x => x.UpdateDependentColumns(It.IsAny<string>())).Throws(new Exception(errorMessage)).Verifiable();

            // Act
            var result = await _excelDataController.UpdateDependentColumnsBySubsection(subsectionName);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult.Value, Is.EqualTo(errorMessage));

            //Verify
            _iexcelDataServicesMock.Verify(x => x.UpdateDependentColumns(subsectionName), Times.Once);
            _loggerMock.Verify(x => x.Error(It.Is<string>(msg => msg == errorMessage)), Times.Once);
        }
    }
}
