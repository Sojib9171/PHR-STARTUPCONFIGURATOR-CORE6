using HBS.ImplementationAutomationConsole.Core.BLL.IServices;
using Moq;
using ILogger = Serilog.ILogger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HBS.ImplementationAutomationConsole.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using HBS.ImplementationAutomationConsole.Core.Models;

namespace HBS.PHRConfigAssistTest
{
    public class HomeControllerTests
    {
        private Mock<ISidebarServices> _sidebarServicesMock;
        private Mock<IHomeServices> _homeServicesMock;
        private Mock<ILogger> _loggerMock;
        private HomeController _homeController;

        [SetUp]
        public void Setup()
        {
            _sidebarServicesMock = new Mock<ISidebarServices>();
            _homeServicesMock = new Mock<IHomeServices>();
            _loggerMock = new Mock<ILogger>();
            _homeController = new HomeController(_homeServicesMock.Object, _sidebarServicesMock.Object, _loggerMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _sidebarServicesMock.Reset();
            _homeServicesMock.Reset();
            _loggerMock.Reset();
        }

        [Test]
        public async Task GetApprovalPercentage_EmptyParameter_ReturnsOkObjectResult()
        {
            // Arrange
            var eimPercentage = 10;
            var absencePercentage = 20;
            var attendancePercentage = 30;
            var expectedList = new List<double>() { 10, 20, 30 };

            _homeServicesMock.Setup(x => x.GetEimApprovalPercentage()).ReturnsAsync(eimPercentage).Verifiable();
            _homeServicesMock.Setup(x => x.GetAbsenceApprovalPercentage()).ReturnsAsync(absencePercentage).Verifiable();
            _homeServicesMock.Setup(x => x.GetAttendanceApprovalPercentage()).ReturnsAsync(attendancePercentage).Verifiable();

            // Act
            var result = await _homeController.GetApprovalPercentage();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(expectedList));

            // Verify
            _homeServicesMock.Verify(x => x.GetEimApprovalPercentage(), Times.Once);
            _homeServicesMock.Verify(x => x.GetAbsenceApprovalPercentage(), Times.Once);
            _homeServicesMock.Verify(x => x.GetAttendanceApprovalPercentage(), Times.Once);
        }


        [Test]
        public async Task GetApprovalPercentage_EmptyParameter_ReturnsBadRequestObjectResult()
        {
            // Arrange
            var errorMessage= "Some Error Occured";

            // Setup
            _homeServicesMock.Setup(x => x.GetEimApprovalPercentage()).Throws(new Exception(errorMessage)).Verifiable();

            // Act
            var result = await _homeController.GetApprovalPercentage();

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestObject = result as BadRequestObjectResult;
            Assert.That(badRequestObject.Value, Is.EqualTo(errorMessage));

            // Verify
            _homeServicesMock.Verify(x => x.GetEimApprovalPercentage(), Times.Once);
            _loggerMock.Verify(x => x.Error(It.Is<string>(msg => msg == errorMessage)), Times.Once);
        }

        [Test]
        public async Task CheckAllSubsectionApproved_EmptyParameter_ReturnsOkObjectResult()
        {
            // Arrange
            var subsectList = new List<SidebarDto>
            {
                new SidebarDto { Subsection_Id = "1",Subsection_Name="demoSubsection", Approval_Status="Approved", Enabled=true, Module_Id=1, Vendor_ApprovalStatus="Approved"},
            };
            var isAllSubsecApproved = true;

            // Setup
            _sidebarServicesMock.Setup(x => x.GetSubsectionItems()).ReturnsAsync(subsectList).Verifiable();
            _homeServicesMock.Setup(x => x.CheckAllSubsectionApproved(subsectList)).ReturnsAsync(isAllSubsecApproved).Verifiable();

            // Act
            var result = await _homeController.CheckAllSubsectionApproved();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(isAllSubsecApproved));

            // Verify
            _sidebarServicesMock.Verify(x => x.GetSubsectionItems(), Times.Once);
            _homeServicesMock.Verify(x => x.CheckAllSubsectionApproved(subsectList), Times.Once);
                
        }


