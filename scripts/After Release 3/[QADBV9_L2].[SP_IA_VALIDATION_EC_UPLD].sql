CREATE OR ALTER Procedure [QADBV9_L2].[SP_IA_VALIDATION_EC_UPLD]
@TEMPLATE_ID NVARCHAR(MAX)
AS
BEGIN
SET NOCOUNT ON;

select h.EMP_NUMBER, ROW_NUMBER() over (order by ID) as ROW_NUM
into #DataWithRowNumber
from HS_HR_IA_EC_UPLOAD h

SELECT i.COLUMN_NAME, i.ORDINAL_POSITION as COL_NUM
into #ColNameWithColNumber
FROM INFORMATION_SCHEMA.COLUMNS i
WHERE TABLE_NAME = 'HS_HR_IA_EC_UPLOAD'
group by i.ORDINAL_POSITION, i.COLUMN_NAME

DELETE FROM [QADBV9_L2].HS_HR_IA_ERRORS;
DBCC CHECKIDENT ('[HS_HR_IA_ERRORS]', RESEED, 0);

--For "Employee No" No 1
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Employee no Does Not Exist In the System', '', 'EMP_NUMBER','' from HS_HR_IA_EC_UPLOAD
where not exists(SELECT 1
    FROM HS_HR_EMPLOYEE
    WHERE HS_HR_IA_EC_UPLOAD.EMP_NUMBER = HS_HR_EMPLOYEE.EMP_DISPLAY_NUMBER) AND EMP_NUMBER IS NOT NULL;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Not a valid value', '', 'EMP_NUMBER','' from HS_HR_IA_EC_UPLOAD
where EMP_NUMBER IS NULL;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Not a valid value', '', 'EMP_NUMBER','' from HS_HR_IA_EC_UPLOAD
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


--For "Contact person full name" No 2
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please specify the Name', '', 'EC_CONT_PER_FULLNAME','' from HS_HR_IA_EC_UPLOAD
where EC_CONT_PER_FULLNAME IS NULL;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please specify the Name', '', 'EC_CONT_PER_FULLNAME','' from HS_HR_IA_EC_UPLOAD
where EC_CONT_PER_FULLNAME IS NOT NULL AND len(EC_CONT_PER_FULLNAME) > 100;

--For "Relationship" No 3
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please specify the Relationship', '', 'EC_RELATIONSHIP','' from HS_HR_IA_EC_UPLOAD
where EC_RELATIONSHIP IS NULL;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please specify the Relationship', '', 'EC_RELATIONSHIP','' from HS_HR_IA_EC_UPLOAD
where EC_RELATIONSHIP IS NOT NULL AND len(EC_RELATIONSHIP) > 20;

--For "Order" No 4
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please specify the Priority Order', '', 'EC_RELATIONSHIP_ORDER','' from HS_HR_IA_EC_UPLOAD
where EC_RELATIONSHIP_ORDER IS NULL;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please specify the Priority Order', '', 'EC_RELATIONSHIP_ORDER','' from HS_HR_IA_EC_UPLOAD
where EC_RELATIONSHIP_ORDER IS NOT NULL AND len(EC_RELATIONSHIP_ORDER) > 3;

--For "Permanent Address" No 5
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Length must be less than 200', '', 'EC_PER_ADDRESS','' from HS_HR_IA_EC_UPLOAD
where EC_PER_ADDRESS IS NOT NULL AND len(EC_PER_ADDRESS) > 200;

--For "Office Telephone no" No 6
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please specify valid Office number', '', 'EC_OFFICE_TELEPHONE','' from HS_HR_IA_EC_UPLOAD
where EC_OFFICE_TELEPHONE IS NOT NULL AND len(EC_OFFICE_TELEPHONE) > 20;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please specify valid Office number', '', 'EC_OFFICE_TELEPHONE','' from HS_HR_IA_EC_UPLOAD
WHERE EC_OFFICE_TELEPHONE NOT LIKE '[+][(][0-9][0-9][0-9][)][ -.][0-9][0-9][0-9][ -.][0-9][0-9][0-9][0-9]' AND EC_OFFICE_TELEPHONE NOT LIKE '[+][(][0-9][0-9][0-9][)][ -.][0-9][0-9][0-9][ -.][0-9][0-9][0-9][0-9][0-9]' AND EC_OFFICE_TELEPHONE NOT LIKE '[+][(][0-9][0-9][0-9][)][ -.][0-9][0-9][0-9][ -.][0-9][0-9][0-9][0-9][0-9][0-9]' AND
EC_OFFICE_TELEPHONE NOT LIKE '[+][0-9][0-9][0-9][ -.][0-9][0-9][0-9][ -.][0-9][0-9][0-9][0-9]' AND EC_OFFICE_TELEPHONE NOT LIKE '[+][0-9][0-9][0-9][ -.][0-9][0-9][0-9][ -.][0-9][0-9][0-9][0-9][0-9]' AND EC_OFFICE_TELEPHONE NOT LIKE '[+][0-9][0-9][0-9][ -.][0-9][0-9][0-9][ -.][0-9][0-9][0-9][0-9][0-9][0-9]' AND
EC_OFFICE_TELEPHONE NOT LIKE '[(][0-9][0-9][0-9][)][ -.][0-9][0-9][0-9][ -.][0-9][0-9][0-9][0-9]' AND EC_OFFICE_TELEPHONE NOT LIKE '[(][0-9][0-9][0-9][)][ -.][0-9][0-9][0-9][ -.][0-9][0-9][0-9][0-9][0-9]' AND EC_OFFICE_TELEPHONE NOT LIKE '[(][0-9][0-9][0-9][)][ -.][0-9][0-9][0-9][ -.][0-9][0-9][0-9][0-9][0-9][0-9]' AND
EC_OFFICE_TELEPHONE NOT LIKE '[0-9][0-9][0-9][ -.][0-9][0-9][0-9][ -.][0-9][0-9][0-9][0-9]' AND EC_OFFICE_TELEPHONE NOT LIKE '[0-9][0-9][0-9][ -.][0-9][0-9][0-9][ -.][0-9][0-9][0-9][0-9][0-9]' AND EC_OFFICE_TELEPHONE NOT LIKE '[0-9][0-9][0-9][ -.][0-9][0-9][0-9][ -.][0-9][0-9][0-9][0-9][0-9][0-9]' AND
EC_OFFICE_TELEPHONE NOT LIKE '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]' AND EC_OFFICE_TELEPHONE NOT LIKE '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]' AND EC_OFFICE_TELEPHONE NOT LIKE '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]';

