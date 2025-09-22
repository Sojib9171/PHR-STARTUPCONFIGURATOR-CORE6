CREATE OR ALTER Procedure [QADBV9_L2].[SP_IA_VALIDATION_EMP_UPLD]
@TEMPLATE_ID NVARCHAR(MAX)
AS
BEGIN
SET NOCOUNT ON;

select h.EMP_NUMBER,h.EMP_NIC_NO,h.EMP_EPF_NUMBER,h.EMP_TAX_ID_NUMBER, ROW_NUMBER() over (order by ID) as ROW_NUM
into #DataWithRowNumber
from HS_HR_IA_EMPLOYEE_UPLOAD h

SELECT i.COLUMN_NAME, i.ORDINAL_POSITION as COL_NUM
into #ColNameWithColNumber
FROM INFORMATION_SCHEMA.COLUMNS i
WHERE TABLE_NAME = 'HS_HR_IA_EMPLOYEE_UPLOAD'
group by i.ORDINAL_POSITION, i.COLUMN_NAME

DELETE FROM [QADBV9_L2].HS_HR_IA_ERRORS;
DBCC CHECKIDENT ('[HS_HR_IA_ERRORS]', RESEED, 0);


--for Employee Number
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Employee Number already exists in the system', '', 'EMP_NUMBER','' from HS_HR_IA_EMPLOYEE_UPLOAD
where exists(SELECT 1
    FROM HS_HR_EMPLOYEE
    WHERE HS_HR_IA_EMPLOYEE_UPLOAD.EMP_NUMBER = HS_HR_EMPLOYEE.EMP_DISPLAY_NUMBER);

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify a valid value for Employee no', '', 'EMP_NUMBER','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_NUMBER IS NULL;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify a valid value for Employee no', '', 'EMP_NUMBER','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_NUMBER IS NOT NULL AND len(EMP_NUMBER) > 8;

select * into #tempTable1 from #DataWithRowNumber where EMP_NUMBER IS NULL ORDER by (SELECT NULL) OFFSET 1 rows;

INSERT INTO HS_HR_IA_ERRORS (EMP_NUMBER,TEMPLATE_ID,ERROR_DETAILS,COL_NUMBER,COL_NAME,ROW_NUMBER) SELECT EMP_NUMBER,@TEMPLATE_ID,'Employee Number Can Not Be Null', '1','EMP_NUMBER', ROW_NUM from #tempTable1;

SELECT * into #tempTable2
FROM #DataWithRowNumber
WHERE EMP_NUMBER IN (
    SELECT EMP_NUMBER
    FROM #DataWithRowNumber
    GROUP BY EMP_NUMBER
    HAVING COUNT(*) > 1
) ORDER BY (SELECT NULL) OFFSET 1 ROWS;

INSERT INTO HS_HR_IA_ERRORS (EMP_NUMBER,TEMPLATE_ID,ERROR_DETAILS,COL_NUMBER,COL_NAME,ROW_NUMBER) SELECT EMP_NUMBER,@TEMPLATE_ID,'Employee Number is duplicated', '1','EMP_NUMBER', ROW_NUM from #tempTable2;

--For Title 1
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please specify valid value for Title', '', 'EMP_TITLE','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_TITLE IS NULL;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please specify valid value for Title', '', 'EMP_TITLE','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_TITLE IS NOT NULL AND len(EMP_TITLE) > 10;


--For Calling Name 2
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please specify valid value for First name', '', 'EMP_CALLING_NAME','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_CALLING_NAME IS NULL;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please specify valid value for First name', '', 'EMP_CALLING_NAME','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_CALLING_NAME IS NOT NULL AND len(EMP_CALLING_NAME) > 50;


--For Surname 3
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Last name', '', 'EMP_SURNAME','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_SURNAME IS NULL;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Last name', '', 'EMP_SURNAME','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_SURNAME IS NOT NULL AND len(EMP_SURNAME) > 50;

--For Initials 4
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for initials', '', 'EMP_MIDDLE_INI','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_MIDDLE_INI IS NULL;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for initials', '', 'EMP_MIDDLE_INI','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_MIDDLE_INI IS NOT NULL AND len(EMP_MIDDLE_INI) > 50;

--For Names denoted by initials 5
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for initial names', '', 'EMP_NAMES_BY_INI','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_NAMES_BY_INI IS NULL;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for initial names', '', 'EMP_NAMES_BY_INI','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_NAMES_BY_INI IS NOT NULL AND len(EMP_NAMES_BY_INI) > 190;

