CREATE OR ALTER PROCEDURE [QADBV9_L2].[SP_IA_VALIDATION_SHORT_UPLD]
@TEMPLATE_ID NVARCHAR(MAX)
AS
BEGIN
--SET NOCOUNT ON;

select h.SHORT_LEAVE_TYPE_CODE, ROW_NUMBER() over (order by ID) as ROW_NUM
into #DataWithRowNumber
from HS_HR_IA_SHORT_LEAVE_UPLOAD h


SELECT i.COLUMN_NAME, i.ORDINAL_POSITION as COL_NUM
into #ColNameWithColNumber
FROM INFORMATION_SCHEMA.COLUMNS i
WHERE TABLE_NAME = 'HS_HR_IA_SHORT_LEAVE_UPLOAD'
group by i.ORDINAL_POSITION, i.COLUMN_NAME

DELETE FROM [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE;
DBCC CHECKIDENT ('[HS_HR_IA_ERRORS_ABSENCE]', RESEED, 0);

--For "SHORT_LEAVE_TYPE_CODE" No 1
insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select SHORT_LEAVE_TYPE_CODE, @TEMPLATE_ID, 'Please specify a valid value Short Leave Type Code', '', 'SHORT_LEAVE_TYPE_CODE','' from HS_HR_IA_SHORT_LEAVE_UPLOAD
where SHORT_LEAVE_TYPE_CODE IS NULL;

insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select SHORT_LEAVE_TYPE_CODE, @TEMPLATE_ID, 'Please specify a valid value Short Leave Type Code', '', 'SHORT_LEAVE_TYPE_CODE','' from HS_HR_IA_SHORT_LEAVE_UPLOAD
where SHORT_LEAVE_TYPE_CODE IS NOT NULL AND len(SHORT_LEAVE_TYPE_CODE) > 6;


--For "Bank Code" No 2
insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select SHORT_LEAVE_TYPE_CODE, @TEMPLATE_ID, 'Please specify a valid value Leave Type  ', '', 'LEAVE_TYPE_NAME','' from HS_HR_IA_SHORT_LEAVE_UPLOAD
where LEAVE_TYPE_NAME IS NULL;

insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select SHORT_LEAVE_TYPE_CODE, @TEMPLATE_ID, 'Please specify a valid value Leave Type ', '', 'LEAVE_TYPE_NAME','' from HS_HR_IA_SHORT_LEAVE_UPLOAD
where LEAVE_TYPE_NAME IS NOT NULL AND len(LEAVE_TYPE_NAME) > 18;

--For "MIN_HOURS_ALLOWED" No 3

insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select SHORT_LEAVE_TYPE_CODE, @TEMPLATE_ID, 'Please specify a valid value Minimum hours allowed', '', 'MIN_HOURS_ALLOWED','' from HS_HR_IA_SHORT_LEAVE_UPLOAD
where MIN_HOURS_ALLOWED IS NULL;

insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select SHORT_LEAVE_TYPE_CODE, @TEMPLATE_ID, 'Please specify a valid value Minimum hours allowed', '', 'MIN_HOURS_ALLOWED','' from HS_HR_IA_SHORT_LEAVE_UPLOAD
where MIN_HOURS_ALLOWED < 1 OR MIN_HOURS_ALLOWED > 24;

insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select SHORT_LEAVE_TYPE_CODE, @TEMPLATE_ID, 'Please specify a valid value Minimum hours allowed', '', 'MIN_HOURS_ALLOWED','' from HS_HR_IA_SHORT_LEAVE_UPLOAD
where len(MIN_HOURS_ALLOWED) > 5;



--For "Maximum hours allowed" No 4
insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select SHORT_LEAVE_TYPE_CODE, @TEMPLATE_ID, 'Please specify a valid value Maximum hours allowed', '', 'MAX_HOURS_ALLOWED','' from HS_HR_IA_SHORT_LEAVE_UPLOAD
where MAX_HOURS_ALLOWED IS NULL 

insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select SHORT_LEAVE_TYPE_CODE, @TEMPLATE_ID, 'Please specify a valid value Maximum hours allowed', '', 'MAX_HOURS_ALLOWED','' from HS_HR_IA_SHORT_LEAVE_UPLOAD
where MAX_HOURS_ALLOWED < MIN_HOURS_ALLOWED;

insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select SHORT_LEAVE_TYPE_CODE, @TEMPLATE_ID, 'Please specify a valid value Maximum hours allowed', '', 'MAX_HOURS_ALLOWED','' from HS_HR_IA_SHORT_LEAVE_UPLOAD
where MAX_HOURS_ALLOWED < 1 OR MAX_HOURS_ALLOWED > 24;

insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select SHORT_LEAVE_TYPE_CODE, @TEMPLATE_ID, 'Please specify a valid value Maximum hours allowed', '', 'MAX_HOURS_ALLOWED','' from HS_HR_IA_SHORT_LEAVE_UPLOAD
where len(MAX_HOURS_ALLOWED) > 5;

--For "Maximum Occurrence" No 5
insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select SHORT_LEAVE_TYPE_CODE, @TEMPLATE_ID, 'Please specify a valid value Maximum Occurrence', '', 'MAX_OCCURRENCE','' from HS_HR_IA_SHORT_LEAVE_UPLOAD
where MAX_OCCURRENCE IS NULL 

insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select SHORT_LEAVE_TYPE_CODE, @TEMPLATE_ID, 'Please specify a valid value Maximum Occurrence', '', 'MAX_OCCURRENCE','' from HS_HR_IA_SHORT_LEAVE_UPLOAD
where MAX_OCCURRENCE < 0;

insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select SHORT_LEAVE_TYPE_CODE, @TEMPLATE_ID, 'Please specify a valid value Maximum Occurrence', '', 'MAX_OCCURRENCE','' from HS_HR_IA_SHORT_LEAVE_UPLOAD
where len(MAX_OCCURRENCE) > 2;

--For "Maximum consecutive hours allowed per application" No 6

insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select SHORT_LEAVE_TYPE_CODE, @TEMPLATE_ID, 'Please specify a valid value Maximum consecutive hours allowed per application', '', 'MAX_CONSECUTIVE_HOURS_ALLOWED','' from HS_HR_IA_SHORT_LEAVE_UPLOAD
where MAX_CONSECUTIVE_HOURS_ALLOWED IS NOT NULL AND len(MAX_CONSECUTIVE_HOURS_ALLOWED) > 5;

insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select SHORT_LEAVE_TYPE_CODE, @TEMPLATE_ID, 'Please specify a valid value Maximum consecutive hours allowed per application', '', 'MAX_CONSECUTIVE_HOURS_ALLOWED','' from HS_HR_IA_SHORT_LEAVE_UPLOAD
where MAX_CONSECUTIVE_HOURS_ALLOWED >= MAX_HOURS_ALLOWED;

--For "Consider All Leave Types " No 7

insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select SHORT_LEAVE_TYPE_CODE, @TEMPLATE_ID, 'Please specify a valid value Consider All Leave Types ', '', 'CONSIDER_ALL_LEAVE_TYPES','' from HS_HR_IA_SHORT_LEAVE_UPLOAD
where CONSIDER_ALL_LEAVE_TYPES NOT IN(0,1);

--For "Maximum No. of Leave per Day" No 8
insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select SHORT_LEAVE_TYPE_CODE, @TEMPLATE_ID, 'Please specify a valid value Maximum No. of Leave per Day', '', 'MAX_NO_OF_LEAVE_PER_DAY','' from HS_HR_IA_SHORT_LEAVE_UPLOAD
where MAX_NO_OF_LEAVE_PER_DAY IS NOT NULL AND len(MAX_NO_OF_LEAVE_PER_DAY) > 5

--For "Leave Time Period (Hours)" No 9
insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select SHORT_LEAVE_TYPE_CODE, @TEMPLATE_ID, 'Please specify a valid valueLeave Time Period (Hours)', '', 'LEAVE_TIME_PERIOD','' from HS_HR_IA_SHORT_LEAVE_UPLOAD
where LEAVE_TIME_PERIOD IS NOT NULL AND len(LEAVE_TIME_PERIOD) > 5


--For "Duration per Slot - Beginning of Shift (Hours)" No 10

insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select SHORT_LEAVE_TYPE_CODE, @TEMPLATE_ID, 'Please specify a valid value Duration per Slot - Beginning of Shift (Hours)', '', 'DURATION_PER_SLOT_BEG_OF_SHIFT','' from HS_HR_IA_SHORT_LEAVE_UPLOAD
where DURATION_PER_SLOT_BEG_OF_SHIFT IS NOT NULL AND len(DURATION_PER_SLOT_BEG_OF_SHIFT) > 5

insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select SHORT_LEAVE_TYPE_CODE, @TEMPLATE_ID, 'Please specify a valid value Duration per Slot - Beginning of Shift (Hours)', '', 'DURATION_PER_SLOT_BEG_OF_SHIFT','' from HS_HR_IA_SHORT_LEAVE_UPLOAD
where DURATION_PER_SLOT_BEG_OF_SHIFT < 0 

insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select SHORT_LEAVE_TYPE_CODE, @TEMPLATE_ID, 'Please specify a valid value Duration per Slot - Beginning of Shift (Hours)', '', 'DURATION_PER_SLOT_BEG_OF_SHIFT','' from HS_HR_IA_SHORT_LEAVE_UPLOAD
where  DURATION_PER_SLOT_BEG_OF_SHIFT < MIN_HOURS_ALLOWED

--For "Duration per Slot - End of Shift (Hours)" No 11

insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select SHORT_LEAVE_TYPE_CODE, @TEMPLATE_ID, 'Please specify a valid value Duration per Slot - End of Shift (Hours)', '', 'DURATION_PER_SLOT_END_OF_SHIFT','' from HS_HR_IA_SHORT_LEAVE_UPLOAD
where DURATION_PER_SLOT_END_OF_SHIFT IS NOT NULL AND len(DURATION_PER_SLOT_END_OF_SHIFT) > 5

insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select SHORT_LEAVE_TYPE_CODE, @TEMPLATE_ID, 'Please specify a valid value Duration per Slot - End of Shift (Hours)', '', 'DURATION_PER_SLOT_END_OF_SHIFT','' from HS_HR_IA_SHORT_LEAVE_UPLOAD
where DURATION_PER_SLOT_END_OF_SHIFT < 0

insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select SHORT_LEAVE_TYPE_CODE, @TEMPLATE_ID, 'Please specify a valid value Duration per Slot - End of Shift (Hours)', '', 'DURATION_PER_SLOT_END_OF_SHIFT','' from HS_HR_IA_SHORT_LEAVE_UPLOAD
where DURATION_PER_SLOT_END_OF_SHIFT > MAX_HOURS_ALLOWED

--For "Maximum Hours Per Month (If applicable for month)" No 12

insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select SHORT_LEAVE_TYPE_CODE, @TEMPLATE_ID, 'Please specify a valid value Maximum Hours Per Month (If applicable for month)', '', 'MAX_HOURS_PER_MONTH','' from HS_HR_IA_SHORT_LEAVE_UPLOAD
where MAX_HOURS_PER_MONTH IS NOT NULL AND len(MAX_HOURS_PER_MONTH) > 5

insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select SHORT_LEAVE_TYPE_CODE, @TEMPLATE_ID, 'Please specify a valid value Maximum Hours Per Month (If applicable for month)', '', 'MAX_HOURS_PER_MONTH','' from HS_HR_IA_SHORT_LEAVE_UPLOAD
where  (MAX_HOURS_PER_MONTH is not null and  MAX_HOURS_PER_YEAR is not null and  APPLICABLE_FOR_WEEKLY is not null) OR
(MAX_HOURS_PER_MONTH is null  and  MAX_HOURS_PER_YEAR is not null and  APPLICABLE_FOR_WEEKLY is not null) OR
(MAX_HOURS_PER_MONTH is not null and  MAX_HOURS_PER_YEAR is null and  APPLICABLE_FOR_WEEKLY is not null) OR
(MAX_HOURS_PER_MONTH is not null and  MAX_HOURS_PER_YEAR is not null and  APPLICABLE_FOR_WEEKLY is null);

--For "Maximum Hours Per Year (If applicable for Year)" No 13

insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select SHORT_LEAVE_TYPE_CODE, @TEMPLATE_ID, 'Please specify a valid value Maximum Hours Per Year (If applicable for Year)', '', 'MAX_HOURS_PER_YEAR','' from HS_HR_IA_SHORT_LEAVE_UPLOAD
where MAX_HOURS_PER_MONTH IS NOT NULL AND len(MAX_HOURS_PER_MONTH) > 5

insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select SHORT_LEAVE_TYPE_CODE, @TEMPLATE_ID, 'Please specify a valid value Maximum Hours Per Year (If applicable for Year)', '', 'MAX_HOURS_PER_YEAR','' from HS_HR_IA_SHORT_LEAVE_UPLOAD
where  (MAX_HOURS_PER_MONTH is not null and  MAX_HOURS_PER_YEAR is not null and  APPLICABLE_FOR_WEEKLY is not null) OR
(MAX_HOURS_PER_MONTH is null  and  MAX_HOURS_PER_YEAR is not null and  APPLICABLE_FOR_WEEKLY is not null) OR
(MAX_HOURS_PER_MONTH is not null and  MAX_HOURS_PER_YEAR is null and  APPLICABLE_FOR_WEEKLY is not null) OR
(MAX_HOURS_PER_MONTH is not null and  MAX_HOURS_PER_YEAR is not null and  APPLICABLE_FOR_WEEKLY is null);

--For "Applicable for weekly" No 14

insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select SHORT_LEAVE_TYPE_CODE, @TEMPLATE_ID, 'Please specify a valid value Applicable for weekly', '', 'APPLICABLE_FOR_WEEKLY','' from HS_HR_IA_SHORT_LEAVE_UPLOAD
where APPLICABLE_FOR_WEEKLY NOT IN(0,1);

insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select SHORT_LEAVE_TYPE_CODE, @TEMPLATE_ID, 'Please specify a valid value Applicable for weekly', '', 'APPLICABLE_FOR_WEEKLY','' from HS_HR_IA_SHORT_LEAVE_UPLOAD
where  (MAX_HOURS_PER_MONTH is not null and  MAX_HOURS_PER_YEAR is not null and  APPLICABLE_FOR_WEEKLY is not null) OR
(MAX_HOURS_PER_MONTH is null  and  MAX_HOURS_PER_YEAR is not null and  APPLICABLE_FOR_WEEKLY is not null) OR
(MAX_HOURS_PER_MONTH is not null and  MAX_HOURS_PER_YEAR is null and  APPLICABLE_FOR_WEEKLY is not null) OR
(MAX_HOURS_PER_MONTH is not null and  MAX_HOURS_PER_YEAR is not null and  APPLICABLE_FOR_WEEKLY is null);

--For "Leave Need Approval " No 15

insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select SHORT_LEAVE_TYPE_CODE, @TEMPLATE_ID, 'Please specify a valid value Leave Need Approval ', '', 'LEAVE_NEED_APPROVAL','' from HS_HR_IA_SHORT_LEAVE_UPLOAD
where LEAVE_NEED_APPROVAL NOT IN(0,1);

--For "Leave Need Approval " No 16

insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select SHORT_LEAVE_TYPE_CODE, @TEMPLATE_ID, 'Please specify a valid value Leave Need Approval ', '', 'ENTITLEMENT_CONFIRMED_EMP','' from HS_HR_IA_SHORT_LEAVE_UPLOAD
where ENTITLEMENT_CONFIRMED_EMP NOT IN(0,1);

--For "Leave Need Approval " No 17
insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select SHORT_LEAVE_TYPE_CODE, @TEMPLATE_ID, 'Please specify a valid value Entitlement for All Un-Confirmed Employees', '', 'ENTITLEMENT_UN_CONFIRMED_EMP','' from HS_HR_IA_SHORT_LEAVE_UPLOAD
where ENTITLEMENT_UN_CONFIRMED_EMP NOT IN(0,1);

--For "Leave Need Approval " No 18
insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select SHORT_LEAVE_TYPE_CODE, @TEMPLATE_ID, 'Please specify a valid value for Comments', '', 'COMMENTS','' from HS_HR_IA_SHORT_LEAVE_UPLOAD
where COMMENTS IS NOT NULL AND len(COMMENTS) > 200

update E set E.COL_NUMBER = c.COL_NUM, E.ROW_NUMBER = d.ROW_NUM
from HS_HR_IA_ERRORS_ABSENCE E
inner join #DataWithRowNumber d on d.SHORT_LEAVE_TYPE_CODE = E.CODE OR (E.CODE IS NULL AND d.SHORT_LEAVE_TYPE_CODE IS NULL)
inner join #ColNameWithColNumber c on c.COLUMN_NAME = E.COL_NAME

select * into #tempTable1 from #DataWithRowNumber where SHORT_LEAVE_TYPE_CODE IS NULL ORDER by (SELECT NULL) OFFSET 1 rows;

INSERT INTO HS_HR_IA_ERRORS_ABSENCE (CODE,TEMPLATE_ID,ERROR_DETAILS,COL_NUMBER,COL_NAME,ROW_NUMBER) SELECT SHORT_LEAVE_TYPE_CODE,@TEMPLATE_ID,'Short Leave Type Code Can Not Be Null', '2','SHORT_LEAVE_TYPE_CODE', ROW_NUM from #tempTable1;

SELECT * into #tempTable2
FROM #DataWithRowNumber
WHERE SHORT_LEAVE_TYPE_CODE IN (
    SELECT SHORT_LEAVE_TYPE_CODE
    FROM #DataWithRowNumber
    GROUP BY SHORT_LEAVE_TYPE_CODE
    HAVING COUNT(*) > 1
) ORDER BY (SELECT NULL) OFFSET 1 ROWS;

INSERT INTO HS_HR_IA_ERRORS_ABSENCE (CODE,TEMPLATE_ID,ERROR_DETAILS,COL_NUMBER,COL_NAME,ROW_NUMBER) SELECT SHORT_LEAVE_TYPE_CODE,@TEMPLATE_ID,'Short Leave Type Code is duplicated', '2','SHORT_LEAVE_TYPE_CODE', ROW_NUM from #tempTable2;

drop table #ColNameWithColNumber;
drop table #DataWithRowNumber;
drop table #tempTable1;
drop table #tempTable2;

END
GO