        [Test]
        public async Task CheckAllSubsectionApproved_EmptyParameter_ReturnsBadRequestObjectResult()
        {
            // Arrange
            var subsectList = new List<SidebarDto>
            {
                new SidebarDto { Subsection_Id = "1",Subsection_Name="demoSubsection", Approval_Status="Approved", Enabled=true, Module_Id=1, Vendor_ApprovalStatus="Approved"},
            };
            var errorMessage = "Some Error Occured";

            // Setup
            _sidebarServicesMock.Setup(x => x.GetSubsectionItems()).ReturnsAsync(subsectList).Verifiable();
            _homeServicesMock.Setup(x => x.CheckAllSubsectionApproved(subsectList)).Throws(new Exception(errorMessage)).Verifiable();

            // Act
            var result = await _homeController.CheckAllSubsectionApproved();

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestObject = result as BadRequestObjectResult;
            Assert.That(badRequestObject.Value, Is.EqualTo(errorMessage));

            // Verify
            _sidebarServicesMock.Verify(x => x.GetSubsectionItems(), Times.Once);
            _homeServicesMock.Verify(x => x.CheckAllSubsectionApproved(subsectList), Times.Once);
            _loggerMock.Verify(x => x.Error(It.Is<string>(msg => msg == errorMessage)), Times.Once);
        }

        [Test]
        public async Task GetClientURL_ValidString_ReturnsOkObjectResult()
        {
            // Arrange
            var subdomainName = "demoSubdomain";
            var clientURL = "http://www.demo.com";

            // Setup
            _homeServicesMock.Setup(x => x.GetClientURL(subdomainName)).ReturnsAsync(clientURL).Verifiable();

            // Act
            var result = await _homeController.GetClientURL(subdomainName);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(clientURL));

            // Verify
            _homeServicesMock.Verify(x => x.GetClientURL(subdomainName), Times.Once);
        }


        [Test]
        public async Task GetClientURL_ValidString_ReturnsBadRequestObjectResult()
        {
            // Arrange
            var subdomainName = "demoSubdomain";
            var errorMessage = "Some Error Occured";

            // Setup
            _homeServicesMock.Setup(x => x.GetClientURL(subdomainName)).Throws(new Exception(errorMessage)).Verifiable();

            // Act
            var result = await _homeController.GetClientURL(subdomainName);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestObject = result as BadRequestObjectResult;
            Assert.That(badRequestObject.Value, Is.EqualTo(errorMessage));

            // Verify
            _homeServicesMock.Verify(x => x.GetClientURL(subdomainName), Times.Once);
            _loggerMock.Verify(x => x.Error(It.Is<string>(msg => msg == errorMessage)), Times.Once);
        }

        [Test]
        public async Task GetModuleOrdersWithModuleId_EmptyParameter_ReturnsOkObjectResult()
        {
            // Arrange
            var response = new List<ModuleOrderDto>
            {
                new ModuleOrderDto { Module_Id = "1", View_Order = 1}
            };

            // Setup
            _homeServicesMock.Setup(x => x.GetModuleOrdersWithModuleId()).ReturnsAsync(response).Verifiable();

            // Act
            var result = await _homeController.GetModuleOrdersWithModuleId();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(response));

            // Verify
            _homeServicesMock.Verify(x => x.GetModuleOrdersWithModuleId(), Times.Once);
        }


        [Test]
        public async Task GetModuleOrdersWithModuleId_EmptyParameter_ReturnsBadRequestObjectResult()
        {
            // Arrange
            var errorMessage = "Some Error Occured";

            // Setup
            _homeServicesMock.Setup(x => x.GetModuleOrdersWithModuleId()).Throws(new Exception(errorMessage)).Verifiable();

            // Act
            var result = await _homeController.GetModuleOrdersWithModuleId();

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestObject = result as BadRequestObjectResult;
            Assert.That(badRequestObject.Value, Is.EqualTo(errorMessage));

            // Verify
            _homeServicesMock.Verify(x => x.GetModuleOrdersWithModuleId(), Times.Once);
            _loggerMock.Verify(x => x.Error(It.Is<string>(msg => msg == errorMessage)), Times.Once);
        }
    }
}