--For Full name 6
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Full name', '', 'EMP_FULLNAME','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_FULLNAME IS NULL;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Full name', '', 'EMP_FULLNAME','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_FULLNAME IS NOT NULL AND len(EMP_FULLNAME) > 200;

--For NIC No 7
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for NIC No', '', 'EMP_NIC_NO','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_NIC_NO IS NULL;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for NIC No', '', 'EMP_NIC_NO','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_NIC_NO IS NOT NULL AND len(EMP_NIC_NO) > 12;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for NIC No', '', 'EMP_NIC_NO','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_NIC_NO NOT LIKE '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]%';

SELECT * into #tempTable3
FROM #DataWithRowNumber
WHERE EMP_NIC_NO IN (
    SELECT EMP_NIC_NO
    FROM #DataWithRowNumber
    GROUP BY EMP_NIC_NO
    HAVING COUNT(*) > 1
) ORDER BY (SELECT NULL) OFFSET 1 ROWS;

INSERT INTO HS_HR_IA_ERRORS (EMP_NUMBER,TEMPLATE_ID,ERROR_DETAILS,COL_NUMBER,COL_NAME,ROW_NUMBER) SELECT EMP_NUMBER,@TEMPLATE_ID,'NIC No is duplicated', '8','EMP_NIC_NO', ROW_NUM from #tempTable3;


--For Date of Birth No 8
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Date of Birth', '', 'EMP_BIRTHDAY','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_BIRTHDAY IS NULL;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Date of Birth', '', 'EMP_BIRTHDAY','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_BIRTHDAY<(select PARA_VALUE from HS_HR_PARAMETER WHERE PARA_NAME='MINJOINDYEAR');

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Date of Birth', '', 'EMP_BIRTHDAY','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_BIRTHDAY IS NOT NULL AND len(EMP_BIRTHDAY) > 10;

--For Gender No 9
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Gender', '', 'EMP_GENDER','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_GENDER IS NULL;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Gender', '', 'EMP_GENDER','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_GENDER IS NOT NULL AND len(EMP_GENDER) > 20;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID , 'Selected value for Gender does not associate with the selected Title', '', 'EMP_GENDER','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_TITLE='Mr.' AND EMP_GENDER!='Male';

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Selected value for Gender does not associate with the selected Title', '', 'EMP_GENDER','' from HS_HR_IA_EMPLOYEE_UPLOAD
where (EMP_TITLE = 'Mrs.' OR EMP_TITLE = 'Miss.' OR EMP_TITLE = 'Ms.') AND EMP_GENDER!='Female';

--For "Maiden Name(If Married)" No 10
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Maiden Name', '', 'EMP_MAIDEN_NAME','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_MAIDEN_NAME IS NOT NULL AND len(EMP_MAIDEN_NAME) > 70;

--For "Other names" No 11
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Other names', '', 'EMP_OTHER_NAMES','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_OTHER_NAMES IS NOT NULL AND len(EMP_OTHER_NAMES) > 200;

--For "EPF No / PF No" No 12
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for EPF No / PF No', '', 'EMP_EPF_NUMBER','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_EPF_NUMBER IS NOT NULL AND len(EMP_EPF_NUMBER) > 25;

SELECT * into #tempTable4
FROM #DataWithRowNumber
WHERE EMP_EPF_NUMBER IN (
    SELECT EMP_EPF_NUMBER
    FROM #DataWithRowNumber
    GROUP BY EMP_EPF_NUMBER
    HAVING COUNT(*) > 1
) ORDER BY (SELECT NULL) OFFSET 1 ROWS;

INSERT INTO HS_HR_IA_ERRORS (EMP_NUMBER,TEMPLATE_ID,ERROR_DETAILS,COL_NUMBER,COL_NAME,ROW_NUMBER) SELECT EMP_NUMBER,@TEMPLATE_ID,'EPF No is duplicated', '13','EMP_EPF_NUMBER', ROW_NUM from #tempTable4;


--For "NIC Date" No 13
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for NIC Date', '', 'EMP_NIC_DATE','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_NIC_DATE IS NOT NULL AND len(EMP_NIC_DATE) > 10;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for NIC Date', '', 'EMP_NIC_DATE','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_NIC_DATE<EMP_BIRTHDAY;

--For "Place of Birth" No 14
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Place of Birth', '', 'EMP_BIRTHPLACE','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_BIRTHPLACE IS NOT NULL AND len(EMP_BIRTHPLACE) > 100;

--For "Nationality" No 15
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Nationality', '', 'NATIONALITY','' from HS_HR_IA_EMPLOYEE_UPLOAD
where NATIONALITY IS NOT NULL AND len(NATIONALITY) > 120;