--For "Home Telephone no" No 7
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please specify valid Home number', '', 'EC_RES_TELEPHONE','' from HS_HR_IA_EC_UPLOAD
where EC_RES_TELEPHONE IS NOT NULL AND len(EC_RES_TELEPHONE) > 20;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please specify valid Home number', '', 'EC_RES_TELEPHONE','' from HS_HR_IA_EC_UPLOAD
WHERE EC_RES_TELEPHONE NOT LIKE '[+][(][0-9][0-9][0-9][)][ -.][0-9][0-9][0-9][ -.][0-9][0-9][0-9][0-9]' AND EC_RES_TELEPHONE NOT LIKE '[+][(][0-9][0-9][0-9][)][ -.][0-9][0-9][0-9][ -.][0-9][0-9][0-9][0-9][0-9]' AND EC_RES_TELEPHONE NOT LIKE '[+][(][0-9][0-9][0-9][)][ -.][0-9][0-9][0-9][ -.][0-9][0-9][0-9][0-9][0-9][0-9]' AND
EC_RES_TELEPHONE NOT LIKE '[+][0-9][0-9][0-9][ -.][0-9][0-9][0-9][ -.][0-9][0-9][0-9][0-9]' AND EC_RES_TELEPHONE NOT LIKE '[+][0-9][0-9][0-9][ -.][0-9][0-9][0-9][ -.][0-9][0-9][0-9][0-9][0-9]' AND EC_RES_TELEPHONE NOT LIKE '[+][0-9][0-9][0-9][ -.][0-9][0-9][0-9][ -.][0-9][0-9][0-9][0-9][0-9][0-9]' AND
EC_RES_TELEPHONE NOT LIKE '[(][0-9][0-9][0-9][)][ -.][0-9][0-9][0-9][ -.][0-9][0-9][0-9][0-9]' AND EC_RES_TELEPHONE NOT LIKE '[(][0-9][0-9][0-9][)][ -.][0-9][0-9][0-9][ -.][0-9][0-9][0-9][0-9][0-9]' AND EC_RES_TELEPHONE NOT LIKE '[(][0-9][0-9][0-9][)][ -.][0-9][0-9][0-9][ -.][0-9][0-9][0-9][0-9][0-9][0-9]' AND
EC_RES_TELEPHONE NOT LIKE '[0-9][0-9][0-9][ -.][0-9][0-9][0-9][ -.][0-9][0-9][0-9][0-9]' AND EC_RES_TELEPHONE NOT LIKE '[0-9][0-9][0-9][ -.][0-9][0-9][0-9][ -.][0-9][0-9][0-9][0-9][0-9]' AND EC_RES_TELEPHONE NOT LIKE '[0-9][0-9][0-9][ -.][0-9][0-9][0-9][ -.][0-9][0-9][0-9][0-9][0-9][0-9]' AND
EC_RES_TELEPHONE NOT LIKE '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]' AND EC_RES_TELEPHONE NOT LIKE '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]' AND EC_RES_TELEPHONE NOT LIKE '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]';

