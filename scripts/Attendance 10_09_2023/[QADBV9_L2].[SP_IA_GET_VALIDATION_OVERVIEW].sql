CREATE OR ALTER PROCEDURE [QADBV9_L2].[SP_IA_GET_VALIDATION_OVERVIEW]
@TABLE_NAME NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;
	DECLARE @totalRowCount INT;
	DECLARE @successRowCount INT;
	DECLARE @errorRowCount INT;
	DECLARE @sql NVARCHAR(MAX);

	if @TABLE_NAME = 'HS_HR_IA_SHORT_LEAVE_UPLOAD' OR @TABLE_NAME = 'HS_HR_IA_STATUTRY_LEAVE_UPLOAD'
	Begin
	
	SET @sql = 'SELECT @totalRowCount = COUNT(*) FROM ' + QUOTENAME(@TABLE_NAME);

	EXECUTE sp_executesql @sql, N'@totalRowCount INT OUTPUT', @totalRowCount OUTPUT


    SELECT @errorRowCount = Count(DISTINCT ROW_NUMBER) from HS_HR_IA_ERRORS_ABSENCE;
    SET @successRowCount = @totalRowCount - @errorRowCount;

    SELECT @totalRowCount, @successRowCount,@errorRowCount;

	end

	ELSE IF  @TABLE_NAME = 'HS_HR_IA_ROSTER_INFO_UPLOAD'
    BEGIN

	SET @sql = 'SELECT @totalRowCount = COUNT(*) FROM ' + QUOTENAME(@TABLE_NAME);

	EXECUTE sp_executesql @sql, N'@totalRowCount INT OUTPUT', @totalRowCount OUTPUT


    SELECT @errorRowCount = Count(DISTINCT ROW_NUMBER) from HS_HR_IA_ERRORS_ROSTER;
    SET @successRowCount = @totalRowCount - @errorRowCount;

    SELECT @totalRowCount, @successRowCount,@errorRowCount;

	end

	ELSE IF  @TABLE_NAME = 'HS_HR_IA_SHIFT_INFO_UPLOAD'
    BEGIN

	SET @sql = 'SELECT @totalRowCount = COUNT(*) FROM ' + QUOTENAME(@TABLE_NAME);

	EXECUTE sp_executesql @sql, N'@totalRowCount INT OUTPUT', @totalRowCount OUTPUT


    SELECT @errorRowCount = Count(DISTINCT ROW_NUMBER) from HS_HR_IA_ERRORS_SHIFT;
    SET @successRowCount = @totalRowCount - @errorRowCount;

    SELECT @totalRowCount, @successRowCount,@errorRowCount;

	end

	ELSE
    BEGIN

	SET @sql = 'SELECT @totalRowCount = COUNT(*) FROM ' + QUOTENAME(@TABLE_NAME);

	EXECUTE sp_executesql @sql, N'@totalRowCount INT OUTPUT', @totalRowCount OUTPUT


    SELECT @errorRowCount = Count(DISTINCT ROW_NUMBER) from HS_HR_IA_ERRORS;
    SET @successRowCount = @totalRowCount - @errorRowCount;

    SELECT @totalRowCount, @successRowCount,@errorRowCount;

	end
END;
GO