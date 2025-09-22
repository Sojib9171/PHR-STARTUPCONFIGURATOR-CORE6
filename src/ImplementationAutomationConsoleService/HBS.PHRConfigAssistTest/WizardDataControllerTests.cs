using HBS.ImplementationAutomationConsole.API.Controllers;
using HBS.ImplementationAutomationConsole.Core.BLL.IServices;
using HBS.ImplementationAutomationConsole.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using ILogger = Serilog.ILogger;

namespace HBS.PHRConfigAssistTest
{
    public class WizardDataControllerTests
    {
        private Mock<IWizardDataServices> _iwizardDataServicesMock;
        private Mock<ILogger> _loggerMock;
        private WizardDataController _wizardDataController;

        [SetUp]
        public void Setup()
        {
            _iwizardDataServicesMock = new Mock<IWizardDataServices>();
            _loggerMock = new Mock<ILogger>();
            _wizardDataController = new WizardDataController(_iwizardDataServicesMock.Object, _loggerMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _iwizardDataServicesMock.Reset();
            _loggerMock.Reset();
        }

        [Test]
        public async Task GetWizardQuestionDetails_ValidString_ReturnsOkObjectResult()
        {
            // Arrange
            var subsectionName = "Demo Subsection";
            var expectedResult = new object();

            _iwizardDataServicesMock.Setup(x => x.GetWizardQuestionDetails(subsectionName)).ReturnsAsync(expectedResult).Verifiable();

            // Act
            var result = await _wizardDataController.GetWizardQuestionDetails(subsectionName);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(expectedResult));

            //Verify
            _iwizardDataServicesMock.Verify(x => x.GetWizardQuestionDetails(subsectionName), Times.Once);
        }


        [Test]
        public async Task DownloadTemplate_ValidString_ReturnsBadRequestObjectResult()
        {
            // Arrange
            var subsectionName = "Demo Subsection";
            var errorMesssage = "Some Error Occured";

            _iwizardDataServicesMock.Setup(x => x.GetWizardQuestionDetails(subsectionName)).Throws(new Exception(errorMesssage)).Verifiable();

            // Act
            var result = await _wizardDataController.GetWizardQuestionDetails(subsectionName);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult.Value, Is.EqualTo(errorMesssage));

            //Verify
            _iwizardDataServicesMock.Verify(x => x.GetWizardQuestionDetails(subsectionName), Times.Once);
            _loggerMock.Verify(x => x.Error(errorMesssage), Times.Once);
        }

        [Test]
        public async Task UploadWizardData_WizardDataDto_ReturnsOkObjectResult()
        {
            // Arrange
            var WizardObject = new WizardDataDto()
            {
                WizardTypes = new List<List<WizarDataInfoDto>>()
                {
                    new List<WizarDataInfoDto>()
                    {
                        new WizarDataInfoDto()
                        {
                        Question_no = 1,
                        Question_statement = "Demo Question",
                        Question_type = "Demo Type",
                        Response = "Demo Response"
                        }
                    }
                },
                SubsectionName = "Demo Subsection"
            };

            _iwizardDataServicesMock.Setup(x => x.UploadWizardData(WizardObject.WizardTypes, WizardObject.SubsectionName)).Verifiable();

            // Act
            var result = await _wizardDataController.UploadWizardData(WizardObject);

            // Assert
            Assert.IsInstanceOf<OkResult>(result);


            //Verify
            _iwizardDataServicesMock.Verify(x => x.UploadWizardData(WizardObject.WizardTypes, WizardObject.SubsectionName), Times.Once);
        }

        [Test]
        public async Task UploadWizardData_WizardDataDto_ReturnsBadRequestObjectResult()
        {
            // Arrange
            var WizardObject = new WizardDataDto()
            {
                WizardTypes = new List<List<WizarDataInfoDto>>()
                {
                    new List<WizarDataInfoDto>()
                    {
                        new WizarDataInfoDto()
                        {
                        Question_no = 1,
                        Question_statement = "Demo Question",
                        Question_type = "Demo Type",
                        Response = "Demo Response"
                        }
                    }
                },
                SubsectionName = "Demo Subsection"
            };
            var errorMesssage = "Some Error Occured";

            _iwizardDataServicesMock.Setup(x => x.UploadWizardData(WizardObject.WizardTypes, WizardObject.SubsectionName)).Throws(new Exception(errorMesssage)).Verifiable();

            // Act
            var result = await _wizardDataController.UploadWizardData(WizardObject);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult.Value, Is.EqualTo(errorMesssage));