--For "Religion" No 16
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Religion', '', 'RELIGION','' from HS_HR_IA_EMPLOYEE_UPLOAD
where RELIGION IS NOT NULL AND len(RELIGION) > 120;

--For "Blood Group" No 17
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Blood Group', '', 'EMP_BLOOD_GROUP','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_BLOOD_GROUP IS NOT NULL AND len(EMP_BLOOD_GROUP) > 120;

--For "Marital status" No 18
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Marital status', '', 'EMP_MARITAL_STATUS','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_MARITAL_STATUS IS NOT NULL AND len(EMP_MARITAL_STATUS) > 20;

--For "Date of Marriage" No 19
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Date of Marriage', '', 'EMP_MARRIED_DATE','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_MARITAL_STATUS='Married' AND EMP_MARRIED_DATE IS NULL;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Date of Marriage', '', 'EMP_MARRIED_DATE','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_MARRIED_DATE>GETDATE();

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Date of Marriage', '', 'EMP_MARRIED_DATE','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_MARRIED_DATE<DATEADD(YEAR, 18, EMP_BIRTHDAY);


--For "Date of Divorce" No 20
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Date of Divorce', '', 'EMP_DIVOCE_DATE','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_MARITAL_STATUS='Divorced' AND EMP_DIVOCE_DATE IS NULL;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Date of Divorce', '', 'EMP_DIVOCE_DATE','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_DIVOCE_DATE>GETDATE();

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Date of Divorce', '', 'EMP_DIVOCE_DATE','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_DIVOCE_DATE<EMP_MARRIED_DATE;

--For "Salary Grade" No 21
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Salary Grade', '', 'SAL_GRD','' from HS_HR_IA_EMPLOYEE_UPLOAD
where SAL_GRD IS NOT NULL AND len(SAL_GRD)>60;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Salary Grade', '', 'SAL_GRD','' from HS_HR_IA_EMPLOYEE_UPLOAD
where SAL_GRD IS NULL;

--For "Corporate Title" No 22
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Corporate title', '', 'CORPORATE_TITLE','' from HS_HR_IA_EMPLOYEE_UPLOAD
where CORPORATE_TITLE IS NOT NULL AND len(CORPORATE_TITLE)>120;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Corporate title', '', 'CORPORATE_TITLE','' from HS_HR_IA_EMPLOYEE_UPLOAD
where CORPORATE_TITLE IS NULL;

--For "Designation" No 23
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Designation', '', 'DESIGNATION','' from HS_HR_IA_EMPLOYEE_UPLOAD
where DESIGNATION IS NOT NULL AND len(DESIGNATION)>120;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Designation', '', 'DESIGNATION','' from HS_HR_IA_EMPLOYEE_UPLOAD
where DESIGNATION IS NULL;

--For "Cost Centre" No 24
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Cost Centre', '', 'COST_CENTRE','' from HS_HR_IA_EMPLOYEE_UPLOAD
where COST_CENTRE IS NOT NULL AND len(COST_CENTRE)>120;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Cost Centre', '', 'COST_CENTRE','' from HS_HR_IA_EMPLOYEE_UPLOAD
where COST_CENTRE IS NULL;

--For "Date of join" No 25
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Date of Join', '', 'EMP_DATE_JOINED','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_DATE_JOINED IS NULL;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Date of Join', '', 'EMP_DATE_JOINED','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_DATE_JOINED<EMP_BIRTHDAY;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Date of Join', '', 'EMP_DATE_JOINED','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_DATE_JOINED IS NOT NULL AND len(EMP_DATE_JOINED)>10;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Date of Join', '', 'EMP_DATE_JOINED','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_DATE_JOINED<(select PARA_VALUE from HS_HR_PARAMETER WHERE PARA_NAME='MINJOINDYEAR');

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Date of Join', '', 'EMP_DATE_JOINED','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_DATE_JOINED<DATEADD(YEAR, 18, EMP_BIRTHDAY);


--For "Date join to the Group" No 26
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Date join to the Group', '', 'EMP_DATE_JOINED_TO_GROUP','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_DATE_JOINED IS NULL;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Date join to the Group', '', 'EMP_DATE_JOINED_TO_GROUP','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_DATE_JOINED>EMP_DATE_JOINED_TO_GROUP;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Date join to the Group', '', 'EMP_DATE_JOINED_TO_GROUP','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_DATE_JOINED_TO_GROUP IS NOT NULL AND len(EMP_DATE_JOINED_TO_GROUP)>10;

