--Modify host file in C:\Windows\System32\drivers\etc\hosts
--Append:
--127.0.0.1 		impaut.localhost.com
--Host the API in IIS using "impaut.localhost.com" hostname
--the subdomain name impaut will be used to get DB connection from below table data:


INSERT INTO [QADBV9_L2].[HS_CLIENT_SYSTEM_CONFIG]
           ([CLISYSCON_ID]
           ,[CLISYSCON_CUS_CODE]
           ,[CLISYSCON_DB_TYPE]
           ,[CLISYSCON_SVR_LIVE]
           ,[CLISYSCON_DB_NAME_LIVE]
           ,[CLISYSCON_DB_USRID_LIVE]
           ,[CLISYSCON_DB_PWD_LIVE]
           ,[CLISYSCON_DSN_LIVE]
           ,[CLISYSCON_REPORT_PATH]
           ,[CLISYSCON_ODBC_DRIVER_NAME]
           ,[CLISYSCON_HOST_NAME]
           ,[CLISYSCON_ISACTIVE]
           ,[CLISYSCON_DB_VERSION]
           ,[CLISYSCON_DEFTB_SPACE]
           ,[CLISYSCON_TEMTB_SPACE]
           ,[CLISYSCON_DB_USRID_MASTER]
           ,[CLISYSCON_DB_PWD_MASTER]
           ,[CLISYSCON_SUBDOMAIN_EHRM]
           ,[CLISYSCON_SUBDOMAIN_SSHR]
           ,[CLISYSCON_ESN_EMAIL_DOMAIN]
           ,[CLISYSCON_ESN_ID]
           ,[CLISYSCON_ESN_URL])
VALUES 
('1', 
'LocalHost', 
'SQL SERVER', 
'DESKTOP-RJU5583', --Change it to your SQL Server Name
'QADBV9_L2', --Change it to your SQL Database Name
'QADBV9_L2', --Change it to your SQL User Name
'QADBV9_L2', --Change it to your SQL User Password
'DESKTOP-RJU5583', --Change it to your SQL Server Name
'/',
'SQL SERVER',
NULL,
'1',
'2018',
'QADBV9_L2', --Change it to your SQL User Name
NULL,
NULL,
NULL,
'configAssist',
'configAssist',
NULL,
NULL,
NULL
)

