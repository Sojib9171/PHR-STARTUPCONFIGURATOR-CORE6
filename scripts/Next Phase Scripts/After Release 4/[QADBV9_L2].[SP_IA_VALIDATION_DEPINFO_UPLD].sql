Create OR ALTER Procedure [QADBV9_L2].[SP_IA_VALIDATION_DEPINFO_UPLD]
@TEMPLATE_ID NVARCHAR(MAX)
AS
BEGIN
SET NOCOUNT ON;

select h.EMP_NUMBER,h.EREL_NIC_NUMBER, ROW_NUMBER() over (order by ID) as ROW_NUM
into #DataWithRowNumber
from HS_HR_IA_DEPINFO_UPLOAD h

SELECT i.COLUMN_NAME, i.ORDINAL_POSITION as COL_NUM
into #ColNameWithColNumber
FROM INFORMATION_SCHEMA.COLUMNS i
WHERE TABLE_NAME = 'HS_HR_IA_DEPINFO_UPLOAD'
group by i.ORDINAL_POSITION, i.COLUMN_NAME

DELETE FROM [QADBV9_L2].HS_HR_IA_ERRORS;
DBCC CHECKIDENT ('[HS_HR_IA_ERRORS]', RESEED, 0);

--For "Employee No" No 1
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Employee no Does Not Exist In the System', '', 'EMP_NUMBER','' from HS_HR_IA_DEPINFO_UPLOAD
where not exists(SELECT 1
    FROM HS_HR_EMPLOYEE
    WHERE HS_HR_IA_DEPINFO_UPLOAD.EMP_NUMBER = HS_HR_EMPLOYEE.EMP_DISPLAY_NUMBER) AND EMP_NUMBER IS NOT NULL;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Not a valid value', '', 'EMP_NUMBER','' from HS_HR_IA_DEPINFO_UPLOAD
where EMP_NUMBER IS NULL;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Not a valid value', '', 'EMP_NUMBER','' from HS_HR_IA_DEPINFO_UPLOAD
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


--For "Dependent Full name" No 2
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please specify the Name', '', 'EREL_RELATIONFULLNAME','' from HS_HR_IA_DEPINFO_UPLOAD
where EREL_RELATIONFULLNAME IS NULL;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please specify the Name', '', 'EREL_RELATIONFULLNAME','' from HS_HR_IA_DEPINFO_UPLOAD
where EREL_RELATIONFULLNAME IS NOT NULL AND len(EREL_RELATIONFULLNAME) > 100;

--For "Relationship" No 3
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please specify the Relationship', '', 'EREL_RELATIONSHIP','' from HS_HR_IA_DEPINFO_UPLOAD
where EREL_RELATIONSHIP IS NULL;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please specify the Relationship', '', 'EREL_RELATIONSHIP','' from HS_HR_IA_DEPINFO_UPLOAD
where EREL_RELATIONSHIP IS NOT NULL AND len(EREL_RELATIONSHIP) > 20;

--For "Gender" No 4
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Selected value for Gender does not associate with the selected Relationship', '', 'EREL_GENDER','' from HS_HR_IA_DEPINFO_UPLOAD
where (EREL_RELATIONSHIP = 'Son' OR EREL_RELATIONSHIP = 'Father' OR EREL_RELATIONSHIP = 'Father-In-Law' OR EREL_RELATIONSHIP ='Brother') AND EREL_GENDER!='Male';

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Selected value for Gender does not associate with the selected Relationship', '', 'EREL_GENDER','' from HS_HR_IA_DEPINFO_UPLOAD
where (EREL_RELATIONSHIP = 'Daughter' OR EREL_RELATIONSHIP = 'Mother' OR EREL_RELATIONSHIP = 'Mother-In-Law' OR EREL_RELATIONSHIP ='Sister') AND EREL_GENDER!='Female';

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please specify a valid value for Gender', '', 'EREL_GENDER','' from HS_HR_IA_DEPINFO_UPLOAD
where EREL_GENDER IS NOT NULL AND len(EREL_GENDER) > 20;