--For "Employement Type" No 27
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Employement Type', '', 'EMP_TYPE','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_TYPE IS NULL;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Employement Type', '', 'EMP_TYPE','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_TYPE IS NOT NULL AND len(EMP_TYPE)>120;

--For "Staff statutory Classification" No 28
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Staff statutory Classification', '', 'STAFFCAT_CLASS','' from HS_HR_IA_EMPLOYEE_UPLOAD
where STAFFCAT_CLASS IS NULL;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Staff statutory Classification', '', 'STAFFCAT_CLASS','' from HS_HR_IA_EMPLOYEE_UPLOAD
where STAFFCAT_CLASS IS NOT NULL AND len(STAFFCAT_CLASS)>120;

--For "Barcode No" No 29
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Barcode No', '', 'EMP_BARCODENO','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_BARCODENO IS NULL;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Barcode No', '', 'EMP_BARCODENO','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_BARCODENO IS NOT NULL AND len(EMP_BARCODENO)>15;

--For "Payroll No" No 30
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Payroll No', '', 'EMP_PAYROLLNO','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_ACTIVE_PAYROLL_FLG = 1 AND EMP_PAYROLLNO IS NULL;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Payroll No', '', 'EMP_PAYROLLNO','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_PAYROLLNO IS NOT NULL AND len(EMP_PAYROLLNO)>15;

--For "HR Active" No 31
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for HR Active', '', 'EMP_ACTIVE_HRM_FLG','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_ACTIVE_HRM_FLG IS NULL;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for HR Active', '', 'EMP_ACTIVE_HRM_FLG','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_ACTIVE_HRM_FLG NOT IN (0,1);

--For "Payroll Active" No 32
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Payroll Active', '', 'EMP_ACTIVE_PAYROLL_FLG','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_ACTIVE_PAYROLL_FLG IS NULL;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Payroll Active', '', 'EMP_ACTIVE_PAYROLL_FLG','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_ACTIVE_PAYROLL_FLG NOT IN (0,1);


--For "Confirmation Status" No 33
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Confirmation Status', '', 'EMP_CONFIRM_FLG','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_CONFIRM_FLG IS NULL;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Confirmation Status', '', 'EMP_CONFIRM_FLG','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_CONFIRM_FLG NOT IN (0,1);

--For "Confirmed Date" No 34
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Confirmed Date', '', 'EMP_CONFIRM_DATE','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_CONFIRM_DATE IS NULL AND EMP_CONFIRM_FLG=1;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Confirmed Date', '', 'EMP_CONFIRM_DATE','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_DATE_JOINED>EMP_CONFIRM_DATE;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Confirmed Date', '', 'EMP_CONFIRM_DATE','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_CONFIRM_DATE>EMP_RETIRE_DATE;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Confirmed Date', '', 'EMP_CONFIRM_DATE','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_CONFIRM_DATE>EMP_RESIGN_DATE;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Confirmed Date', '', 'EMP_CONFIRM_DATE','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_CONFIRM_DATE IS NOT NULL AND len(EMP_CONFIRM_DATE)>10;

--For "Date of Confirmation" No 35
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Date of Confirmation', '', 'EMP_DATE_FOR_CONFIRM','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_CONFIRM_DATE>EMP_DATE_FOR_CONFIRM;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Date of Confirmation', '', 'EMP_DATE_FOR_CONFIRM','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_DATE_FOR_CONFIRM>GETDATE();

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Date of Confirmation', '', 'EMP_DATE_FOR_CONFIRM','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_DATE_FOR_CONFIRM IS NOT NULL AND len(EMP_DATE_FOR_CONFIRM)>10;

--For "Date of Resignation" No 36
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Date of Resignation', '', 'EMP_RESIGN_DATE','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_RESIGN_DATE<EMP_DATE_JOINED;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Date of Resignation', '', 'EMP_RESIGN_DATE','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_RESIGN_DATE<EMP_DATE_JOINED_TO_GROUP;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Date of Resignation', '', 'EMP_RESIGN_DATE','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_RESIGN_DATE IS NOT NULL AND len(EMP_RESIGN_DATE)>10;

--For "Date of Retirement" No 37
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Date of Retirement', '', 'EMP_RETIRE_DATE','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_RETIRE_DATE<EMP_DATE_JOINED;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Date of Retirement', '', 'EMP_RETIRE_DATE','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_RETIRE_DATE<EMP_DATE_JOINED_TO_GROUP;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Date of Retirement', '', 'EMP_RETIRE_DATE','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_RETIRE_DATE<(select PARA_VALUE from HS_HR_PARAMETER WHERE PARA_NAME='MINRETIREMENTYEAR');

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Date of Retirement', '', 'EMP_RETIRE_DATE','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_RETIRE_DATE IS NOT NULL AND len(EMP_RETIRE_DATE)>10;

