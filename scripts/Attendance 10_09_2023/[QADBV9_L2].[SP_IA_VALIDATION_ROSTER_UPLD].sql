Create OR ALTER Procedure [QADBV9_L2].[SP_IA_VALIDATION_ROSTER_UPLD]
@TEMPLATE_ID NVARCHAR(MAX)
AS
BEGIN
SET NOCOUNT ON;

select h.ROSTER_CODE, ROW_NUMBER() over (order by ID) as ROW_NUM
into #DataWithRowNumber
from HS_HR_IA_ROSTER_INFO_UPLOAD h

SELECT i.COLUMN_NAME, i.ORDINAL_POSITION as COL_NUM
into #ColNameWithColNumber
FROM INFORMATION_SCHEMA.COLUMNS i
WHERE TABLE_NAME = 'HS_HR_IA_ROSTER_INFO_UPLOAD'
group by i.ORDINAL_POSITION, i.COLUMN_NAME

DELETE FROM HS_HR_IA_ERRORS_ROSTER;
DBCC CHECKIDENT ('[HS_HR_IA_ERRORS_ROSTER]', RESEED, 0);

--For "ROSTER_CODE" No 1
insert into HS_HR_IA_ERRORS_ROSTER
select ROSTER_CODE, @TEMPLATE_ID, 'Roster Code Can Not Be Null', '', 'ROSTER_CODE','' from HS_HR_IA_ROSTER_INFO_UPLOAD
where ROSTER_CODE IS NULL;

select * into #tempTable1 from #DataWithRowNumber where ROSTER_CODE IS NULL ORDER by (SELECT NULL) OFFSET 1 rows;

INSERT INTO HS_HR_IA_ERRORS_ROSTER (ROSTER_CODE,TEMPLATE_ID,ERROR_DETAILS,COL_NUMBER,COL_NAME,ROW_NUMBER) SELECT ROSTER_CODE,@TEMPLATE_ID,'Roster Code Can Not Be Null', '1','ROSTER_CODE', ROW_NUM from #tempTable1;

SELECT * into #tempTable2
FROM #DataWithRowNumber
WHERE ROSTER_CODE IN (
    SELECT ROSTER_CODE
    FROM #DataWithRowNumber
    GROUP BY ROSTER_CODE
    HAVING COUNT(*) > 1
) ORDER BY (SELECT NULL) OFFSET 1 ROWS;

INSERT INTO HS_HR_IA_ERRORS_ROSTER (ROSTER_CODE,TEMPLATE_ID,ERROR_DETAILS,COL_NUMBER,COL_NAME,ROW_NUMBER) SELECT ROSTER_CODE,@TEMPLATE_ID,'Roster Code is duplicated', '1','ROSTER_CODE', ROW_NUM from #tempTable2;


--For "Roster Name" No 2
insert into [QADBV9_L2].HS_HR_IA_ERRORS_ROSTER
select ROSTER_CODE, @TEMPLATE_ID, 'Please Specify a valid value for Roster Name', '', 'ROSTER_NAME','' from HS_HR_IA_ROSTER_INFO_UPLOAD
where ROSTER_NAME IS NULL;

--For "ROS_IS_ACTIVE" No 3
insert into [QADBV9_L2].HS_HR_IA_ERRORS_ROSTER
select ROSTER_CODE, @TEMPLATE_ID, 'Please Specify a valid value for Active', '', 'ROS_IS_ACTIVE','' from HS_HR_IA_ROSTER_INFO_UPLOAD
where ROS_IS_ACTIVE IS NULL;


--For "ROS_GRP_NAME" No 4
insert into [QADBV9_L2].HS_HR_IA_ERRORS_ROSTER
select ROSTER_CODE, @TEMPLATE_ID, 'Please Specify a valid value for Roster Group Name', '', 'ROS_GRP_NAME','' from HS_HR_IA_ROSTER_INFO_UPLOAD
where ROS_GRP_NAME IS NULL;

--For "ROS_GRP_DESC" No 5
insert into [QADBV9_L2].HS_HR_IA_ERRORS_ROSTER
select ROSTER_CODE, @TEMPLATE_ID, 'Please Specify a valid value for Roster Group Description', '', 'ROS_GRP_DESC','' from HS_HR_IA_ROSTER_INFO_UPLOAD
where ROS_GRP_DESC IS NULL;

--For "ROS_HIE" No 7
insert into [QADBV9_L2].HS_HR_IA_ERRORS_ROSTER
select ROSTER_CODE, @TEMPLATE_ID, 'Please Specify a valid value for Hierarchy', '', 'ROS_HIE','' from HS_HR_IA_ROSTER_INFO_UPLOAD
where ROS_HIE IS NULL;

--For "ROS_COMP_CALENDER" No 8
insert into [QADBV9_L2].HS_HR_IA_ERRORS_ROSTER
select ROSTER_CODE, @TEMPLATE_ID, 'Please Specify a valid value for Company Calender', '', 'ROS_COMP_CALENDER','' from HS_HR_IA_ROSTER_INFO_UPLOAD
where ROS_COMP_CALENDER IS NULL;

--For "ROS_WRKFLW_TYPE" No 9
insert into [QADBV9_L2].HS_HR_IA_ERRORS_ROSTER
select ROSTER_CODE, @TEMPLATE_ID, 'Please Specify a valid value for WorkFlow Type', '', 'ROS_WRKFLW_TYPE','' from HS_HR_IA_ROSTER_INFO_UPLOAD
where ROS_WRKFLW_TYPE IS NULL;


update E set E.COL_NUMBER = c.COL_NUM, E.ROW_NUMBER = d.ROW_NUM
from HS_HR_IA_ERRORS_ROSTER E
inner join #DataWithRowNumber d on d.ROSTER_CODE = E.ROSTER_CODE OR (E.ROSTER_CODE IS NULL AND d.ROSTER_CODE IS NULL)
inner join #ColNameWithColNumber c on c.COLUMN_NAME = E.COL_NAME

drop table #ColNameWithColNumber;
drop table #DataWithRowNumber;
drop table #tempTable1;
drop table #tempTable2;
END
GO