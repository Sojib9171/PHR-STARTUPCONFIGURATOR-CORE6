using HBS.ImplementationAutomationConsole.Core.DataAccess.Data;
using HBS.ImplementationAutomationConsole.Core.DataAccess.IRepository;
using HBS.ImplementationAutomationConsole.Core.Models;
using System.Data;

namespace HBS.ImplementationAutomationConsole.Core.DataAccess.Repository
{
    public class ValidationRepository : IValidationRepository
    {
        private readonly IBaseDataAccess _iBaseDataAccess;
        public ValidationRepository(IBaseDataAccess iBaseDataAccess)
        {
            this._iBaseDataAccess = iBaseDataAccess;
        }

        public async Task ValidateUploadedData(string templateId)
        {
            string query = @"EXEC [SP_IA_VLDT_DATA] @TEMPLATE_ID";
            List<Param> @params = new List<Param>()
            {
                new Param{ SqlDbType = SqlDbType.VarChar, ParamName="@TEMPLATE_ID", SqlValue = templateId}
            };

            await _iBaseDataAccess.ExecuteQueryAsync(query,@params);
        }

        public async Task<List<int>> GetValidationCounts(string tableName)
        {
            string query = @"EXEC [SP_IA_GET_VALIDATION_OVERVIEW] @TABLE_NAME";

            List<Param> @params = new List<Param>()
            {
                new Param{ SqlDbType = SqlDbType.VarChar, ParamName="@TABLE_NAME", SqlValue = tableName}
            };
            using (var reader = await _iBaseDataAccess.ExecuteReaderAsync(query, @params))
            {
                List<int> counts = new List<int>();

                if (reader.Read())
                {
                    counts.Add(reader.GetInt32(0));
                    counts.Add(reader.GetInt32(1));
                    counts.Add(reader.GetInt32(2));
                }

                return counts;
            }
        }
        public async Task<List<ValidationDataModel>> GetValidationResult(string subsectionName)
        {
            string query = await getQueryStatementForValidationModels(subsectionName);

            using (var reader = await _iBaseDataAccess.ExecuteReaderAsync(query))
            {
                List<ValidationDataModel> models = new List<ValidationDataModel>();

                while (reader.Read())
                {
                    var model = new ValidationDataModel();
                    if (!reader.IsDBNull(0))
                    {
                        model.Identifier = reader.GetString(0);
                    }
                    else
                    {
                        model.Identifier = string.Empty;
                    }
                    model.TemplateID = reader.GetString(1);
                    model.ErrorDetails = reader.GetString(2);
                    model.ColumnNumber = reader.GetString(3);
                    model.ColumnName = reader.GetString(4);
                    model.RowNumber = reader.GetString(5);
                    model.ErrorID = reader.GetInt32(6);

                    models.Add(model);
                }

                return models;
            }
        }

        private async Task<string> getQueryStatementForValidationModels(string subsectionName)
        {
            switch (subsectionName)
            {
                case "Short Leave": 
                case "Statutory Leave":
                    return "EXEC [SP_IA_GET_VALDT_DATA_ABSENCE]";
                case "Shift Information":
                    return "EXEC [SP_IA_GET_VLDT_DATA_SHIFT]";
                case "Roster Information":
                    return "EXEC [SP_IA_GET_VLDT_DATA_ROSTER]";
                case "Employee Data":
                case "Bank Details":
                case "Reporting Hierarchy":
                case "Dependent Information":
                case "Emergency Contact":
                    return "EXEC [SP_IA_GET_VALIDATED_DATA]";
                default:
                    throw new Exception("Subsection Not Found");
            }
        }

        //public async Task<List<ValidationDataModel>> GetValidationResultAbsence()
        //{
        //    string query = @"EXEC [SP_IA_GET_VALDT_DATA_ABSENCE]";

        //    using (var reader = await _iBaseDataAccess.ExecuteReaderAsync(query))
        //    {
        //        List<ValidationDataModel> models = new List<ValidationDataModel>();

        //        while (reader.Read())
        //        {
        //            var model = new ValidationDataModel();
        //            if (!reader.IsDBNull(0))
        //            {
        //                model.Identifier = reader.GetString(0);
        //            }
        //            else
        //            {
        //                model.Identifier = string.Empty;
        //            }
        //            model.TemplateID = reader.GetString(1);
        //            model.ErrorDetails = reader.GetString(2);
        //            model.ColumnNumber = reader.GetString(3);
        //            model.ColumnName = reader.GetString(4);
        //            model.RowNumber = reader.GetString(5);
        //            model.ErrorID = reader.GetInt32(6);

        //            models.Add(model);
        //        }

        //        return models;
        //    }
        //}

        //public async Task<List<ValidationDataModel>> GetValidationResultRoster()
        //{
        //    string query = @"EXEC [SP_IA_GET_VLDT_DATA_ROSTER]";

        //    using (var reader = await _iBaseDataAccess.ExecuteReaderAsync(query))
        //    {
        //        List<ValidationDataModel> models = new List<ValidationDataModel>();

        //        while (reader.Read())
        //        {
        //            var model = new ValidationDataModel();
        //            if (!reader.IsDBNull(0))
        //            {
        //                model.Identifier = reader.GetString(0);
        //            }
        //            else
        //            {
        //                model.Identifier = string.Empty;
        //            }
        //            model.TemplateID = reader.GetString(1);
        //            model.ErrorDetails = reader.GetString(2);
        //            model.ColumnNumber = reader.GetString(3);
        //            model.ColumnName = reader.GetString(4);
        //            model.RowNumber = reader.GetString(5);
        //            model.ErrorID = reader.GetInt32(6);

        //            models.Add(model);
        //        }
        //        return models;
        //    }
        //}

    }
}