--For "Hours of Work per day" No 38
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Hours of Work per day', '', 'EMP_WORKHOURS','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_WORKHOURS<0;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Hours of Work per day', '', 'EMP_WORKHOURS','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_WORKHOURS>24;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Hours of Workper day', '', 'EMP_WORKHOURS','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_WORKHOURS LIKE '.';

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Hours of Workper day', '', 'EMP_WORKHOURS','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_WORKHOURS LIKE '%.%.%';

--For "Location" No 39
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Location', '', 'LOCATION','' from HS_HR_IA_EMPLOYEE_UPLOAD
where LOCATION IS NOT NULL AND len(LOCATION)>100;

--For "Sub Location" No 40
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Sub Location', '', 'SUB_LOCATION','' from HS_HR_IA_EMPLOYEE_UPLOAD
where SUB_LOCATION IS NOT NULL AND len(SUB_LOCATION)>100;

--For "Contract start date" No 41
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Contract start date', '', 'EMP_CONTRACT_START_DATE','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_CONTRACT_START_DATE<EMP_DATE_JOINED;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Contract start date', '', 'EMP_RESIGN_DATE','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_RESIGN_DATE<EMP_DATE_JOINED_TO_GROUP;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Contract start date', '', 'EMP_CONTRACT_START_DATE','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_CONTRACT_START_DATE IS NOT NULL AND len(EMP_CONTRACT_START_DATE)>10;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Contract start date', '', 'EMP_CONTRACT_START_DATE','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_TYPE='Contract' AND EMP_CONTRACT_START_DATE IS NULL;

--For "Contract end date" No 42
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Contract end date', '', 'EMP_CONTRACT_END_DATE','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_CONTRACT_END_DATE<EMP_DATE_JOINED;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Contract end date', '', 'EMP_CONTRACT_END_DATE','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_CONTRACT_END_DATE<EMP_CONTRACT_START_DATE;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Contract end date', '', 'EMP_CONTRACT_END_DATE','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_CONTRACT_END_DATE IS NOT NULL AND len(EMP_CONTRACT_END_DATE)>10;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Contract end date', '', 'EMP_CONTRACT_END_DATE','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_TYPE='Contract' AND EMP_CONTRACT_END_DATE IS NULL;

--For "Contract to permanent date" No 43
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Contract to permanent date', '', 'EMP_CONT_TO_PERM_DATE','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_CONT_TO_PERM_DATE IS NOT NULL AND len(EMP_CONT_TO_PERM_DATE)>10;

--For "Employee Category" No 44
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Employee Category', '', 'EMP_CATEGORY','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_CATEGORY IS NOT NULL AND len(EMP_CATEGORY)>120;

--For "Recruiment Company" No 45
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Recruiment Company', '', 'RECR_COMPANY','' from HS_HR_IA_EMPLOYEE_UPLOAD
where RECR_COMPANY IS NOT NULL AND len(RECR_COMPANY)>120;

--For "Employee Group" No 46
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Employee Group', '', 'EMP_GROUP','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_GROUP IS NOT NULL AND len(EMP_GROUP)>120;

--For "Group" No 47
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Group', '', 'HIE_LEVEL_1','' from HS_HR_IA_EMPLOYEE_UPLOAD
where HIE_LEVEL_1 IS NULL;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Group', '', 'HIE_LEVEL_1','' from HS_HR_IA_EMPLOYEE_UPLOAD
where HIE_LEVEL_1 IS NOT NULL AND len(HIE_LEVEL_1)>70;

DECLARE @HIE_LEVEL NVARCHAR(70);
SELECT TOP 1 @HIE_LEVEL = HIE_LEVEL_1 FROM HS_HR_IA_EMPLOYEE_UPLOAD ORDER BY ID ASC;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Group', '', 'HIE_LEVEL_1','' from HS_HR_IA_EMPLOYEE_UPLOAD
where HIE_LEVEL_1!=@HIE_LEVEL;

--For "Level2" No 48
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Level2', '', 'HIE_LEVEL_2','' from HS_HR_IA_EMPLOYEE_UPLOAD
where HIE_LEVEL_2 IS NOT NULL AND len(HIE_LEVEL_2)>70;

--For "Level3" No 49
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Level3', '', 'HIE_LEVEL_3','' from HS_HR_IA_EMPLOYEE_UPLOAD
where HIE_LEVEL_3 IS NOT NULL AND len(HIE_LEVEL_3)>70;

