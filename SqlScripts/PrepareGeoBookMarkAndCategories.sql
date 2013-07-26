---------------------------------------------------------------------------------------------
-- THIS SAMPLE SETUP THAT WILL SET AUTOMATICALLY ON WEB ADMINTOOL
-- USE [OasisDB-Atlantis]
---------------------------------------------------------------------------------------------
DECLARE @cmd NVARCHAR(4000)
DECLARE cmds CURSOR FOR
SELECT N' 
    EXEC sp_RENAME ''' + INFORMATION_SCHEMA.columns.Table_Name + '.[sGUID]'' , ''GUID1'', ''COLUMN''
'
FROM  INFORMATION_SCHEMA.columns
INNER JOIN INFORMATION_SCHEMA.tables ON INFORMATION_SCHEMA.tables.TABLE_NAME = INFORMATION_SCHEMA.columns.TABLE_NAME AND INFORMATION_SCHEMA.tables.TABLE_TYPE = 'BASE TABLE' 
WHERE ( right(INFORMATION_SCHEMA.columns.TABLE_NAME, 12) = 'GeoBookMarks' OR right(INFORMATION_SCHEMA.columns.TABLE_NAME, 22) = 'GeoBookMarksCategories')
AND Information_SCHEMA.columns.COLUMN_NAME = 'sGUID'
OPEN cmds
WHILE 1=1
BEGIN
	FETCH cmds INTO @cmd
	IF @@fetch_status != 0 BREAK
	EXEC(@cmd)
END
DEALLOCATE cmds
GO