--For "Mobile no" No 8
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please specify valid Mobile number', '', 'EC_MOBILE','' from HS_HR_IA_EC_UPLOAD
where EC_MOBILE IS NULL;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please specify valid Mobile number', '', 'EC_MOBILE','' from HS_HR_IA_EC_UPLOAD
where EC_MOBILE IS NOT NULL AND len(EC_MOBILE) > 20;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please specify valid Mobile number', '', 'EC_MOBILE','' from HS_HR_IA_EC_UPLOAD
WHERE EC_MOBILE NOT LIKE '[+][(][0-9][0-9][0-9][)][ -.][0-9][0-9][0-9][ -.][0-9][0-9][0-9][0-9]' AND EC_MOBILE NOT LIKE '[+][(][0-9][0-9][0-9][)][ -.][0-9][0-9][0-9][ -.][0-9][0-9][0-9][0-9][0-9]' AND EC_MOBILE NOT LIKE '[+][(][0-9][0-9][0-9][)][ -.][0-9][0-9][0-9][ -.][0-9][0-9][0-9][0-9][0-9][0-9]' AND
EC_MOBILE NOT LIKE '[+][0-9][0-9][0-9][ -.][0-9][0-9][0-9][ -.][0-9][0-9][0-9][0-9]' AND EC_MOBILE NOT LIKE '[+][0-9][0-9][0-9][ -.][0-9][0-9][0-9][ -.][0-9][0-9][0-9][0-9][0-9]' AND EC_MOBILE NOT LIKE '[+][0-9][0-9][0-9][ -.][0-9][0-9][0-9][ -.][0-9][0-9][0-9][0-9][0-9][0-9]' AND
EC_MOBILE NOT LIKE '[(][0-9][0-9][0-9][)][ -.][0-9][0-9][0-9][ -.][0-9][0-9][0-9][0-9]' AND EC_MOBILE NOT LIKE '[(][0-9][0-9][0-9][)][ -.][0-9][0-9][0-9][ -.][0-9][0-9][0-9][0-9][0-9]' AND EC_MOBILE NOT LIKE '[(][0-9][0-9][0-9][)][ -.][0-9][0-9][0-9][ -.][0-9][0-9][0-9][0-9][0-9][0-9]' AND
EC_MOBILE NOT LIKE '[0-9][0-9][0-9][ -.][0-9][0-9][0-9][ -.][0-9][0-9][0-9][0-9]' AND EC_MOBILE NOT LIKE '[0-9][0-9][0-9][ -.][0-9][0-9][0-9][ -.][0-9][0-9][0-9][0-9][0-9]' AND EC_MOBILE NOT LIKE '[0-9][0-9][0-9][ -.][0-9][0-9][0-9][ -.][0-9][0-9][0-9][0-9][0-9][0-9]' AND
EC_MOBILE NOT LIKE '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]' AND EC_MOBILE NOT LIKE '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]' AND EC_MOBILE NOT LIKE '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]';

--For "Contact Type" No 9
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Not a valid value', '', 'EC_CONTACT_TYPE_FLG','' from HS_HR_IA_EC_UPLOAD
where EC_CONTACT_TYPE_FLG NOT IN(0,1);

--For "Official Address" No 10
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Length must be less than 200', '', 'EC_OFFICIAL_ADDRESS','' from HS_HR_IA_EC_UPLOAD
where EC_OFFICIAL_ADDRESS IS NOT NULL AND len(EC_OFFICIAL_ADDRESS) > 200;

--For "Employee no of contact person (if internal only)" No 11
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Employee no of contact person does not exist In the system', '', 'EC_EMP_NUMBER','' from HS_HR_IA_EC_UPLOAD
where not exists(SELECT 1
    FROM HS_HR_EMPLOYEE
    WHERE HS_HR_IA_EC_UPLOAD.EC_EMP_NUMBER = HS_HR_EMPLOYEE.EMP_DISPLAY_NUMBER) AND EC_EMP_NUMBER IS NOT NULL;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Not a valid value', '', 'EC_EMP_NUMBER','' from HS_HR_IA_EC_UPLOAD
where EC_EMP_NUMBER IS NOT NULL AND len(EC_EMP_NUMBER) > 8;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please select an Employee Name to proceed', '', 'EC_EMP_NUMBER','' from HS_HR_IA_EC_UPLOAD
where EC_EMP_NUMBER IS NULL AND EC_CONTACT_TYPE_FLG=1;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Employee No cannot be the same', '', 'EC_EMP_NUMBER','' from HS_HR_IA_EC_UPLOAD
where EC_EMP_NUMBER=EMP_NUMBER;

--For "Telephone extension (If internal only)" No 12
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Not a valid value', '', 'EC_TEL_EXT','' from HS_HR_IA_EC_UPLOAD
where EC_TEL_EXT IS NOT NULL AND len(EC_TEL_EXT) > 20;


update E set E.COL_NUMBER = c.COL_NUM, E.ROW_NUMBER = d.ROW_NUM
from HS_HR_IA_ERRORS E
inner join #DataWithRowNumber d on d.EMP_NUMBER = E.EMP_NUMBER OR (E.EMP_NUMBER IS NULL AND d.EMP_NUMBER IS NULL)
inner join #ColNameWithColNumber c on c.COLUMN_NAME = E.COL_NAME

drop table #ColNameWithColNumber;
drop table #DataWithRowNumber;
drop table #tempTable1;
drop table #tempTable2;

END
GO