--For "Date of Birth" No 5
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please specify the Date of Birth', '', 'EREL_BIRTHDAY','' from HS_HR_IA_DEPINFO_UPLOAD
where EREL_BIRTHDAY IS NULL;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Date of Birth should not be a future date', '', 'EREL_BIRTHDAY','' from HS_HR_IA_DEPINFO_UPLOAD
where EREL_BIRTHDAY>GETDATE();

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please specify a valid Date of Birth', '', 'EREL_BIRTHDAY','' from HS_HR_IA_DEPINFO_UPLOAD
where YEAR(EREL_BIRTHDAY) < 1753;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select a.EMP_NUMBER, @TEMPLATE_ID, 'Please specify the Date of Birth', '', 'EREL_BIRTHDAY','' from HS_HR_IA_DEPINFO_UPLOAD a Inner join HS_HR_IA_EMPLOYEE_UPLOAD b ON a.EMP_NUMBER=b.EMP_NUMBER
where (EREL_RELATIONSHIP = 'Son' OR EREL_RELATIONSHIP = 'Daughter') AND EREL_BIRTHDAY<EMP_BIRTHDAY;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select a.EMP_NUMBER, @TEMPLATE_ID, 'Please specify the Date of Birth', '', 'EREL_BIRTHDAY','' from HS_HR_IA_DEPINFO_UPLOAD a Inner join HS_HR_IA_EMPLOYEE_UPLOAD b ON a.EMP_NUMBER=b.EMP_NUMBER
where (EREL_RELATIONSHIP = 'Father' OR EREL_RELATIONSHIP = 'Mother') AND EREL_BIRTHDAY>EMP_BIRTHDAY;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, ' Please specify a valid Date of Birth', '', 'EREL_BIRTHDAY','' from HS_HR_IA_DEPINFO_UPLOAD
where EREL_BIRTHDAY IS NOT NULL AND len(EREL_BIRTHDAY) > 10;

--For "Telephone No" No 6
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please specify valid Telephone number', '', 'EREL_TELEPHONE','' from HS_HR_IA_DEPINFO_UPLOAD
where EREL_TELEPHONE IS NOT NULL AND len(EREL_TELEPHONE) > 20;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please specify valid Telephone number', '', 'EREL_TELEPHONE','' from HS_HR_IA_DEPINFO_UPLOAD
WHERE EREL_TELEPHONE NOT LIKE '[+][(][0-9][0-9][0-9][)][ -.][0-9][0-9][0-9][ -.][0-9][0-9][0-9][0-9]' AND EREL_TELEPHONE NOT LIKE '[+][(][0-9][0-9][0-9][)][ -.][0-9][0-9][0-9][ -.][0-9][0-9][0-9][0-9][0-9]' AND EREL_TELEPHONE NOT LIKE '[+][(][0-9][0-9][0-9][)][ -.][0-9][0-9][0-9][ -.][0-9][0-9][0-9][0-9][0-9][0-9]' AND
EREL_TELEPHONE NOT LIKE '[+][0-9][0-9][0-9][ -.][0-9][0-9][0-9][ -.][0-9][0-9][0-9][0-9]' AND EREL_TELEPHONE NOT LIKE '[+][0-9][0-9][0-9][ -.][0-9][0-9][0-9][ -.][0-9][0-9][0-9][0-9][0-9]' AND EREL_TELEPHONE NOT LIKE '[+][0-9][0-9][0-9][ -.][0-9][0-9][0-9][ -.][0-9][0-9][0-9][0-9][0-9][0-9]' AND
EREL_TELEPHONE NOT LIKE '[(][0-9][0-9][0-9][)][ -.][0-9][0-9][0-9][ -.][0-9][0-9][0-9][0-9]' AND EREL_TELEPHONE NOT LIKE '[(][0-9][0-9][0-9][)][ -.][0-9][0-9][0-9][ -.][0-9][0-9][0-9][0-9][0-9]' AND EREL_TELEPHONE NOT LIKE '[(][0-9][0-9][0-9][)][ -.][0-9][0-9][0-9][ -.][0-9][0-9][0-9][0-9][0-9][0-9]' AND
EREL_TELEPHONE NOT LIKE '[0-9][0-9][0-9][ -.][0-9][0-9][0-9][ -.][0-9][0-9][0-9][0-9]' AND EREL_TELEPHONE NOT LIKE '[0-9][0-9][0-9][ -.][0-9][0-9][0-9][ -.][0-9][0-9][0-9][0-9][0-9]' AND EREL_TELEPHONE NOT LIKE '[0-9][0-9][0-9][ -.][0-9][0-9][0-9][ -.][0-9][0-9][0-9][0-9][0-9][0-9]' AND
EREL_TELEPHONE NOT LIKE '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]' AND EREL_TELEPHONE NOT LIKE '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]' AND EREL_TELEPHONE NOT LIKE '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]';


--For "Entitled for Medical Benefits" No 7
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Not a valid value', '', 'EREL_ENTMEDICALBENIFIT_FLG','' from HS_HR_IA_DEPINFO_UPLOAD
where EREL_ENTMEDICALBENIFIT_FLG NOT IN(0,1);

--For "Working in same company" No 8
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Not a valid value', '', 'EREL_WORK_SAME_COMP_FLG','' from HS_HR_IA_DEPINFO_UPLOAD
where EREL_WORK_SAME_COMP_FLG NOT IN(0,1);

