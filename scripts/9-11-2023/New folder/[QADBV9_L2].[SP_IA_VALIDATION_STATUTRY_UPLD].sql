ALTER PROCEDURE [QADBV9_L2].[SP_IA_VALIDATION_STATUTRY_UPLD]
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
insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select [LEAVE_TYPE], @TEMPLATE_ID, 'Please specify a valid value Leave Type Code', '', 'LEAVE_TYPE','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where [LEAVE_TYPE] IS NULL;

insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please specify a valid value Leave Type Code', '', 'LEAVE_TYPE','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where LEAVE_TYPE IS NOT NULL AND len(LEAVE_TYPE) > 50;


--For "LEAVE_DESCRIPTION" No 2

insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select [LEAVE_TYPE], @TEMPLATE_ID, 'Please specify a valid value for Leave Description', '', 'LEAVE_DESCRIPTION','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where LEAVE_DESCRIPTION IS NULL;

insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please specify a valid value for Leave Description', '', 'LEAVE_DESCRIPTION','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where LEAVE_DESCRIPTION IS NOT NULL AND len(LEAVE_TYPE) > 100;



--For "ACTIVE" No 3

insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please Specify a valid value for Active', '', 'ACTIVE','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where ACTIVE NOT IN(0,1);


--For "COVERING_EMP_REQ" No 4 
insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please Specify a valid value for Covering Emp. Required', '', 'COVERING_EMP_REQ','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where COVERING_EMP_REQ is not null and COVERING_EMP_REQ NOT IN (0,1);

--For "ALLOW_HALF_DAYS" No 5
insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please Specify a valid value for Allow Half Days', '', 'ALLOW_HALF_DAYS','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where ALLOW_HALF_DAYS is not null and ALLOW_HALF_DAYS  NOT IN (0,1);

--For "AUTO_CAL_END_DATE" No 6 
insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please Specify a valid value for Auto Calculate End Date ', '', 'AUTO_CAL_END_DATE','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where AUTO_CAL_END_DATE is not null and AUTO_CAL_END_DATE  NOT IN (0,1);

--For "HOLIDAY_AS_LEAVE" No 7
insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please Specify a valid value for Holiday as Leave ', '', 'HOLIDAY_AS_LEAVE','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where HOLIDAY_AS_LEAVE NOT IN (0,1);

--For "LIEU_LEAVE" No 8
insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please Specify a valid value for Lieu Leave ', '', 'LIEU_LEAVE','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where LIEU_LEAVE NOT IN (0,1);

--For "MATERNITY_LEAVE" No 9
insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please Specify a valid value for Maternity Leave', '', 'MATERNITY_LEAVE','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where MATERNITY_LEAVE NOT IN (0,1);

--For "NO_PAY_LEAVE" No 10
insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please Specify a valid value for No Pay Leave ', '', 'NO_PAY_LEAVE','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where NO_PAY_LEAVE NOT IN (0,1);

--For "QUARTER_LEAVE" No 11
insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please Specify a valid value for Quarter Leave', '', 'QUARTER_LEAVE','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where QUARTER_LEAVE NOT IN (0,1);


--For "SHOW_BALANCE" No 12
insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please Specify a valid value for Show Balance ', '', 'SHOW_BALANCE','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where SHOW_BALANCE NOT IN (0,1);

--For "EARNED_CF_BREAK_DOWN" No 13
insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please Specify a valid value for Earned/CF Break Down', '', 'EARNED_CF_BREAK_DOWN','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where EARNED_CF_BREAK_DOWN NOT IN (0,1);


--For "EXTENDED_PERIOD" No 14
insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please Specify a valid value for Extended Period', '', 'EXTENDED_PERIOD','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where EXTENDED_PERIOD NOT IN (0,1);

--For "ALLOW_PLANNING" No 15
insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please Specify a valid value for Allow Planning', '', 'ALLOW_PLANNING','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where ALLOW_PLANNING NOT IN (0,1);



--For "ATTACHMENT_REQUIRED" No 16
insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please specify a valid value for Attachment Required', '', 'ATTACHMENT_REQUIRED','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where ATTACHMENT_REQUIRED NOT IN (0,1);

--For "VIEW_ADDITIONAL_FIELD" No 17
insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please specify a valid value for View Additional Field', '', 'VIEW_ADDITIONAL_FIELD','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where VIEW_ADDITIONAL_FIELD NOT IN (0,1);


