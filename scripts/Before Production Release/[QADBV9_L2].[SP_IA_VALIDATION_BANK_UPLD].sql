Create OR ALTER Procedure [QADBV9_L2].[SP_IA_VALIDATION_BANK_UPLD]
@TEMPLATE_ID NVARCHAR(MAX)
AS
BEGIN
SET NOCOUNT ON;

select h.EMP_NUMBER,h.EBANK_ACC_NO, ROW_NUMBER() over (order by ID) as ROW_NUM
into #DataWithRowNumber
from HS_HR_IA_BANK_UPLOAD h

SELECT i.COLUMN_NAME, i.ORDINAL_POSITION as COL_NUM
into #ColNameWithColNumber
FROM INFORMATION_SCHEMA.COLUMNS i
WHERE TABLE_NAME = 'HS_HR_IA_BANK_UPLOAD'
group by i.ORDINAL_POSITION, i.COLUMN_NAME

DELETE FROM [QADBV9_L2].HS_HR_IA_ERRORS;
DBCC CHECKIDENT ('[HS_HR_IA_ERRORS]', RESEED, 0);

--For "Employee No" No 1
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Employee no Does Not Exist In the System', '', 'EMP_NUMBER','' from HS_HR_IA_BANK_UPLOAD
where not exists(SELECT 1
    FROM HS_HR_EMPLOYEE
    WHERE HS_HR_IA_BANK_UPLOAD.EMP_NUMBER = HS_HR_EMPLOYEE.EMP_DISPLAY_NUMBER) AND EMP_NUMBER IS NOT NULL;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify a valid value for Employee no', '', 'EMP_NUMBER','' from HS_HR_IA_BANK_UPLOAD
where EMP_NUMBER IS NULL;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify a valid value for Employee no', '', 'EMP_NUMBER','' from HS_HR_IA_BANK_UPLOAD
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



--For "Bank Code" No 2
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify a valid value for Bank Code', '', 'BANK_CODE','' from HS_HR_IA_BANK_UPLOAD
where BANK_CODE IS NULL;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify a valid value for Bank Code', '', 'BANK_CODE','' from HS_HR_IA_BANK_UPLOAD
where BANK_CODE IS NOT NULL AND len(BANK_CODE) > 6;

--For "Bank Name" No 3
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify a valid value for Bank Name', '', 'BANK_NAME','' from HS_HR_IA_BANK_UPLOAD
where BANK_NAME IS NULL;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify a valid value for Bank Name', '', 'BANK_NAME','' from HS_HR_IA_BANK_UPLOAD
where BANK_NAME IS NOT NULL AND len(BANK_NAME) > 100;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select SUBQUERY.EMP_NUMBER, @TEMPLATE_ID, 'Please Specify only one value for Bank Name for each Bank Code', '', 'BANK_NAME','' from (
	SELECT DISTINCT(A.EMP_NUMBER) 
	FROM HS_HR_IA_BANK_UPLOAD A LEFT JOIN HS_HR_IA_BANK_UPLOAD B ON A.BANK_CODE=B.BANK_CODE 
	WHERE A.BANK_CODE=B.BANK_CODE AND A.BANK_NAME!=B.BANK_NAME
	) AS SUBQUERY;

--For "Branch Code" No 4
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify a valid value for Branch Code', '', 'BRANCH_CODE','' from HS_HR_IA_BANK_UPLOAD
where BRANCH_CODE IS NULL;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify a valid value for Branch Code', '', 'BRANCH_CODE','' from HS_HR_IA_BANK_UPLOAD
where BRANCH_CODE IS NOT NULL AND len(BRANCH_CODE) > 6;

--For "Branch Name" No 5
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify a valid value for Branch Name', '', 'BRANCH_NAME','' from HS_HR_IA_BANK_UPLOAD
where BRANCH_NAME IS NULL;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify a valid value for Branch Name', '', 'BRANCH_NAME','' from HS_HR_IA_BANK_UPLOAD
where BRANCH_NAME IS NOT NULL AND len(BRANCH_NAME) > 120;

--For "Account No" No 6
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify a valid value for Account No', '', 'EBANK_ACC_NO','' from HS_HR_IA_BANK_UPLOAD
where EBANK_ACC_NO IS NULL;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify a valid value for Account No', '', 'EBANK_ACC_NO','' from HS_HR_IA_BANK_UPLOAD
where EBANK_ACC_NO IS NOT NULL AND len(EBANK_ACC_NO) > 30;

SELECT * into #tempTable3
FROM #DataWithRowNumber
WHERE EBANK_ACC_NO IN (
    SELECT EBANK_ACC_NO
    FROM #DataWithRowNumber
    GROUP BY EBANK_ACC_NO
    HAVING COUNT(*) > 1
) ORDER BY (SELECT NULL) OFFSET 1 ROWS;

INSERT INTO HS_HR_IA_ERRORS (EMP_NUMBER,TEMPLATE_ID,ERROR_DETAILS,COL_NUMBER,COL_NAME,ROW_NUMBER) SELECT EMP_NUMBER,@TEMPLATE_ID,'Bank Account Number is duplicated', '6','EBANK_ACC_NO', ROW_NUM from #tempTable3;


--For "Account Type" No 7
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify a valid value for Account Type', '', 'ACCTYPE_ID','' from HS_HR_IA_BANK_UPLOAD
where ACCTYPE_ID IS NULL;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify a valid value for Account Type', '', 'ACCTYPE_ID','' from HS_HR_IA_BANK_UPLOAD
where ACCTYPE_ID IS NOT NULL AND len(ACCTYPE_ID) > 30;

--For "Currency Symbol" No 8
insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify a valid value for Currency Symbol', '', 'CURRENCY_SYMBOL','' from HS_HR_IA_BANK_UPLOAD
where CURRENCY_SYMBOL IS NULL;

insert into [QADBV9_L2].HS_HR_IA_ERRORS
select EMP_NUMBER, @TEMPLATE_ID, 'Please Specify a valid value for Currency', '', 'CURRENCY_SYMBOL','' from HS_HR_IA_BANK_UPLOAD
where CURRENCY_SYMBOL IS NOT NULL AND len(CURRENCY_SYMBOL) > 50;


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