--For "Level4" No 50
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Level4', '', 'HIE_LEVEL_4','' from HS_HR_IA_EMPLOYEE_UPLOAD
where HIE_LEVEL_4 IS NOT NULL AND len(HIE_LEVEL_4)>70;

--For "Level5" No 51
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Level5', '', 'HIE_LEVEL_5','' from HS_HR_IA_EMPLOYEE_UPLOAD
where HIE_LEVEL_5 IS NOT NULL AND len(HIE_LEVEL_5)>70;

--For "Level6" No 52
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Level6', '', 'HIE_LEVEL_6','' from HS_HR_IA_EMPLOYEE_UPLOAD
where HIE_LEVEL_6 IS NOT NULL AND len(HIE_LEVEL_6)>70;

--For "Level7" No 53
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Level7', '', 'HIE_LEVEL_7','' from HS_HR_IA_EMPLOYEE_UPLOAD
where HIE_LEVEL_7 IS NOT NULL AND len(HIE_LEVEL_7)>70;

--For "Level8" No 54
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Level8', '', 'HIE_LEVEL_8','' from HS_HR_IA_EMPLOYEE_UPLOAD
where HIE_LEVEL_8 IS NOT NULL AND len(HIE_LEVEL_8)>70;

--For "Level9" No 55
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Level9', '', 'HIE_LEVEL_9','' from HS_HR_IA_EMPLOYEE_UPLOAD
where HIE_LEVEL_9 IS NOT NULL AND len(HIE_LEVEL_9)>70;

--For "Tax on tax" No 56
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Tax on tax', '', 'EMP_TAXONTAX_FLG','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_TAXONTAX_FLG NOT IN (0,1);

--For "Tax Id no" No 57
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Tax Id no', '', 'EMP_TAX_ID_NUMBER','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_TAX_ID_NUMBER IS NOT NULL AND len(EMP_TAX_ID_NUMBER)>20;

SELECT * into #tempTable5
FROM #DataWithRowNumber
WHERE EMP_TAX_ID_NUMBER IN (
    SELECT EMP_TAX_ID_NUMBER
    FROM #DataWithRowNumber
    GROUP BY EMP_TAX_ID_NUMBER
    HAVING COUNT(*) > 1
) ORDER BY (SELECT NULL) OFFSET 1 ROWS;

INSERT INTO HS_HR_IA_ERRORS (EMP_NUMBER,TEMPLATE_ID,ERROR_DETAILS,COL_NUMBER,COL_NAME,ROW_NUMBER) SELECT EMP_NUMBER,@TEMPLATE_ID,'Tax ID No is duplicated', '59','EMP_TAX_ID_NUMBER', ROW_NUM from #tempTable5;

--For "EPF Eligible" No 58
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for EPF Eligible', '', 'EMP_EPF_ELIGIBLE_FLG','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_EPF_ELIGIBLE_FLG NOT IN (0,1);

--For "EPF Employee amount" No 59
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for EPF Employee amount', '', 'EMP_EPF_EMPLOYEE_AMOUNT','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_EPF_EMPLOYEE_AMOUNT IS NOT NULL AND len(EMP_EPF_EMPLOYEE_AMOUNT)>3;

--For "EPF Employer Amount" No 60
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for EPF Employer Amount', '', 'EMP_EPF_EMPLOYER_AMOUNT','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_EPF_EMPLOYER_AMOUNT IS NOT NULL AND len(EMP_EPF_EMPLOYER_AMOUNT)>3;

--For "ETF Eligible" No 61
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for ETF Eligible', '', 'EMP_ETF_ELIGIBLE_FLG','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_ETF_ELIGIBLE_FLG NOT IN (0,1);

--For "ETF No" No 62
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for ETF No', '', 'EMP_ETF_NUMBER','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_ETF_NUMBER IS NOT NULL AND len(EMP_ETF_NUMBER)>20;

--For "ETF Employee amount" No 63
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for ETF Employee amount', '', 'EMP_ETF_EMPLOYEE_AMOUNT','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_ETF_EMPLOYEE_AMOUNT IS NOT NULL AND len(EMP_ETF_EMPLOYEE_AMOUNT)>3;

--For "ETF Date" No 64
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for ETF Date', '', 'EMP_ETF_DATE','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_ETF_DATE IS NOT NULL AND len(EMP_ETF_DATE)>10;

--For "MSPS eligible" No 65
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for MSPS eligible', '', 'EMP_MSPS_ELIGIBLE_FLG','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_MSPS_ELIGIBLE_FLG NOT IN (0,1);