--For "ALLOW_NEGATIVE" No 18
insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please specify a valid value for Allow Negative', '', 'ALLOW_NEGATIVE','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where ALLOW_NEGATIVE NOT IN (0,1);

--For "KIOSK_ALLOWED" No 19
insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please specify a valid value for Kiosk Allowed', '', 'KIOSK_ALLOWED','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where KIOSK_ALLOWED NOT IN (0,1);


--For "HIDE_ZERO_ENTITLEMENT" No 20 
insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please specify a valid value for Hide Zero Entitlement', '', 'HIDE_ZERO_ENTITLEMENT','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where HIDE_ZERO_ENTITLEMENT NOT IN (0,1);

--For "ALLOW_SELECT_APPROVER" No 21
insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please specify a valid value for Allow Select Approver', '', 'ALLOW_SELECT_APPROVER','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where ALLOW_SELECT_APPROVER NOT IN (0,1);


--For "SHOW_LEAVE_REASON" No 22
insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please specify a valid value for Show Leave Reason', '', 'SHOW_LEAVE_REASON','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where SHOW_LEAVE_REASON NOT IN (0,1);

--For "CONSIDER_IN_DASHBOARD" No 23
insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please specify a valid value for Consider in Dashboard', '', 'CONSIDER_IN_DASHBOARD','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where CONSIDER_IN_DASHBOARD NOT IN (0,1);


--For "SHOW_IN_OFFBOARDING" No 24 
insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please specify a valid value for Show in Offboarding', '', 'SHOW_IN_OFFBOARDING','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where SHOW_IN_OFFBOARDING NOT IN (0,1);

--For "CONSIDER_FOR_TIME_LOST" No 25
insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please specify a valid value for Consider for Time Lost', '', 'CONSIDER_FOR_TIME_LOST','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where CONSIDER_FOR_TIME_LOST NOT IN (0,1);


--For "COMMENT_MANDATORY" No 26
insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please specify a valid value for Comment Mandatory', '', 'COMMENT_MANDATORY','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where COMMENT_MANDATORY NOT IN (0,1);

--For "LONG_LEAVE" No 27
insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please specify a valid value for Long Leave', '', 'LONG_LEAVE','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where LONG_LEAVE NOT IN (0,1);


--For "HIDE_IN_SELF_APP" No 28
insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please specify a valid value for Hide in Self Application', '', 'HIDE_IN_SELF_APP','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where HIDE_IN_SELF_APP NOT IN (0,1);


--For "APPLY_SEQUENCE_IN_DASH" No 29
insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please specify a valid value for Apply Sequence in Dashboard', '', 'APPLY_SEQUENCE_IN_DASH','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where APPLY_SEQUENCE_IN_DASH IS NOT NULL AND len(APPLY_SEQUENCE_IN_DASH) > 4

insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please specify a valid value for Apply Sequence in Dashboard', '', 'APPLY_SEQUENCE_IN_DASH','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where APPLY_SEQUENCE_IN_DASH IS NOT NULL AND APPLY_SEQUENCE_IN_DASH=0;

insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE 
SELECT LEAVE_TYPE, 'temp7', 'Please specify a valid value for Apply Sequence in Dashboard', '', 'APPLY_SEQUENCE_IN_DASH',''
FROM HS_HR_IA_STATUTRY_LEAVE_UPLOAD
WHERE APPLY_SEQUENCE_IN_DASH IN (
    SELECT APPLY_SEQUENCE_IN_DASH
    FROM HS_HR_IA_STATUTRY_LEAVE_UPLOAD
    GROUP BY APPLY_SEQUENCE_IN_DASH
    HAVING COUNT(*) > 1
)


--For "LEV_TYPE_MAX_DAYS" No 30 
insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please specify a valid value for Maximum Leave Days in a Single Instance', '', 'LEV_TYPE_MAX_DAYS','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where LEV_TYPE_MAX_DAYS IS NOT NULL AND len(LEV_TYPE_MAX_DAYS) > 7

--For "YEARLY_ENTITLEMENT" No 4 
insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please specify a valid value for Yearly Entitlement (Days)', '', 'LEV_TYPE_MAX_DAYS','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where  YEARLY_ENTITLEMENT IS NOT NULL AND len(YEARLY_ENTITLEMENT) > 18

