using HBS.ImplementationAutomationConsole.API.Controllers;
using HBS.ImplementationAutomationConsole.Core.BLL.IServices;
using HBS.ImplementationAutomationConsole.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using ILogger = Serilog.ILogger;

namespace HBS.PHRConfigAssistTest
{
    public class SectionEnableDisableControllerTests
    {
        private Mock<ISignOffService> _isignOffServicesMock;
        private Mock<ISectionEnableDisableServices> _isectionEnableDisableServicesMock;
        private Mock<ILogger> _loggerMock;
        private SectionEnableDisableController _sectionEnableDisableController;

        [SetUp]
        public void Setup()
        {
            _isignOffServicesMock = new Mock<ISignOffService>();
            _isectionEnableDisableServicesMock = new Mock<ISectionEnableDisableServices>();
            _loggerMock = new Mock<ILogger>();
            _sectionEnableDisableController = new SectionEnableDisableController(_loggerMock.Object, _isignOffServicesMock.Object, _isectionEnableDisableServicesMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _isignOffServicesMock.Reset();
            _isectionEnableDisableServicesMock.Reset();
            _loggerMock.Reset();
        }

        [Test]
        public async Task UpdateOrdersForModule_ModuleEnableDisableDto_ReturnsOkObjectResult()
        {
            //Arrange
            var models = new ModuleEnableDisableDto
            {
                Modules = new List<ModuleModel>
                {
                    new ModuleModel {ModuleId=1, ModuleName="DemoModule", Order=1 }
                }
            };

            //Mock setup
            _isectionEnableDisableServicesMock.Setup(x => x.UpdateOrdersForModule(models.Modules)).Verifiable();

            //Act
            var result = await _sectionEnableDisableController.UpdateOrdersForModule(models);

            //Assert
            Assert.IsInstanceOf<OkResult>(result);

            //Verify
            _isectionEnableDisableServicesMock.Verify(x => x.UpdateOrdersForModule(models.Modules), Times.Once);
        }


        [Test]
        public async Task UpdateOrdersForModule_ModuleEnableDisableDto_ReturnsBadRequestObjectResult()
        {
            //Arrange
            var models = new ModuleEnableDisableDto
            {
                Modules = new List<ModuleModel>
                {
                    new ModuleModel {ModuleId=1, ModuleName="DemoModule", Order=1 }
                }
            };
            var errorMessage = "Some Error Occured";

            //Mock setup
            _isectionEnableDisableServicesMock.Setup(x => x.UpdateOrdersForModule(models.Modules))
                .Throws(new Exception(errorMessage)).Verifiable();

            //Act
            var result = await _sectionEnableDisableController.UpdateOrdersForModule(models);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestObject = result as BadRequestObjectResult;
            Assert.That(badRequestObject.Value, Is.EqualTo(errorMessage));

            // Verify
            _isectionEnableDisableServicesMock.Verify(x => x.UpdateOrdersForModule(models.Modules), Times.Once);
            _loggerMock.Verify(x => x.Error(It.Is<string>(msg => msg == errorMessage)), Times.Once);
        }

        [Test]
        public async Task GetConfigControlApprovalStatus_EmptyParameters_ReturnsOkObjectResult()
        {
            //Arrange
            var expectedResponse = new object();

            //Mock setup
            _isectionEnableDisableServicesMock.Setup(x => x.GetConfigControlApprovalStatus()).ReturnsAsync(expectedResponse).Verifiable();

            //Act
            var result = await _sectionEnableDisableController.GetConfigControlApprovalStatus();

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okObjectResult = result as OkObjectResult;
            Assert.That(okObjectResult.Value, Is.EqualTo(expectedResponse));

            //Verify
            _isectionEnableDisableServicesMock.Verify(x => x.GetConfigControlApprovalStatus(), Times.Once);
        }


        [Test]
        public async Task GetConfigControlApprovalStatus_EmptyParameters_ReturnsBadRequestObjectResult()
        {
            //Arrange
            var expectedResponse = new object();
            var errorMessage = "Some Error Occured";

            //Mock setup
            _isectionEnableDisableServicesMock.Setup(x => x.GetConfigControlApprovalStatus()).Throws(new Exception(errorMessage)).Verifiable();

            //Act
            var result = await _sectionEnableDisableController.GetConfigControlApprovalStatus();

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestObject = result as BadRequestObjectResult;
            Assert.That(badRequestObject.Value, Is.EqualTo(errorMessage));

            // Verify
            _isectionEnableDisableServicesMock.Verify(x => x.GetConfigControlApprovalStatus(), Times.Once);
            _loggerMock.Verify(x => x.Error(It.Is<string>(msg => msg == errorMessage)), Times.Once);
        }

        [Test]
        public async Task GetAdvanceConfigActiveStatus_EmptyParameters_ReturnsOkObjectResult()
        {
            //Arrange
            var expectedResponse = true;

            //Mock setup
            _isectionEnableDisableServicesMock.Setup(x => x.GetAdvanceConfigActiveStatus()).ReturnsAsync(expectedResponse).Verifiable();

            //Act
            var result = await _sectionEnableDisableController.GetAdvanceConfigActiveStatus();

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okObjectResult = result as OkObjectResult;
            Assert.That(okObjectResult.Value, Is.EqualTo(expectedResponse));

            //Verify
            _isectionEnableDisableServicesMock.Verify(x => x.GetAdvanceConfigActiveStatus(), Times.Once);
        }


        [Test]
        public async Task GetAdvanceConfigActiveStatus_EmptyParameters_ReturnsBadRequestObjectResult()
        {
            //Arrange
            var expectedResponse = new object();
            var errorMessage = "Some Error Occured";

            //Mock setup
            _isectionEnableDisableServicesMock.Setup(x => x.GetAdvanceConfigActiveStatus()).Throws(new Exception(errorMessage)).Verifiable();

            //Act
            var result = await _sectionEnableDisableController.GetAdvanceConfigActiveStatus();

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestObject = result as BadRequestObjectResult;
            Assert.That(badRequestObject.Value, Is.EqualTo(errorMessage));

            // Verify
            _isectionEnableDisableServicesMock.Verify(x => x.GetAdvanceConfigActiveStatus(), Times.Once);
            _loggerMock.Verify(x => x.Error(It.Is<string>(msg => msg == errorMessage)), Times.Once);
        }

        [Test]
        public async Task GetAdvanceConfigActiveStatusForSidebar_EmptyParameters_ReturnsOkObjectResult()
        {
            //Arrange
            var expectedResponse = true;

            //Mock setup
            _isectionEnableDisableServicesMock.Setup(x => x.GetAdvanceConfigActiveStatusForSidebar()).ReturnsAsync(expectedResponse).Verifiable();

            //Act
            var result = await _sectionEnableDisableController.GetAdvanceConfigActiveStatusForSidebar();

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okObjectResult = result as OkObjectResult;
            Assert.That(okObjectResult.Value, Is.EqualTo(expectedResponse));

            //Verify
            _isectionEnableDisableServicesMock.Verify(x => x.GetAdvanceConfigActiveStatusForSidebar(), Times.Once);
        }


        [Test]
        public async Task GetAdvanceConfigActiveStatusForSidebar_EmptyParameters_ReturnsBadRequestObjectResult()
        {
            //Arrange
            var expectedResponse = new object();
            var errorMessage = "Some Error Occured";

            //Mock setup
            _isectionEnableDisableServicesMock.Setup(x => x.GetAdvanceConfigActiveStatusForSidebar()).Throws(new Exception(errorMessage)).Verifiable();

            //Act
            var result = await _sectionEnableDisableController.GetAdvanceConfigActiveStatusForSidebar();

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestObject = result as BadRequestObjectResult;
            Assert.That(badRequestObject.Value, Is.EqualTo(errorMessage));

            // Verify
            _isectionEnableDisableServicesMock.Verify(x => x.GetAdvanceConfigActiveStatusForSidebar(), Times.Once);
            _loggerMock.Verify(x => x.Error(It.Is<string>(msg => msg == errorMessage)), Times.Once);
        }

        [Test]
        public async Task UpdateOrdersForSubsection_SubsectionEnableDisableDto_ReturnsOkObjectResult()
        {
            //Arrange
            SubsectionEnableDisableDto models = new SubsectionEnableDisableDto
            {
                EimObjects = new List<SubsectionModel>
                {
                    new SubsectionModel {SubsectionId="1", SubsectionName="DemoSubsection1", Order=1 }
                },
                AbsenceObjects = new List<SubsectionModel>
                {
                    new SubsectionModel {SubsectionId="2", SubsectionName="DemoSubsection2", Order=2 }
                },
                AttendanceObjects = new List<SubsectionModel>
                {
                    new SubsectionModel {SubsectionId="3", SubsectionName="DemoSubsection3", Order=3 }
                }
            };

            //Mock setup
            _isectionEnableDisableServicesMock.Setup(x => x.UpdateOrdersForEIM(models.EimObjects)).Verifiable();
            _isectionEnableDisableServicesMock.Setup(x => x.UpdateOrdersForAbsence(models.AbsenceObjects)).Verifiable();
            _isectionEnableDisableServicesMock.Setup(x => x.UpdateOrdersForAttendance(models.AttendanceObjects)).Verifiable();

            //Act
            var result = await _sectionEnableDisableController.UpdateOrdersForSubsection(models);

            //Assert
            Assert.IsInstanceOf<OkResult>(result);

            //Verify
            _isectionEnableDisableServicesMock.Verify(x => x.UpdateOrdersForEIM(models.EimObjects), Times.Once);
            _isectionEnableDisableServicesMock.Verify(x => x.UpdateOrdersForAbsence(models.AbsenceObjects), Times.Once);
            _isectionEnableDisableServicesMock.Verify(x => x.UpdateOrdersForAttendance(models.AttendanceObjects), Times.Once);
        }


        [Test]
        public async Task UpdateOrdersForSubsection_SubsectionEnableDisableDto_ReturnsBadRequestObjectResult()
        {
            //Arrange
            SubsectionEnableDisableDto models = new SubsectionEnableDisableDto
            {
                EimObjects = new List<SubsectionModel>
                {
                    new SubsectionModel {SubsectionId="1", SubsectionName="DemoSubsection1", Order=1 }
                },
                AbsenceObjects = new List<SubsectionModel>
                {
                    new SubsectionModel {SubsectionId="2", SubsectionName="DemoSubsection2", Order=2 }
                },
                AttendanceObjects = new List<SubsectionModel>
                {
                    new SubsectionModel {SubsectionId="3", SubsectionName="DemoSubsection3", Order=3 }
                }
            };
            var errorMessage = "Some Error Occured";

            //Mock setup
            _isectionEnableDisableServicesMock.Setup(x => x.UpdateOrdersForEIM(models.EimObjects)).Throws(new Exception(errorMessage)).Verifiable();

            //Act
            var result = await _sectionEnableDisableController.UpdateOrdersForSubsection(models);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestObject = result as BadRequestObjectResult;
            Assert.That(badRequestObject.Value, Is.EqualTo(errorMessage));

            // Verify
            _isectionEnableDisableServicesMock.Verify(x => x.UpdateOrdersForEIM(models.EimObjects), Times.Once);
            _loggerMock.Verify(x => x.Error(It.Is<string>(msg => msg == errorMessage)), Times.Once);
        }

        [Test]
        public async Task UploadConfigControlData_ConfigControlPostDto_ReturnsOkObjectResult()
        {
            // Arrange
            var configControlPostDto = new ConfigControlPostDto()
            {
                UserName = "demoUser",
                Name = "demoUserName",
                Comment = "demoComment",
                ApprovalStatus = "Approved",
                ApprovalDate = new DateTime(2021, 1, 1),
                SignOffPdfData = new byte[1] { 0 },
            };

            var htmlString = "htmlString";
            var expectedString = "Data successfully inserted";

            // Mock setup
            _isignOffServicesMock.Setup(x => x.GetSignOffHtmlStringForConfiguration(configControlPostDto.Name, configControlPostDto.ApprovalDate, configControlPostDto.ApprovalStatus, configControlPostDto.Comment, configControlPostDto.UserName, "Configuration Control"))
                .Returns(Task.FromResult(htmlString)).Verifiable();

            _isectionEnableDisableServicesMock.Setup(x => x.InsertConfigControlInfo(configControlPostDto)).Verifiable();


            // Act
            var result = await _sectionEnableDisableController.UploadConfigControlData(configControlPostDto);

            // Assert

            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(expectedString, okResult.Value);

            //Verify
            _isignOffServicesMock.Verify(x => x.GetSignOffHtmlStringForConfiguration(configControlPostDto.Name, configControlPostDto.ApprovalDate, configControlPostDto.ApprovalStatus, configControlPostDto.Comment, configControlPostDto.UserName, "Configuration Control"), Times.Once);
        }

        [Test]
        public async Task UploadConfigControlData_ConfigControlPostDto_ReturnsBadRequestObjectResult()
        {
            // Arrange
            var configControlPostDto = new ConfigControlPostDto()
            {
                UserName = "demoUser",
                Name = "demoUserName",
                Comment = "demoComment",
                ApprovalStatus = "Approved",
                ApprovalDate = new DateTime(2021, 1, 1),
                SignOffPdfData = new byte[1] { 0 },
            };

            var htmlString = "htmlString";
            var errorMessage = "Some error occurred";

            // Mock setup
            _isignOffServicesMock.Setup(x => x.GetSignOffHtmlStringForConfiguration(configControlPostDto.Name, configControlPostDto.ApprovalDate, configControlPostDto.ApprovalStatus, configControlPostDto.Comment, configControlPostDto.UserName, "Configuration Control"))
                .Returns(Task.FromResult(htmlString)).Verifiable();

            _isectionEnableDisableServicesMock.Setup(x => x.InsertConfigControlInfo(configControlPostDto)).Throws(new Exception(errorMessage)).Verifiable();


            // Act
            var result = await _sectionEnableDisableController.UploadConfigControlData(configControlPostDto);


            // Assert
            _isignOffServicesMock.Verify(x => x.GetSignOffHtmlStringForConfiguration(configControlPostDto.Name, configControlPostDto.ApprovalDate, configControlPostDto.ApprovalStatus, configControlPostDto.Comment, configControlPostDto.UserName, "Configuration Control"), Times.Once);
            _isectionEnableDisableServicesMock.Verify(x => x.InsertConfigControlInfo(configControlPostDto), Times.Once);

            _loggerMock.Verify(x => x.Error(It.Is<string>(msg => msg == errorMessage)), Times.Once);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult.Value, Is.EqualTo(errorMessage));
        }