--For "Education Center" No 9
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'The maximum field length allowed for Education Center is 100 characters', '', 'EREL_EDU_CENTRE','' from HS_HR_IA_DEPINFO_UPLOAD
where EREL_EDU_CENTRE IS NOT NULL AND len(EREL_EDU_CENTRE) > 100;

--For "Home address" No 10
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'The maximum field length allowed for Home Address is 200 characters', '', 'EREL_HOUSE_ADDRESS','' from HS_HR_IA_DEPINFO_UPLOAD
where EREL_HOUSE_ADDRESS IS NOT NULL AND len(EREL_HOUSE_ADDRESS) > 200;

--For "Office Address" No 11
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'The maximum field length allowed for Office Address is 200 characters', '', 'EREL_EDU_CENTRE','' from HS_HR_IA_DEPINFO_UPLOAD
where EREL_OFFICE_ADDRESS IS NOT NULL AND len(EREL_OFFICE_ADDRESS) > 200;

--For "PF Ratio" No 12
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please specify the PF Ratio', '', 'EREL_PF_RATIO','' from HS_HR_IA_DEPINFO_UPLOAD
where EREL_PF_RATIO IS NOT NULL AND len(EREL_PF_RATIO) > 5;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please specify the PF Ratio', '', 'EREL_PF_RATIO','' from HS_HR_IA_DEPINFO_UPLOAD
where EREL_PF_RATIO IS NULL AND EREL_PF_NOMINEE_FLG=1;

--For "PF nominee" No 13
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Not a valid value', '', 'EREL_PF_NOMINEE_FLG','' from HS_HR_IA_DEPINFO_UPLOAD
where EREL_PF_NOMINEE_FLG NOT IN(0,1);

--For "Entitled for death donations" No 14
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Not a valid value', '', 'EREL_ENTDEATHDONATION_FLG','' from HS_HR_IA_DEPINFO_UPLOAD
where EREL_ENTDEATHDONATION_FLG NOT IN(0,1);

--For "Living" No 15

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Not a valid value', '', 'EREL_LIVINGORNOT_FLG','' from HS_HR_IA_DEPINFO_UPLOAD
where EREL_LIVINGORNOT_FLG NOT IN(0,1);

--For "NIC number" No 16
SELECT * into #tempTable3
FROM #DataWithRowNumber
WHERE EREL_NIC_NUMBER IN (
    SELECT EREL_NIC_NUMBER
    FROM #DataWithRowNumber
    GROUP BY EREL_NIC_NUMBER
    HAVING COUNT(*) > 1
) ORDER BY (SELECT NULL) OFFSET 1 ROWS;

INSERT INTO HS_HR_IA_ERRORS (EMP_NUMBER,TEMPLATE_ID,ERROR_DETAILS,COL_NUMBER,COL_NAME,ROW_NUMBER) SELECT EMP_NUMBER,@TEMPLATE_ID,'NIC Number is duplicated', '16','EREL_NIC_NUMBER', ROW_NUM from #tempTable3;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Not a valid value', '', 'EREL_NIC_NUMBER','' from HS_HR_IA_DEPINFO_UPLOAD
where EREL_NIC_NUMBER IS NOT NULL AND len(EREL_NIC_NUMBER) > 20;

