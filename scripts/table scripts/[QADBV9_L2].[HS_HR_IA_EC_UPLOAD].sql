CREATE TABLE [QADBV9_L2].[HS_HR_IA_EC_UPLOAD](
	[EMP_NUMBER] [varchar](8) NULL,
	[EC_CONT_PER_FULLNAME] [varchar](100) NULL,
	[EC_RELATIONSHIP] [varchar](20) NULL,
	[EC_RELATIONSHIP_ORDER] [int] NULL,
	[EC_PER_ADDRESS] [varchar](200) NULL,
	[EC_OFFICE_TELEPHONE] [varchar](20) NULL,
	[EC_RES_TELEPHONE] [varchar](20) NULL,
	[EC_MOBILE] [varchar](20) NULL,
	[EC_CONTACT_TYPE_FLG] [smallint] NULL,
	[EC_OFFICIAL_ADDRESS] [varchar](200) NULL,
	[EC_EMP_NUMBER] [varchar](8) NULL,
	[EC_TEL_EXT] [varchar](20) NULL
) ON [PRIMARY]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Internal Employee Contact' , @level0type=N'SCHEMA',@level0name=N'QADBV9_L2', @level1type=N'TABLE',@level1name=N'HS_HR_IA_EC_UPLOAD', @level2type=N'COLUMN',@level2name=N'EC_EMP_NUMBER'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Internal Telephone Extension' , @level0type=N'SCHEMA',@level0name=N'QADBV9_L2', @level1type=N'TABLE',@level1name=N'HS_HR_IA_EC_UPLOAD', @level2type=N'COLUMN',@level2name=N'EC_TEL_EXT'
GO


