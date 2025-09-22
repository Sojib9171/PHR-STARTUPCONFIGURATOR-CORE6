INSERT INTO [QADBV9_L2].[HS_HR_PARAMETER]
           ([PARA_SEQNO]
           ,[PARA_NAME]
           ,[PARA_VALUE]
           ,[MODULE_ID]
           ,[PARA_DESC]
           ,[PARA_CONTROLTYPE]
           ,[PARA_SQL]
           ,[PARA_TYPE]
           ,[PARA_VALUES]
           ,[PARA_CAP]
           ,[PARA_VALUEHIDDEN]
           ,[PAR_STACK])
     VALUES
           ((SELECT TOP 1 PARA_SEQNO FROM HS_HR_PARAMETER ORDER BY PARA_SEQNO DESC)+1
		   ,'PHR_CONFIG_ASSIST_COPYRIGHTS'
           ,'PeoplesHR, 2023'
           ,NULL
           ,NULL
           ,NULL
           ,NULL
           ,NULL
           ,NULL
           ,NULL
           ,NULL
           ,NULL)
GO