--For "Spouse Telephone No" No 17
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, '"Please specify valid Official Telephone number', '', 'EREL_SPOUSE_TELEPHONE','' from HS_HR_IA_DEPINFO_UPLOAD
where EREL_SPOUSE_TELEPHONE IS NOT NULL AND len(EREL_SPOUSE_TELEPHONE) > 20;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, '"Please specify valid Official Telephone number', '', 'EREL_SPOUSE_TELEPHONE','' from HS_HR_IA_DEPINFO_UPLOAD
WHERE EREL_SPOUSE_TELEPHONE NOT LIKE '[+][(][0-9][0-9][0-9][)][ -.][0-9][0-9][0-9][ -.][0-9][0-9][0-9][0-9]' AND EREL_SPOUSE_TELEPHONE NOT LIKE '[+][(][0-9][0-9][0-9][)][ -.][0-9][0-9][0-9][ -.][0-9][0-9][0-9][0-9][0-9]' AND EREL_SPOUSE_TELEPHONE NOT LIKE '[+][(][0-9][0-9][0-9][)][ -.][0-9][0-9][0-9][ -.][0-9][0-9][0-9][0-9][0-9][0-9]' AND
EREL_SPOUSE_TELEPHONE NOT LIKE '[+][0-9][0-9][0-9][ -.][0-9][0-9][0-9][ -.][0-9][0-9][0-9][0-9]' AND EREL_SPOUSE_TELEPHONE NOT LIKE '[+][0-9][0-9][0-9][ -.][0-9][0-9][0-9][ -.][0-9][0-9][0-9][0-9][0-9]' AND EREL_SPOUSE_TELEPHONE NOT LIKE '[+][0-9][0-9][0-9][ -.][0-9][0-9][0-9][ -.][0-9][0-9][0-9][0-9][0-9][0-9]' AND
EREL_SPOUSE_TELEPHONE NOT LIKE '[(][0-9][0-9][0-9][)][ -.][0-9][0-9][0-9][ -.][0-9][0-9][0-9][0-9]' AND EREL_SPOUSE_TELEPHONE NOT LIKE '[(][0-9][0-9][0-9][)][ -.][0-9][0-9][0-9][ -.][0-9][0-9][0-9][0-9][0-9]' AND EREL_SPOUSE_TELEPHONE NOT LIKE '[(][0-9][0-9][0-9][)][ -.][0-9][0-9][0-9][ -.][0-9][0-9][0-9][0-9][0-9][0-9]' AND
EREL_SPOUSE_TELEPHONE NOT LIKE '[0-9][0-9][0-9][ -.][0-9][0-9][0-9][ -.][0-9][0-9][0-9][0-9]' AND EREL_SPOUSE_TELEPHONE NOT LIKE '[0-9][0-9][0-9][ -.][0-9][0-9][0-9][ -.][0-9][0-9][0-9][0-9][0-9]' AND EREL_SPOUSE_TELEPHONE NOT LIKE '[0-9][0-9][0-9][ -.][0-9][0-9][0-9][ -.][0-9][0-9][0-9][0-9][0-9][0-9]' AND
EREL_SPOUSE_TELEPHONE NOT LIKE '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]' AND EREL_SPOUSE_TELEPHONE NOT LIKE '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]' AND EREL_SPOUSE_TELEPHONE NOT LIKE '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]';


--For "Comments" No 18
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'The maximum field length allowed for Comments is 200 characters', '', 'EREL_COMMENTS','' from HS_HR_IA_DEPINFO_UPLOAD
where EREL_COMMENTS IS NOT NULL AND len(EREL_COMMENTS) > 200;

--For "Married" No 19
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Not a valid value', '', 'EREL_IS_MARRIED','' from HS_HR_IA_DEPINFO_UPLOAD
where EREL_IS_MARRIED NOT IN(0,1);

--For "Employed" No 20
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Not a valid value', '', 'EREL_IS_WORKING','' from HS_HR_IA_DEPINFO_UPLOAD
where EREL_IS_WORKING NOT IN(0,1);

--For "Employee no of the dependant (If working in the same company)" No 21
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Employee no of the dependant Does Not Exist In the System', '', 'EREL_EMP_NUMBER','' from HS_HR_IA_DEPINFO_UPLOAD
where not exists(SELECT 1
    FROM HS_HR_EMPLOYEE
    WHERE HS_HR_IA_DEPINFO_UPLOAD.EREL_EMP_NUMBER = HS_HR_EMPLOYEE.EMP_DISPLAY_NUMBER) AND EREL_EMP_NUMBER IS NOT NULL;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select a.EMP_NUMBER, @TEMPLATE_ID, 'Employee No cannot be the same', '', 'EREL_EMP_NUMBER','' from HS_HR_IA_DEPINFO_UPLOAD a Inner join HS_HR_IA_EMPLOYEE_UPLOAD b ON a.EMP_NUMBER=b.EMP_NUMBER
where a.EREL_EMP_NUMBER=b.EMP_NUMBER;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please select an Employee Name to proceed', '', 'EREL_EMP_NUMBER','' from HS_HR_IA_DEPINFO_UPLOAD
where EREL_EMP_NUMBER IS NULL AND EREL_WORK_SAME_COMP_FLG=1;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Not a valid value', '', 'EREL_EMP_NUMBER','' from HS_HR_IA_DEPINFO_UPLOAD
where EREL_EMP_NUMBER IS NOT NULL AND len(EREL_EMP_NUMBER) > 8;

update E set E.COL_NUMBER = c.COL_NUM, E.ROW_NUMBER = d.ROW_NUM
from HS_HR_IA_ERRORS E
inner join #DataWithRowNumber d on d.EMP_NUMBER = E.EMP_NUMBER OR (E.EMP_NUMBER IS NULL AND d.EMP_NUMBER IS NULL)
inner join #ColNameWithColNumber c on c.COLUMN_NAME = E.COL_NAME

drop table #ColNameWithColNumber;
drop table #DataWithRowNumber;
drop table #tempTable1;
drop table #tempTable2;
drop table #tempTable3;
END
GO