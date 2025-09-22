--..........................................................supervisor data....................................................................................

if  EXISTS (select * from sysobjects where [name] ='SP_AI_UPLOAD_REPORTTO')
drop  PROCEDURE SP_AI_UPLOAD_REPORTTO
go

CREATE PROCEDURE SP_AI_UPLOAD_REPORTTO
AS 
BEGIN

BEGIN TRY  

-- insert direct supervisor
INSERT INTO [HS_HR_EMP_REPORTTO]
           ([EREP_SUB_EMP_NUMBER]
           ,[EREP_REPORTING_MODE]
           ,[EREP_REPORTING_SEQUENCE]
           ,[EREP_SUP_EMP_NUMBER])
SELECT E.EMP_NUMBER,1,NULL,EE.EMP_NUMBER FROM  HS_HR_IA_RH_UPLOAD R
LEFT JOIN HS_HR_EMPLOYEE E ON E.EMP_DISPLAY_NUMBER=R.EMP_NUMBER
LEFT JOIN HS_HR_EMPLOYEE EE ON EE.EMP_DISPLAY_NUMBER=R.DIRECT_SUPERVISOR_NUMBER

-- insert indirect supervisor
INSERT INTO [HS_HR_EMP_REPORTTO]
           ([EREP_SUB_EMP_NUMBER]
           ,[EREP_REPORTING_MODE]
           ,[EREP_REPORTING_SEQUENCE]
           ,[EREP_SUP_EMP_NUMBER])
SELECT E.EMP_NUMBER,2,NULL,EE.EMP_NUMBER FROM  HS_HR_IA_RH_UPLOAD R
LEFT JOIN HS_HR_EMPLOYEE E ON E.EMP_DISPLAY_NUMBER=R.EMP_NUMBER
LEFT JOIN HS_HR_EMPLOYEE EE ON EE.EMP_DISPLAY_NUMBER=R.INDIRECT_SUPERVISOR_NUMBER

     select 'True' as status , 'Successfully added' as message
END TRY  
BEGIN CATCH  
	SELECT  
		'False' as status  
       ,ERROR_MESSAGE() AS message;  
END CATCH  
END