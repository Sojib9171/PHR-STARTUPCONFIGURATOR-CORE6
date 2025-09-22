DELETE FROM HS_HR_IA_COMN_CNFG_RADIO_OPTNS

DELETE FROM HS_HR_IA_COMMON_CNFG_QUESTIONS

INSERT INTO HS_HR_IA_COMMON_CNFG_QUESTIONS(QUESTION_NO,QUESTION_STATEMENT,QUESTION_TYPE)
VALUES
(1,'Please choose a preferred username for your accounts'+ CHAR(13)+CHAR(10) +'Instruction: Select one of the options below', 'Select Option'),
(2,'Would you like to change the Login page logo? If yes, please upload the new logo image. If not, we will proceed with the default logo'+ CHAR(13)+CHAR(10) +'Instruction: Accepted file formats include JPEG & PNG, Ensure that the image is of high quality and has appropriate dimensions for optimal display', 'Image'),
(3,'Do you have an employee number that needs to be manually entered into the system?'+ CHAR(13)+CHAR(10) +'Note: Make sure to accurately indicate whether you have an employee number or not, as this information will determine the next steps in the process', 'Yes/No'),
(4,'Would you like to change the login screen logo in the mobile app?'+ CHAR(13)+CHAR(10) +'Instruction: If have the new logo image ready, please proceed with uploading it. If you continue without uploading, we will proceed with the default logo in the mobile app', 'Image'),
(5,'Would you like to enable the Report Navigator (My Report) feature in the self-service?'+ CHAR(13)+CHAR(10) +'Instruction: Please select one of the following options', 'Yes/No'),
(6,'If there are any other configurations that should be included, please feel free to list them down here', 'Text')

INSERT INTO [QADBV9_L2].[HS_HR_IA_COMN_CNFG_RADIO_OPTNS]
(QUESTION_NO, OPTION_LABEL,OPTION_VALUE) VALUES
(1,'Full Name: We can extract a username from your full name','Full Name'),
(1,'First Name and Last Name: We can combine your first name and last name to form a username','First Name and Last Name'),
(1,'Employee No: we can use it as your username','Employee No'),
(1,'Employee Official Email: We can use your official email address without the domain extension as your username','Employee Official Email'),
(3,'YES: If you have an employee number, you will be entering manually','Yes'),
(3,'NO: If you do not have an employee number, you can proceed to the next step','No'),
(5,'YES: If you want to enable the Report Navigator (My Report) feature in the self-service','Yes'),
(5,'NO: If you prefer to keep the current setting','No')