--For "MINIMUM_DAYS_IN_SNGLE_LEA_APP" No 31

insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please specify a valid value for Minimum days in a single leave application', '', 'MINIMUM_DAYS_IN_SNGLE_LEA_APP','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where  MINIMUM_DAYS_IN_SNGLE_LEA_APP IS NOT NULL AND len(MINIMUM_DAYS_IN_SNGLE_LEA_APP) > 7

insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please specify a valid value for Minimum days in a single leave application', '', 'MINIMUM_DAYS_IN_SNGLE_LEA_APP','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where MINIMUM_DAYS_IN_SNGLE_LEA_APP <= 0


insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please specify a valid value for Minimum days in a single leave application', '', 'MINIMUM_DAYS_IN_SNGLE_LEA_APP','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where MINIMUM_DAYS_IN_SNGLE_LEA_APP IS NULL AND MAXIMUM_DAYS_IN_SNGLE_LEA_APP IS NOT NULL;

--For "MAXIMUM_DAYS_IN_SNGLE_LEA_APP" No 32

insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please specify a valid value for Maximum days in a single leave application', '', 'MAXIMUM_DAYS_IN_SNGLE_LEA_APP','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where  MAXIMUM_DAYS_IN_SNGLE_LEA_APP IS NOT NULL AND len(MAXIMUM_DAYS_IN_SNGLE_LEA_APP) > 7

insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please specify a valid value for Maximum days in a single leave application', '', 'MAXIMUM_DAYS_IN_SNGLE_LEA_APP','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where  MAXIMUM_DAYS_IN_SNGLE_LEA_APP < MINIMUM_DAYS_IN_SNGLE_LEA_APP;

insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please specify a valid value for Maximum days in a single leave application', '', 'MAXIMUM_DAYS_IN_SNGLE_LEA_APP','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where MAXIMUM_DAYS_IN_SNGLE_LEA_APP IS NULL AND MINIMUM_DAYS_IN_SNGLE_LEA_APP IS NOT NULL;



--For "REQ_MEDICAL_CERTIFICATE" No 32 

insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please specify a valid value for Require medical certificate after how many number of absence days', '', 'REQ_MEDICAL_CERTIFICATE','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where  REQ_MEDICAL_CERTIFICATE IS NOT NULL AND len(REQ_MEDICAL_CERTIFICATE) > 6

insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please specify a valid value for Require medical certificate after how many number of absence days', '', 'REQ_MEDICAL_CERTIFICATE','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where  REQ_MEDICAL_CERTIFICATE is not null and REQ_MEDICAL_CERTIFICATE  <= 0

--For "APPLY_TGTHER_WITH_LEA_TYPE" No 33

insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please specify a valid value for Specify leave types not allowed to apply together with this leave type', '', 'APPLY_TGTHER_WITH_LEA_TYPE','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where  APPLY_TGTHER_WITH_LEA_TYPE IS NOT NULL AND len(APPLY_TGTHER_WITH_LEA_TYPE) > 50

--For "FUTURE_LEAVE_UTILIZED_AS" No 34

insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please specify a valid value for Future Leave Utilized As', '', 'FUTURE_LEAVE_UTILIZED_AS','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where  FUTURE_LEAVE_UTILIZED_AS IS NOT NULL AND len(FUTURE_LEAVE_UTILIZED_AS) > 50

insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please specify a valid value for Future Leave Utilized As', '', 'FUTURE_LEAVE_UTILIZED_AS','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where  FUTURE_LEAVE_UTILIZED_AS IS NULL AND UTILIZATION_PERIOD IS NOT NULL;

--For "UTILIZATION_PERIOD" No 35 

insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please specify a valid value for Utilization Period (Months)', '', 'UTILIZATION_PERIOD','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where  UTILIZATION_PERIOD IS NOT NULL AND len(UTILIZATION_PERIOD) > 4;

insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please specify a valid value for Utilization Period (Months)', '', 'UTILIZATION_PERIOD','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where  FUTURE_LEAVE_UTILIZED_AS IS NOT NULL AND (UTILIZATION_PERIOD IS NULL OR UTILIZATION_PERIOD<=0);

insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please specify a valid value for Utilization Period (Months)', '', 'UTILIZATION_PERIOD','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where UTILIZATION_PERIOD > 12;

insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please specify a valid value for Utilization Period (Months)', '', 'UTILIZATION_PERIOD','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where  FUTURE_LEAVE_UTILIZED_AS IS NOT NULL AND UTILIZATION_PERIOD IS NULL;


-- For Future Leave Type Name
insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'If balance carry forward as future leave, what is the name of the future leave type', '', 'FUTURE_LEAVE_TYPE_NAME','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where  FUTURE_LEAVE_TYPE_NAME IS NULL AND (LEV_TYPE_CFWD_YEARLY IS NOT NULL OR LEV_TYPE_CFWD_LIMIT IS NOT NULL);



--For "LEV_TYPE_CFWD_YEARLY" No 37 
insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please specify a valid value for Maximum allowed to carry forward as future leave', '', 'LEV_TYPE_CFWD_YEARLY','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where  LEV_TYPE_CFWD_YEARLY IS NOT NULL AND len(LEV_TYPE_CFWD_YEARLY) > 7

insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please specify a valid value for Maximum allowed to carry forward as future leave', '', 'LEV_TYPE_CFWD_YEARLY','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where  LEV_TYPE_CFWD_LIMIT IS NOT NULL AND LEV_TYPE_CFWD_YEARLY IS NULL;

insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please specify a valid value for Maximum allowed to carry forward as future leave', '', 'LEV_TYPE_CFWD_YEARLY','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where  MAXIMUM_DAYS_IN_SNGLE_LEA_APP IS NOT NULL AND (LEV_TYPE_CFWD_YEARLY IS NULL OR LEV_TYPE_CFWD_YEARLY =0);

insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please specify a valid value for Maximum allowed to carry forward as future leave', '', 'LEV_TYPE_CFWD_YEARLY','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where  LEV_TYPE_CFWD_YEARLY IS NOT NULL AND (LEV_TYPE_CFWD_LIMIT IS NULL OR FUTURE_LEAVE_TYPE_NAME IS NULL);

--For "LEV_TYPE_CFWD_LIMIT" No 38

insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please specify a valid value for What is the carry forward maximum limit(Ceiling limit)', '', 'LEV_TYPE_CFWD_LIMIT','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where  LEV_TYPE_CFWD_LIMIT IS NOT NULL AND len(LEV_TYPE_CFWD_LIMIT) > 7

insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please specify a valid value for What is the carry forward maximum limit(Ceiling limit)', '', 'LEV_TYPE_CFWD_LIMIT','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where  MAXIMUM_DAYS_IN_SNGLE_LEA_APP IS NOT NULL AND (LEV_TYPE_CFWD_LIMIT IS NULL OR LEV_TYPE_CFWD_LIMIT=0);

insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please specify a valid value for What is the carry forward maximum limit(Ceiling limit)', '', 'LEV_TYPE_CFWD_LIMIT','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where  FUTURE_LEAVE_UTILIZED_AS IS NOT NULL AND (LEV_TYPE_CFWD_LIMIT IS NULL OR LEV_TYPE_CFWD_LIMIT=0);

insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please specify a valid value for What is the carry forward maximum limit(Ceiling limit)', '', 'LEV_TYPE_CFWD_LIMIT','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where  LEV_TYPE_CFWD_LIMIT IS NOT NULL AND (LEV_TYPE_CFWD_YEARLY IS NULL OR FUTURE_LEAVE_TYPE_NAME IS NULL);

--For "NOTICE_PERIOD" No 39 

insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please specify a valid value for If notice required, notice Period in days', '', 'NOTICE_PERIOD','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where  NOTICE_PERIOD IS NOT NULL AND NOTICE_PERIOD = 0

--For "ENTITLEMENT_CRITERIA" No 44

insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please specify a valid value for Entitlement Criteria', '', 'ENTITLEMENT_CRITERIA','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where  ENTITLEMENT_CRITERIA IS NULL;

insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please specify a valid value for Entitlement Criteria', '', 'ENTITLEMENT_CRITERIA','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
where  ENTITLEMENT_CRITERIA IS NOT NULL AND len(ENTITLEMENT_CRITERIA) > 100

--For "COMMENTS" No 45 

insert into [QADBV9_L2].HS_HR_IA_ERRORS_ABSENCE
select LEAVE_TYPE, @TEMPLATE_ID, 'Please specify a valid value for Comments', '', 'COMMENTS','' from HS_HR_IA_STATUTRY_LEAVE_UPLOAD
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