            //Verify
            _iwizardDataServicesMock.Verify(x => x.UploadWizardData(WizardObject.WizardTypes, WizardObject.SubsectionName), Times.Once);
            _loggerMock.Verify(x => x.Error(errorMesssage), Times.Once);
        }

        [Test]
        public async Task GetDropdownOptions_ValidString_ReturnsOkObjectResult()
        {
            // Arrange
            var parameters = "{\"subsectionName\":\"demo Subsection\",\"questionNo\":\"1\"}";
            var wizardDropdownDto = JsonConvert.DeserializeObject<WizardDropdownDto>(parameters);
            var expectedResult = new List<string>()
            {
                "String 1","String 2"
            };

            _iwizardDataServicesMock.Setup(x => x.GetWizardDropdownOptions(wizardDropdownDto.subsectionName, wizardDropdownDto.questionNo)).ReturnsAsync(expectedResult).Verifiable();

            //Act
            var result = await _wizardDataController.GetDropdownOptions(parameters);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(expectedResult));

            //Verify
            _iwizardDataServicesMock.Verify(x => x.GetWizardDropdownOptions(wizardDropdownDto.subsectionName, wizardDropdownDto.questionNo), Times.Once);
        }

        [Test]
        public async Task GetDropdownOptions_ValidString_ReturnsBadRequestObjectResult()
        {
            // Arrange
            var parameters = "{\"subsectionName\":\"demo Subsection\",\"questionNo\":\"1\"}";
            var wizardDropdownDto = JsonConvert.DeserializeObject<WizardDropdownDto>(parameters);
            var errorMesssage = "Some Error Occured";

            _iwizardDataServicesMock.Setup(x => x.GetWizardDropdownOptions(wizardDropdownDto.subsectionName, wizardDropdownDto.questionNo)).Throws(new Exception(errorMesssage)).Verifiable();

            //Act
            var result = await _wizardDataController.GetDropdownOptions(parameters);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult.Value, Is.EqualTo(errorMesssage));

            //Verify
            _iwizardDataServicesMock.Verify(x => x.GetWizardDropdownOptions(wizardDropdownDto.subsectionName, wizardDropdownDto.questionNo), Times.Once);
            _loggerMock.Verify(x => x.Error(errorMesssage), Times.Once);
        }

        [Test]
        public async Task InsertFirstRecordForAbsence_AbsenceWizardDto_ReturnsOkObjectResult()
        {
            // Arrange
            var absenceWizardDto = new AbsenceWizardDto()
            {
                UserID = "Demo User",
                SubsectionName = "Demo Subsection",
                DateValue = new DateTime(2021, 12, 12),
                ResponseText = "Demo Response",
                QuestionNo = 1,
                QuestionType = "Demo Type"
            };

            var Id = 1;
            _iwizardDataServicesMock.Setup(x => x.InsertAndGetLastInsertedId(absenceWizardDto.QuestionNo, absenceWizardDto.ResponseText, absenceWizardDto.SubsectionName)).ReturnsAsync(Id).Verifiable();

            // Act
            var result = await _wizardDataController.InsertFirstRecordForAbsence(absenceWizardDto);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(Id));

            //Verify
            _iwizardDataServicesMock.Verify(x => x.InsertAndGetLastInsertedId(absenceWizardDto.QuestionNo, absenceWizardDto.ResponseText, absenceWizardDto.SubsectionName), Times.Once);
        }

        [Test]
        public async Task InsertFirstRecordForAbsence_AbsenceWizardDto_ReturnsBadRequestObjectResult()
        {
            // Arrange
            var absenceWizardDto = new AbsenceWizardDto()
            {
                UserID = "Demo User",
                SubsectionName = "Demo Subsection",
                DateValue = new DateTime(2021, 12, 12),
                ResponseText = "Demo Response",
                QuestionNo = 1,
                QuestionType = "Demo Type"
            };
            var errorMesssage = "Some Error Occured";

            _iwizardDataServicesMock.Setup(x => x.InsertAndGetLastInsertedId(absenceWizardDto.QuestionNo, absenceWizardDto.ResponseText, absenceWizardDto.SubsectionName)).Throws(new Exception(errorMesssage)).Verifiable();

            // Act
            var result = await _wizardDataController.InsertFirstRecordForAbsence(absenceWizardDto);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult.Value, Is.EqualTo(errorMesssage));

            //Verify
            _iwizardDataServicesMock.Verify(x => x.InsertAndGetLastInsertedId(absenceWizardDto.QuestionNo, absenceWizardDto.ResponseText, absenceWizardDto.SubsectionName), Times.Once);
            _loggerMock.Verify(x => x.Error(errorMesssage), Times.Once);
        }


        [Test]
        public async Task UpdateRecordForAbsenceWizard_AbsenceWizardUpdateDtoTextTypeQuestion_ReturnsOkObjectResult()
        {
            //Arrange
            var absenceWizardUpdateDto = new AbsenceWizardUpdateDto()
            {
                RecordId = 1,
                ResponseText = "Demo Response",
                QuestionNo = 1,
                SubsectionName = "Demo Subsection",
                QuestionType = "Text"
            };

            _iwizardDataServicesMock.Setup(x => x.UpdateTextColumnByRowId(absenceWizardUpdateDto.RecordId, absenceWizardUpdateDto.QuestionNo, absenceWizardUpdateDto.ResponseText.ToString(), absenceWizardUpdateDto.SubsectionName)).Verifiable();

            //Act
            var result = await _wizardDataController.UpdateRecordForAbsenceWizard(absenceWizardUpdateDto);

            //Assert
            Assert.IsInstanceOf<OkResult>(result);
        }


        [Test]
        public async Task UpdateRecordForAbsenceWizard_AbsenceWizardUpdateDtoYesNoTypeQuestion_ReturnsOkObjectResult()
        {
            //Arrange
            var absenceWizardUpdateDto = new AbsenceWizardUpdateDto()
            {
                RecordId = 1,
                ResponseText = "true",
                QuestionNo = 1,
                SubsectionName = "Demo Subsection",
                QuestionType = "Yes/No"
            };

            var response = true;
            _iwizardDataServicesMock.Setup(x => x.UpdateOptionsColumnByRowId(absenceWizardUpdateDto.RecordId, absenceWizardUpdateDto.QuestionNo, response, absenceWizardUpdateDto.SubsectionName)).Verifiable();

            //Act
            var result = await _wizardDataController.UpdateRecordForAbsenceWizard(absenceWizardUpdateDto);

            //Assert
            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public async Task UpdateRecordForAbsenceWizard_AbsenceWizardUpdateDtoNumericTypeQuestion_ReturnsOkObjectResult()
        {
            //Arrange
            var absenceWizardUpdateDto = new AbsenceWizardUpdateDto()
            {
                RecordId = 1,
                ResponseText = "true",
                QuestionNo = 1,
                SubsectionName = "Demo Subsection",
                QuestionType = "Yes/No"
            };

            var response = 1.5m;
            _iwizardDataServicesMock.Setup(x => x.UpdateNumericColumnByRowId(absenceWizardUpdateDto.RecordId, absenceWizardUpdateDto.QuestionNo, response, absenceWizardUpdateDto.SubsectionName)).Verifiable();

            //Act
            var result = await _wizardDataController.UpdateRecordForAbsenceWizard(absenceWizardUpdateDto);

            //Assert
            Assert.IsInstanceOf<OkResult>(result);
        }


        [Test]
        public async Task UpdateRecordForAbsenceWizard_AbsenceWizardUpdateDtoTextTypeQuestion_ReturnsBadRequestObjectResult()
        {
            //Arrange
            var absenceWizardUpdateDto = new AbsenceWizardUpdateDto()
            {
                RecordId = 1,
                ResponseText = "Demo Response",
                QuestionNo = 1,
                SubsectionName = "Demo Subsection",
                QuestionType = "Text"
            };
            var errorMesssage = "Some Error Occured";

            _iwizardDataServicesMock.Setup(x => x.UpdateTextColumnByRowId(absenceWizardUpdateDto.RecordId, absenceWizardUpdateDto.QuestionNo, absenceWizardUpdateDto.ResponseText.ToString(), absenceWizardUpdateDto.SubsectionName)).Throws(new Exception(errorMesssage)).Verifiable();

            //Act
            var result = await _wizardDataController.UpdateRecordForAbsenceWizard(absenceWizardUpdateDto);

            //Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult.Value, Is.EqualTo(errorMesssage));

            //Verify
            _iwizardDataServicesMock.Verify(x => x.UpdateTextColumnByRowId(absenceWizardUpdateDto.RecordId, absenceWizardUpdateDto.QuestionNo, absenceWizardUpdateDto.ResponseText.ToString(), absenceWizardUpdateDto.SubsectionName), Times.Once);
            _loggerMock.Verify(x => x.Error(errorMesssage), Times.Once);
        }

        [Test]
        public async Task GetAbsenceWizardSelectedOptionsSummary_ValidIntegerAndValidString_ReturnsOkObjectResult()
        {
            //Arrange
            var rowId = 1;
            var subsectionName = "Demo Subsection";
            var expectedResult = new object();

            _iwizardDataServicesMock.Setup(x => x.GetAbsenceWizardSelectedOptionsSummary(rowId, subsectionName)).ReturnsAsync(expectedResult).Verifiable();

            //Act
            var result = await _wizardDataController.GetAbsenceWizardSelectedOptionsSummary(rowId, subsectionName);

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(expectedResult));
        }

        [Test]
        public async Task GetAbsenceWizardSelectedOptionsSummary_ValidIntegerAndValidString_ReturnsBadRequestObjectResult()
        {
            //Arrange
            var rowId = 1;
            var subsectionName = "Demo Subsection";
            var errorMesssage = "Some Error Occured";

            _iwizardDataServicesMock.Setup(x => x.GetAbsenceWizardSelectedOptionsSummary(rowId, subsectionName)).Throws(new Exception(errorMesssage)).Verifiable();

            //Act
            var result = await _wizardDataController.GetAbsenceWizardSelectedOptionsSummary(rowId, subsectionName);

            //Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult.Value, Is.EqualTo(errorMesssage));

            //Verify
            _iwizardDataServicesMock.Verify(x => x.GetAbsenceWizardSelectedOptionsSummary(rowId, subsectionName), Times.Once);
            _loggerMock.Verify(x => x.Error(errorMesssage), Times.Once);
        }

        [Test]
        public async Task DeleteDataByRowId_DeleteByRowDto_ReturnsOkObjectResult()
        {
            //Arrange
            var deleteByRowDto = new DeleteByRowDto()
            {
                tableRowId = 1,
                subsectionName = "Demo Subsection"
            };

            _iwizardDataServicesMock.Setup(x => x.DeleteFromDraftTable(deleteByRowDto.tableRowId, deleteByRowDto.subsectionName)).Verifiable();
            _iwizardDataServicesMock.Setup(x => x.DeleteFromMainTable(deleteByRowDto.tableRowId, deleteByRowDto.subsectionName)).Verifiable();

            //Act
            var result = await _wizardDataController.DeleteDataByRowId(deleteByRowDto);

            //Assert
            Assert.IsInstanceOf<OkResult>(result);

            //Verify
            _iwizardDataServicesMock.Verify(x => x.DeleteFromDraftTable(deleteByRowDto.tableRowId, deleteByRowDto.subsectionName), Times.Once);
            _iwizardDataServicesMock.Verify(x => x.DeleteFromMainTable(deleteByRowDto.tableRowId, deleteByRowDto.subsectionName), Times.Once);
        }


        [Test]
        public async Task DeleteDataByRowId_DeleteByRowDto_ReturnsBadRequestObjectResult()
        {
            //Arrange
            var deleteByRowDto = new DeleteByRowDto()
            {
                tableRowId = 1,
                subsectionName = "Demo Subsection"
            };
            var errorMesssage = "Some Error Occured";

            _iwizardDataServicesMock.Setup(x => x.DeleteFromMainTable(deleteByRowDto.tableRowId, deleteByRowDto.subsectionName)).Throws(new Exception(errorMesssage)).Verifiable();

            //Act
            var result = await _wizardDataController.DeleteDataByRowId(deleteByRowDto);

            //Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult.Value, Is.EqualTo(errorMesssage));

            //Verify
            _iwizardDataServicesMock.Verify(x => x.DeleteFromMainTable(deleteByRowDto.tableRowId, deleteByRowDto.subsectionName), Times.Once);
            _loggerMock.Verify(x => x.Error(errorMesssage), Times.Once);
        }

        [Test]
        public async Task DeleteDataFromDraftAndMainTable_DeleteBySubsectionDto_ReturnsOkObjectResult()
        {
            //Arrange
            var model = new DeleteBySubsectionDto
            {
                subsectionName = "demoSubsectionName"
            };

            _iwizardDataServicesMock.Setup(x => x.DeleteDataFromDraftAndMainTable(model.subsectionName)).Verifiable();

            //Act
            var result = await _wizardDataController.DeleteDataFromDraftAndMainTable(model);

            //Assert
            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public async Task DeleteDataFromDraftAndMainTable_DeleteBySubsectionDto_ReturnsBadRequestObjectResult()
        {
            //Arrange
            var model = new DeleteBySubsectionDto
            {
                subsectionName = "demoSubsectionName"
            };
            var errorMesssage = "Some Error Occured";

            _iwizardDataServicesMock.Setup(x => x.DeleteDataFromDraftAndMainTable(model.subsectionName)).Throws(new Exception(errorMesssage)).Verifiable();

            //Act
            var result = await _wizardDataController.DeleteDataFromDraftAndMainTable(model);

            //Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult.Value, Is.EqualTo(errorMesssage));

            //Verify
            _iwizardDataServicesMock.Verify(x => x.DeleteDataFromDraftAndMainTable(model.subsectionName), Times.Once);
            _loggerMock.Verify(x => x.Error(errorMesssage), Times.Once);
        }

        [Test]
        public async Task DeleteDataFromDependentTable_DeleteBySubsectionDto_ReturnsOkObjectResult()
        {
            //Arrange
            var model = new DeleteBySubsectionDto
            {
                subsectionName = "Short Leave"
            };

            _iwizardDataServicesMock.Setup(x => x.DeleteDataFromDependentTable(model.subsectionName)).Verifiable();

            //Act
            var result = await _wizardDataController.DeleteDataFromDependentTable(model);

            //Assert
            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public async Task DeleteDataFromDependentTable_DeleteBySubsectionDto_ReturnsBadRequestObjectResult()
        {
            //Arrange
            var model = new DeleteBySubsectionDto
            {
                subsectionName = "Short Leave"
            };
            var errorMesssage = "Some Error Occured";

            _iwizardDataServicesMock.Setup(x => x.DeleteDataFromDependentTable(model.subsectionName)).Throws(new Exception(errorMesssage)).Verifiable();

            //Act
            var result = await _wizardDataController.DeleteDataFromDependentTable(model);

            //Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult.Value, Is.EqualTo(errorMesssage));

            //Verify
            _iwizardDataServicesMock.Verify(x => x.DeleteDataFromDependentTable(model.subsectionName), Times.Once);
            _loggerMock.Verify(x => x.Error(errorMesssage), Times.Once);
        }

        [Test]
        public async Task DeleteMultipleAbsenceRecords_AbsenceWizardMultipleDeleteDto_ReturnsOkObjectResult()
        {
            //Arrange
            var model = new AbsenceWizardMultipleDeleteDto
            {
                selectedRows = new List<AbsenceWizardDeleteRowDto>()
                {
                    new AbsenceWizardDeleteRowDto()
                    {
                        record_id = 1,
                        leaveTypeCode = "Demo Leave Type",
                        originalIndex = 1,
                        vgtSelected = true,
                        vgt_id=2
                    }
                },
                subsectionName = "Demo Subsection"
            };
            var rowIds = model.selectedRows.Select(x => x.record_id).ToList();

            _iwizardDataServicesMock.Setup(x => x.DeleteSelectedRowsFromDraftAndMainTable(rowIds, model.subsectionName)).Verifiable();

            //Act
            var result = await _wizardDataController.DeleteMultipleAbsenceRecords(model);

            //Assert
            Assert.IsInstanceOf<OkResult>(result);

            //Verify
            _iwizardDataServicesMock.Verify(x => x.DeleteSelectedRowsFromDraftAndMainTable(rowIds, model.subsectionName), Times.Once);
        }

        [Test]
        public async Task DeleteMultipleAbsenceRecords_AbsenceWizardMultipleDeleteDto_ReturnsBadRequestObjectResult()
        {
            //Arrange
            var model = new AbsenceWizardMultipleDeleteDto
            {
                selectedRows = new List<AbsenceWizardDeleteRowDto>()
                {
                    new AbsenceWizardDeleteRowDto()
                    {
                        record_id = 1,
                        leaveTypeCode = "Demo Leave Type",
                        originalIndex = 1,
                        vgtSelected = true,
                        vgt_id=2
                    }
                },
                subsectionName = "Demo Subsection"
            };
            var rowIds = model.selectedRows.Select(x => x.record_id).ToList();
            var errorMesssage = "Some Error Occured";

            _iwizardDataServicesMock.Setup(x => x.DeleteSelectedRowsFromDraftAndMainTable(rowIds, model.subsectionName)).Throws(new Exception(errorMesssage)).Verifiable();

            //Act
            var result = await _wizardDataController.DeleteMultipleAbsenceRecords(model);

            //Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult.Value, Is.EqualTo(errorMesssage));

            //Verify
            _iwizardDataServicesMock.Verify(x => x.DeleteSelectedRowsFromDraftAndMainTable(rowIds, model.subsectionName), Times.Once);
            _loggerMock.Verify(x => x.Error(errorMesssage), Times.Once);
        }

        [Test]
        public async Task GetPendingLeaveTypes_ValidString_ReturnsOkObjectResult()
        {
            // Arrange
            var serverParams = "{ \"SortField\":\"exampleSortField\",\"SortType\":\"exampleSortType\",\"Page\":1,\"PerPage\":10,\"SearchText\":\"exampleSearchText\",\"Subsection\":\"exampleSubsection\",\"RowIds\":[1,2,3]}";
            var parameters = JsonConvert.DeserializeObject<AbsenceServerParamsDto>(serverParams);
            var expectedResult = new object();

            _iwizardDataServicesMock.Setup(x => x.GetPendingLeaveTypesData(It.IsAny<AbsenceServerParamsDto>())).ReturnsAsync(expectedResult).Verifiable();

            //Act
            var result = await _wizardDataController.GetPendingLeaveTypes(serverParams);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(expectedResult));

            //Verify
            _iwizardDataServicesMock.Verify(x => x.GetPendingLeaveTypesData(It.IsAny<AbsenceServerParamsDto>()), Times.Once);
        }

        [Test]
        public async Task GetPendingLeaveTypes_ValidString_ReturnsBadRequestObjectResult()
        {
            // Arrange
            var serverParams = "{ \"SortField\":\"exampleSortField\",\"SortType\":\"exampleSortType\",\"Page\":1,\"PerPage\":10,\"SearchText\":\"exampleSearchText\",\"Subsection\":\"exampleSubsection\",\"RowIds\":[1,2,3]}";
            var parameters = JsonConvert.DeserializeObject<AbsenceServerParamsDto>(serverParams);
            var errorMessage = "Some Error Occured";

            _iwizardDataServicesMock.Setup(x => x.GetPendingLeaveTypesData(It.IsAny<AbsenceServerParamsDto>())).Throws(new Exception(errorMessage)).Verifiable();

            //Act
            var result = await _wizardDataController.GetPendingLeaveTypes(serverParams);

            //Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult.Value, Is.EqualTo(errorMessage));

            //Verify
            _iwizardDataServicesMock.Verify(x => x.GetPendingLeaveTypesData(It.IsAny<AbsenceServerParamsDto>()), Times.Once);
            _loggerMock.Verify(x => x.Error(errorMessage), Times.Once);
        }

        [Test]
        public async Task UpdateDraftStatusForAbsence_AbsenceWizardDrftDto_ReturnsOkResult()
        {
            //Arrange
            var absenceWizardDrftDto = new AbsenceWizardDrftDto()
            {
                TableRowId = 1,
                SubsectionName = "Demo Subsection",
            };

            _iwizardDataServicesMock.Setup(x => x.UpdateDraftStatusForAbsence(absenceWizardDrftDto.TableRowId, absenceWizardDrftDto.SubsectionName)).Verifiable();

            //Act
            var result = await _wizardDataController.UpdateDraftStatusForAbsence(absenceWizardDrftDto);

            //Assert
            Assert.IsInstanceOf<OkResult>(result);

            //Verify
            _iwizardDataServicesMock.Verify(x => x.UpdateDraftStatusForAbsence(absenceWizardDrftDto.TableRowId, absenceWizardDrftDto.SubsectionName), Times.Once);
        }

        [Test]
        public async Task UpdateDraftStatusForAbsence_AbsenceWizardDrftDto_ReturnsBadRequestObjectResult()
        {
            //Arrange
            var absenceWizardDrftDto = new AbsenceWizardDrftDto()
            {
                TableRowId = 1,
                SubsectionName = "Demo Subsection",
            };
            var errorMesssage = "Some Error Occured";

            _iwizardDataServicesMock.Setup(x => x.UpdateDraftStatusForAbsence(absenceWizardDrftDto.TableRowId, absenceWizardDrftDto.SubsectionName)).Throws(new Exception(errorMesssage)).Verifiable();

            //Act
            var result = await _wizardDataController.UpdateDraftStatusForAbsence(absenceWizardDrftDto);

            //Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult.Value, Is.EqualTo(errorMesssage));

            //Verify
            _iwizardDataServicesMock.Verify(x => x.UpdateDraftStatusForAbsence(absenceWizardDrftDto.TableRowId, absenceWizardDrftDto.SubsectionName), Times.Once);
            _loggerMock.Verify(x => x.Error(errorMesssage), Times.Once);
        }

        [Test]
        public async Task GetAbsenceWizardPendingOptionsRowNumber_ValidString_ReturnsOkObjectResult()
        {
            //Arrange
            var subsectionName = "Demo Subsection";
            var expectedResult = new List<int>()
            {
                1,2,3
            };

            _iwizardDataServicesMock.Setup(x => x.GetAbsenceWizardPendingOptionsRowNumber(subsectionName)).ReturnsAsync(expectedResult).Verifiable();

            //Act
            var result = await _wizardDataController.GetAbsenceWizardPendingOptionsRowNumber(subsectionName);

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(expectedResult));

            //Verify
            _iwizardDataServicesMock.Verify(x => x.GetAbsenceWizardPendingOptionsRowNumber(subsectionName), Times.Once);
        }

        [Test]
        public async Task GetAbsenceWizardPendingOptionsRowNumber_ValidString_ReturnsBadRequestObjectResult()
        {
            //Arrange
            var subsectionName = "Demo Subsection";
            var errorMesssage = "Some Error Occured";

            _iwizardDataServicesMock.Setup(x => x.GetAbsenceWizardPendingOptionsRowNumber(subsectionName)).Throws(new Exception(errorMesssage)).Verifiable();

            //Act
            var result = await _wizardDataController.GetAbsenceWizardPendingOptionsRowNumber(subsectionName);

            //Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult.Value, Is.EqualTo(errorMesssage));

            //Verify
            _iwizardDataServicesMock.Verify(x => x.GetAbsenceWizardPendingOptionsRowNumber(subsectionName), Times.Once);
            _loggerMock.Verify(x => x.Error(errorMesssage), Times.Once);
        }


        [Test]
        public async Task GetIsDraftStatusForAbsence_ValidUserIdValidSubsectionName_ReturnsOkObjectResult()
        {
            //Arrange
            var userId = "Demo User";
            var subsectionName = "Demo Subsection";
            var expectedResult = new AbsenceWizardDrftStatusDto();

            _iwizardDataServicesMock.Setup(x => x.GetIsDraftStatusForAbsence(userId, subsectionName)).ReturnsAsync(expectedResult).Verifiable();

            //Act
            var result = await _wizardDataController.GetIsDraftStatusForAbsence(userId, subsectionName);

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(expectedResult));

            //Verify
            _iwizardDataServicesMock.Verify(x => x.GetIsDraftStatusForAbsence(userId, subsectionName), Times.Once);
        }

        [Test]
        public async Task GetIsDraftStatusForAbsence_ValidUserIdValidSubsectionName_ReturnsBadRequestObjectResult()
        {
            //Arrange
            var userId = "Demo User";
            var subsectionName = "Demo Subsection";
            var errorMesssage = "Some Error Occured";

            _iwizardDataServicesMock.Setup(x => x.GetIsDraftStatusForAbsence(userId, subsectionName)).Throws(new Exception(errorMesssage)).Verifiable();

            //Act
            var result = await _wizardDataController.GetIsDraftStatusForAbsence(userId, subsectionName);

            //Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult.Value, Is.EqualTo(errorMesssage));

            //Verify
            _iwizardDataServicesMock.Verify(x => x.GetIsDraftStatusForAbsence(userId, subsectionName), Times.Once);
            _loggerMock.Verify(x => x.Error(errorMesssage), Times.Once);
        }

        [Test]
        public async Task CheckForDuplicateLeaveTypeStatutoryLeave_ValidResponseTextValidUserId_ReturnsOkObjectResult()
        {
            //Arrange
            var responseText = "Demo Response";
            var userId = "Demo User";
            var expectedResult = false;

            _iwizardDataServicesMock.Setup(x => x.CheckForDuplicateLeaveTypeStatutoryLeave(responseText, userId)).ReturnsAsync(expectedResult).Verifiable();
            _iwizardDataServicesMock.Setup(x => x.CheckExistingLeaveTypeStatutoryLeave(responseText)).ReturnsAsync(expectedResult).Verifiable();

            //Act
            var result = await _wizardDataController.CheckForDuplicateLeaveTypeStatutoryLeave(responseText, userId);

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(expectedResult));

            //Verify
            _iwizardDataServicesMock.Verify(x => x.CheckForDuplicateLeaveTypeStatutoryLeave(responseText, userId), Times.Once);
            _iwizardDataServicesMock.Verify(x => x.CheckExistingLeaveTypeStatutoryLeave(responseText), Times.Once);
        }

        [Test]
        public async Task CheckForDuplicateLeaveTypeStatutoryLeave_ValidResponseTextValidUserId_ReturnsBadRequestObjectResult()
        {
            //Arrange
            var responseText = "Demo Response";
            var userId = "Demo User";
            var errorMesssage = "Some Error Occured";

            _iwizardDataServicesMock.Setup(x => x.CheckForDuplicateLeaveTypeStatutoryLeave(responseText, userId)).Throws(new Exception(errorMesssage)).Verifiable();

            //Act
            var result = await _wizardDataController.CheckForDuplicateLeaveTypeStatutoryLeave(responseText, userId);

            //Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult.Value, Is.EqualTo(errorMesssage));

            //Verify
            _iwizardDataServicesMock.Verify(x => x.CheckForDuplicateLeaveTypeStatutoryLeave(responseText, userId), Times.Once);
            _loggerMock.Verify(x => x.Error(errorMesssage), Times.Once);
        }

        [Test]
        public async Task CheckForDuplicateLeaveTypeShortLeave_ValidResponseTextValidUserId_ReturnsOkObjectResult()
        {
            //Arrange
            var responseText = "Demo Response";
            var userId = "Demo User";
            var expectedResult = false;

            _iwizardDataServicesMock.Setup(x => x.CheckForDuplicateLeaveTypeShortLeave(responseText, userId)).ReturnsAsync(expectedResult).Verifiable();
            _iwizardDataServicesMock.Setup(x => x.CheckExistingLeaveTypeShortLeave(responseText)).ReturnsAsync(expectedResult).Verifiable();

            //Act
            var result = await _wizardDataController.CheckForDuplicateLeaveTypeShortLeave(responseText, userId);

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(expectedResult));

            //Verify
            _iwizardDataServicesMock.Verify(x => x.CheckForDuplicateLeaveTypeShortLeave(responseText, userId), Times.Once);
            _iwizardDataServicesMock.Verify(x => x.CheckExistingLeaveTypeShortLeave(responseText), Times.Once);
        }

        [Test]
        public async Task CheckForDuplicateLeaveTypeShortLeave_ValidResponseTextValidUserId_ReturnsBadRequestObjectResult()
        {
            //Arrange
            var responseText = "Demo Response";
            var userId = "Demo User";
            var errorMesssage = "Some Error Occured";

            _iwizardDataServicesMock.Setup(x => x.CheckForDuplicateLeaveTypeShortLeave(responseText, userId)).Throws(new Exception(errorMesssage)).Verifiable();

            //Act
            var result = await _wizardDataController.CheckForDuplicateLeaveTypeShortLeave(responseText, userId);

            //Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult.Value, Is.EqualTo(errorMesssage));

            //Verify
            _iwizardDataServicesMock.Verify(x => x.CheckForDuplicateLeaveTypeShortLeave(responseText, userId), Times.Once);
            _loggerMock.Verify(x => x.Error(errorMesssage), Times.Once);
        }

        [Test]
        public async Task CheckForDuplicateLeaveTypeStatutoryLeaveFirst_ValidResponseText_ReturnsOkObjectResult()
        {
            //Arrange
            var responseText = "Demo Response";
            var expectedResult = false;

            _iwizardDataServicesMock.Setup(x => x.CheckForDuplicateLeaveTypeStatutoryLeaveFirst(responseText)).ReturnsAsync(expectedResult).Verifiable();
            _iwizardDataServicesMock.Setup(x => x.CheckExistingLeaveTypeStatutoryLeave(responseText)).ReturnsAsync(expectedResult).Verifiable();

            //Act
            var result = await _wizardDataController.CheckForDuplicateLeaveTypeStatutoryLeaveFirst(responseText);

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(expectedResult));

            //Verify
            _iwizardDataServicesMock.Verify(x => x.CheckForDuplicateLeaveTypeStatutoryLeaveFirst(responseText), Times.Once);
            _iwizardDataServicesMock.Verify(x => x.CheckExistingLeaveTypeStatutoryLeave(responseText), Times.Once);
        }

        [Test]
        public async Task CheckForDuplicateLeaveTypeStatutoryLeaveFirst_ValidResponseText_ReturnsBadRequestObjectResult()
        {
            //Arrange
            var responseText = "Demo Response";
            var errorMesssage = "Some Error Occured";

            _iwizardDataServicesMock.Setup(x => x.CheckForDuplicateLeaveTypeStatutoryLeaveFirst(responseText)).Throws(new Exception(errorMesssage)).Verifiable();

            //Act
            var result = await _wizardDataController.CheckForDuplicateLeaveTypeStatutoryLeaveFirst(responseText);

            //Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult.Value, Is.EqualTo(errorMesssage));

            //Verify
            _iwizardDataServicesMock.Verify(x => x.CheckForDuplicateLeaveTypeStatutoryLeaveFirst(responseText), Times.Once);
            _loggerMock.Verify(x => x.Error(errorMesssage), Times.Once);
        }

        [Test]
        public async Task CheckForDuplicateLeaveTypeShortLeaveFirst_ValidResponseText_ReturnsOkObjectResult()
        {
            //Arrange
            var responseText = "Demo Response";
            var expectedResult = false;

            _iwizardDataServicesMock.Setup(x => x.CheckForDuplicateLeaveTypeShortLeaveFirst(responseText)).ReturnsAsync(expectedResult).Verifiable();
            _iwizardDataServicesMock.Setup(x => x.CheckExistingLeaveTypeShortLeave(responseText)).ReturnsAsync(expectedResult).Verifiable();

            //Act
            var result = await _wizardDataController.CheckForDuplicateLeaveTypeShortLeaveFirst(responseText);

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(expectedResult));

            //Verify
            _iwizardDataServicesMock.Verify(x => x.CheckForDuplicateLeaveTypeShortLeaveFirst(responseText), Times.Once);
            _iwizardDataServicesMock.Verify(x => x.CheckExistingLeaveTypeShortLeave(responseText), Times.Once);
        }

        [Test]
        public async Task CheckForDuplicateLeaveTypeShortLeaveFirst_ValidResponseText_ReturnsBadRequestObjectResult()
        {
            //Arrange
            var responseText = "Demo Response";
            var errorMesssage = "Some Error Occured";

            _iwizardDataServicesMock.Setup(x => x.CheckForDuplicateLeaveTypeShortLeaveFirst(responseText)).Throws(new Exception(errorMesssage)).Verifiable();

            //Act
            var result = await _wizardDataController.CheckForDuplicateLeaveTypeShortLeaveFirst(responseText);

            //Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult.Value, Is.EqualTo(errorMesssage));

            //Verify
            _iwizardDataServicesMock.Verify(x => x.CheckForDuplicateLeaveTypeShortLeaveFirst(responseText), Times.Once);
            _loggerMock.Verify(x => x.Error(errorMesssage), Times.Once);
        }

        [Test]
        public async Task checkDuplicateApplySeqForStatutoryLeave_ValidResponseTextValidUserId_ReturnsOkObjectResult()
        {
            //Arrange
            var responseText = "Demo Response";
            var userId = "Demo User";
            var expectedResult = false;

            _iwizardDataServicesMock.Setup(x => x.checkDuplicateApplySeqForStatutoryLeave(responseText, userId)).ReturnsAsync(expectedResult).Verifiable();

            //Act
            var result = await _wizardDataController.checkDuplicateApplySeqForStatutoryLeave(responseText, userId);

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(expectedResult));

            //Verify
            _iwizardDataServicesMock.Verify(x => x.checkDuplicateApplySeqForStatutoryLeave(responseText, userId), Times.Once);
        }

        [Test]
        public async Task checkDuplicateApplySeqForStatutoryLeave_ValidResponseTextValidUserId_ReturnsBadRequestObjectResult()
        {
            //Arrange
            var responseText = "Demo Response";
            var userId = "Demo User";
            var errorMesssage = "Some Error Occured";

            _iwizardDataServicesMock.Setup(x => x.checkDuplicateApplySeqForStatutoryLeave(responseText, userId)).Throws(new Exception(errorMesssage)).Verifiable();

            //Act
            var result = await _wizardDataController.checkDuplicateApplySeqForStatutoryLeave(responseText, userId);

            //Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult.Value, Is.EqualTo(errorMesssage));

            //Verify
            _iwizardDataServicesMock.Verify(x => x.checkDuplicateApplySeqForStatutoryLeave(responseText, userId), Times.Once);
            _loggerMock.Verify(x => x.Error(errorMesssage), Times.Once);
        }

        [Test]
        public async Task CheckIfSmallerThanPreviousColumnValue_ValidResponseTextValidQuestionNoValidRowIdValidSubsectionName_ReturnsOkObjectResult()
        {
            //Arrange
            var responseText = "Demo Response";
            var questionNo = 1;
            var rowId = 1;
            var subsectionName = "Demo Subsection";
            var expectedResult = true;

            _iwizardDataServicesMock.Setup(x => x.CheckIfSmallerThanPreviousColumnValue(responseText, questionNo, rowId, subsectionName)).ReturnsAsync(expectedResult).Verifiable();

            //Act
            var result = await _wizardDataController.CheckIfSmallerThanPreviousColumnValue(responseText, questionNo, rowId, subsectionName);

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(expectedResult));

            //Verify
            _iwizardDataServicesMock.Verify(x => x.CheckIfSmallerThanPreviousColumnValue(responseText, questionNo, rowId, subsectionName), Times.Once);
        }

        [Test]
        public async Task CheckIfSmallerThanPreviousColumnValue_ValidResponseTextValidQuestionNoValidRowIdValidSubsectionName_ReturnsBadRequestObjectResult()
        {
            //Arrange
            var responseText = "Demo Response";
            var questionNo = 1;
            var rowId = 1;
            var subsectionName = "Demo Subsection";
            var errorMesssage = "Some Error Occured";

            _iwizardDataServicesMock.Setup(x => x.CheckIfSmallerThanPreviousColumnValue(responseText, questionNo, rowId, subsectionName)).Throws(new Exception(errorMesssage)).Verifiable();

            //Act
            var result = await _wizardDataController.CheckIfSmallerThanPreviousColumnValue(responseText, questionNo, rowId, subsectionName);

            //Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult.Value, Is.EqualTo(errorMesssage));

            //Verify
            _iwizardDataServicesMock.Verify(x => x.CheckIfSmallerThanPreviousColumnValue(responseText, questionNo, rowId, subsectionName), Times.Once);
            _loggerMock.Verify(x => x.Error(errorMesssage), Times.Once);
        }

        [Test]
        public async Task CheckIfPreviousColumnIsNullValueForStatLeave_ValidQuestionNoValidRowId_ReturnsOkObjectResult()
        {
            //Arrange
            var questionNo = 1;
            var rowId = 1;
            var expectedResult = true;

            _iwizardDataServicesMock.Setup(x => x.CheckIfPreviousColumnIsNotNullForStatLeave(questionNo, rowId)).ReturnsAsync(expectedResult).Verifiable();

            //Act
            var result = await _wizardDataController.CheckIfPreviousColumnIsNullValueForStatLeave(questionNo, rowId);

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(expectedResult));

            //Verify
            _iwizardDataServicesMock.Verify(x => x.CheckIfPreviousColumnIsNotNullForStatLeave(questionNo, rowId), Times.Once);
        }

        [Test]
        public async Task CheckIfPreviousColumnIsNullValueForStatLeave_ValidQuestionNoValidRowId_ReturnsBadRequestObjectResult()
        {
            //Arrange
            var questionNo = 1;
            var rowId = 1;
            var errorMesssage = "Some Error Occured";

            _iwizardDataServicesMock.Setup(x => x.CheckIfPreviousColumnIsNotNullForStatLeave(questionNo, rowId)).Throws(new Exception(errorMesssage)).Verifiable();

            //Act
            var result = await _wizardDataController.CheckIfPreviousColumnIsNullValueForStatLeave(questionNo, rowId);

            //Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult.Value, Is.EqualTo(errorMesssage));

            //Verify
            _iwizardDataServicesMock.Verify(x => x.CheckIfPreviousColumnIsNotNullForStatLeave(questionNo, rowId), Times.Once);
            _loggerMock.Verify(x => x.Error(errorMesssage), Times.Once);
        }

        [Test]
        public async Task CheckIfPreviousColumnIsNotNullForShortLeave_ValidQuestionNoValidRowId_ReturnsOkObjectResult()
        {
            //Arrange
            var questionNo = 1;
            var rowId = 1;
            var expectedResult = true;

            _iwizardDataServicesMock.Setup(x => x.CheckIfPreviousColumnIsNotNullForShortLeave(questionNo, rowId)).ReturnsAsync(expectedResult).Verifiable();

            //Act
            var result = await _wizardDataController.CheckIfPreviousColumnIsNotNullForShortLeave(questionNo, rowId);

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(expectedResult));

            //Verify
            _iwizardDataServicesMock.Verify(x => x.CheckIfPreviousColumnIsNotNullForShortLeave(questionNo, rowId), Times.Once);
        }


        [Test]
        public async Task CheckIfPreviousColumnIsNotNullForShortLeave_ValidQuestionNoValidRowId_ReturnsBadRequestObjectResult()
        {
            //Arrange
            var questionNo = 1;
            var rowId = 1;
            var errorMesssage = "Some Error Occured";

            _iwizardDataServicesMock.Setup(x => x.CheckIfPreviousColumnIsNotNullForShortLeave(questionNo, rowId)).Throws(new Exception(errorMesssage)).Verifiable();

            //Act
            var result = await _wizardDataController.CheckIfPreviousColumnIsNotNullForShortLeave(questionNo, rowId);

            //Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult.Value, Is.EqualTo(errorMesssage));

            //Verify
            _iwizardDataServicesMock.Verify(x => x.CheckIfPreviousColumnIsNotNullForShortLeave(questionNo, rowId), Times.Once);
            _loggerMock.Verify(x => x.Error(errorMesssage), Times.Once);
        }

        [Test]
        public async Task CheckIfColumnBeforeTheLastIsNotNullForShortLeave_ValidQuestionNoValidRowId_ReturnsOkObjectResult()
        {
            //Arrange
            var questionNo = 1;
            var rowId = 1;
            var expectedResult = true;

            _iwizardDataServicesMock.Setup(x => x.CheckIfColumnBeforeTheLastIsNotNullForShortLeave(questionNo, rowId)).ReturnsAsync(expectedResult).Verifiable();

            //Act
            var result = await _wizardDataController.CheckIfColumnBeforeTheLastIsNotNullForShortLeave(questionNo, rowId);

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(expectedResult));

            //Verify
            _iwizardDataServicesMock.Verify(x => x.CheckIfColumnBeforeTheLastIsNotNullForShortLeave(questionNo, rowId), Times.Once);
        }

        [Test]
        public async Task CheckIfColumnBeforeTheLastIsNotNullForShortLeave_ValidQuestionNoValidRowId_ReturnsBadRequestObjectResult()
        {
            //Arrange
            var questionNo = 1;
            var rowId = 1;
            var errorMesssage = "Some Error Occured";

            _iwizardDataServicesMock.Setup(x => x.CheckIfColumnBeforeTheLastIsNotNullForShortLeave(questionNo, rowId)).Throws(new Exception(errorMesssage)).Verifiable();

            //Act
            var result = await _wizardDataController.CheckIfColumnBeforeTheLastIsNotNullForShortLeave(questionNo, rowId);

            //Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult.Value, Is.EqualTo(errorMesssage));

            //Verify
            _iwizardDataServicesMock.Verify(x => x.CheckIfColumnBeforeTheLastIsNotNullForShortLeave(questionNo, rowId), Times.Once);
            _loggerMock.Verify(x => x.Error(errorMesssage), Times.Once);
        }


        [Test]
        public async Task checkIfPreviousTwoColumnsAreNotNullForShortLeave_ValidQuestionNoValidRowId_ReturnsOkObjectResult()
        {
            //Arrange
            var questionNo = 1;
            var rowId = 1;
            var expectedResult = true;

            _iwizardDataServicesMock.Setup(x => x.checkIfPreviousTwoColumnsAreNotNullForShortLeave(questionNo, rowId)).ReturnsAsync(expectedResult).Verifiable();

            //Act
            var result = await _wizardDataController.checkIfPreviousTwoColumnsAreNotNullForShortLeave(questionNo, rowId);

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(expectedResult));

            //Verify
            _iwizardDataServicesMock.Verify(x => x.checkIfPreviousTwoColumnsAreNotNullForShortLeave(questionNo, rowId), Times.Once);
        }

        [Test]
        public async Task checkIfPreviousTwoColumnsAreNotNullForShortLeave_ValidQuestionNoValidRowId_ReturnBadRequestObjectResult()
        {
            //Arrange
            var questionNo = 1;
            var rowId = 1;
            var errorMesssage = "Some Error Occured";

            _iwizardDataServicesMock.Setup(x => x.checkIfPreviousTwoColumnsAreNotNullForShortLeave(questionNo, rowId)).Throws(new Exception(errorMesssage)).Verifiable();

            //Act
            var result = await _wizardDataController.checkIfPreviousTwoColumnsAreNotNullForShortLeave(questionNo, rowId);

            //Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult.Value, Is.EqualTo(errorMesssage));

            //Verify
            _iwizardDataServicesMock.Verify(x => x.checkIfPreviousTwoColumnsAreNotNullForShortLeave(questionNo, rowId), Times.Once);
            _loggerMock.Verify(x => x.Error(errorMesssage), Times.Once);
        }

        [Test]
        public async Task CheckIfZeroOrNullValue_ValidQuestionNoValidRowIdValidSubsectionName_ReturnsOkObjectResult()
        {
            //Arrange
            var questionNo = 1;
            var rowId = 1;
            var subsectionName = "Demo Subsection";
            var expectedResult = true;

            _iwizardDataServicesMock.Setup(x => x.CheckIfZeroOrNullValue(questionNo, rowId, subsectionName)).ReturnsAsync(expectedResult).Verifiable();

            //Act
            var result = await _wizardDataController.CheckIfZeroOrNullValue(questionNo, rowId, subsectionName);

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(expectedResult));

            //Verify
            _iwizardDataServicesMock.Verify(x => x.CheckIfZeroOrNullValue(questionNo, rowId, subsectionName), Times.Once);
        }

        [Test]
        public async Task CheckIfZeroOrNullValue_ValidQuestionNoValidRowIdValidSubsectionName_ReturnsBadRequestObjectResult()
        {
            //Arrange
            var questionNo = 1;
            var rowId = 1;
            var subsectionName = "Demo Subsection";
            var errorMesssage = "Some Error Occured";

            _iwizardDataServicesMock.Setup(x => x.CheckIfZeroOrNullValue(questionNo, rowId, subsectionName)).Throws(new Exception(errorMesssage)).Verifiable();

            //Act
            var result = await _wizardDataController.CheckIfZeroOrNullValue(questionNo, rowId, subsectionName);

            //Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult.Value, Is.EqualTo(errorMesssage));

            //Verify
            _iwizardDataServicesMock.Verify(x => x.CheckIfZeroOrNullValue(questionNo, rowId, subsectionName), Times.Once);
            _loggerMock.Verify(x => x.Error(errorMesssage), Times.Once);
        }

        [Test]
        public async Task CheckIfGreaterThanMaximumForShortLeave_ValidResponsetextValidQuestionNoValidRowId_ReturnsOkObjectResult()
        {
            //Arrange
            var questionNo = 1;
            var rowId = 1;
            var responseText = "Demo response";
            var expectedResult = true;

            _iwizardDataServicesMock.Setup(x => x.CheckIfGreaterThanMaximumForShortLeave(responseText, questionNo, rowId)).ReturnsAsync(expectedResult).Verifiable();

            //Act
            var result = await _wizardDataController.CheckIfGreaterThanMaximumForShortLeave(responseText, questionNo, rowId);

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(expectedResult));

            //Verify
            _iwizardDataServicesMock.Verify(x => x.CheckIfGreaterThanMaximumForShortLeave(responseText, questionNo, rowId), Times.Once);
        }


        [Test]
        public async Task CheckIfGreaterThanMaximumForShortLeave_ValidResponsetextValidQuestionNoValidRowId_ReturnsBadRequestObjectResult()
        {
            //Arrange
            var questionNo = 1;
            var rowId = 1;
            var responseText = "Demo response";
            var errorMesssage = "Some Error Occured";

            _iwizardDataServicesMock.Setup(x => x.CheckIfGreaterThanMaximumForShortLeave(responseText, questionNo, rowId)).Throws(new Exception(errorMesssage)).Verifiable();

            //Act
            var result = await _wizardDataController.CheckIfGreaterThanMaximumForShortLeave(responseText, questionNo, rowId);

            //Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult.Value, Is.EqualTo(errorMesssage));

            //Verify
            _iwizardDataServicesMock.Verify(x => x.CheckIfGreaterThanMaximumForShortLeave(responseText, questionNo, rowId), Times.Once);
            _loggerMock.Verify(x => x.Error(errorMesssage), Times.Once);
        }

        [Test]
        public async Task CheckIfLessThanMaximumForShortLeave_ValidResponsetextValidQuestionNoValidRowId_ReturnsOkObjectResult()
        {
            //Arrange
            var questionNo = 1;
            var rowId = 1;
            var responseText = "Demo response";
            var expectedResult = true;

            _iwizardDataServicesMock.Setup(x => x.CheckIfLessThanMaximumForShortLeave(responseText, questionNo, rowId)).ReturnsAsync(expectedResult).Verifiable();

            //Act
            var result = await _wizardDataController.CheckIfLessThanMaximumForShortLeave(responseText, questionNo, rowId);

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(expectedResult));

            //Verify
            _iwizardDataServicesMock.Verify(x => x.CheckIfLessThanMaximumForShortLeave(responseText, questionNo, rowId), Times.Once);
        }

        [Test]
        public async Task CheckIfLessThanMaximumForShortLeave_ValidResponsetextValidQuestionNoValidRowId_ReturnsBadRequestObjectResult()
        {
            //Arrange
            var questionNo = 1;
            var rowId = 1;
            var responseText = "Demo response";
            var errorMesssage = "Some Error Occured";

            _iwizardDataServicesMock.Setup(x => x.CheckIfLessThanMaximumForShortLeave(responseText, questionNo, rowId)).Throws(new Exception(errorMesssage)).Verifiable();

            //Act
            var result = await _wizardDataController.CheckIfLessThanMaximumForShortLeave(responseText, questionNo, rowId);

            //Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult.Value, Is.EqualTo(errorMesssage));

            //Verify
            _iwizardDataServicesMock.Verify(x => x.CheckIfLessThanMaximumForShortLeave(responseText, questionNo, rowId), Times.Once);
            _loggerMock.Verify(x => x.Error(errorMesssage), Times.Once);
        }

        [Test]
        public async Task CheckIfLessThanMinimumForShortLeave_ValidResponsetextValidQuestionNoValidRowId_ReturnsOkObjectResult()
        {
            //Arrange
            var questionNo = 1;
            var rowId = 1;
            var responseText = "Demo response";
            var expectedResult = true;

            _iwizardDataServicesMock.Setup(x => x.CheckIfLessThanMinimumForShortLeave(responseText, questionNo, rowId)).ReturnsAsync(expectedResult).Verifiable();

            //Act
            var result = await _wizardDataController.CheckIfLessThanMinimumForShortLeave(responseText, questionNo, rowId);

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(expectedResult));

            //Verify
            _iwizardDataServicesMock.Verify(x => x.CheckIfLessThanMinimumForShortLeave(responseText, questionNo, rowId), Times.Once);
        }

        [Test]
        public async Task CheckIfLessThanMinimumForShortLeave_ValidResponsetextValidQuestionNoValidRowId_ReturnsBadRequestObjectResult()
        {
            //Arrange
            var questionNo = 1;
            var rowId = 1;
            var responseText = "Demo response";
            var errorMesssage = "Some Error Occured";

            _iwizardDataServicesMock.Setup(x => x.CheckIfLessThanMinimumForShortLeave(responseText, questionNo, rowId)).Throws(new Exception(errorMesssage)).Verifiable();

            //Act
            var result = await _wizardDataController.CheckIfLessThanMinimumForShortLeave(responseText, questionNo, rowId);

            //Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult.Value, Is.EqualTo(errorMesssage));

            //Verify
            _iwizardDataServicesMock.Verify(x => x.CheckIfLessThanMinimumForShortLeave(responseText, questionNo, rowId), Times.Once);
            _loggerMock.Verify(x => x.Error(errorMesssage), Times.Once);
        }
    }
}