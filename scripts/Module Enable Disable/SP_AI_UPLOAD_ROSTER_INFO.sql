---------------------------------Create Roster-----------------------------

IF  EXISTS (SELECT * FROM SYSOBJECTS WHERE [NAME] ='SP_AI_UPLOAD_ROSTER_INFO')
DROP PROCEDURE SP_AI_UPLOAD_ROSTER_INFO
GO

CREATE PROCEDURE SP_AI_UPLOAD_ROSTER_INFO
AS 
BEGIN
DECLARE @RGP_ID_CAL VARCHAR(9)
DECLARE @ROS_CODE_CAL VARCHAR(8)
DECLARE @CG_GRP_CODE_CAL VARCHAR(20)


BEGIN TRY  
--Get ROster Group Code
SET @RGP_ID_CAL = RIGHT('000000' + CAST((SELECT ISNULL(MAX(RGP_ID), 0)  FROM HS_TA_ROSTERGROUP) AS VARCHAR(9)), 6)

--Get Shift Code
SET @ROS_CODE_CAL = RIGHT('000000' + CAST((SELECT ISNULL(MAX(ROS_CODE), 0)  FROM HS_TA_ROSTER_DEF) AS VARCHAR(8)), 6)

SELECT * INTO #temp1 FROM (SELECT RIGHT('000000'+CAST(@RGP_ID_CAL + ROW_NUMBER() OVER (ORDER BY [ID]) AS varchar(25)),6) AS RGP_ID,
RG.ROS_GRP_NAME AS RG_NAME,RG.ROS_GRP_DESC RG_DESC,
(
        SELECT
            CASE
                WHEN EXISTS (
                    SELECT 1
                    FROM HS_HR_LCALENDER_GROUP
                    WHERE CG_GRP_DESC = RG.ROS_COMP_CALENDER
                )
                THEN (
                    SELECT CG_GRP_CODE
                    FROM HS_HR_LCALENDER_GROUP
                    WHERE CG_GRP_DESC = RG.ROS_COMP_CALENDER
                )
                ELSE 'Default'
            END
    ) AS HOL_GRP_CODE,
(
        SELECT
            CASE
                WHEN EXISTS (
                    SELECT 1
                    FROM HS_HR_COMPANY_HIERARCHY
                    WHERE HIE_NAME = RG.ROS_HIE
                )
                THEN (
                    SELECT HIE_CODE
                    FROM HS_HR_COMPANY_HIERARCHY
                    WHERE HIE_NAME = RG.ROS_HIE
                )
                ELSE 'Default'
            END
    ) AS HIE_CODE,
1 AS RG_IS_ACTIVE,0 AS RG_IS_DEFAULT, RG.ROS_WRKFLW_TYPE AS RG_DEFAULT_WF_TYPE, RG.ROS_GRP_CMNTS AS RG_COMMENT, RG.DYN_ROS_SUPERVISOR AS DYNMIC_ROS_SUPERVISOR, NULL AS DBGROUP_ID, 1 AS RG_MOVEMENT_TRACK_MODE,ROW_NUMBER() over (partition by RG.ROS_GRP_NAME order by RG.ROS_GRP_NAME) As row_num
FROM  HS_HR_IA_ROSTER_INFO_UPLOAD RG) AS X;



-- Insert Roster Group
INSERT INTO [HS_TA_ROSTERGROUP]
           ([RGP_ID]
           ,[RG_NAME]
           ,[RG_DESC]
           ,[HOL_GRP_CODE]
		   ,[HIE_CODE]
		   ,[RG_IS_ACTIVE]
		   ,[RG_IS_DEFAULT]
		   ,[RG_DEFAULT_WF_TYPE]
		   ,[RG_COMMENT]
		   ,[DYNMIC_ROS_SUPERVISOR]
		   ,[DBGROUP_ID]
		   ,[RG_MOVEMENT_TRACK_MODE])
	SELECT [RGP_ID]
           ,[RG_NAME]
           ,[RG_DESC]
           ,[HOL_GRP_CODE]
		   ,[HIE_CODE]
		   ,[RG_IS_ACTIVE]
		   ,[RG_IS_DEFAULT]
		   ,[RG_DEFAULT_WF_TYPE]
		   ,[RG_COMMENT]
		   ,[DYNMIC_ROS_SUPERVISOR]
		   ,[DBGROUP_ID]
		   ,[RG_MOVEMENT_TRACK_MODE] FROM #temp1 where row_num=1;

DROP TABLE #temp1;

--Insert Roster
INSERT INTO [HS_TA_ROSTER_DEF]
           ([ROS_CODE]
           ,[ROS_DIS_CODE]
           ,[ROS_NAME]
           ,[RGP_ID]
		   ,[ROS_IS_ACTIVE]
		   ,[ROS_SUP_EMP]
		   ,[DBGROUP_ID]
		   ,[ROS_IS_PRIOR_OT_ENABLED]
		   ,[ROS_COMMENT])
SELECT RIGHT('000000'+CAST(@ROS_CODE_CAL + ROW_NUMBER() OVER (ORDER BY [ID]) AS varchar(25)),6), 
RG.ROSTER_CODE, RG.ROSTER_NAME,
(SELECT RGP_ID FROM HS_TA_ROSTERGROUP WHERE RG_NAME = RG.ROS_GRP_NAME AND RG_DESC = RG.ROS_GRP_DESC),
RG.ROS_IS_ACTIVE, NULL, NULL, 0, RG.ROSTER_COMMENT 
FROM  HS_HR_IA_ROSTER_INFO_UPLOAD RG

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