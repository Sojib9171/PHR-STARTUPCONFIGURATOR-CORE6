DELETE FROM [HS_HR_IA_PARAMETERS];
GO

DROP TABLE [HS_HR_IA_PARAMETERS];
GO

CREATE TABLE [HS_HR_IA_PARAMETERS](
	[PARA_NAME] [nvarchar](50) NOT NULL,
	[PARA_VALUE] [nvarchar](400) NULL,
PRIMARY KEY CLUSTERED 
(
	[PARA_NAME] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


INSERT INTO [HS_HR_IA_PARAMETERS]
           ([PARA_NAME]
           ,[PARA_VALUE])
     VALUES
           ('PHR_CONFIG_ASSIST_COPYRIGHTS'
           ,'PeoplesHR, 2023')
GO

ALTER TABLE HS_HR_IA_USERS ADD HAS_USER_LOGGED_IN_BEFORE bit;
GO

CREATE OR ALTER PROCEDURE [SP_IA_CHK_USER_LOG_IN_BEFORE]
@USER_ID varchar(20)
AS
BEGIN

SELECT HAS_USER_LOGGED_IN_BEFORE from [HS_HR_IA_USERS] U where U.[USER_ID] = @USER_ID

END
GO

CREATE OR ALTER PROCEDURE [SP_IA_UPDT_USER_LOG_IN_BEFORE]
@USER_ID varchar(20)
AS
BEGIN

UPDATE [HS_HR_IA_USERS]
SET HAS_USER_LOGGED_IN_BEFORE = 1
WHERE USER_ID = @USER_ID AND (HAS_USER_LOGGED_IN_BEFORE <> 1 OR HAS_USER_LOGGED_IN_BEFORE IS NULL);

END
GO

CREATE OR ALTER   PROCEDURE [SP_AI_UPLOAD_SHIFT_INFO]
AS 
BEGIN
DECLARE @SFTCodeCal VARCHAR(25)
DECLARE @SFTDefaultCal VARCHAR(25)

BEGIN TRY  
--Get SFT code
SET @SFTCodeCal = RIGHT('000000' + CAST((SELECT ISNULL(MAX(SFT_CODE), 0) FROM HS_TA_SHIFTDEF) AS VARCHAR(25)), 6)

--Get SFT Default value
SET @SFTDefaultCal = 
    CASE 
        WHEN @SFTCodeCal = '000001' THEN 1
        ELSE 0
    END;

-- Insert Shift (Basic Shift)
INSERT INTO [HS_TA_SHIFTDEF]
           ([SFT_CODE]
		   ,[SFT_DIS_CODE]
		   ,[SFT_ABBRV]
		   ,[SFT_LEAVEAMOUNT]
		   ,[SFT_MIDNIGHT]
		   ,[SFT_FLXSHIFT]
		   ,[SFT_MAX_CNT_SHIFTS]
		   ,[SFT_CONTINUE]
		   ,[SFT_ACTIVE]
		   ,[SFT_OFFSHIFT]
		   ,[SFT_OT_AUTO_FIX]
		   ,[SFT_MULSHIFTS]
		   ,[SFT_SPECIAL]
		   ,[SFT_COLUR]
		   ,[SFT_TIMEBASE]
		   ,[SFT_AUTO_MIDNIGHT]
		   ,[SFT_DEFAULT]
		   ,[DBGROUP_ID]
		   ,[SFT_LTECOVER]
		   ,[SFT_IS_LATECAL]
		   ,[SFT_DEMPBRK_HRS_FRMOT]
		   ,[SFT_FLEXIHRS])
SELECT RIGHT('000000'+CAST(@SFTCodeCal + ROW_NUMBER() OVER (ORDER BY [ID]) AS varchar(25)),6),
SG.SHIFT_NAME, SG.SHIFT_ABR, SG.LEAVE_DAYS, (CASE WHEN SG.FLEXI_SHIFT = 1 THEN 0 ELSE SG.NEXT_DAY_SHIFT_OUT_TIME END), (CASE WHEN SG.FLEXI_SHIFT IS NULL THEN 0 ELSE SG.FLEXI_SHIFT END), 0, SG.CONTINUE_SHIFT, 1, SG.OFF_SHIFT,
0, 0, 0, '', 1, SG.AUTO_MID_NIGHT_FIX, @SFTDefaultCal, NULL, SG.LATE_COVER, SG.BRK_ALLOW_LATE_HRS_CALC, SG.ALLOW_DEDCT_OUT_HRS, (CASE WHEN SG.FLEXI_SHIFT = 1 THEN (SG.END_TIME - SG.START_TIME)  ELSE 0.00 END)
FROM  HS_HR_IA_SHIFT_INFO_UPLOAD SG

--Insert Sift Segment (Basic Shift Segment)
INSERT INTO [HS_TA_SHIFTDEF_SEGMENT]
           ([SFT_CODE]
		   ,[SEG_NAME]
		   ,[SEG_SEQUENCE]
		   ,[SEG_TIMEIN]
		   ,[SEG_TIMEOUT]
		   ,[SEG_MIDIN_HRS]
		   ,[SEG_MIDOUT_HRS]
		   ,[SEG_ST_CUTHRS]
		   ,[SEG_ENDOUT_CUTHRS]
		   ,[SEG_TIMEIN_DAY]
		   ,[SEG_TIMEOUT_DAY]
		   ,[SEG_MIN_OT_HRS]
		   ,[SEG_MAX_OT_HRS]
		   ,[SEG_SFT_COLOR]
		   ,[SEG_MIN_POST_OT_HRS]
		   ,[SEG_MAX_POST_OT_HRS])
SELECT RIGHT('000000'+CAST(@SFTCodeCal + ROW_NUMBER() OVER (ORDER BY [ID]) AS varchar(25)),6), 
SG.SHIFT_NAME, 1, (CASE WHEN SG.FLEXI_SHIFT = 1 THEN 0.00  ELSE SG.START_TIME END), (CASE WHEN SG.FLEXI_SHIFT = 1 THEN 0.00  ELSE SG.END_TIME END), SG.FIRST_HALF_DUR, SG.SECOND_HALF_DUR, SG.START_TIME, SG.END_TIME , 1, (CASE WHEN SG.NEXT_DAY_SHIFT_OUT_TIME = 1 THEN 2  ELSE 1 END),
10.0, 0.0, '#3f6b98', 10.0, 0.0
FROM  HS_HR_IA_SHIFT_INFO_UPLOAD SG

--Insert Day Basic Shift
INSERT INTO [HS_TA_SHIFTDEF_DAY_CONFIG]
           ([SFT_CODE]
		   ,[HT_CODE]
		   ,[SFT_MIDNIGHT]
		   ,[SFT_FLXSHIFT]
		   ,[SFT_CONTINUE]
		   ,[SFT_OFFSHIFT]
		   ,[SFT_COLUR]
		   ,[SFT_TIMEBASE]
		   ,[SFT_TIMEIN]
		   ,[SFT_TIMEOUT]
		   ,[SFT_MIDIN_HRS]
		   ,[SFT_MIDOUT_HRS]
		   ,[SFT_LEAVEAMOUNT]
		   ,[SFT_AUTO_MIDNIGHT]
		   ,[SFT_ST_CUTHRS]
		   ,[SFT_ENDOUT_CUTHRS]
		   ,[SFT_TIMEIN_DAY]
		   ,[SFT_TIMEOUT_DAY]
		   ,[SFT_MAX_OT_HRS]
		   ,[SFT_MIN_OT_HRS]
		   ,[SFT_MAX_POST_OT_HRS]
		   ,[SFT_MIN_POST_OT_HRS]
		   ,[SFT_SOT_ENBL]
		   ,[SFT_EOT_ENBL]
		   ,[SFT_LTECOVER]
		   ,[SFT_DEMPBRK_HRS_FRMOT]
		   ,[SFT_FLEXIHRS])
SELECT RIGHT('000000'+CAST(@SFTCodeCal + ROW_NUMBER() OVER (ORDER BY [ID]) AS varchar(25)),6), 
0, (CASE WHEN SG.FLEXI_SHIFT = 1 THEN 0 ELSE SG.NEXT_DAY_SHIFT_OUT_TIME END), (CASE WHEN SG.FLEXI_SHIFT IS NULL THEN 0 ELSE SG.FLEXI_SHIFT END), SG.CONTINUE_SHIFT, SG.OFF_SHIFT, '#3f6b98', 1, 
(CASE WHEN SG.FLEXI_SHIFT = 1 THEN 0.00  ELSE SG.START_TIME END), (CASE WHEN SG.FLEXI_SHIFT = 1 THEN 0.00  ELSE SG.END_TIME END), SG.FIRST_HALF_DUR, SG.SECOND_HALF_DUR, SG.LEAVE_DAYS, SG.AUTO_MID_NIGHT_FIX, 
SG.START_TIME, SG.END_TIME , 1,
(CASE WHEN SG.NEXT_DAY_SHIFT_OUT_TIME = 1 THEN 2  ELSE 1 END),
10.0, 0.0, 10.0, 0.0, 1, 1, SG.LATE_COVER, SG.ALLOW_DEDCT_OUT_HRS, (CASE WHEN SG.FLEXI_SHIFT = 1 THEN (SG.END_TIME - SG.START_TIME)  ELSE 0.00 END)
FROM  HS_HR_IA_SHIFT_INFO_UPLOAD SG

select 'True' as status , 'Successfully added' as message
END TRY  
BEGIN CATCH
	INSERT INTO [HS_HR_IA_ERROR_LOGS]
			   ([ERROR_LINE]
			   ,[ERROR_MESSAGE]
			   ,[ERROR_NUMBER]
			   ,[ERROR_PROCEDURE]
			   ,[ERROR_SEVERITY]
			   ,[ERROR_STATE]
			   ,[ERROR_DATE])
	SELECT  
	ERROR_LINE () as ErrorLine,  
	Error_Message() as ErrorMessage,  
	Error_Number() as ErrorNumber,  
	Error_Procedure() as 'Proc',  
	Error_Severity() as ErrorSeverity,  
	Error_State() as ErrorState,  
	GETDATE () as DateErrorRaised 

	SELECT  
		'False' as status  
       ,ERROR_MESSAGE() AS message;  
END CATCH  

END
GO

ALTER     PROCEDURE [SP_IA_VALIDATION_SHORT_UPLD]
@TEMPLATE_ID NVARCHAR(MAX)
AS
BEGIN
--SET NOCOUNT ON;

select h.TBT_CODE, ROW_NUMBER() over (order by ID) as ROW_NUM
into #DataWithRowNumber
from HS_HR_IA_SHORT_LEAVE_UPLOAD h


SELECT i.COLUMN_NAME, i.ORDINAL_POSITION as COL_NUM
into #ColNameWithColNumber
FROM INFORMATION_SCHEMA.COLUMNS i
WHERE TABLE_NAME = 'HS_HR_IA_SHORT_LEAVE_UPLOAD'
group by i.ORDINAL_POSITION, i.COLUMN_NAME

DELETE FROM HS_HR_IA_ERRORS_ABSENCE;
DBCC CHECKIDENT ('[HS_HR_IA_ERRORS_ABSENCE]', RESEED, 0);

--For "TBT_CODE" No 1
insert into HS_HR_IA_ERRORS_ABSENCE
select TBT_CODE, @TEMPLATE_ID, 'Please specify a valid Short Leave Type Code with a character length of 6 or less', '', 'TBT_CODE','' from HS_HR_IA_SHORT_LEAVE_UPLOAD
where TBT_CODE IS NULL;

insert into HS_HR_IA_ERRORS_ABSENCE
select TBT_CODE, @TEMPLATE_ID, 'Please specify a valid Short Leave Type Code with a character length of 6 or less', '', 'TBT_CODE','' from HS_HR_IA_SHORT_LEAVE_UPLOAD
where TBT_CODE IS NOT NULL AND len(TBT_CODE) > 6;

insert into HS_HR_IA_ERRORS_ABSENCE
select TBT_CODE, @TEMPLATE_ID, 'Short Leave Type Code already exists', '', 'TBT_CODE','' from HS_HR_IA_SHORT_LEAVE_UPLOAD
where exists(SELECT 1
    FROM HS_HR_LTIMEBASE_TYPE
    WHERE HS_HR_IA_SHORT_LEAVE_UPLOAD.TBT_CODE = HS_HR_LTIMEBASE_TYPE.TBT_CODE);

--For "Bank Code" No 2
insert into HS_HR_IA_ERRORS_ABSENCE
select TBT_CODE, @TEMPLATE_ID, 'Please specify a valid Leave Type with a character length of 18 or less', '', 'LEAVE_TYPE_NAME','' from HS_HR_IA_SHORT_LEAVE_UPLOAD
where LEAVE_TYPE_NAME IS NULL;

insert into HS_HR_IA_ERRORS_ABSENCE
select TBT_CODE, @TEMPLATE_ID, 'Please specify a valid Leave Type with a character length of 18 or less', '', 'LEAVE_TYPE_NAME','' from HS_HR_IA_SHORT_LEAVE_UPLOAD
where LEAVE_TYPE_NAME IS NOT NULL AND len(LEAVE_TYPE_NAME) > 18;

--For "MIN_HOURS_ALLOWED" No 3

insert into HS_HR_IA_ERRORS_ABSENCE
select TBT_CODE, @TEMPLATE_ID, 'Please specify a valid value for Minimum hours allowed (0 to 23)', '', 'MIN_HOURS_ALLOWED','' from HS_HR_IA_SHORT_LEAVE_UPLOAD
where MIN_HOURS_ALLOWED IS NULL;

insert into HS_HR_IA_ERRORS_ABSENCE
select TBT_CODE, @TEMPLATE_ID, 'Please specify a valid value for Minimum hours allowed (0 to 23)', '', 'MIN_HOURS_ALLOWED','' from HS_HR_IA_SHORT_LEAVE_UPLOAD
where MIN_HOURS_ALLOWED < 0 OR MIN_HOURS_ALLOWED > 23;


--For "Maximum hours allowed" No 4
insert into HS_HR_IA_ERRORS_ABSENCE
select TBT_CODE, @TEMPLATE_ID, 'Please specify a valid value for Maximum hours allowed (0 to 23)', '', 'MAX_HOURS_ALLOWED','' from HS_HR_IA_SHORT_LEAVE_UPLOAD
where MAX_HOURS_ALLOWED IS NULL 

insert into HS_HR_IA_ERRORS_ABSENCE
select TBT_CODE, @TEMPLATE_ID, 'Please specify a valid value for Maximum hours allowed (0 to 23)', '', 'MAX_HOURS_ALLOWED','' from HS_HR_IA_SHORT_LEAVE_UPLOAD
where MAX_HOURS_ALLOWED < MIN_HOURS_ALLOWED;

insert into HS_HR_IA_ERRORS_ABSENCE
select TBT_CODE, @TEMPLATE_ID, 'Please specify a valid value for Maximum hours allowed (0 to 23)', '', 'MAX_HOURS_ALLOWED','' from HS_HR_IA_SHORT_LEAVE_UPLOAD
where MAX_HOURS_ALLOWED < 0 OR MAX_HOURS_ALLOWED > 23;


--For "Maximum Occurrence" No 5
insert into HS_HR_IA_ERRORS_ABSENCE
select TBT_CODE, @TEMPLATE_ID, 'Please specify a valid value for Maximum Occurrence (greater than or equal to zero)', '', 'MAX_OCCURRENCE','' from HS_HR_IA_SHORT_LEAVE_UPLOAD
where MAX_OCCURRENCE IS NULL 

insert into HS_HR_IA_ERRORS_ABSENCE
select TBT_CODE, @TEMPLATE_ID, 'Please specify a valid value for Maximum Occurrence (greater than or equal to zero)', '', 'MAX_OCCURRENCE','' from HS_HR_IA_SHORT_LEAVE_UPLOAD
where MAX_OCCURRENCE < 0 OR MAX_OCCURRENCE>99;


--For "Maximum consecutive hours allowed per application" No 6

insert into HS_HR_IA_ERRORS_ABSENCE
select TBT_CODE, @TEMPLATE_ID, 'Please specify a valid value for Maximum consecutive hours allowed per application (equal or greater than the maximum hours allowed)', '', 'MAX_CONSECUTIVE_HOURS_ALLOWED','' from HS_HR_IA_SHORT_LEAVE_UPLOAD
where MAX_CONSECUTIVE_HOURS_ALLOWED IS NOT NULL AND (TRY_CAST(LTRIM(RTRIM(MAX_CONSECUTIVE_HOURS_ALLOWED)) AS FLOAT) < 0 OR TRY_CAST(LTRIM(RTRIM(MAX_CONSECUTIVE_HOURS_ALLOWED)) AS FLOAT) > 23);

insert into HS_HR_IA_ERRORS_ABSENCE
select TBT_CODE, @TEMPLATE_ID, 'Please specify a valid value for Maximum consecutive hours allowed per application (equal or greater than the maximum hours allowed)', '', 'MAX_CONSECUTIVE_HOURS_ALLOWED','' from HS_HR_IA_SHORT_LEAVE_UPLOAD
where TRY_CAST(LTRIM(RTRIM(MAX_CONSECUTIVE_HOURS_ALLOWED)) AS FLOAT) < MAX_HOURS_ALLOWED;

insert into HS_HR_IA_ERRORS_ABSENCE
select TBT_CODE, @TEMPLATE_ID, 'Please specify a valid value for Maximum consecutive hours allowed per application (equal or greater than the maximum hours allowed)', '', 'MAX_CONSECUTIVE_HOURS_ALLOWED','' from HS_HR_IA_SHORT_LEAVE_UPLOAD
where MAX_CONSECUTIVE_HOURS_ALLOWED IS NULL AND TBT_ISCONSEC_ALL IS NOT NULL;

--For "Consider All Leave Types" No 7

insert into HS_HR_IA_ERRORS_ABSENCE
select TBT_CODE, @TEMPLATE_ID, 'Please select either 0 or 1 for Consider All Leave Types', '', 'TBT_ISCONSEC_ALL','' from HS_HR_IA_SHORT_LEAVE_UPLOAD
where TBT_ISCONSEC_ALL NOT IN(0,1);

insert into HS_HR_IA_ERRORS_ABSENCE
select TBT_CODE, @TEMPLATE_ID, 'Please select either 0 or 1 for Consider All Leave Types', '', 'TBT_ISCONSEC_ALL','' from HS_HR_IA_SHORT_LEAVE_UPLOAD
where TBT_ISCONSEC_ALL IS NULL AND MAX_CONSECUTIVE_HOURS_ALLOWED IS NOT NULL;

--For "Maximum No. of Leave per Day" No 8
insert into HS_HR_IA_ERRORS_ABSENCE
select TBT_CODE, @TEMPLATE_ID, 'Please specify a valid value between 0 and 99 for Maximum No. of Leave per Day', '', 'MAX_NO_OF_LEAVE_PER_DAY','' from HS_HR_IA_SHORT_LEAVE_UPLOAD
where MAX_NO_OF_LEAVE_PER_DAY IS NOT NULL AND len(MAX_NO_OF_LEAVE_PER_DAY) > 5

--For "Leave Time Period (Hours)" No 9
insert into HS_HR_IA_ERRORS_ABSENCE
select TBT_CODE, @TEMPLATE_ID, 'Please specify a valid numeric value between 0 and 23 for Leave Time Period (Hours)', '', 'LEAVE_TIME_PERIOD','' from HS_HR_IA_SHORT_LEAVE_UPLOAD
where LEAVE_TIME_PERIOD IS NOT NULL AND (LEAVE_TIME_PERIOD <0 OR LEAVE_TIME_PERIOD >23) ;

insert into HS_HR_IA_ERRORS_ABSENCE
select TBT_CODE, @TEMPLATE_ID, 'Please specify a valid numeric value between 0 and 23 for Leave Time Period (Hours)', '', 'LEAVE_TIME_PERIOD','' from HS_HR_IA_SHORT_LEAVE_UPLOAD
where LEAVE_TIME_PERIOD IS NOT NULL AND (DURATION_BEGIN_SHIFT IS NOT NULL OR DURATION_END_SHIFT IS NOT NULL);


--For "Duration per Slot - Beginning of Shift (Hours)" No 10

insert into HS_HR_IA_ERRORS_ABSENCE
select TBT_CODE, @TEMPLATE_ID, 'Please specify a valid numeric value between 0 and 23 for Duration per Slot - Beginning of Shift (Hours) (zero or more) and greater than or equal to the minimum hour', '', 'DURATION_BEGIN_SHIFT','' from HS_HR_IA_SHORT_LEAVE_UPLOAD
where DURATION_BEGIN_SHIFT < 0  OR DURATION_BEGIN_SHIFT >23 

insert into HS_HR_IA_ERRORS_ABSENCE
select TBT_CODE, @TEMPLATE_ID, 'Please specify a valid numeric value between 0 and 23 for Duration per Slot - Beginning of Shift (Hours) (zero or more) and greater than or equal to the minimum hour', '', 'DURATION_BEGIN_SHIFT','' from HS_HR_IA_SHORT_LEAVE_UPLOAD
where  DURATION_BEGIN_SHIFT < MIN_HOURS_ALLOWED;

insert into HS_HR_IA_ERRORS_ABSENCE
select TBT_CODE, @TEMPLATE_ID, 'Leave Time Periods cannot be used with Short Leave Duration', '', 'DURATION_BEGIN_SHIFT','' from HS_HR_IA_SHORT_LEAVE_UPLOAD
where  LEAVE_TIME_PERIOD IS NOT NULL AND DURATION_BEGIN_SHIFT IS NOT NULL;

insert into HS_HR_IA_ERRORS_ABSENCE
select TBT_CODE, @TEMPLATE_ID, 'Please specify a valid numeric value between 0 and 23 for Duration per Slot - Beginning of Shift (Hours) (zero or more) and greater than or equal to the minimum hour', '', 'DURATION_BEGIN_SHIFT','' from HS_HR_IA_SHORT_LEAVE_UPLOAD
where  LEAVE_TIME_PERIOD IS NULL AND (DURATION_BEGIN_SHIFT IS NULL AND DURATION_END_SHIFT IS NOT NULL);

insert into HS_HR_IA_ERRORS_ABSENCE
select TBT_CODE, @TEMPLATE_ID, 'Please specify a valid numeric value between 0 and 23 for Duration per Slot - Beginning of Shift (Hours) (zero or more) and greater than or equal to the minimum hour', '', 'DURATION_BEGIN_SHIFT','' from HS_HR_IA_SHORT_LEAVE_UPLOAD
where DURATION_BEGIN_SHIFT IS NULL AND DURATION_END_SHIFT IS NOT NULL;

--For "Duration per Slot - End of Shift (Hours)" No 11

insert into HS_HR_IA_ERRORS_ABSENCE
select TBT_CODE, @TEMPLATE_ID, 'Please specify a valid value for Duration per Slot - End of Shift (Hours) (zero or more) and less than or equal to the maximum allowed', '', 'DURATION_END_SHIFT','' from HS_HR_IA_SHORT_LEAVE_UPLOAD
where DURATION_END_SHIFT < 0 OR DURATION_END_SHIFT > 23

insert into HS_HR_IA_ERRORS_ABSENCE
select TBT_CODE, @TEMPLATE_ID, 'Please specify a valid value for Duration per Slot - End of Shift (Hours) (zero or more) and less than or equal to the maximum allowed', '', 'DURATION_END_SHIFT','' from HS_HR_IA_SHORT_LEAVE_UPLOAD
where DURATION_END_SHIFT > MAX_HOURS_ALLOWED;

insert into HS_HR_IA_ERRORS_ABSENCE
select TBT_CODE, @TEMPLATE_ID, 'Leave Time Periods cannot use with Short Leave Duration', '', 'DURATION_END_SHIFT','' from HS_HR_IA_SHORT_LEAVE_UPLOAD
where  LEAVE_TIME_PERIOD IS NOT NULL AND DURATION_END_SHIFT IS NOT NULL;

insert into HS_HR_IA_ERRORS_ABSENCE
select TBT_CODE, @TEMPLATE_ID, 'Please specify a valid value for Duration per Slot - End of Shift (Hours) (zero or more) and less than or equal to the maximum allowed', '', 'DURATION_END_SHIFT','' from HS_HR_IA_SHORT_LEAVE_UPLOAD
where LEAVE_TIME_PERIOD IS NOT NULL AND (DURATION_BEGIN_SHIFT IS NOT NULL AND DURATION_END_SHIFT IS NULL);

insert into HS_HR_IA_ERRORS_ABSENCE
select TBT_CODE, @TEMPLATE_ID, 'Please specify a valid value for Duration per Slot - End of Shift (Hours) (zero or more) and less than or equal to the maximum allowed', '', 'DURATION_END_SHIFT','' from HS_HR_IA_SHORT_LEAVE_UPLOAD
where DURATION_BEGIN_SHIFT IS NOT NULL AND DURATION_END_SHIFT IS NULL;

--For "Maximum Hours Per Month (If applicable for month)" No 12

insert into HS_HR_IA_ERRORS_ABSENCE
select TBT_CODE, @TEMPLATE_ID, 'Please specify a valid numeric value between 0 and 999 for Maximum Hours Per Month', '', 'MAX_HOURS_PER_MONTH','' from HS_HR_IA_SHORT_LEAVE_UPLOAD
where MAX_HOURS_PER_MONTH IS NOT NULL AND (MAX_HOURS_PER_MONTH < 0 OR MAX_HOURS_PER_MONTH > 999);

insert into HS_HR_IA_ERRORS_ABSENCE
select TBT_CODE, @TEMPLATE_ID, 'Please specify a valid numeric value between 0 and 999 for Maximum Hours Per Month', '', 'MAX_HOURS_PER_MONTH','' from HS_HR_IA_SHORT_LEAVE_UPLOAD
where  (MAX_HOURS_PER_MONTH is not null and  MAX_HOURS_PER_YEAR is not null and  APPLICABLE_FOR_WEEKLY is not null) OR
(MAX_HOURS_PER_MONTH is null  and  MAX_HOURS_PER_YEAR is not null and  APPLICABLE_FOR_WEEKLY is not null) OR
(MAX_HOURS_PER_MONTH is not null and  MAX_HOURS_PER_YEAR is null and  APPLICABLE_FOR_WEEKLY is not null) OR
(MAX_HOURS_PER_MONTH is not null and  MAX_HOURS_PER_YEAR is not null and  APPLICABLE_FOR_WEEKLY is null);

--For "Maximum Hours Per Year (If applicable for Year)" No 13

insert into HS_HR_IA_ERRORS_ABSENCE
select TBT_CODE, @TEMPLATE_ID, 'Please specify a valid numeric value between 0 and 999 for Maximum Hours Per Year', '', 'MAX_HOURS_PER_YEAR','' from HS_HR_IA_SHORT_LEAVE_UPLOAD
where MAX_HOURS_PER_MONTH IS NOT NULL AND (MAX_HOURS_PER_MONTH < 0 OR MAX_HOURS_PER_MONTH > 999)

insert into HS_HR_IA_ERRORS_ABSENCE
select TBT_CODE, @TEMPLATE_ID, 'Please specify a valid numeric value between 0 and 999 for Maximum Hours Per Year', '', 'MAX_HOURS_PER_YEAR','' from HS_HR_IA_SHORT_LEAVE_UPLOAD
where  (MAX_HOURS_PER_MONTH is not null and  MAX_HOURS_PER_YEAR is not null and  APPLICABLE_FOR_WEEKLY is not null) OR
(MAX_HOURS_PER_MONTH is null  and  MAX_HOURS_PER_YEAR is not null and  APPLICABLE_FOR_WEEKLY is not null) OR
(MAX_HOURS_PER_MONTH is not null and  MAX_HOURS_PER_YEAR is null and  APPLICABLE_FOR_WEEKLY is not null) OR
(MAX_HOURS_PER_MONTH is not null and  MAX_HOURS_PER_YEAR is not null and  APPLICABLE_FOR_WEEKLY is null);

--For "Applicable for weekly" No 14

insert into HS_HR_IA_ERRORS_ABSENCE
select TBT_CODE, @TEMPLATE_ID, 'Please select either 0 or 1 for Applicable for weekly', '', 'APPLICABLE_FOR_WEEKLY','' from HS_HR_IA_SHORT_LEAVE_UPLOAD
where APPLICABLE_FOR_WEEKLY NOT IN(0,1);

insert into HS_HR_IA_ERRORS_ABSENCE
select TBT_CODE, @TEMPLATE_ID, 'Please select either 0 or 1 for Applicable for weekly', '', 'APPLICABLE_FOR_WEEKLY','' from HS_HR_IA_SHORT_LEAVE_UPLOAD
where  (MAX_HOURS_PER_MONTH is not null and  MAX_HOURS_PER_YEAR is not null and  APPLICABLE_FOR_WEEKLY is not null) OR
(MAX_HOURS_PER_MONTH is null  and  MAX_HOURS_PER_YEAR is not null and  APPLICABLE_FOR_WEEKLY is not null) OR
(MAX_HOURS_PER_MONTH is not null and  MAX_HOURS_PER_YEAR is null and  APPLICABLE_FOR_WEEKLY is not null) OR
(MAX_HOURS_PER_MONTH is not null and  MAX_HOURS_PER_YEAR is not null and  APPLICABLE_FOR_WEEKLY is null);

--For "Leave Need Approval " No 18
insert into HS_HR_IA_ERRORS_ABSENCE
select TBT_CODE, @TEMPLATE_ID, 'Please specify a valid value for Comments with maximum of 200 characters', '', 'COMMENTS','' from HS_HR_IA_SHORT_LEAVE_UPLOAD
where COMMENTS IS NOT NULL AND len(COMMENTS) > 200

update E set E.COL_NUMBER = c.COL_NUM, E.ROW_NUMBER = d.ROW_NUM
from HS_HR_IA_ERRORS_ABSENCE E
inner join #DataWithRowNumber d on d.TBT_CODE = E.CODE OR (E.CODE IS NULL AND d.TBT_CODE IS NULL)
inner join #ColNameWithColNumber c on c.COLUMN_NAME = E.COL_NAME

select * into #tempTable1 from #DataWithRowNumber where TBT_CODE IS NULL ORDER by (SELECT NULL) OFFSET 1 rows;

INSERT INTO HS_HR_IA_ERRORS_ABSENCE (CODE,TEMPLATE_ID,ERROR_DETAILS,COL_NUMBER,COL_NAME,ROW_NUMBER) SELECT TBT_CODE,@TEMPLATE_ID,'Short Leave Type Code Can Not Be Null', '2','TBT_CODE', ROW_NUM from #tempTable1;

SELECT * into #tempTable2
FROM #DataWithRowNumber
WHERE TBT_CODE IN (
    SELECT TBT_CODE
    FROM #DataWithRowNumber
    GROUP BY TBT_CODE
    HAVING COUNT(*) > 1
) ORDER BY (SELECT NULL) OFFSET 1 ROWS;

INSERT INTO HS_HR_IA_ERRORS_ABSENCE (CODE,TEMPLATE_ID,ERROR_DETAILS,COL_NUMBER,COL_NAME,ROW_NUMBER) SELECT TBT_CODE,@TEMPLATE_ID,'Short Leave Type Code is duplicated', '2','SHORT_LEAVE_TYPE_CODE', ROW_NUM from #tempTable2;

drop table #ColNameWithColNumber;
drop table #DataWithRowNumber;
drop table #tempTable1;
drop table #tempTable2;

END
GO

ALTER  PROCEDURE [SP_IA_VALIDATION_STATUTRY_UPLD]
@TEMPLATE_ID NVARCHAR(MAX)
AS
BEGIN

select h.LEAVE_TYPE, ROW_NUMBER() over (order by ID) as ROW_NUM
into #DataWithRowNumber
from HS_HR_IA_STATUTRY_LEAVE_UPLOAD h

SELECT i.COLUMN_NAME, i.ORDINAL_POSITION as COL_NUM
into #ColNameWithColNumber
FROM INFORMATION_SCHEMA.COLUMNS i
WHERE TABLE_NAME = 'HS_HR_IA_STATUTRY_LEAVE_UPLOAD'
group by i.ORDINAL_POSITION, i.COLUMN_NAME

DELETE FROM HS_HR_IA_ERRORS_ABSENCE;
DBCC CHECKIDENT ('[HS_HR_IA_ERRORS_ABSENCE]', RESEED, 0);


--For "[LEAVE_TYPE] " No 1
insert into HS_HR_IA_ERRORS_ABSENCE
select [LEAVE_TYPE], @TEMPLATE_ID, 'Please Specify a valid value for Leave Type with a maximum of 6 characters', '', 'LEAVE_TYPE','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where [LEAVE_TYPE] IS NULL;

insert into HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please Specify a valid value for Leave Type with a maximum of 6 characters', '', 'LEAVE_TYPE','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where LEAVE_TYPE IS NOT NULL AND len(LEAVE_TYPE) > 6;

insert into HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Leave Type already exists', '', 'LEAVE_TYPE','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where exists(SELECT 1
    FROM HS_HR_LEAVE_TYPE
    WHERE HS_HR_IA_STATUTRY_LEAVE_UPLOAD.LEAVE_TYPE = HS_HR_LEAVE_TYPE.LEV_TYPE_CODE);


--For "LEAVE_DESCRIPTION" No 2

insert into HS_HR_IA_ERRORS_ABSENCE
select [LEAVE_TYPE], @TEMPLATE_ID, 'Please Specify a valid value for Leave Description with a maximum of 100 characters', '', 'LEAVE_DESCRIPTION','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where LEAVE_DESCRIPTION IS NULL;

insert into HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please Specify a valid value for Leave Description with a maximum of 100 characters', '', 'LEAVE_DESCRIPTION','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where LEAVE_DESCRIPTION IS NOT NULL AND len(LEAVE_TYPE) > 100;



--For "ACTIVE" No 3

insert into HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please select either 0 or 1  for Active', '', 'ACTIVE','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where ACTIVE NOT IN(0,1);


--For "COVERING_EMP_REQ" No 4 
insert into HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please Specify a valid value for Covering Emp. Required', '', 'COVERING_EMP_REQ','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where COVERING_EMP_REQ is not null and COVERING_EMP_REQ NOT IN (0,1);

--For "ALLOW_HALF_DAYS" No 5
insert into HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please select either 0 or 1 for Allow Half Days', '', 'ALLOW_HALF_DAYS','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where ALLOW_HALF_DAYS is not null and ALLOW_HALF_DAYS  NOT IN (0,1);

--For "AUTO_CAL_END_DATE" No 6 
insert into HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please select either 0 or 1  for Auto Calculate End Date', '', 'AUTO_CAL_END_DATE','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where AUTO_CAL_END_DATE is not null and AUTO_CAL_END_DATE  NOT IN (0,1);

--For "HOLIDAY_AS_LEAVE" No 7
insert into HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please select either 0 or 1  for Holiday as Leave', '', 'HOLIDAY_AS_LEAVE','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where HOLIDAY_AS_LEAVE NOT IN (0,1);

--For "LIEU_LEAVE" No 8
insert into HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please select either 0 or 1  for Lieu Leave', '', 'LIEU_LEAVE','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where LIEU_LEAVE NOT IN (0,1);

--For "MATERNITY_LEAVE" No 9
insert into HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please select either 0 or 1 for Maternity Leave', '', 'MATERNITY_LEAVE','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where MATERNITY_LEAVE NOT IN (0,1);

--For "NO_PAY_LEAVE" No 10
insert into HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please select either 0 or 1  for No Pay Leave', '', 'NO_PAY_LEAVE','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where NO_PAY_LEAVE NOT IN (0,1);

--For "QUARTER_LEAVE" No 11
insert into HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please select either 0 or 1  for Quarter Leave', '', 'QUARTER_LEAVE','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where QUARTER_LEAVE NOT IN (0,1);


--For "SHOW_BALANCE" No 12
insert into HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please select either 0 or 1  for Show Balance', '', 'SHOW_BALANCE','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where SHOW_BALANCE NOT IN (0,1);

--For "EARNED_CF_BREAK_DOWN" No 13
insert into HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please select either 0 or 1  for Earned/CF Break Down', '', 'EARNED_CF_BREAK_DOWN','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where EARNED_CF_BREAK_DOWN NOT IN (0,1);


--For "EXTENDED_PERIOD" No 14
insert into HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please select either 0 or 1  for Extended Period', '', 'EXTENDED_PERIOD','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where EXTENDED_PERIOD NOT IN (0,1);

--For "ALLOW_PLANNING" No 15
insert into HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please select either 0 or 1  for Allow Planning', '', 'ALLOW_PLANNING','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where ALLOW_PLANNING NOT IN (0,1);



--For "ATTACHMENT_REQUIRED" No 16
insert into HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please select either 0 or 1  for Attachment Required', '', 'ATTACHMENT_REQUIRED','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where ATTACHMENT_REQUIRED NOT IN (0,1);

--For "VIEW_ADDITIONAL_FIELD" No 17
insert into HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please select either 0 or 1  for View Additional Field', '', 'VIEW_ADDITIONAL_FIELD','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where VIEW_ADDITIONAL_FIELD NOT IN (0,1);


--For "ALLOW_NEGATIVE" No 18
insert into HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please select either 0 or 1 for Allow Negative', '', 'ALLOW_NEGATIVE','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where ALLOW_NEGATIVE NOT IN (0,1);

--For "KIOSK_ALLOWED" No 19
insert into HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please select either 0 or 1  for Kiosk Allowed', '', 'KIOSK_ALLOWED','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where KIOSK_ALLOWED NOT IN (0,1);


--For "HIDE_ZERO_ENTITLEMENT" No 20 
insert into HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please select either 0 or 1 for Hide Zero Entitlement', '', 'HIDE_ZERO_ENTITLEMENT','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where HIDE_ZERO_ENTITLEMENT NOT IN (0,1);

--For "ALLOW_SELECT_APPROVER" No 21
insert into HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please select either 0 or 1 for Allow Select Approver', '', 'ALLOW_SELECT_APPROVER','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where ALLOW_SELECT_APPROVER NOT IN (0,1);


--For "SHOW_LEAVE_REASON" No 22
insert into HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please select either 0 or 1  for Show Leave Reason', '', 'SHOW_LEAVE_REASON','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where SHOW_LEAVE_REASON NOT IN (0,1);

--For "CONSIDER_IN_DASHBOARD" No 23
insert into HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please select either 0 or 1 for Consider in Dashboard', '', 'CONSIDER_IN_DASHBOARD','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where CONSIDER_IN_DASHBOARD NOT IN (0,1);


--For "SHOW_IN_OFFBOARDING" No 24 
insert into HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please select either 0 or 1 for Show in Offboarding', '', 'SHOW_IN_OFFBOARDING','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where SHOW_IN_OFFBOARDING NOT IN (0,1);

--For "CONSIDER_FOR_TIME_LOST" No 25
insert into HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please select either 0 or 1  for Consider for Time Lost', '', 'CONSIDER_FOR_TIME_LOST','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where CONSIDER_FOR_TIME_LOST NOT IN (0,1);


--For "COMMENT_MANDATORY" No 26
insert into HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please select either 0 or 1 for Comment Mandatory', '', 'COMMENT_MANDATORY','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where COMMENT_MANDATORY NOT IN (0,1);

--For "LONG_LEAVE" No 27
insert into HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please select either 0 or 1 for Long Leave', '', 'LONG_LEAVE','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where LONG_LEAVE NOT IN (0,1);


--For "HIDE_IN_SELF_APP" No 28
insert into HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please select either 0 or 1 for Hide in Self Application', '', 'HIDE_IN_SELF_APP','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where HIDE_IN_SELF_APP NOT IN (0,1);


--For "APPLY_SEQUENCE_IN_DASH" No 29
insert into HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please specify a unique and non-zero value for Apply Sequence in Dashboard and maximum of 1 digits are allowed', '', 'APPLY_SEQUENCE_IN_DASH','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where APPLY_SEQUENCE_IN_DASH IS NOT NULL AND len(APPLY_SEQUENCE_IN_DASH) > 1

insert into HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please specify a unique and non-zero value for Apply Sequence in Dashboard and maximum of 1 digits are allowed', '', 'APPLY_SEQUENCE_IN_DASH','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where APPLY_SEQUENCE_IN_DASH IS NOT NULL AND APPLY_SEQUENCE_IN_DASH=0;

insert into HS_HR_IA_ERRORS_ABSENCE 
SELECT LEAVE_TYPE, 'temp7', 'Please specify a unique and non-zero value for Apply Sequence in Dashboard and maximum of 1 digits are allowed', '', 'APPLY_SEQUENCE_IN_DASH',''
FROM HS_HR_IA_STATUTRY_LEAVE_UPLOAD
WHERE APPLY_SEQUENCE_IN_DASH IN (
    SELECT APPLY_SEQUENCE_IN_DASH
    FROM HS_HR_IA_STATUTRY_LEAVE_UPLOAD
    GROUP BY APPLY_SEQUENCE_IN_DASH
    HAVING COUNT(*) > 1
)



--For "LEV_TYPE_MAX_DAYS" No 30 
insert into HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please specify a valid numeric value between 1 and 365 for Maximum Leave Days', '', 'LEV_TYPE_MAX_DAYS','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where LEV_TYPE_MAX_DAYS IS NOT NULL AND (LEV_TYPE_MAX_DAYS <1 OR LEV_TYPE_MAX_DAYS >365);

--For "YEARLY_ENTITLEMENT" No 4 
insert into HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please specify a valid numeric value between 1 and 365 for Yearly Entitlement (Days)', '', 'YEARLY_ENTITLEMENT','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where  YEARLY_ENTITLEMENT IS NOT NULL AND (LEV_TYPE_MAX_DAYS <1 OR LEV_TYPE_MAX_DAYS >365);

--For "MINIMUM_DAYS_IN_SNGLE_LEA_APP" No 31
insert into HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please specify a valid numeric value between 1 and 365 for for Minimum days in a single leave application', '', 'MINIMUM_DAYS_IN_SNGLE_LEA_APP','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where MINIMUM_DAYS_IN_SNGLE_LEA_APP <= 0 OR MINIMUM_DAYS_IN_SNGLE_LEA_APP >365;


insert into HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please specify a valid numeric value between 1 and 365 for Minimum days in a single leave application', '', 'MINIMUM_DAYS_IN_SNGLE_LEA_APP','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where MINIMUM_DAYS_IN_SNGLE_LEA_APP IS NULL AND MAXIMUM_DAYS_IN_SNGLE_LEA_APP IS NOT NULL;

--For "MAXIMUM_DAYS_IN_SNGLE_LEA_APP" No 32

insert into HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please specify a valid numeric value between 1 and 365 for Maximum days in a single leave application', '', 'MAXIMUM_DAYS_IN_SNGLE_LEA_APP','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where  MAXIMUM_DAYS_IN_SNGLE_LEA_APP IS NOT NULL AND (MAXIMUM_DAYS_IN_SNGLE_LEA_APP <1 OR MAXIMUM_DAYS_IN_SNGLE_LEA_APP >365);

insert into HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please specify a valid numeric value between 1 and 365 for Maximum days in a single leave application', '', 'MAXIMUM_DAYS_IN_SNGLE_LEA_APP','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where  MAXIMUM_DAYS_IN_SNGLE_LEA_APP < MINIMUM_DAYS_IN_SNGLE_LEA_APP;

insert into HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please specify a valid numeric value between 1 and 365 for Maximum days in a single leave application', '', 'MAXIMUM_DAYS_IN_SNGLE_LEA_APP','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where MAXIMUM_DAYS_IN_SNGLE_LEA_APP IS NULL AND MINIMUM_DAYS_IN_SNGLE_LEA_APP IS NOT NULL;



--For "REQ_MEDICAL_CERTIFICATE" No 32 
insert into HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please specify a valid numeric value between 1 and 365 for Require medical certificate after how many number of absence days', '', 'REQ_MEDICAL_CERTIFICATE','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where  REQ_MEDICAL_CERTIFICATE is not null and (REQ_MEDICAL_CERTIFICATE  < 1 OR REQ_MEDICAL_CERTIFICATE  > 365);

--For "APPLY_TGTHER_WITH_LEA_TYPE" No 33
insert into HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please specify a valid value for Specify leave types not allowed to apply together with this leave type from the drop-down. A maximum 50 Characters length is allowed', '', 'APPLY_TGTHER_WITH_LEA_TYPE','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where  APPLY_TGTHER_WITH_LEA_TYPE IS NOT NULL AND len(APPLY_TGTHER_WITH_LEA_TYPE) > 50

--For "FUTURE_LEAVE_UTILIZED_AS" No 34

insert into HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please specify a valid value for Future Leave Utilized As from the drop-down. A maximum 50 Characters length is allowed', '', 'FUTURE_LEAVE_UTILIZED_AS','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where  FUTURE_LEAVE_UTILIZED_AS IS NOT NULL AND len(FUTURE_LEAVE_UTILIZED_AS) > 50

insert into HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please specify a valid value for Future Leave Utilized As from the drop-down. A maximum 50 Characters length is allowed', '', 'FUTURE_LEAVE_UTILIZED_AS','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where  FUTURE_LEAVE_UTILIZED_AS IS NULL AND UTILIZATION_PERIOD IS NOT NULL;

--For "UTILIZATION_PERIOD" No 35 

insert into HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please specify a valid numeric value between 1 and 12 for Utilization Period (Months)', '', 'UTILIZATION_PERIOD','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where  UTILIZATION_PERIOD IS NOT NULL AND len(UTILIZATION_PERIOD) > 4;

insert into HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please specify a valid numeric value between 1 and 12 for Utilization Period (Months)', '', 'UTILIZATION_PERIOD','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where  FUTURE_LEAVE_UTILIZED_AS IS NOT NULL AND (UTILIZATION_PERIOD IS NULL OR UTILIZATION_PERIOD<=0);

insert into HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please specify a valid numeric value between 1 and 12 for Utilization Period (Months))', '', 'UTILIZATION_PERIOD','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where UTILIZATION_PERIOD < 1 OR UTILIZATION_PERIOD > 12;

insert into HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please specify a valid numeric value between 1 and 12 for Utilization Period (Months)', '', 'UTILIZATION_PERIOD','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where  FUTURE_LEAVE_UTILIZED_AS IS NOT NULL AND UTILIZATION_PERIOD IS NULL;


-- For Future Leave Type Name
insert into HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please specify a valid value for If balance carry forward as future leave, what is the name of the future leave type from the drop-down. A maximum 50 Characters length is allowed', '', 'FUTURE_LEAVE_TYPE_NAME','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where  FUTURE_LEAVE_TYPE_NAME IS NULL AND (LEV_TYPE_CFWD_YEARLY IS NOT NULL OR LEV_TYPE_CFWD_LIMIT IS NOT NULL);



--For "LEV_TYPE_CFWD_YEARLY" No 37 
insert into HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please specify a valid numeric value between 1 and 365 for Maximum allowed to carry forward as future leave', '', 'LEV_TYPE_CFWD_YEARLY','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where  LEV_TYPE_CFWD_YEARLY IS NOT NULL AND (LEV_TYPE_CFWD_YEARLY < 1 AND LEV_TYPE_CFWD_YEARLY > 365);

insert into HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please specify a valid numeric value between 1 and 365 for Maximum allowed to carry forward as future leave', '', 'LEV_TYPE_CFWD_YEARLY','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where  LEV_TYPE_CFWD_LIMIT IS NOT NULL AND LEV_TYPE_CFWD_YEARLY IS NULL;

insert into HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please specify a valid numeric value between 1 and 365 for Maximum allowed to carry forward as future leave', '', 'LEV_TYPE_CFWD_YEARLY','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where  MAXIMUM_DAYS_IN_SNGLE_LEA_APP IS NOT NULL AND (LEV_TYPE_CFWD_YEARLY IS NULL OR LEV_TYPE_CFWD_YEARLY =0);

insert into HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please specify a valid numeric value between 1 and 365 for Maximum allowed to carry forward as future leave', '', 'LEV_TYPE_CFWD_YEARLY','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where  LEV_TYPE_CFWD_YEARLY IS NOT NULL AND (LEV_TYPE_CFWD_LIMIT IS NULL OR FUTURE_LEAVE_TYPE_NAME IS NULL);

--For "LEV_TYPE_CFWD_LIMIT" No 38

insert into HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please specify a valid numeric value between 1 and 365 for What is the carry forward maximum limit(Ceiling limit)', '', 'LEV_TYPE_CFWD_LIMIT','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where  LEV_TYPE_CFWD_LIMIT IS NOT NULL AND (LEV_TYPE_CFWD_LIMIT < 1 OR LEV_TYPE_CFWD_LIMIT >365);

insert into HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please specify a valid numeric value between 1 and 365 for What is the carry forward maximum limit(Ceiling limit)', '', 'LEV_TYPE_CFWD_LIMIT','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where  MAXIMUM_DAYS_IN_SNGLE_LEA_APP IS NOT NULL AND (LEV_TYPE_CFWD_LIMIT IS NULL OR LEV_TYPE_CFWD_LIMIT=0);

insert into HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please specify a valid numeric value between 1 and 365 for What is the carry forward maximum limit(Ceiling limit)', '', 'LEV_TYPE_CFWD_LIMIT','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where  FUTURE_LEAVE_UTILIZED_AS IS NOT NULL AND (LEV_TYPE_CFWD_LIMIT IS NULL OR LEV_TYPE_CFWD_LIMIT=0);

insert into HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please specify a valid numeric value between 1 and 365 for What is the carry forward maximum limit(Ceiling limit)', '', 'LEV_TYPE_CFWD_LIMIT','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where  LEV_TYPE_CFWD_LIMIT IS NOT NULL AND (LEV_TYPE_CFWD_YEARLY IS NULL OR FUTURE_LEAVE_TYPE_NAME IS NULL);

--For "NOTICE_PERIOD" No 39 
insert into HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please specify a valid numeric value between 1 and 365 for If notice required, notice Period in days', '', 'NOTICE_PERIOD','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where  NOTICE_PERIOD IS NOT NULL AND (NOTICE_PERIOD < 1 OR NOTICE_PERIOD >365);

insert into HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please specify a valid numeric value between 1 and 365 for If notice required, notice Period in days', '', 'NOTICE_PERIOD','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where  NOTICE_PERIOD IS NOT NULL AND NOTICE_PERIOD = 0

--For "ENTITLEMENT_CRITERIA" No 44

insert into HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please specify a valid value for Entitlement Criteria from the drop-down. A maximum 100 Characters length is allowed', '', 'ENTITLEMENT_CRITERIA','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where  ENTITLEMENT_CRITERIA IS NULL;

insert into HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please specify a valid value for Entitlement Criteria from the drop-down. A maximum 100 Characters length is allowed', '', 'ENTITLEMENT_CRITERIA','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where  ENTITLEMENT_CRITERIA IS NOT NULL AND len(ENTITLEMENT_CRITERIA) > 100

--For "COMMENTS" No 45 

insert into HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please specify a valid value for Comments with a maximum of 200 characters', '', 'COMMENTS','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where  COMMENTS IS NOT NULL AND len(COMMENTS) > 200


update E set E.COL_NUMBER = c.COL_NUM, E.ROW_NUMBER = d.ROW_NUM
from HS_HR_IA_ERRORS_ABSENCE E
inner join #DataWithRowNumber d on d.LEAVE_TYPE = E.CODE OR (E.CODE IS NULL AND d.LEAVE_TYPE IS NULL)
inner join #ColNameWithColNumber c on c.COLUMN_NAME = E.COL_NAME


select * into #tempTable1 from #DataWithRowNumber where LEAVE_TYPE IS NULL ORDER by (SELECT NULL) OFFSET 1 rows;

INSERT INTO HS_HR_IA_ERRORS_ABSENCE (CODE,TEMPLATE_ID,ERROR_DETAILS,COL_NUMBER,COL_NAME,ROW_NUMBER) SELECT LEAVE_TYPE,@TEMPLATE_ID,'Leave Type Can Not Be Null', '2','SHORT_LEAVE_TYPE_CODE', ROW_NUM from #tempTable1;

SELECT * into #tempTable2
FROM #DataWithRowNumber
WHERE LEAVE_TYPE IN (
    SELECT LEAVE_TYPE
    FROM #DataWithRowNumber
    GROUP BY LEAVE_TYPE
    HAVING COUNT(*) > 1
) ORDER BY (SELECT NULL) OFFSET 1 ROWS;

INSERT INTO HS_HR_IA_ERRORS_ABSENCE (CODE,TEMPLATE_ID,ERROR_DETAILS,COL_NUMBER,COL_NAME,ROW_NUMBER) SELECT LEAVE_TYPE,@TEMPLATE_ID,'Leave Type is duplicated', '2','SHORT_LEAVE_TYPE_CODE', ROW_NUM from #tempTable2;

drop table #ColNameWithColNumber;
drop table #DataWithRowNumber;
drop table #tempTable1;
drop table #tempTable2;

END
GO

ALTER PROCEDURE [SP_SHORT_LEAVE_UPLOAD]
AS 
BEGIN

DECLARE @SHORTLEAVETYPECODE AS VARCHAR(100)
DECLARE @LEAVETYPENAME AS VARCHAR(100)
DECLARE @MINHOURSALLOWED AS FLOAT
DECLARE @MAXHOURSALLOWED AS FLOAT
DECLARE @MAXOCCURRENCE AS FLOAT
DECLARE @MAXCONSECUTIVEHOURSALLOWED AS CHAR(18)
DECLARE @MAXNOOFLEAVEPERDAY AS INT
DECLARE @LEAVETIMEPERIOD AS FLOAT
DECLARE @MAXOURSPERMONTH AS NUMERIC(5,2)
DECLARE @MAXHOURSPERYEAR AS NUMERIC(5,2)

DECLARE @TBT_ISCONSEC_ALL AS INT

DECLARE @TBT_PREDEFINED_ONE AS NUMERIC(5,2)
DECLARE @TBT_PREDEFINED_TWO AS NUMERIC(5,2)
 


DECLARE @CursorAttribute CURSOR SET @CursorAttribute = CURSOR FAST_FORWARD
FOR

SELECT TBT_CODE,LEAVE_TYPE_NAME,MIN_HOURS_ALLOWED,MAX_HOURS_ALLOWED,MAX_OCCURRENCE,MAX_CONSECUTIVE_HOURS_ALLOWED,MAX_NO_OF_LEAVE_PER_DAY,LEAVE_TIME_PERIOD,ISNULL(MAX_HOURS_PER_MONTH, 0) ,ISNULL(MAX_HOURS_PER_YEAR,0),TBT_ISCONSEC_ALL,DURATION_BEGIN_SHIFT,DURATION_END_SHIFT
FROM HS_HR_IA_SHORT_LEAVE_UPLOAD

OPEN @CursorAttribute
FETCH NEXT
FROM @CursorAttribute
INTO @SHORTLEAVETYPECODE
	,@LEAVETYPENAME
	,@MINHOURSALLOWED
	,@MAXHOURSALLOWED
	,@MAXOCCURRENCE 
	,@MAXCONSECUTIVEHOURSALLOWED 
	,@MAXNOOFLEAVEPERDAY 
	,@LEAVETIMEPERIOD 
	,@MAXOURSPERMONTH 
	,@MAXHOURSPERYEAR 
	
	
	,@TBT_ISCONSEC_ALL

	,@TBT_PREDEFINED_ONE
	,@TBT_PREDEFINED_TWO

	
WHILE @@FETCH_STATUS = 0
BEGIN
BEGIN TRY  
If Not Exists(select * from HS_HR_LTIMEBASE_TYPE where TBT_CODE = @SHORTLEAVETYPECODE)
Begin
INSERT INTO HS_HR_LTIMEBASE_TYPE 
	(
	TBT_CODE,
	TBT_NAME,
	TBT_MINHOURS,
	TBT_MAXHOURS,
	TBT_MAXOCCURRENCE,

	TBT_ISCONSEC, -- check box
	TBT_MAXCONSECHOURS,
	TBT_ISCONSEC_ALL,

	TBT_ISMAXOCCURPERDAY, --check box
	TBT_MAXOCCURPERDAY,

	TBT_ISPERIOD, -- check box

	TBT_ISPREDEFINEDTIMES , --check box
	TBT_PREDEFINED_ONE,
	TBT_PREDEFINED_TWO,

	TBT_ISMONTHWISE, -- radio
	TBT_ISMONTHWISEHOURS, -- checkbox
	TBT_MONTHWISEHOURS, 

	TBT_ISYEARLY, --radio
	TBT_ISYEARLYHOURS, --check box
	TBT_YEARLYHOURS

	)
VALUES
	(
	@SHORTLEAVETYPECODE, 
	@LEAVETYPENAME,
	@MINHOURSALLOWED,
	@MAXHOURSALLOWED,
	@MAXOCCURRENCE,

	(CASE WHEN TRY_CAST(LTRIM(RTRIM(@MAXCONSECUTIVEHOURSALLOWED)) AS FLOAT) > 0 THEN 1 ELSE 0 END), --@TBT_ISCONSEC,
	@MAXCONSECUTIVEHOURSALLOWED,
	@TBT_ISCONSEC_ALL,

	(CASE WHEN @MAXNOOFLEAVEPERDAY > 0 THEN 1 ELSE 0 END),
	@MAXNOOFLEAVEPERDAY,

	@LEAVETIMEPERIOD,	

  	(CASE WHEN (@TBT_PREDEFINED_ONE > 0 OR @TBT_PREDEFINED_TWO > 0) THEN 1 ELSE 0 END), --@TBT_ISPREDEFINEDTIMES ,
	@TBT_PREDEFINED_ONE,
	@TBT_PREDEFINED_TWO,

	(CASE WHEN @MAXOURSPERMONTH > 0 THEN 1 ELSE 0 END),
	(CASE WHEN @MAXOURSPERMONTH > 0 THEN 1 ELSE 0 END),
	@MAXOURSPERMONTH,

	(CASE WHEN @MAXHOURSPERYEAR > 0 THEN 1 ELSE 0 END),
	(CASE WHEN @MAXHOURSPERYEAR > 0 THEN 1 ELSE 0 END),
	@MAXHOURSPERYEAR

	)
END
	
    select 'True' as status , 'Successfully added' as message
END TRY  
BEGIN CATCH  

INSERT INTO [HS_HR_IA_ERROR_LOGS]
			   ([ERROR_LINE]
			   ,[ERROR_MESSAGE]
			   ,[ERROR_NUMBER]
			   ,[ERROR_PROCEDURE]
			   ,[ERROR_SEVERITY]
			   ,[ERROR_STATE]
			   ,[ERROR_DATE])
	SELECT  
	ERROR_LINE () as ErrorLine,  
	Error_Message() as ErrorMessage,  
	Error_Number() as ErrorNumber,  
	Error_Procedure() as 'Proc',  
	Error_Severity() as ErrorSeverity,  
	Error_State() as ErrorState,  
	GETDATE () as DateErrorRaised 

    select 'False' as status , ERROR_MESSAGE() as message
END CATCH  

FETCH NEXT
	FROM @CursorAttribute
	INTO  @SHORTLEAVETYPECODE
	,@LEAVETYPENAME
	,@MINHOURSALLOWED
	,@MAXHOURSALLOWED
	,@MAXOCCURRENCE 
	,@MAXCONSECUTIVEHOURSALLOWED 
	,@MAXNOOFLEAVEPERDAY 
	,@LEAVETIMEPERIOD 
	,@MAXOURSPERMONTH 
	,@MAXHOURSPERYEAR 
	,@TBT_ISCONSEC_ALL

	,@TBT_PREDEFINED_ONE
	,@TBT_PREDEFINED_TWO

END
CLOSE @CursorAttribute

DEALLOCATE @CursorAttribute

MERGE INTO [HS_HR_LTIMEBASE_PERIOD] AS Target
USING [HS_HR_IA_LTIMEBASE_PERIOD_UPLD] AS Source
ON (Target.TBP_CODE = Source.TBP_CODE AND Target.TBT_CODE = Source.TBT_CODE)
WHEN MATCHED THEN
    UPDATE SET
        Target.TBP_PERIOD = Source.TBP_PERIOD
WHEN NOT MATCHED BY TARGET THEN
    INSERT (TBP_CODE, TBT_CODE, TBP_PERIOD)
    VALUES (Source.TBP_CODE, Source.TBT_CODE, Source.TBP_PERIOD);
END
GO

ALTER PROCEDURE [SP_STATUTORY_LEAVE_UPLOAD]
AS 
BEGIN

DECLARE @LEAVETYPECODE AS VARCHAR(250)
DECLARE @LEAVETYPEDESCRIPTION AS VARCHAR(250)
DECLARE @ACTIVE AS INT
DECLARE @COVERINGEMPREQ AS INT
DECLARE @ALLOWHALFDAYS AS INT 
DECLARE @AUTOCALCULATEENDDATE AS INT
DECLARE @HOLIDAY_AS_LEAVE AS int
DECLARE @LIEU_LEAVE INT
DECLARE @MATERNITY_LEAVE INT
DECLARE @NO_PAY_LEAVE INT
DECLARE @QUARTER_LEAVE INT
DECLARE @SHOW_BALANCE INT
DECLARE @EARNED_CF_BREAK_DOWN INT
DECLARE @EXTENDED_PERIOD INT
DECLARE @ALLOW_PLANNING INT
DECLARE @ATTACHMENT_REQUIRED INT
DECLARE @VIEW_ADDITIONAL_FIELD INT
DECLARE @ALLOW_NEGATIVE INT
DECLARE @KIOSK_ALLOWED INT
DECLARE @HIDE_ZERO_ENTITLEMENT INT
DECLARE @ALLOW_SELECT_APPROVER INT
DECLARE @SHOW_LEAVE_REASON INT
DECLARE @CONSIDER_IN_DASHBOARD INT
DECLARE @SHOW_IN_OFFBOARDING INT
DECLARE @CONSIDER_FOR_TIME_LOST INT
DECLARE @COMMENT_MANDATORY INT
DECLARE @LONG_LEAVE INT
DECLARE @HIDE_IN_SELF_APP INT

DECLARE @APPLY_SEQUENCE_IN_DASH INT
DECLARE @MAXIMUM_LEA_DAYS_IN_SINGL_INS NUMERIC(18,2)
DECLARE @YEARLY_ENTITLEMENT NUMERIC(18,2)
DECLARE @MINIMUM_DAYS_IN_SNGLE_LEA_APP NUMERIC(5,2)
DECLARE @MAXIMUM_DAYS_IN_SNGLE_LEA_APP NUMERIC(18,2)
DECLARE @REQ_MEDICAL_CERTIFICATE NUMERIC(18,2)
DECLARE @APPLY_TGTHER_WITH_LEA_TYPE VARCHAR(250)
DECLARE @FUTURE_LEAVE_UTILIZED_AS VARCHAR(250)
DECLARE @UTILIZATION_PERIOD INT
DECLARE @FUTURE_LEAVE_TYPE smallint

DECLARE @NOTICE_PERIOD INT
DECLARE @ENTITLEMENT_CRITERIA VARCHAR(250)
--DECLARE @COMMENTS varchar(250) 

DECLARE @LEV_TYPE_CFWD_YEARLY NUMERIC(18,2)
DECLARE	@LEV_TYPE_CFWD_LIMIT NUMERIC(18,2)
DECLARE	@LEV_TYPE_CFWD INT

DECLARE @LEV_TYPE_MAX_DAYS NUMERIC(18,2)
DECLARE @FUTURE_LEAVE_TYPE_NAME VARCHAR(250)


DECLARE @CursorAttribute CURSOR SET @CursorAttribute = CURSOR FAST_FORWARD
FOR
SELECT [LEAVE_TYPE],[LEAVE_DESCRIPTION],[ACTIVE],[COVERING_EMP_REQ],[ALLOW_HALF_DAYS],[AUTO_CAL_END_DATE],[HOLIDAY_AS_LEAVE],[LIEU_LEAVE],[MATERNITY_LEAVE],[NO_PAY_LEAVE]
      ,[QUARTER_LEAVE],[SHOW_BALANCE],[EARNED_CF_BREAK_DOWN],[EXTENDED_PERIOD],[ALLOW_PLANNING],[ATTACHMENT_REQUIRED],[VIEW_ADDITIONAL_FIELD],[ALLOW_NEGATIVE],[KIOSK_ALLOWED],[HIDE_ZERO_ENTITLEMENT]
	  ,[ALLOW_SELECT_APPROVER],[SHOW_LEAVE_REASON],[CONSIDER_IN_DASHBOARD],[SHOW_IN_OFFBOARDING],[CONSIDER_FOR_TIME_LOST],[COMMENT_MANDATORY],[LONG_LEAVE],[HIDE_IN_SELF_APP],[APPLY_SEQUENCE_IN_DASH],[YEARLY_ENTITLEMENT]
	  ,[MINIMUM_DAYS_IN_SNGLE_LEA_APP],[MAXIMUM_DAYS_IN_SNGLE_LEA_APP],[REQ_MEDICAL_CERTIFICATE],[APPLY_TGTHER_WITH_LEA_TYPE],[FUTURE_LEAVE_UTILIZED_AS],[UTILIZATION_PERIOD],[NOTICE_PERIOD],[ENTITLEMENT_CRITERIA],[LEV_TYPE_CFWD_YEARLY],
	   [LEV_TYPE_CFWD_LIMIT],[NOTICE_PERIOD],[LEV_TYPE_MAX_DAYS],[FUTURE_LEAVE_TYPE_NAME]
	  --[COMMENTS],
FROM HS_HR_IA_STATUTRY_LEAVE_UPLOAD


OPEN @CursorAttribute
FETCH NEXT
FROM @CursorAttribute
INTO  @LEAVETYPECODE,@LEAVETYPEDESCRIPTION ,@ACTIVE,@COVERINGEMPREQ,@ALLOWHALFDAYS ,@AUTOCALCULATEENDDATE ,@HOLIDAY_AS_LEAVE ,@LIEU_LEAVE ,@MATERNITY_LEAVE ,@NO_PAY_LEAVE 
	 ,@QUARTER_LEAVE ,@SHOW_BALANCE ,@EARNED_CF_BREAK_DOWN ,@EXTENDED_PERIOD ,@ALLOW_PLANNING ,@ATTACHMENT_REQUIRED ,@VIEW_ADDITIONAL_FIELD ,@ALLOW_NEGATIVE ,@KIOSK_ALLOWED ,@HIDE_ZERO_ENTITLEMENT 
	 ,@ALLOW_SELECT_APPROVER ,@SHOW_LEAVE_REASON ,@CONSIDER_IN_DASHBOARD ,@SHOW_IN_OFFBOARDING ,@CONSIDER_FOR_TIME_LOST ,@COMMENT_MANDATORY ,@LONG_LEAVE ,@HIDE_IN_SELF_APP ,@APPLY_SEQUENCE_IN_DASH ,@YEARLY_ENTITLEMENT 
     ,@MINIMUM_DAYS_IN_SNGLE_LEA_APP ,@MAXIMUM_DAYS_IN_SNGLE_LEA_APP ,@REQ_MEDICAL_CERTIFICATE ,@APPLY_TGTHER_WITH_LEA_TYPE ,@FUTURE_LEAVE_UTILIZED_AS,@UTILIZATION_PERIOD,@NOTICE_PERIOD,@ENTITLEMENT_CRITERIA,@LEV_TYPE_CFWD_YEARLY
	 ,@LEV_TYPE_CFWD_LIMIT,@NOTICE_PERIOD,@LEV_TYPE_MAX_DAYS,@FUTURE_LEAVE_TYPE_NAME

	  --,@COMMENTS 
WHILE @@FETCH_STATUS = 0
BEGIN
BEGIN TRY  
If Not Exists(select * from hs_hr_leave_type where LEV_TYPE_CODE = @LEAVETYPECODE)
Begin
INSERT INTO hs_hr_leave_type 
	(
	   [LEV_TYPE_CODE]
      ,[LEV_TYPE_NAME]
      ,[LEV_TYPE_DESC]

	  ,[LEV_TYPE_ACTIVE]
	  ,[LEV_TYPE_ACTPER_REQ]
	  ,[LEV_TYPE_HALFDAY]

	  ,[LEV_TYPE_AUTO_CALCULATE]
	  ,[LEV_TYPE_HOLIDAY]
	  ,[LEV_TYPE_ISENCASH]
	  ,[LEV_TYPE_MATERNITY]
	  ,[LEV_TYPE_WITHPAY]
	  ,[LEV_TYPE_QLEAVE_ALLOW]

	  ,[LEV_TYPE_ISSHOWBAL]
	  ,[LEV_TYPE_ISEARNED]
	  ,[LEV_EXT_PERIOD]
	  ,[LEV_TYPE_ISPLANNABLE]
	  ,[LEV_TYPE_ISATTACH_REQ]

	  ,[LEV_SHOW_OTHER_FIELD]
	  ,[LEV_ALLOW_NEGATIVE]
	  ,[LEV_ALLOW_KIOSK]
	  ,[LEV_HIDE_ZERO_ENTITLMENT]
	  ,[LEV_ALLOW_SELECT_APPROVER]

	  ,[LEV_SHOW_LEAVE_REASON]
	  ,[LEV_CONSIDER_FOR_DASHBOARD]
	  ,[LEV_SHOW_IN_OFFBOARDING]
	  ,[LEV_CONSIDER_FOR_TIME_LOST]
	  ,[LEV_TYPE_COMMENT_MANDATORY]

	  ,[LEV_TYP_IS_LNG_LEAVE]
	  ,[IS_DISPLAY_SELF_APPLICATION]
	  ----------------------------------
	  ,[LEV_SEQ_APPLY_FROM_DASHBOARD]
	  ,[LEV_TYPE_YEARLY_ENT]
	  ,LEV_TYPE_MAX_DAYS

	  ,[LEV_TYPE_COMPULSORY] --check box
      ,[LEV_TYPE_COM_MINDAYS]
	  ,[LEV_TYPE_COM_MAXDAYS]

	  ,[LEV_TYPE_ISMEDICALREQ] --check box
	  ,[LEV_TYPE_MEDICAL_DAYS]

	  ,[LEV_TYPE_IMMIDIATELEV]
	 ,[LEV_TYPE_IMMIDIATE_TYPE]

	  ,[LEV_TYPE_FUTURE]
	  ,[LEV_TYPE_FUTURE_LEV]
	  ,[LEV_TYPE_FUTURE_PERIOD]

	  ,[LEV_TYPE_CFWD] 
	  ,[LEV_TYPE_CFWD_YEARLY]
	  ,[LEV_TYPE_CFWD_LIMIT] 
	  ,[LEV_TYPE_CFWD_LEVTYPE]
	 
	  ,[LEV_HAS_NOTICE_PERIOD]
	  ,[LEV_NOTICE_PERIOD]

	  ,[FRMDTL_CODE]
	  )


VALUES
	(
	  @LEAVETYPECODE 
	 ,@LEAVETYPECODE 
	 ,@LEAVETYPEDESCRIPTION 
	 ,@ACTIVE 
	 ,@COVERINGEMPREQ
	 ,@ALLOWHALFDAYS 

	 ,@AUTOCALCULATEENDDATE 
	 ,@HOLIDAY_AS_LEAVE 
	 ,@LIEU_LEAVE 
	 ,@MATERNITY_LEAVE 
	 ,@NO_PAY_LEAVE 
	 ,@QUARTER_LEAVE 
	 
	 ,@SHOW_BALANCE 
	 ,@EARNED_CF_BREAK_DOWN 
	 ,@EXTENDED_PERIOD 
	 ,@ALLOW_PLANNING 
	 ,@ATTACHMENT_REQUIRED 
	 
	 ,@VIEW_ADDITIONAL_FIELD 
	 ,@ALLOW_NEGATIVE 
	 ,@KIOSK_ALLOWED 
	 ,@HIDE_ZERO_ENTITLEMENT 
	 ,@ALLOW_SELECT_APPROVER 
	 
	 ,@SHOW_LEAVE_REASON 
	 ,@CONSIDER_IN_DASHBOARD 
	 ,@SHOW_IN_OFFBOARDING 
	 ,@CONSIDER_FOR_TIME_LOST 
	 ,@COMMENT_MANDATORY 
	 
	 ,@LONG_LEAVE 
	 ,@HIDE_IN_SELF_APP 
	 ,@APPLY_SEQUENCE_IN_DASH 
	 -------------------------------------------------------

	 ,@YEARLY_ENTITLEMENT 
	 ,@LEV_TYPE_MAX_DAYS

	 ,(CASE WHEN (@MINIMUM_DAYS_IN_SNGLE_LEA_APP > 0 OR @MAXIMUM_DAYS_IN_SNGLE_LEA_APP > 0) THEN 1 ELSE 0 END)
     ,@MINIMUM_DAYS_IN_SNGLE_LEA_APP      
	 ,@MAXIMUM_DAYS_IN_SNGLE_LEA_APP 

	 ,(CASE WHEN @REQ_MEDICAL_CERTIFICATE > 0  THEN 1 ELSE 0 END)
     ,@REQ_MEDICAL_CERTIFICATE 

	 ,(CASE WHEN @REQ_MEDICAL_CERTIFICATE is not null THEN 1 ELSE 0 END)
     ,@APPLY_TGTHER_WITH_LEA_TYPE 

     ,(CASE WHEN @UTILIZATION_PERIOD > 0  THEN 1 ELSE 0 END)
     ,@FUTURE_LEAVE_UTILIZED_AS
     ,@UTILIZATION_PERIOD
     
	 ,(CASE WHEN (@LEV_TYPE_CFWD_YEARLY > 0 OR @LEV_TYPE_CFWD_LIMIT > 0)  THEN 1 ELSE 0 END)--,@LEV_TYPE_CFWD
	 ,@LEV_TYPE_CFWD_YEARLY
	 ,@LEV_TYPE_CFWD_LIMIT
	 ,@FUTURE_LEAVE_TYPE_NAME

	 ,(CASE WHEN @NOTICE_PERIOD > 0  THEN 1 ELSE 0 END)
     ,@NOTICE_PERIOD
	 
     ,@ENTITLEMENT_CRITERIA 
     --,@COMMENTS 
	)
END
    select 'True' as status , 'Successfully added' as message
END TRY  
BEGIN CATCH  

INSERT INTO [HS_HR_IA_ERROR_LOGS]
			   ([ERROR_LINE]
			   ,[ERROR_MESSAGE]
			   ,[ERROR_NUMBER]
			   ,[ERROR_PROCEDURE]
			   ,[ERROR_SEVERITY]
			   ,[ERROR_STATE]
			   ,[ERROR_DATE])
	SELECT  
	ERROR_LINE () as ErrorLine,  
	Error_Message() as ErrorMessage,  
	Error_Number() as ErrorNumber,  
	Error_Procedure() as 'Proc',  
	Error_Severity() as ErrorSeverity,  
	Error_State() as ErrorState,  
	GETDATE () as DateErrorRaised 

    select 'False' as status , ERROR_MESSAGE() as message
END CATCH  

FETCH NEXT
	FROM @CursorAttribute
	INTO @LEAVETYPECODE,@LEAVETYPEDESCRIPTION ,@ACTIVE,@COVERINGEMPREQ,@ALLOWHALFDAYS ,@AUTOCALCULATEENDDATE ,@HOLIDAY_AS_LEAVE ,@LIEU_LEAVE ,@MATERNITY_LEAVE ,@NO_PAY_LEAVE 
	 ,@QUARTER_LEAVE ,@SHOW_BALANCE ,@EARNED_CF_BREAK_DOWN ,@EXTENDED_PERIOD ,@ALLOW_PLANNING ,@ATTACHMENT_REQUIRED ,@VIEW_ADDITIONAL_FIELD ,@ALLOW_NEGATIVE ,@KIOSK_ALLOWED ,@HIDE_ZERO_ENTITLEMENT 
	 ,@ALLOW_SELECT_APPROVER ,@SHOW_LEAVE_REASON ,@CONSIDER_IN_DASHBOARD ,@SHOW_IN_OFFBOARDING ,@CONSIDER_FOR_TIME_LOST ,@COMMENT_MANDATORY ,@LONG_LEAVE ,@HIDE_IN_SELF_APP ,@APPLY_SEQUENCE_IN_DASH ,@YEARLY_ENTITLEMENT 
     ,@MINIMUM_DAYS_IN_SNGLE_LEA_APP ,@MAXIMUM_DAYS_IN_SNGLE_LEA_APP ,@REQ_MEDICAL_CERTIFICATE ,@APPLY_TGTHER_WITH_LEA_TYPE ,@FUTURE_LEAVE_UTILIZED_AS,@UTILIZATION_PERIOD,@NOTICE_PERIOD,@ENTITLEMENT_CRITERIA,@LEV_TYPE_CFWD_YEARLY
	 ,@LEV_TYPE_CFWD_LIMIT,@NOTICE_PERIOD,@LEV_TYPE_MAX_DAYS,@FUTURE_LEAVE_TYPE_NAME
END
CLOSE @CursorAttribute

DEALLOCATE @CursorAttribute

END
GO

ALTER  PROCEDURE [SP_IA_VALIDATION_SHIFT_UPLD]
@TEMPLATE_ID NVARCHAR(MAX)
AS
BEGIN
SET NOCOUNT ON;

select h.SHIFT_NAME,SHIFT_ABR, ROW_NUMBER() over (order by ID) as ROW_NUM
into #DataWithRowNumber
from HS_HR_IA_SHIFT_INFO_UPLOAD h

SELECT i.COLUMN_NAME, i.ORDINAL_POSITION as COL_NUM
into #ColNameWithColNumber
FROM INFORMATION_SCHEMA.COLUMNS i
WHERE TABLE_NAME = 'HS_HR_IA_SHIFT_INFO_UPLOAD'
group by i.ORDINAL_POSITION, i.COLUMN_NAME

DELETE FROM HS_HR_IA_ERRORS_SHIFT;
DBCC CHECKIDENT ('[HS_HR_IA_ERRORS_SHIFT]', RESEED, 0);

--For "SHIFT_NAME" No 1
insert into HS_HR_IA_ERRORS_SHIFT
select SHIFT_NAME, @TEMPLATE_ID, 'Please specify a valid Shift Name with a character length of 25 or less', '', 'SHIFT_NAME','' from HS_HR_IA_SHIFT_INFO_UPLOAD
where SHIFT_NAME IS NULL;

select * into #tempTable1 from #DataWithRowNumber where SHIFT_NAME IS NULL ORDER by (SELECT NULL) OFFSET 1 rows;

INSERT INTO HS_HR_IA_ERRORS_SHIFT (SHIFT_NAME,TEMPLATE_ID,ERROR_DETAILS,COL_NUMBER,COL_NAME,ROW_NUMBER) SELECT SHIFT_NAME,@TEMPLATE_ID,'Please specify a valid Shift Name with a character length of 25 or less', '1','SHIFT_NAME', ROW_NUM from #tempTable1;

SELECT * into #tempTable2
FROM #DataWithRowNumber
WHERE SHIFT_NAME IN (
    SELECT SHIFT_NAME
    FROM #DataWithRowNumber
    GROUP BY SHIFT_NAME
    HAVING COUNT(*) > 1
) ORDER BY (SELECT NULL) OFFSET 1 ROWS;

INSERT INTO HS_HR_IA_ERRORS_SHIFT (SHIFT_NAME,TEMPLATE_ID,ERROR_DETAILS,COL_NUMBER,COL_NAME,ROW_NUMBER) SELECT SHIFT_NAME,@TEMPLATE_ID,'Shift Name is duplicated', '1','SHIFT_NAME', ROW_NUM from #tempTable2;

insert into HS_HR_IA_ERRORS_SHIFT
select SHIFT_NAME, @TEMPLATE_ID, 'Please specify a valid Shift Name with a character length of 25 or less which does not already exist', '', 'SHIFT_NAME','' from HS_HR_IA_SHIFT_INFO_UPLOAD
where exists(SELECT 1
    FROM HS_TA_SHIFTDEF_SEGMENT
    WHERE HS_HR_IA_SHIFT_INFO_UPLOAD.SHIFT_NAME = HS_TA_SHIFTDEF_SEGMENT.SEG_NAME) AND SHIFT_NAME IS NOT NULL;

--For "SHIFT_ABR" No 2
insert into HS_HR_IA_ERRORS_SHIFT
select SHIFT_NAME, @TEMPLATE_ID, 'Please specify a valid Shift Abbreviation with a character length of 5 or less which does not already exist', '', 'SHIFT_ABR','' from HS_HR_IA_SHIFT_INFO_UPLOAD
where SHIFT_ABR IS NULL;

insert into HS_HR_IA_ERRORS_SHIFT
select SHIFT_NAME, @TEMPLATE_ID, 'Please specify a valid Shift Abbreviation with a character length of 5 or less which does not already exist', '', 'SHIFT_ABR','' from HS_HR_IA_SHIFT_INFO_UPLOAD
where exists(SELECT 1
    FROM HS_TA_SHIFTDEF
    WHERE HS_HR_IA_SHIFT_INFO_UPLOAD.SHIFT_ABR = HS_TA_SHIFTDEF.SFT_ABBRV) AND SHIFT_ABR IS NOT NULL;

SELECT * into #tempTable3
FROM #DataWithRowNumber
WHERE SHIFT_ABR IN (
    SELECT SHIFT_ABR
    FROM #DataWithRowNumber
    GROUP BY SHIFT_ABR
    HAVING COUNT(*) > 1
) ORDER BY (SELECT NULL) OFFSET 1 ROWS;

INSERT INTO HS_HR_IA_ERRORS_SHIFT (SHIFT_NAME,TEMPLATE_ID,ERROR_DETAILS,COL_NUMBER,COL_NAME,ROW_NUMBER) SELECT SHIFT_NAME,@TEMPLATE_ID,'Shift Abbriviation is duplicated', '2','SHIFT_ABR', ROW_NUM from #tempTable3;


--For "START_TIME" No 3
insert into HS_HR_IA_ERRORS_SHIFT
select SHIFT_NAME, @TEMPLATE_ID, 'Please specify a valid numeric value between 0 and 24 for Start Time', '', 'START_TIME','' from HS_HR_IA_SHIFT_INFO_UPLOAD
where START_TIME IS NULL;

insert into HS_HR_IA_ERRORS_SHIFT
select SHIFT_NAME, @TEMPLATE_ID, 'Please specify a valid numeric value between 0 and 24 for Start Time', '', 'START_TIME','' from HS_HR_IA_SHIFT_INFO_UPLOAD
where START_TIME < 0 OR START_TIME>24;

--For "END_TIME" No 4
insert into HS_HR_IA_ERRORS_SHIFT
select SHIFT_NAME, @TEMPLATE_ID, 'Please specify a valid numeric value between 0 and 24 for End Time (greater than Start Time). If the end date time is the next day, Select "1" for Next day shift out time', '', 'END_TIME','' from HS_HR_IA_SHIFT_INFO_UPLOAD
where END_TIME IS NULL;

insert into HS_HR_IA_ERRORS_SHIFT
select SHIFT_NAME, @TEMPLATE_ID, 'Please specify a valid numeric value between 0 and 24 for End Time (greater than Start Time). If the end date time is the next day, Select "1" for Next day shift out time', '', 'END_TIME','' from HS_HR_IA_SHIFT_INFO_UPLOAD
WHERE END_TIME<START_TIME AND (NEXT_DAY_SHIFT_OUT_TIME IS NULL OR NEXT_DAY_SHIFT_OUT_TIME=0);

insert into HS_HR_IA_ERRORS_SHIFT
select SHIFT_NAME, @TEMPLATE_ID, 'Please specify a valid numeric value between 0 and 24 for End Time (greater than Start Time). If the end date time is the next day, Select "1" for Next day shift out time', '', 'END_TIME','' from HS_HR_IA_SHIFT_INFO_UPLOAD
where END_TIME < 0 OR END_TIME>24;

--For "FIRST_HALF_DUR" No 5
insert into HS_HR_IA_ERRORS_SHIFT
select SHIFT_NAME, @TEMPLATE_ID, 'Please specify a valid numeric value between 0 and 24 for First Half Duration', '', 'FIRST_HALF_DUR','' from HS_HR_IA_SHIFT_INFO_UPLOAD
where FIRST_HALF_DUR IS NULL;

insert into HS_HR_IA_ERRORS_SHIFT
select SHIFT_NAME, @TEMPLATE_ID, 'Please specify a valid numeric value between 0 and 24 for First Half Duration', '', 'FIRST_HALF_DUR','' from HS_HR_IA_SHIFT_INFO_UPLOAD
where FIRST_HALF_DUR < 0 OR FIRST_HALF_DUR>24;

--For "SECOND_HALF_DUR" No 7
insert into HS_HR_IA_ERRORS_SHIFT
select SHIFT_NAME, @TEMPLATE_ID, 'Please specify a valid numeric value between 0 and 24 for Second Half Duration', '', 'SECOND_HALF_DUR','' from HS_HR_IA_SHIFT_INFO_UPLOAD
where SECOND_HALF_DUR IS NULL;

insert into HS_HR_IA_ERRORS_SHIFT
select SHIFT_NAME, @TEMPLATE_ID, 'Please specify a valid numeric value between 0 and 24 for Second Half Duration', '', 'SECOND_HALF_DUR','' from HS_HR_IA_SHIFT_INFO_UPLOAD
where SECOND_HALF_DUR < 0 OR SECOND_HALF_DUR>24;

--For "FLEXI_SHIFT"
insert into HS_HR_IA_ERRORS_SHIFT
select SHIFT_NAME, @TEMPLATE_ID, 'Please select either 0 or 1 for Flexi Shift', '', 'FLEXI_SHIFT','' from HS_HR_IA_SHIFT_INFO_UPLOAD
where FLEXI_SHIFT NOT IN (0,1);

insert into HS_HR_IA_ERRORS_SHIFT
select SHIFT_NAME, @TEMPLATE_ID, 'Flexi Shift and Next Day Shift Out Time can not be 1 at the same time', '', 'FLEXI_SHIFT','' from HS_HR_IA_SHIFT_INFO_UPLOAD
where FLEXI_SHIFT=1 AND NEXT_DAY_SHIFT_OUT_TIME=1;

--For "OFF_SHIFT"
insert into HS_HR_IA_ERRORS_SHIFT
select SHIFT_NAME, @TEMPLATE_ID, 'Please select either 0 or 1 for Off Shift', '', 'OFF_SHIFT','' from HS_HR_IA_SHIFT_INFO_UPLOAD
where OFF_SHIFT NOT IN (0,1);

--For "CONTINUE_SHIFT"
insert into HS_HR_IA_ERRORS_SHIFT
select SHIFT_NAME, @TEMPLATE_ID, 'Please select either 0 or 1 for Continue Shift', '', 'CONTINUE_SHIFT','' from HS_HR_IA_SHIFT_INFO_UPLOAD
where CONTINUE_SHIFT NOT IN (0,1);

--For "LATE_COVER"
insert into HS_HR_IA_ERRORS_SHIFT
select SHIFT_NAME, @TEMPLATE_ID, 'Please select either 0 or 1 for Late Covering', '', 'LATE_COVER','' from HS_HR_IA_SHIFT_INFO_UPLOAD
where LATE_COVER NOT IN (0,1);

--For "BRK_ALLOW_LATE_HRS_CALC"
insert into HS_HR_IA_ERRORS_SHIFT
select SHIFT_NAME, @TEMPLATE_ID, 'Please select either 0 or 1 for Break Allow to Late Hours Calculation', '', 'BRK_ALLOW_LATE_HRS_CALC','' from HS_HR_IA_SHIFT_INFO_UPLOAD
where BRK_ALLOW_LATE_HRS_CALC NOT IN (0,1);

--For "ALLOW_DEDCT_OUT_HRS"
insert into HS_HR_IA_ERRORS_SHIFT
select SHIFT_NAME, @TEMPLATE_ID, 'Please select either 0 or 1 for Allow Deduct Out Hours from OT Hours', '', 'ALLOW_DEDCT_OUT_HRS','' from HS_HR_IA_SHIFT_INFO_UPLOAD
where ALLOW_DEDCT_OUT_HRS NOT IN (0,1);

--For "AUTO_MID_NIGHT_FIX"
insert into HS_HR_IA_ERRORS_SHIFT
select SHIFT_NAME, @TEMPLATE_ID, 'Please select either 0 or 1 for Automatic Midnight Crossover Fix', '', 'AUTO_MID_NIGHT_FIX','' from HS_HR_IA_SHIFT_INFO_UPLOAD
where AUTO_MID_NIGHT_FIX NOT IN (0,1);

--For "NEXT_DAY_SHIFT_OUT_TIME"
insert into HS_HR_IA_ERRORS_SHIFT
select SHIFT_NAME, @TEMPLATE_ID, 'Please select either 0 or 1 for Next Day Shift Out Time', '', 'NEXT_DAY_SHIFT_OUT_TIME','' from HS_HR_IA_SHIFT_INFO_UPLOAD
where NEXT_DAY_SHIFT_OUT_TIME NOT IN (0,1);

insert into HS_HR_IA_ERRORS_SHIFT
select SHIFT_NAME, @TEMPLATE_ID, 'Flexi Shift and Next Day Shift Out Time can not be 1 at the same time', '', 'NEXT_DAY_SHIFT_OUT_TIME','' from HS_HR_IA_SHIFT_INFO_UPLOAD
where FLEXI_SHIFT=1 AND NEXT_DAY_SHIFT_OUT_TIME=1;

--For "LEAVE_DAYS"
insert into HS_HR_IA_ERRORS_SHIFT
select SHIFT_NAME, @TEMPLATE_ID, 'Please specify a valid numeric value between 0 and 365 for Leave Days', '', 'LEAVE_DAYS','' from HS_HR_IA_SHIFT_INFO_UPLOAD
where LEAVE_DAYS < 0 OR LEAVE_DAYS>365;

update E set E.COL_NUMBER = c.COL_NUM, E.ROW_NUMBER = d.ROW_NUM
from HS_HR_IA_ERRORS_SHIFT E
inner join #DataWithRowNumber d on d.SHIFT_NAME = E.SHIFT_NAME OR (E.SHIFT_NAME IS NULL AND d.SHIFT_NAME IS NULL)
inner join #ColNameWithColNumber c on c.COLUMN_NAME = E.COL_NAME

drop table #ColNameWithColNumber;
drop table #DataWithRowNumber;
drop table #tempTable1;
drop table #tempTable2;
drop table #tempTable3;
END
GO