        [Test]
        public async Task GetConfigControlHistory_ValidString_ReturnsOkObjectResult()
        {
            // Arrange
            var serverParams = "{\"sortField\":\"module_name\",\"sortType\":\"asc\",\"page\":1,\"perPage\":10,\"searchText\":\"\"}";
            var parameters = JsonConvert.DeserializeObject<ConfigControlHistoryServerParamsDto>(serverParams);
            var expectedResult = new object();

            // Mock setup
            _isectionEnableDisableServicesMock.Setup(x => x.GetConfigControlHistory(It.IsAny<ConfigControlHistoryServerParamsDto>()))
                .Returns(Task.FromResult(expectedResult)).Verifiable();

            // Act
            var result = await _sectionEnableDisableController.GetConfigControlHistory(serverParams);

            // Assert
            _isectionEnableDisableServicesMock.Verify(x => x.GetConfigControlHistory(It.IsAny<ConfigControlHistoryServerParamsDto>()), Times.Once);
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(expectedResult));
        }

        [Test]
        public async Task GetConfigControlHistory_ValidString_ReturnsBadRequestObjectResult()
        {
            // Arrange
            var serverParams = "{\"sortField\":\"module_name\",\"sortType\":\"asc\",\"page\":1,\"perPage\":10,\"searchText\":\"\"}";
            var parameters = JsonConvert.DeserializeObject<ConfigControlHistoryServerParamsDto>(serverParams);
            var errorMessage = "Some error occurred";

            // Mock setup
            _isectionEnableDisableServicesMock.Setup(x => x.GetConfigControlHistory(It.IsAny<ConfigControlHistoryServerParamsDto>()))
                .Throws(new Exception(errorMessage)).Verifiable();

            // Act
            var result = await _sectionEnableDisableController.GetConfigControlHistory(serverParams);

            // Assert
            _isectionEnableDisableServicesMock.Verify(x => x.GetConfigControlHistory(It.IsAny<ConfigControlHistoryServerParamsDto>()), Times.Once);
            _loggerMock.Verify(x => x.Error(It.Is<string>(msg => msg == errorMessage)), Times.Once);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult.Value, Is.EqualTo(errorMessage));
        }

        [Test]
        public async Task CheckIfAllSubsectionIsEnabled_EmptyParameters_ReturnsOkObjectResult()
        {
            //Arrange
            var expectedResponse = true;

            //Mock setup
            _isectionEnableDisableServicesMock.Setup(x => x.CheckIfAllSubsectionIsEnabled()).ReturnsAsync(expectedResponse).Verifiable();

            //Act
            var result = await _sectionEnableDisableController.CheckIfAllSubsectionIsEnabled();

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okObjectResult = result as OkObjectResult;
            Assert.That(okObjectResult.Value, Is.EqualTo(expectedResponse));

            //Verify
            _isectionEnableDisableServicesMock.Verify(x => x.CheckIfAllSubsectionIsEnabled(), Times.Once);
        }


        [Test]
        public async Task CheckIfAllSubsectionIsEnabled_EmptyParameters_ReturnsBadRequestObjectResult()
        {
            //Arrange
            var errorMessage = "Some Error Occured";

            //Mock setup
            _isectionEnableDisableServicesMock.Setup(x => x.CheckIfAllSubsectionIsEnabled()).Throws(new Exception(errorMessage)).Verifiable();

            //Act
            var result = await _sectionEnableDisableController.CheckIfAllSubsectionIsEnabled();

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestObject = result as BadRequestObjectResult;
            Assert.That(badRequestObject.Value, Is.EqualTo(errorMessage));

            // Verify
            _isectionEnableDisableServicesMock.Verify(x => x.CheckIfAllSubsectionIsEnabled(), Times.Once);
            _loggerMock.Verify(x => x.Error(It.Is<string>(msg => msg == errorMessage)), Times.Once);
        }
    }
}