--For "MSPF Employer Amount" No 66
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for MSPF Employer Amount', '', 'EMP_MSPS_EMPLOYER_AMOUNT','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_MSPS_EMPLOYER_AMOUNT IS NOT NULL AND len(EMP_MSPS_EMPLOYER_AMOUNT)>3;

--For "MSPF Employee Amount" No 67
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for MSPF Employee Amount', '', 'EMP_MSPS_EMPLOYEE_AMOUNT','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_MSPS_EMPLOYEE_AMOUNT IS NOT NULL AND len(EMP_MSPS_EMPLOYEE_AMOUNT)>3;

--For "Permanent Address 1 / House No" No 68
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Permanent Address 1 / House No', '', 'EMP_PER_ADDRESS1','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_PER_ADDRESS1 IS NULL;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Permanent Address 1 / House No', '', 'EMP_PER_ADDRESS1','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_PER_ADDRESS1 IS NOT NULL AND len(EMP_PER_ADDRESS1)>50;

--For "Permanent Address 2 / Street 1" No 69
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Permanent Address 2 / Street 1', '', 'EMP_PER_ADDRESS2','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_PER_ADDRESS2 IS NOT NULL AND len(EMP_PER_ADDRESS2)>50;

--For "Permanent Address 3 / Street 2" No 70
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Permanent Address 3 / Street 2', '', 'EMP_PER_ADDRESS3','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_PER_ADDRESS3 IS NOT NULL AND len(EMP_PER_ADDRESS3)>50;

--For "Permanent City" No 71
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Permanent City', '', 'EMP_PER_CITY','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_PER_CITY IS NOT NULL AND len(EMP_PER_CITY)>30;

--For "Permanent telephone no" No 72
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Permanent telephone no', '', 'EMP_PER_TELEPHONE','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_PER_TELEPHONE like '%[a-zA-z]%'

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Permanent telephone no', '', 'EMP_PER_TELEPHONE','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_PER_TELEPHONE IS NOT NULL AND len(EMP_PER_TELEPHONE)>20;

--For "Permanent mobile no" No 73
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Permanent mobile no', '', 'EMP_PER_MOBILE','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_PER_MOBILE like '%[a-zA-z]%'

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Permanent mobile no', '', 'EMP_PER_MOBILE','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_PER_MOBILE IS NOT NULL AND len(EMP_PER_MOBILE)>20;

--For "Permanent fax no" No 74
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Permanent fax no', '', 'EMP_PER_FAX','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_PER_FAX IS NOT NULL AND len(EMP_PER_FAX)>20;

--For "Permanent email" No 75
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Permanent email', '', 'EMP_PER_EMAIL','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_PER_EMAIL NOT LIKE '%_@_%._%';

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Permanent email', '', 'EMP_PER_EMAIL','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_PER_EMAIL IS NOT NULL AND len(EMP_PER_EMAIL)>100;

--For "Permanent Country" No 76
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Permanent Country', '', 'EMP_PER_COUNTRY','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_PER_COUNTRY IS NOT NULL AND len(EMP_PER_COUNTRY)>120;

--For "Permanent Province" No 77
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Permanent Province', '', 'EMP_PER_PROVINCE','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_PER_PROVINCE IS NOT NULL AND len(EMP_PER_PROVINCE)>120;

--For "Permanent District" No 78
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Permanent District', '', 'EMP_PER_DISTRICT','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_PER_DISTRICT IS NOT NULL AND len(EMP_PER_DISTRICT)>120;

--For "Permanent Electorate" No 79
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Permanent Electorate', '', 'EMP_PER_ELECTORATE_CODE','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_PER_ELECTORATE_CODE IS NOT NULL AND len(EMP_PER_ELECTORATE_CODE)>120;

--For "Temporary Address 1" No 80
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Temporary Address 1', '', 'EMP_TEM_ADDRESS1','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_TEM_ADDRESS1 IS NOT NULL AND len(EMP_TEM_ADDRESS1)>50;

--For "Temporary Address 2" No 81
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Temporary Address 2', '', 'EMP_TEM_ADDRESS2','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_TEM_ADDRESS2 IS NOT NULL AND len(EMP_TEM_ADDRESS2)>50;

--For "Temporary Address 3" No 82
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Temporary Address 3', '', 'EMP_TEM_ADDRESS3','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_TEM_ADDRESS3 IS NOT NULL AND len(EMP_TEM_ADDRESS3)>50;

--For "Temporary City" No 83
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Temporary City', '', 'EMP_TEM_CITY','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_TEM_CITY IS NOT NULL AND len(EMP_TEM_CITY)>30;

