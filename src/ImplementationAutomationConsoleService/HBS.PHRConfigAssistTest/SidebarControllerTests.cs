using HBS.ImplementationAutomationConsole.API.Controllers;
using HBS.ImplementationAutomationConsole.Core.BLL.IServices;
using HBS.ImplementationAutomationConsole.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using ILogger = Serilog.ILogger;

namespace HBS.PHRConfigAssistTest
{
    public class SidebarControllerTests
    {
        private Mock<ISidebarServices> _isidebarServicesMock;
        private Mock<ILogger> _loggerMock;
        private SidebarController _sidebarController;

        [SetUp]
        public void Setup()
        {
            _isidebarServicesMock = new Mock<ISidebarServices>();
            _loggerMock = new Mock<ILogger>();
            _sidebarController = new SidebarController(_isidebarServicesMock.Object, _loggerMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _isidebarServicesMock.Reset();
            _loggerMock.Reset();
        }

        [Test]
        public async Task GetSidebarComponents_EmptyParameters_ReturnsOkObjectResult()
        {
            // Arrange
            var sidebarDto = new List<SidebarDto>
            {
                new SidebarDto {
                    Subsection_Id = "1",
                    Subsection_Name = "DemoSubsection",
                    Module_Id=1,
                    Approval_Status = "Approved",
                    Vendor_ApprovalStatus="Approved",
                    Enabled = true,
                }
            };

            _isidebarServicesMock.Setup(x => x.GetSubsectionItems()).ReturnsAsync(sidebarDto).Verifiable();

            // Act
            var result = await _sidebarController.GetSidebarComponents();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            var actualResult = okResult.Value as List<SidebarDto>;
            Assert.That(JsonConvert.SerializeObject(actualResult), Is.EqualTo(JsonConvert.SerializeObject(sidebarDto)));

            // Verify
            _isidebarServicesMock.Verify(x => x.GetSubsectionItems(), Times.Once);
        }


        [Test]
        public async Task GetSidebarComponents_EmptyParameters_ReturnsBadRequestObjectResult()
        {
            // Arrange
            var errorMessage = "Some Error Occured";

            _isidebarServicesMock.Setup(x => x.GetSubsectionItems()).Throws(new Exception(errorMessage)).Verifiable();

            // Act
            var result = await _sidebarController.GetSidebarComponents();

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestObject = result as BadRequestObjectResult;
            Assert.That(badRequestObject.Value, Is.EqualTo(errorMessage));

            // Verify
            _isidebarServicesMock.Verify(x => x.GetSubsectionItems(), Times.Once);
            _loggerMock.Verify(x => x.Error(It.Is<string>(msg => msg == errorMessage)), Times.Once);
        }

        [Test]
        public async Task GetActiveModules_EmptyParameters_ReturnsOkObjectResult()
        {
            // Arrange
            var moduleList = new List<ModuleDto>
            {
                new ModuleDto
                {
                    moduleId = 1,
                    moduleName = "DemoModule",
                    order=1,
                    is_enable=true,
                }
            };

            _isidebarServicesMock.Setup(x => x.GetActiveModules()).ReturnsAsync(moduleList).Verifiable();

            // Act
            var result = await _sidebarController.GetActiveModules();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            var actualResult = okResult.Value as List<ModuleDto>;
            Assert.That(JsonConvert.SerializeObject(actualResult), Is.EqualTo(JsonConvert.SerializeObject(moduleList)));

            // Verify
            _isidebarServicesMock.Verify(x => x.GetActiveModules(), Times.Once);
        }

        [Test]
        public async Task GetActiveModules_EmptyParameters_ReturnsBadRequestObjectResult()
        {
            // Arrange
            var errorMessage = "Some Error Occured";

            _isidebarServicesMock.Setup(x => x.GetActiveModules()).Throws(new Exception(errorMessage)).Verifiable();

            // Act
            var result = await _sidebarController.GetActiveModules();

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestObject = result as BadRequestObjectResult;
            Assert.That(badRequestObject.Value, Is.EqualTo(errorMessage));

            // Verify
            _isidebarServicesMock.Verify(x => x.GetActiveModules(), Times.Once);
            _loggerMock.Verify(x => x.Error(It.Is<string>(msg => msg == errorMessage)), Times.Once);
        }

        [Test]
        public async Task GetActiveModulesFromDashboard_EmptyParameters_ReturnsOkObjectResult()
        {
            // Arrange
            var moduleList = new List<ModuleDto>
            {
                new ModuleDto
                {
                    moduleId = 1,
                    moduleName = "DemoModule",
                    order=1,
                    is_enable=true,
                }
            };

            _isidebarServicesMock.Setup(x => x.GetActiveModulesFromDashboard()).ReturnsAsync(moduleList).Verifiable();

            // Act
            var result = await _sidebarController.GetActiveModulesFromDashboard();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            var actualResult = okResult.Value as List<ModuleDto>;
            Assert.That(JsonConvert.SerializeObject(actualResult), Is.EqualTo(JsonConvert.SerializeObject(moduleList)));

            // Verify
            _isidebarServicesMock.Verify(x => x.GetActiveModulesFromDashboard(), Times.Once);
        }

        [Test]
        public async Task GetActiveModulesFromDashboard_EmptyParameters_ReturnsBadRequestObjectResult()
        {
            // Arrange
            var errorMessage = "Some Error Occured";

            _isidebarServicesMock.Setup(x => x.GetActiveModulesFromDashboard()).Throws(new Exception(errorMessage)).Verifiable();

            // Act
            var result = await _sidebarController.GetActiveModulesFromDashboard();

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestObject = result as BadRequestObjectResult;
            Assert.That(badRequestObject.Value, Is.EqualTo(errorMessage));

            // Verify
            _isidebarServicesMock.Verify(x => x.GetActiveModulesFromDashboard(), Times.Once);
            _loggerMock.Verify(x => x.Error(It.Is<string>(msg => msg == errorMessage)), Times.Once);
        }

        [Test]
        public async Task GetActiveModuleIds_EmptyParameters_ReturnsOkObjectResult()
        {
            // Arrange
            var moduleList = new List<int> { 1, 2, 3 };

            _isidebarServicesMock.Setup(x => x.GetActiveModuleIDs()).ReturnsAsync(moduleList).Verifiable();

            // Act
            var result = await _sidebarController.GetActiveModuleIds();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            var actualResult = okResult.Value as List<int>;
            Assert.That(JsonConvert.SerializeObject(actualResult), Is.EqualTo(JsonConvert.SerializeObject(moduleList)));

            // Verify
            _isidebarServicesMock.Verify(x => x.GetActiveModuleIDs(), Times.Once);
        }

        [Test]
        public async Task GetActiveModuleIds_EmptyParameters_ReturnsBadRequestObjectResult()
        {
            // Arrange
            var errorMessage = "Some Error Occured";

            _isidebarServicesMock.Setup(x => x.GetActiveModuleIDs()).Throws(new Exception(errorMessage)).Verifiable();

            // Act
            var result = await _sidebarController.GetActiveModuleIds();

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestObject = result as BadRequestObjectResult;
            Assert.That(badRequestObject.Value, Is.EqualTo(errorMessage));

            // Verify
            _isidebarServicesMock.Verify(x => x.GetActiveModuleIDs(), Times.Once);
            _loggerMock.Verify(x => x.Error(It.Is<string>(msg => msg == errorMessage)), Times.Once);
        }

        [Test]
        public async Task GetActiveSubsectionsEim_EmptyParameters_ReturnsOkObjectResult()
        {
            // Arrange
            var subsectionList = new List<ActiveSubsectionDto>
            {
                new ActiveSubsectionDto
                {
                    SubsectionName="DemoSubsection",
                    SubsectionId="1",
                    Order=1
                }
            };

            _isidebarServicesMock.Setup(x => x.GetActiveSubsectionsEim()).ReturnsAsync(subsectionList).Verifiable();

            // Act
            var result = await _sidebarController.GetActiveSubsectionsEim();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            var actualResult = okResult.Value as List<ActiveSubsectionDto>;
            Assert.That(JsonConvert.SerializeObject(actualResult), Is.EqualTo(JsonConvert.SerializeObject(subsectionList)));

            // Verify
            _isidebarServicesMock.Verify(x => x.GetActiveSubsectionsEim(), Times.Once);
        }

        [Test]
        public async Task GetActiveSubsectionsEim_EmptyParameters_ReturnsBadRequestObjectResult()
        {
            // Arrange
            var errorMessage = "Some Error Occured";

            _isidebarServicesMock.Setup(x => x.GetActiveSubsectionsEim()).Throws(new Exception(errorMessage)).Verifiable();

            // Act
            var result = await _sidebarController.GetActiveSubsectionsEim();

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestObject = result as BadRequestObjectResult;
            Assert.That(badRequestObject.Value, Is.EqualTo(errorMessage));

            // Verify
            _isidebarServicesMock.Verify(x => x.GetActiveSubsectionsEim(), Times.Once);
            _loggerMock.Verify(x => x.Error(It.Is<string>(msg => msg == errorMessage)), Times.Once);
        }

        [Test]
        public async Task GetActiveSubsectionsLeave_EmptyParameters_ReturnsOkObjectResult()
        {
            // Arrange
            var subsectionList = new List<ActiveSubsectionDto>
            {
                new ActiveSubsectionDto
                {
                    SubsectionName="DemoSubsection",
                    SubsectionId="1",
                    Order=1
                }
            };

            _isidebarServicesMock.Setup(x => x.GetActiveSubsectionsLeave()).ReturnsAsync(subsectionList).Verifiable();

            // Act
            var result = await _sidebarController.GetActiveSubsectionsLeave();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            var actualResult = okResult.Value as List<ActiveSubsectionDto>;
            Assert.That(JsonConvert.SerializeObject(actualResult), Is.EqualTo(JsonConvert.SerializeObject(subsectionList)));

            // Verify
            _isidebarServicesMock.Verify(x => x.GetActiveSubsectionsLeave(), Times.Once);
        }

        [Test]
        public async Task GetActiveSubsectionsLeave_EmptyParameters_ReturnsBadRequestObjectResult()
        {
            // Arrange
            var errorMessage = "Some Error Occured";

            _isidebarServicesMock.Setup(x => x.GetActiveSubsectionsLeave()).Throws(new Exception(errorMessage)).Verifiable();

            // Act
            var result = await _sidebarController.GetActiveSubsectionsLeave();

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestObject = result as BadRequestObjectResult;
            Assert.That(badRequestObject.Value, Is.EqualTo(errorMessage));

            // Verify
            _isidebarServicesMock.Verify(x => x.GetActiveSubsectionsLeave(), Times.Once);
            _loggerMock.Verify(x => x.Error(It.Is<string>(msg => msg == errorMessage)), Times.Once);
        }

        [Test]
        public async Task GetActiveSubsectionsAttendance_EmptyParameters_ReturnsOkObjectResult()
        {
            // Arrange
            var subsectionList = new List<ActiveSubsectionDto>
            {
                new ActiveSubsectionDto
                {
                    SubsectionName="DemoSubsection",
                    SubsectionId="1",
                    Order=1
                }
            };

            _isidebarServicesMock.Setup(x => x.GetActiveSubsectionsAttendance()).ReturnsAsync(subsectionList).Verifiable();

            // Act
            var result = await _sidebarController.GetActiveSubsectionsAttendance();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            var actualResult = okResult.Value as List<ActiveSubsectionDto>;
            Assert.That(JsonConvert.SerializeObject(actualResult), Is.EqualTo(JsonConvert.SerializeObject(subsectionList)));

            // Verify
            _isidebarServicesMock.Verify(x => x.GetActiveSubsectionsAttendance(), Times.Once);
        }

        [Test]
        public async Task GetActiveSubsectionsAttendance_EmptyParameters_ReturnsBadRequestObjectResult()
        {
            // Arrange
            var errorMessage = "Some Error Occured";

            _isidebarServicesMock.Setup(x => x.GetActiveSubsectionsAttendance()).Throws(new Exception(errorMessage)).Verifiable();

            // Act
            var result = await _sidebarController.GetActiveSubsectionsAttendance();

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestObject = result as BadRequestObjectResult;
            Assert.That(badRequestObject.Value, Is.EqualTo(errorMessage));

            // Verify
            _isidebarServicesMock.Verify(x => x.GetActiveSubsectionsAttendance(), Times.Once);
            _loggerMock.Verify(x => x.Error(It.Is<string>(msg => msg == errorMessage)), Times.Once);
        }


        [Test]
        public async Task GetCopyRightText_EmptyParameters_ReturnsOkObjectResult()
        {
            // Arrange
            var copyRightText = "Demo Copy Right Text";

            _isidebarServicesMock.Setup(x => x.GetCopyRightText()).ReturnsAsync(copyRightText).Verifiable();

            // Act
            var result = await _sidebarController.GetCopyRightText();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(copyRightText));

            // Verify
            _isidebarServicesMock.Verify(x => x.GetCopyRightText(), Times.Once);
        }

