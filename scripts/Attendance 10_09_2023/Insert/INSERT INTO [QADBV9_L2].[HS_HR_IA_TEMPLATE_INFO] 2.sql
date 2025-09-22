INSERT INTO [QADBV9_L2].[HS_HR_IA_TEMPLATE_INFO]
           ([TEMPLATE_ID]
           ,[COL_DBNAME]
           ,[COL_EXCELNAME]
           ,[COL_DATATYPE])
     VALUES
           ('temp8','SHIFT_NAME','Shift Name','varchar'),
		   ('temp8','SHIFT_ABR','Shift Abriviation','varchar'),
		   ('temp8','START_TIME','Start Time','numeric'),
		   ('temp8','END_TIME','End Time','numeric'),
		   ('temp8','FIRST_HALF_DUR','First Half Duration','numeric'),
		   ('temp8','SECOND_HALF_DUR','Second Half Duration','numeric'),
		   ('temp8','FLEXI_SHIFT','Flexi shift','int'),
		   ('temp8','OFF_SHIFT','Off shift','int'),
		   ('temp8','CONTINUE_SHIFT','Continue Shift','int'),
		   ('temp8','LATE_COVER','Late covering','int'),
		   ('temp8','BRK_ALLOW_LATE_HRS_CALC','Break allow to late hrs calculation','int'),
		   ('temp8','ALLOW_DEDCT_OUT_HRS','Allow deduct out hrs from OT hrs','int'),
		   ('temp8','AUTO_MID_NIGHT_FIX','Automatic mid night cross-over fix','int'),
		   ('temp8','NEXT_DAY_SHIFT_OUT_TIME','Next day shift out time','int'),
		   ('temp8','LEAVE_DAYS','Leave Days','numeric'),
		   ('temp8','OT_ROUND_METHOD','OT round method','varchar'),
		   ('temp8','OT_ROUNDING','OT rounding (hh:mm:)','numeric'),
		   ('temp8','GRACE_PERIOD','Grace period','numeric'),
		   ('temp8','BREAK_NAME','Break name','varchar'),
		   ('temp8','BREAK_START_TIME','Break start time','numeric'),
		   ('temp8','BREAK_DURATION','Break duration (hours)','numeric'),
		   ('temp8','SHIFT_CODE','Shift Code','varchar')
GO