--For "Temporary Postal" No 84
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Temporary Postal', '', 'EMP_TEM_POSTALCODE','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_TEM_POSTALCODE IS NOT NULL AND len(EMP_TEM_POSTALCODE)>20;

--For "Temporary telephone no" No 85
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Temporary telephone no', '', 'EMP_TEM_TELEPHONE','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_TEM_TELEPHONE IS NOT NULL AND len(EMP_TEM_TELEPHONE)>20;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Temporary telephone no', '', 'EMP_TEM_TELEPHONE','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_TEM_TELEPHONE like '%[a-zA-z]%';

--For "Temporary mobile no" No 86
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Temporary mobile no', '', 'EMP_TEM_MOBILE','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_TEM_MOBILE IS NOT NULL AND len(EMP_TEM_MOBILE)>20;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Temporary mobile no', '', 'EMP_TEM_MOBILE','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_TEM_MOBILE like '%[a-zA-z]%';

--For "Temporary fax no" No 87
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Temporary fax no', '', 'EMP_TEM_FAX','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_TEM_FAX IS NOT NULL AND len(EMP_TEM_FAX)>20;

--For "Temporary email" No 88
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Temporary email', '', 'EMP_TEM_EMAIL','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_TEM_EMAIL NOT LIKE '%_@_%._%';

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Temporary email', '', 'EMP_TEM_EMAIL','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_TEM_EMAIL IS NOT NULL AND len(EMP_TEM_EMAIL)>100;

--For "Temporary Country" No 89
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Temporary Country', '', 'EMP_TEM_COUNTRY','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_TEM_COUNTRY IS NOT NULL AND len(EMP_TEM_COUNTRY)>120;

--For "Temporary Provice" No 90
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Temporary Provice', '', 'EMP_TEM_PROVINCE','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_TEM_PROVINCE IS NOT NULL AND len(EMP_TEM_PROVINCE)>120;

--For "Temporary District" No 91
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Temporary District', '', 'EMP_TEM_DISTRICT','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_TEM_DISTRICT IS NOT NULL AND len(EMP_TEM_DISTRICT)>120;

--For "Temporary Electorate" No 92
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Temporary Electorate', '', 'EMP_TEM_ELECTORATE','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_TEM_DISTRICT IS NOT NULL AND len(EMP_TEM_DISTRICT)>120;

--For "Office Email" No 93
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Office Email', '', 'EMP_OFFICE_EMAIL','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_OFFICE_EMAIL NOT LIKE '%_@_%._%';

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Office Email', '', 'EMP_OFFICE_EMAIL','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_OFFICE_EMAIL NOT LIKE '%_@_%._%';

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Office Email', '', 'EMP_OFFICE_EMAIL','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_OFFICE_EMAIL IS NOT NULL AND len(EMP_OFFICE_EMAIL)>100;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Office Email', '', 'EMP_OFFICE_EMAIL','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_OFFICE_EMAIL IS NULL;

--For "Office Extension" No 94
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Office Extension', '', 'EMP_OFFICE_EXTN','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_OFFICE_EXTN IS NOT NULL AND len(EMP_OFFICE_EXTN)>10;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Office Extension', '', 'EMP_OFFICE_EXTN','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_OFFICE_EXTN like '%[a-zA-z]%';

--For "Office Phone No" No 95
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Office Phone No', '', 'EMP_OFFICE_PHONE','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_OFFICE_PHONE IS NOT NULL AND len(EMP_OFFICE_PHONE)>20;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Office Phone No', '', 'EMP_OFFICE_PHONE','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_OFFICE_PHONE like '%[a-zA-z]%';

--For "Attendance Active" No 96
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Attendance Active', '', 'EMP_ACTIVE_ATT_FLG','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_ACTIVE_ATT_FLG IS NULL;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify valid value for Attendance Active', '', 'EMP_ACTIVE_ATT_FLG','' from HS_HR_IA_EMPLOYEE_UPLOAD
where EMP_ACTIVE_ATT_FLG NOT IN (0,1);


update E set E.COL_NUMBER = c.COL_NUM, E.ROW_NUMBER = d.ROW_NUM
from HS_HR_IA_ERRORS E
inner join #DataWithRowNumber d on d.EMP_NUMBER = E.EMP_NUMBER OR (E.EMP_NUMBER IS NULL AND d.EMP_NUMBER IS NULL)
inner join #ColNameWithColNumber c on c.COLUMN_NAME = E.COL_NAME

drop table #ColNameWithColNumber;
drop table #DataWithRowNumber;
drop table #tempTable1;
drop table #tempTable2;
drop table #tempTable3;
drop table #tempTable4;
drop table #tempTable5;
END
GO