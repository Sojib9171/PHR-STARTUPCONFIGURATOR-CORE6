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
    public class CommonConfigControllerTests
    {
        private Mock<ICommonConfigServices> _iCommonConfigServicesMock;
        private Mock<ISignOffService> _isignOffServicesMock;
        private Mock<ILogger> _loggerMock;
        private CommonConfigController _commonConfigController;

        [SetUp]
        public void Setup()
        {
            _iCommonConfigServicesMock = new Mock<ICommonConfigServices>();
            _isignOffServicesMock = new Mock<ISignOffService>();
            _loggerMock = new Mock<ILogger>();
            _commonConfigController = new CommonConfigController(_iCommonConfigServicesMock.Object, _isignOffServicesMock.Object, _loggerMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _iCommonConfigServicesMock.Reset();
            _isignOffServicesMock.Reset();
            _loggerMock.Reset();
        }

        [Test]
        public async Task GetCommonConfigQuestionDetails_NoParameter_ReturnsOkObjectResult()
        {
            // Arrange
            var expectedResult = new object();

            // Mock setup
            _iCommonConfigServicesMock.Setup(x => x.GetCommonConfigQuestionDetails())
                .Returns(Task.FromResult(expectedResult)).Verifiable();

            // Act
            var result = await _commonConfigController.GetCommonConfigQuestionDetails();

            // Assert
            _iCommonConfigServicesMock.Verify(x => x.GetCommonConfigQuestionDetails(), Times.Once);
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(expectedResult));
        }

        [Test]
        public async Task GetCommonConfigQuestionDetails_NoParameter_ReturnsBadRequestObjectResult()
        {
            // Arrange
            var errorMessage = "Some error occurred";

            // Mock setup

            _iCommonConfigServicesMock.Setup(x => x.GetCommonConfigQuestionDetails())
                .Throws(new Exception(errorMessage)).Verifiable();

            // Act
            var result = await _commonConfigController.GetCommonConfigQuestionDetails();


            // Assert
            _iCommonConfigServicesMock.Verify(x => x.GetCommonConfigQuestionDetails(), Times.Once);
            _loggerMock.Verify(x => x.Error(It.Is<string>(msg => msg == errorMessage)), Times.Once);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult.Value, Is.EqualTo(errorMessage));
        }

        [Test]
        public async Task GetCommonConfigQuestionDetailsWithAnswers_ValidInteger_ReturnsOkObjectResult()
        {
            // Arrange
            var rowId = 1;
            var expectedResult = new object();

            // Mock setup
            _iCommonConfigServicesMock.Setup(x => x.GetCommonConfigSummary(rowId))
                .Returns(Task.FromResult(expectedResult)).Verifiable();

            // Act
            var result = await _commonConfigController.GetCommonConfigQuestionDetailsWithAnswers(rowId);

            // Assert
            _iCommonConfigServicesMock.Verify(x => x.GetCommonConfigSummary(rowId), Times.Once);
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(expectedResult));
        }

        [Test]
        public async Task GetCommonConfigQuestionDetailsWithAnswers_ValidInteger_ReturnsBadRequestObjectResult()
        {
            // Arrange
            var rowId = 1;
            var errorMessage = "Some error occurred";

            // Mock setup
            _iCommonConfigServicesMock.Setup(x => x.GetCommonConfigSummary(rowId))
                .Throws(new Exception(errorMessage)).Verifiable();

            // Act
            var result = await _commonConfigController.GetCommonConfigQuestionDetailsWithAnswers(rowId);

            // Assert
            _iCommonConfigServicesMock.Verify(x => x.GetCommonConfigSummary(rowId), Times.Once);
            _loggerMock.Verify(x => x.Error(It.Is<string>(msg => msg == errorMessage)), Times.Once);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult.Value, Is.EqualTo(errorMessage));
        }

        [Test]
        public async Task InsertFirstDataForCommonConfig_CommonConfigDto_ReturnsOkObjectResult()
        {
            // Arrange
            var commonConfigDTO = new CommonConfigDto()
            {
                DateValue = new DateTime(2021, 1, 1),
                UserID = "demoUser",
                ResponseText = "Success",
                QuestionType = "Text",
                QuestionNo = 1
            };

            var expectedId = 2;

            // Mock setup
            _iCommonConfigServicesMock.Setup(x => x.InsertAndGetLastInsertedId(commonConfigDTO.QuestionNo, commonConfigDTO.ResponseText))
                .Returns(Task.FromResult(expectedId)).Verifiable();

            _iCommonConfigServicesMock.Setup(x => x.InsertIntoDraftTable(commonConfigDTO.UserID, expectedId,true,commonConfigDTO.DateValue)).Verifiable();


            // Act
            var result = await _commonConfigController.InsertFirstDataForCommonConfig(commonConfigDTO);

            // Assert
            _iCommonConfigServicesMock.Verify(x => x.InsertAndGetLastInsertedId(commonConfigDTO.QuestionNo, commonConfigDTO.ResponseText), Times.Once);
            _iCommonConfigServicesMock.Verify(x => x.InsertIntoDraftTable(commonConfigDTO.UserID, expectedId, true, commonConfigDTO.DateValue), Times.Once);

            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(expectedId, okResult.Value);
        }

        [Test]
        public async Task InsertFirstDataForCommonConfig_CommonConfigDto_ReturnsBadRequestObjectResult()
        {
            //Arrange
            var commonConfigDTO = new CommonConfigDto()
            {
                DateValue = new DateTime(2021, 1, 1),
                UserID = "demoUser",
                ResponseText = "Success",
                QuestionType = "Text",
                QuestionNo = 1
            };

            var expectedId = 2;
            var errorMessage = "Some error occurred";



            // Mock setup
            _iCommonConfigServicesMock.Setup(x => x.InsertAndGetLastInsertedId(commonConfigDTO.QuestionNo, commonConfigDTO.ResponseText))
                .Throws(new Exception(errorMessage)).Verifiable();

            // Act
            var result = await _commonConfigController.InsertFirstDataForCommonConfig(commonConfigDTO);

            // Assert
            _iCommonConfigServicesMock.Verify(x => x.InsertAndGetLastInsertedId(commonConfigDTO.QuestionNo, commonConfigDTO.ResponseText), Times.Once);

            _loggerMock.Verify(x => x.Error(It.Is<string>(msg => msg == errorMessage)), Times.Once);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult.Value, Is.EqualTo(errorMessage));
        }

        [Test]
        public async Task GetCommonConfigRadioOptions_ValidInteger_ReturnsOkObjectResult()
        {
            // Arrange
            var expectedResult = new object();
            var questionNo = 1;

            // Mock setup
            _iCommonConfigServicesMock.Setup(x => x.GetCommonConfigRadioOptions(questionNo))
                .Returns(Task.FromResult(expectedResult)).Verifiable();

            // Act
            var result = await _commonConfigController.GetCommonConfigRadioOptions(questionNo);

            // Assert
            _iCommonConfigServicesMock.Verify(x => x.GetCommonConfigRadioOptions(questionNo), Times.Once);
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(expectedResult));
        }

        [Test]
        public async Task GetCommonConfigRadioOptions_ValidInteger_ReturnsBadRequestObjectResult()
        {
            // Arrange
            var errorMessage = "Some error occurred";
            var questionNo = 1;

            // Mock setup
            _iCommonConfigServicesMock.Setup(x => x.GetCommonConfigRadioOptions(questionNo))
                .Throws(new Exception(errorMessage)).Verifiable();

            // Act
            var result = await _commonConfigController.GetCommonConfigRadioOptions(questionNo);


            // Assert
            _iCommonConfigServicesMock.Verify(x => x.GetCommonConfigRadioOptions(questionNo), Times.Once);
            _loggerMock.Verify(x => x.Error(It.Is<string>(msg => msg == errorMessage)), Times.Once);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult.Value, Is.EqualTo(errorMessage));
        }

        [Test]
        public async Task UpdateRecordForCommonConfig_WhenQuestionTypeIsSelectOption_ShouldCallUpdateTextColumnByRowId()
        {
            // Arrange
            var commonConfigUpdateDto = new CommonConfigUpdateDto
            {
                QuestionType = "Select Option",
                RecordId = 1,
                QuestionNo = 2,
                ResponseText = "SampleText"
            };

            // Act
            await _commonConfigController.UpdateRecordForCommonConfig(commonConfigUpdateDto);

            // Assert
            _iCommonConfigServicesMock.Verify(x => x.UpdateTextColumnByRowId(commonConfigUpdateDto.RecordId, commonConfigUpdateDto.QuestionNo, commonConfigUpdateDto.ResponseText.ToString()), Times.Once);
        }

        [Test]
        public async Task UpdateRecordForCommonConfig_WhenQuestionTypeIsImage_ShouldCallUpdateImageColumnByRowId()
        {
            // Arrange
            var commonConfigUpdateDto = new CommonConfigUpdateDto
            {
                QuestionType = "Image",
                RecordId = 1,
                QuestionNo = 2,
                ResponseText = Convert.ToBase64String(Encoding.UTF8.GetBytes("SampleImage")),
                LogoImageName = "LogoImageName",
                MobileLogoImageName = "MobileLogoImageName"
            };

            // Act
            await _commonConfigController.UpdateRecordForCommonConfig(commonConfigUpdateDto);

            // Assert
            _iCommonConfigServicesMock.Verify(x => x.UpdateImageColumnByRowId(commonConfigUpdateDto.RecordId, commonConfigUpdateDto.QuestionNo, It.IsAny<byte[]>()), Times.Once);
            _iCommonConfigServicesMock.Verify(x => x.UpdateImageNameColumnByRowId(commonConfigUpdateDto.RecordId, commonConfigUpdateDto.LogoImageName), Times.Once);
            _iCommonConfigServicesMock.Verify(x => x.UpdateMobileImageNameColumnByRowId(commonConfigUpdateDto.RecordId, commonConfigUpdateDto.MobileLogoImageName), Times.Once);
        }

        [Test]
        public async Task UpdateRecordForCommonConfig_WhenQuestionTypeIsYesNo_ShouldCallUpdateOptionsColumnByRowId()
        {
            // Arrange
            var commonConfigUpdateDto = new CommonConfigUpdateDto
            {
                QuestionType = "Yes/No",
                RecordId = 1,
                QuestionNo = 2,
                ResponseText = "true"
            };

            // Act
            await _commonConfigController.UpdateRecordForCommonConfig(commonConfigUpdateDto);

            // Assert
            _iCommonConfigServicesMock.Verify(x => x.UpdateOptionsColumnByRowId(commonConfigUpdateDto.RecordId, commonConfigUpdateDto.QuestionNo, true), Times.Once);
        }

        [Test]
        public async Task UpdateRecordForCommonConfig_WhenExceptionOccurs_ShouldReturnBadRequest()
        {
            // Arrange
            var commonConfigUpdateDto = new CommonConfigUpdateDto
            {
                QuestionType = "InvalidType",
                RecordId = 1,
                QuestionNo = 2,
                ResponseText = "SampleText"
            };
            var errorMessage = "Invalid Question Type";

            _iCommonConfigServicesMock.Setup(x => x.UpdateTextColumnByRowId(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
                .Throws(new Exception(errorMessage));

            // Act
            var result = await _commonConfigController.UpdateRecordForCommonConfig(commonConfigUpdateDto);

            // Assert
            _loggerMock.Verify(x => x.Error(It.Is<string>(msg => msg == errorMessage)), Times.Once);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult.Value, Is.EqualTo(errorMessage));
        }

        [Test]
        public async Task GetCommonConfigSummary_ValidInteger_ReturnsOkObjectResult()
        {
            // Arrange
            var rowId = 1;
            var expectedResult = new object();

            // Mock setup
            _iCommonConfigServicesMock.Setup(x => x.GetCommonConfigSummary(rowId))
                .Returns(Task.FromResult(expectedResult)).Verifiable();

            // Act
            var result = await _commonConfigController.GetCommonConfigSummary(rowId);

            // Assert
            _iCommonConfigServicesMock.Verify(x => x.GetCommonConfigSummary(rowId), Times.Once);
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(expectedResult));
        }

        [Test]
        public async Task GetCommonConfigSummary_ValidInteger_ReturnsBadRequestObjectResult()
        {
            // Arrange
            var rowId = 1;
            var errorMessage = "Some error occurred";

            // Mock setup
            _iCommonConfigServicesMock.Setup(x => x.GetCommonConfigSummary(rowId))
                .Throws(new Exception(errorMessage)).Verifiable();

            // Act
            var result = await _commonConfigController.GetCommonConfigSummary(rowId);

            // Assert
            _iCommonConfigServicesMock.Verify(x => x.GetCommonConfigSummary(rowId), Times.Once);
            _loggerMock.Verify(x => x.Error(It.Is<string>(msg => msg == errorMessage)), Times.Once);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult.Value, Is.EqualTo(errorMessage));
        }

        [Test]
        public async Task GetCommonConfigApprovalStatus_StringUserId_ReturnsOkObjectResult()
        {
            // Arrange
            var userId = "a";
            var expectedResult = true;
            
            // Mock setup
            _iCommonConfigServicesMock.Setup(x => x.GetCommonConfigApprovalStatus(userId))
                .Returns(Task.FromResult(true)).Verifiable();

            // Act
            var result = await _commonConfigController.GetCommonConfigApprovalStatus(userId);

            // Assert
            _iCommonConfigServicesMock.Verify(x => x.GetCommonConfigApprovalStatus(userId), Times.Once);
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(expectedResult));
        }

        [Test]
        public async Task GetCommonConfigApprovalStatus_StringUserId_ReturnsBadRequestObjectResult()
        {
            // Arrange
            var userId = "a";
            var expectedResult = new object();
            var errorMessage = "Some error occurred";

            // Mock setup
            _iCommonConfigServicesMock.Setup(x => x.GetCommonConfigApprovalStatus(userId))
                .Throws(new Exception(errorMessage)).Verifiable();

            // Act
            var result = await _commonConfigController.GetCommonConfigApprovalStatus(userId);

            // Assert
            _iCommonConfigServicesMock.Verify(x => x.GetCommonConfigApprovalStatus(userId), Times.Once);
            _loggerMock.Verify(x => x.Error(It.Is<string>(msg => msg == errorMessage)), Times.Once);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult.Value, Is.EqualTo(errorMessage));
        }

        [Test]
        public async Task UpdateDraftStatusForCommonConfig_CommonConfigDrftDto_ReturnsOkObjectResult()
        {
            // Arrange
            var commonConfigDrftDto = new CommonConfigDrftDto()
            {
                TableRowId = 1
            };

            // Mock setup
            _iCommonConfigServicesMock.Setup(x => x.UpdateDraftStatusForCommonConfig(commonConfigDrftDto.TableRowId)).Verifiable();

            // Act
            await _commonConfigController.UpdateDraftStatusForCommonConfig(commonConfigDrftDto);

            // Assert
            _iCommonConfigServicesMock.Verify(x => x.UpdateDraftStatusForCommonConfig(commonConfigDrftDto.TableRowId), Times.Once);
        }

        [Test]
        public async Task UpdateDraftStatusForCommonConfig_CommonConfigDrftDto_ReturnsBadRequestObjectResult()
        {
            // Arrange
            var commonConfigDrftDto = new CommonConfigDrftDto()
            {
                TableRowId = 1
            };
            var errorMessage="Some error occurred";

            // Mock setup
            _iCommonConfigServicesMock.Setup(x => x.UpdateDraftStatusForCommonConfig(commonConfigDrftDto.TableRowId)).
                Throws(new Exception(errorMessage)).Verifiable();

            // Act
            var result = await _commonConfigController.UpdateDraftStatusForCommonConfig(commonConfigDrftDto);


            // Assert
            _iCommonConfigServicesMock.Verify(x => x.UpdateDraftStatusForCommonConfig(commonConfigDrftDto.TableRowId), Times.Once);
            _loggerMock.Verify(x => x.Error(It.Is<string>(msg => msg == errorMessage)), Times.Once);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult.Value, Is.EqualTo(errorMessage));
        }

        [Test]
        public async Task UploadCommonConfigData_CommonConfigPostDto_ReturnsOkObjectResult()
        {
            // Arrange
            var commonConfigPostDto = new CommonConfigPostDto()
            {
                UserName = "demoUser",
                Name = "demoUserName",
                Comment = "demoComment",
                ApprovalStatus = "Approved",
                ApprovalDate= new DateTime(2021, 1, 1),
                SignOffPdfData= new byte[1] {0},
                TableRowId=1
            };

            var htmlString = "htmlString";
            var expectedString = "Data successfully inserted";

            // Mock setup
            _isignOffServicesMock.Setup(x => x.GetSignOffHtmlStringForConfiguration(commonConfigPostDto.Name, commonConfigPostDto.ApprovalDate, commonConfigPostDto.ApprovalStatus, commonConfigPostDto.Comment, commonConfigPostDto.UserName, "Common Configuration"))
                .Returns(Task.FromResult(htmlString)).Verifiable();

            _iCommonConfigServicesMock.Setup(x => x.InsertCommonConfigInfo(commonConfigPostDto)).Verifiable();


            // Act
            var result = await _commonConfigController.UploadCommonConfigData(commonConfigPostDto);

            // Assert
            _isignOffServicesMock.Verify(x => x.GetSignOffHtmlStringForConfiguration(commonConfigPostDto.Name, commonConfigPostDto.ApprovalDate, commonConfigPostDto.ApprovalStatus, commonConfigPostDto.Comment, commonConfigPostDto.UserName, "Common Configuration"), Times.Once);
            _iCommonConfigServicesMock.Verify(x => x.InsertCommonConfigInfo(commonConfigPostDto), Times.Once);

            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(expectedString, okResult.Value);
        }

        [Test]
        public async Task UploadCommonConfigData_CommonConfigPostDto_ReturnsBadRequestObjectResult()
        {
            // Arrange
            var commonConfigPostDto = new CommonConfigPostDto()
            {
                UserName = "demoUser",
                Name = "demoUserName",
                Comment = "demoComment",
                ApprovalStatus = "Approved",
                ApprovalDate = new DateTime(2021, 1, 1),
                SignOffPdfData = new byte[1] { 0 },
                TableRowId = 1
            };

            var htmlString = "htmlString";
            var errorMessage = "Some error occurred";

            // Mock setup
            _isignOffServicesMock.Setup(x => x.GetSignOffHtmlStringForConfiguration(commonConfigPostDto.Name, commonConfigPostDto.ApprovalDate, commonConfigPostDto.ApprovalStatus, commonConfigPostDto.Comment, commonConfigPostDto.UserName, "Common Configuration"))
                .Returns(Task.FromResult(htmlString)).Verifiable();

            _iCommonConfigServicesMock.Setup(x => x.InsertCommonConfigInfo(commonConfigPostDto)).Throws(new Exception(errorMessage)).Verifiable();


            // Act
            var result = await _commonConfigController.UploadCommonConfigData(commonConfigPostDto);

            // Assert
            _isignOffServicesMock.Verify(x => x.GetSignOffHtmlStringForConfiguration(commonConfigPostDto.Name, commonConfigPostDto.ApprovalDate, commonConfigPostDto.ApprovalStatus, commonConfigPostDto.Comment, commonConfigPostDto.UserName, "Common Configuration"), Times.Once);
            _iCommonConfigServicesMock.Verify(x => x.InsertCommonConfigInfo(commonConfigPostDto), Times.Once);

            _loggerMock.Verify(x => x.Error(It.Is<string>(msg => msg == errorMessage)), Times.Once);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult.Value, Is.EqualTo(errorMessage));
        }

        [Test]
        public async Task DeleteDataByRowId_DeleteByRowOnlyDto_ReturnsOkObjectResult()
        {
            // Arrange
            var model = new DeleteByRowOnlyDto()
            {
                tableRowId = 1
            };

            // Mock setup
            _iCommonConfigServicesMock.Setup(x => x.DeleteFromDraftTable(model.tableRowId)).Verifiable();
            _iCommonConfigServicesMock.Setup(x => x.DeleteFromMainTable(model.tableRowId)).Verifiable();

            // Act
            var result = await _commonConfigController.DeleteDataByRowId(model);

            // Assert
            _iCommonConfigServicesMock.Verify(x => x.DeleteFromDraftTable(model.tableRowId), Times.Once);
            _iCommonConfigServicesMock.Verify(x => x.DeleteFromMainTable(model.tableRowId), Times.Once);

            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public async Task DeleteDataByRowId_DeleteByRowOnlyDto_ReturnsBadRequestObjectResult()
        {
            // Arrange
            var model = new DeleteByRowOnlyDto()
            {
                tableRowId = 1
            };
            var errorMessage = "Some error occurred";


            // Mock setup
            _iCommonConfigServicesMock.Setup(x => x.DeleteFromDraftTable(model.tableRowId)).Verifiable();
            _iCommonConfigServicesMock.Setup(x => x.DeleteFromMainTable(model.tableRowId)).Throws(new Exception(errorMessage)).Verifiable();

            // Act
            var result = await _commonConfigController.DeleteDataByRowId(model);

            // Assert
            _iCommonConfigServicesMock.Verify(x => x.DeleteFromDraftTable(model.tableRowId), Times.Once);
            _iCommonConfigServicesMock.Verify(x => x.DeleteFromMainTable(model.tableRowId), Times.Once);


            _loggerMock.Verify(x => x.Error(It.Is<string>(msg => msg == errorMessage)), Times.Once);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult.Value, Is.EqualTo(errorMessage));
        }

        [Test]
        public async Task GetIsDraftStatus_StringUserId_ReturnsOkObjectResult()
        {
            // Arrange
            var userId = "a";
            var expectedResult = new CommonConfigDrftStatusDto() { IsDraft = true, TableRowId = 1 } ;

            // Mock setup
            _iCommonConfigServicesMock.Setup(x => x.GetIsDraftStatus(userId))
                .Returns(Task.FromResult(new CommonConfigDrftStatusDto() { IsDraft=true, TableRowId=1})).Verifiable();

            // Act
            var result = await _commonConfigController.GetIsDraftStatus(userId);

            // Assert
            _iCommonConfigServicesMock.Verify(x => x.GetIsDraftStatus(userId), Times.Once);
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(expectedResult.IsDraft, ((CommonConfigDrftStatusDto)okResult.Value).IsDraft);
            Assert.AreEqual(expectedResult.TableRowId, ((CommonConfigDrftStatusDto)okResult.Value).TableRowId);
        }

        [Test]
        public async Task GetIsDraftStatus_StringUserId_ReturnsBadRequestObjectResult()
        {
            // Arrange
            var userId = "a";
            var expectedResult = new object();
            var errorMessage = "Some error occurred";

            // Mock setup
            _iCommonConfigServicesMock.Setup(x => x.GetIsDraftStatus(userId))
                .Throws(new Exception(errorMessage)).Verifiable();

            // Act
            var result = await _commonConfigController.GetIsDraftStatus(userId);

            // Assert
            _iCommonConfigServicesMock.Verify(x => x.GetIsDraftStatus(userId), Times.Once);
            _loggerMock.Verify(x => x.Error(It.Is<string>(msg => msg == errorMessage)), Times.Once);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult.Value, Is.EqualTo(errorMessage));
        }

        [Test]
        public async Task GetCommonConfigHistory_ValidString_ReturnsOkObjectResult()
        {
            // Arrange
            var serverParams = "{\"userid\":\"a\",\"sortField\":\"approval_date\",\"sortType\":\"desc\",\"page\":1,\"perPage\":10,\"searchText\":\"\"}";
            var parameters = JsonConvert.DeserializeObject<CommonConfigHistoryServerParamsDto>(serverParams);
            var expectedResult = new object();

            // Mock setup
            _iCommonConfigServicesMock.Setup(x => x.GetCommonConfigHistory(It.IsAny<CommonConfigHistoryServerParamsDto>()))
                .Returns(Task.FromResult(expectedResult)).Verifiable();

            // Act
            var result = await _commonConfigController.GetCommonConfigHistory(serverParams);

            // Assert
            _iCommonConfigServicesMock.Verify(x => x.GetCommonConfigHistory(It.IsAny<CommonConfigHistoryServerParamsDto>()), Times.Once);
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(expectedResult));
        }

        [Test]
        public async Task GetCommonConfigHistory_ValidString_ReturnsBadRequestObjectResult()
        {
            // Arrange
            var serverParams = "{\"sortField\":\"module_name\",\"sortType\":\"asc\",\"page\":1,\"perPage\":10,\"searchText\":\"\"}";
            var parameters = JsonConvert.DeserializeObject<CommonConfigHistoryServerParamsDto>(serverParams);
            var errorMessage = "Some error occurred";

            // Mock setup
            _iCommonConfigServicesMock.Setup(x => x.GetCommonConfigHistory(It.IsAny<CommonConfigHistoryServerParamsDto>()))
                .Throws(new Exception(errorMessage)).Verifiable();

            // Act
            var result = await _commonConfigController.GetCommonConfigHistory(serverParams);

            // Assert
            _iCommonConfigServicesMock.Verify(x => x.GetCommonConfigHistory(It.IsAny<CommonConfigHistoryServerParamsDto>()), Times.Once);
            _loggerMock.Verify(x => x.Error(It.Is<string>(msg => msg == errorMessage)), Times.Once);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult.Value, Is.EqualTo(errorMessage));
        }
    }
}