        [Test]
        public async Task GetCopyRightText_EmptyParameters_ReturnsBadRequestObjectResult()
        {
            // Arrange
            var errorMessage = "Some Error Occured";

            _isidebarServicesMock.Setup(x => x.GetCopyRightText()).Throws(new Exception(errorMessage)).Verifiable();

            // Act
            var result = await _sidebarController.GetCopyRightText();

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestObject = result as BadRequestObjectResult;
            Assert.That(badRequestObject.Value, Is.EqualTo(errorMessage));

            // Verify
            _isidebarServicesMock.Verify(x => x.GetCopyRightText(), Times.Once);
            _loggerMock.Verify(x => x.Error(It.Is<string>(msg => msg == errorMessage)), Times.Once);
        }

        [Test]
        public async Task CheckIfSubsectionApproved_EmptyParameters_ReturnsOkObjectResult()
        {
            // Arrange
            var subsectionName = "DemoSubsection";
            var isApproved = true;

            _isidebarServicesMock.Setup(x => x.CheckIfSubsectionApproved(subsectionName)).ReturnsAsync(isApproved).Verifiable();

            // Act
            var result = await _sidebarController.CheckIfSubsectionApproved(subsectionName);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(isApproved));

            // Verify
            _isidebarServicesMock.Verify(x => x.CheckIfSubsectionApproved(subsectionName), Times.Once);
        }

        [Test]
        public async Task CheckIfSubsectionApproved_EmptyParameters_ReturnsBadRequestObjectResult()
        {
            // Arrange
            var errorMessage = "Some Error Occured";
            var subsectionName = "DemoSubsection";

            _isidebarServicesMock.Setup(x => x.CheckIfSubsectionApproved(subsectionName)).Throws(new Exception(errorMessage)).Verifiable();

            // Act
            var result = await _sidebarController.CheckIfSubsectionApproved(subsectionName);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestObject = result as BadRequestObjectResult;
            Assert.That(badRequestObject.Value, Is.EqualTo(errorMessage));

            // Verify
            _isidebarServicesMock.Verify(x => x.CheckIfSubsectionApproved(subsectionName), Times.Once);
            _loggerMock.Verify(x => x.Error(It.Is<string>(msg => msg == errorMessage)), Times.Once);
        }
    }
}