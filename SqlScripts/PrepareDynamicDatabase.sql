---------------------------------------------------------------------------------------------
-- THIS SAMPLE SETUP THAT WILL SET AUTOMATICALLY ON WEB ADMINTOOL
-- USE [OasisDB-Atlantis]
-- declare @ddname nvarchar(255)
-- set @ddname = 'dd_Yemen_'
---------------------------------------------------------------------------------------------

--------------------------------------------------------------------------------------------- 
-- DO NOT CHANGE ANY CODE AFTER THIS POINT  
--------------------------------------------------------------------------------------------- 
declare @cmd varchar(4000)

IF NOT EXISTS (select * from Information_SCHEMA.columns where Table_name= + @ddname + 'SynchHistoryOverview')
BEGIN
	exec('CREATE TABLE [' + @ddname + 'SynchHistoryOverview] (
		[sTableName] [nvarchar](255) NULL,
		[sWhenMinSearched] [nvarchar](255) NULL,
		[sWhenMaxSearched] [nvarchar](255) NULL
	) ON [PRIMARY]')
END

IF NOT EXISTS (select * from Information_SCHEMA.columns where Table_name= + @ddname + 'ComboRelations')
BEGIN
	exec('CREATE TABLE [dbo].[' + @ddname + 'ComboRelations](
		[GUID1] [nvarchar](255) NULL,
		[sTableName] [nvarchar](255) NULL,
		[sFieldName] [nvarchar](255) NULL,
		[sParentFieldName] [nvarchar](255) NULL
	) ON [PRIMARY]')
END

IF NOT EXISTS (select * from Information_SCHEMA.columns where Table_name= + @ddname + 'ChartSettings')
BEGIN
	exec('CREATE TABLE [dbo].[' + @ddname + 'ChartSettings](
		[GUID1] [nvarchar](255) NULL,
		[QueryName] [nvarchar](255) NULL,
		[OCTSettings] [image] NULL,
		[SQLCommand] [ntext] NULL,
		[MSSQLCommand] [ntext] NULL,
		[WebTemplate] [ntext] NULL,
		[UseChart] [bit] NULL,
		[bAutoLoadReport] [bit] NULL,
		[FilterSQL] [ntext] NULL,
		[FilterMSSQL] [ntext] NULL,
		[Group] [nvarchar](255) NULL
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]')

END

IF NOT EXISTS (select * from Information_SCHEMA.columns where Table_name= + @ddname + 'SynchHistory')
BEGIN
	exec('CREATE TABLE [dbo].[' + @ddname + 'SynchHistory](
		[sID] [nvarchar](255) NULL,
		[sGUID] [nvarchar](255) NULL,
		[sTableName] [nvarchar](255) NULL,
		[swhen] [nvarchar](255) NULL,
		[sStatus] [nvarchar](255) NULL,
		[sequence] [int] NULL,
		[sBy] [nvarchar](255) NULL,
		[sdelete] [nvarchar](255) NULL,
		[updates] [nvarchar](255) NULL,
		[noconflict] [nvarchar](255) NULL,
		[GUID1] [nvarchar](255) NULL
	) ON [PRIMARY]')
END

IF NOT EXISTS (select * from Information_SCHEMA.columns where Table_name= + @ddname + 'Queries')
BEGIN
	exec('CREATE TABLE [dbo].[' + @ddname + 'Queries](
		[GUID1] [nvarchar](255) NULL,
		[QueryName] [nvarchar](255) NULL,
		[QuerySQL] [ntext] NULL,
		[QueryMSSQL] [ntext] NULL
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]')
END

IF NOT EXISTS (select * from Information_SCHEMA.columns where Table_name= + @ddname + 'NearbyFeatures')
BEGIN
	exec('CREATE TABLE [dbo].[' + @ddname + 'NearbyFeatures](
	 [GUID1] [nvarchar](255) NULL,
	 [sLayerName] [nvarchar](255) NULL,
	 [sLayerFieldName] [nvarchar](255) NULL,
	 [sDataEntryTableName] [nvarchar](255) NULL,
	 [sDataEntryFieldName] [nvarchar](255) NULL,
	 [bCalculateDistance] [bit] NULL
	) ON [PRIMARY]')
END

IF NOT EXISTS (select * from Information_SCHEMA.columns where Table_name= + @ddname + 'Validation')
BEGIN
	exec('CREATE TABLE [dbo].[' + @ddname + 'Validation](
	 [GUID1] [nvarchar](255) NULL,
	 [sDataEntryTableName] [nvarchar](255) NULL,
	 [sDataEntryFieldName] [nvarchar](255) NULL,
	 [bRequired] [bit] NULL,
	 [sEditMask] [nvarchar](255) NULL,
	 [sValidation] [nvarchar](255) NULL
	) ON [PRIMARY]')
END

IF NOT EXISTS (select * from Information_SCHEMA.columns where Table_name= + @ddname + 'Specification')
BEGIN
	exec('CREATE TABLE [dbo].[' + @ddname + 'Specification](
		[GUID1] [nvarchar](255) NULL,
		[lRank] [int] NULL,
		[sTableName] [nvarchar](255) NULL,
		[sCaption] [nvarchar](255) NULL,
		[sDescription] [ntext] NULL,
		[lDescFontSize] [int] NULL,
		[sDataEntryFields] [ntext] NULL,
		[bIsMasterTable] [bit] NOT NULL,
		[bIsLinkedTable] [bit] NOT NULL,
		[sGridQuery] [ntext] NULL,
		[sGridQueryMSSQL] [ntext] NULL
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]')
END

-- add GUID1 field to table if it does not exist already

declare cmds cursor for 
Select'

	IF NOT EXISTS	
	(	select * 
		from Information_SCHEMA.columns
		where Table_name= ''' + Table_Name + ''' and column_name=''GUID1'' 
	)
	BEGIN
		IF NOT(right(''' + Table_name + ''',4) = ''_WKB'') and NOT(right(''' + Table_name + ''',4) = ''_GEO'') and  NOT(right(''' + Table_name + ''',13) = ''_SynchHistory'') and  NOT(right(''' + Table_name + ''',21) = ''_SynchHistoryOverview'')
		BEGIN
			exec(''ALTER TABLE [' + Table_Name + '] ADD [GUID1] nvarchar(255)'')
			exec(''update [' + Table_Name + '] set [guid1] = newid()'')
		END
	END

	IF EXISTS	
	(	select * 
		from Information_SCHEMA.columns
		where Table_name= ''' + Table_Name + ''' and column_name=''GUID1'' and (right(''' + Table_name + ''',4) = ''_GEO'' OR  (right(''' + Table_name + ''',13) = ''_SynchHistory'') OR  (right(''' + Table_name + ''',21) = ''_SynchHistoryOverview''))
	)
	BEGIN
		exec(''ALTER TABLE [' + Table_Name + '] DROP COLUMN [GUID1]'')
	END
'
From    INFORMATION_SCHEMA.TABLES
Where   Table_Name like @ddname + '%' and not table_type = 'view' AND  Table_Name not like '%_qry%' AND  Table_Name not like '%_WKB'

open cmds
while 1=1
begin
    fetch cmds into @cmd
    if @@fetch_status != 0 break
    exec(@cmd)
end

deallocate cmds

-- modify column type if = nvarchar(MAX) - change to ntext

declare cmds cursor for 
Select'

	exec(''ALTER TABLE [' + Table_Name + '] ALTER COLUMN ' + column_name + ' ntext'')
	
'
From    INFORMATION_SCHEMA.columns 
Where   Table_Name like @ddname + '%' and character_maximum_length = -1 and data_type = 'nvarchar' AND  Table_Name not like '%_qry%' AND  Table_Name not like '%_WKB'

open cmds
while 1=1
begin
    fetch cmds into @cmd
    if @@fetch_status != 0 break
    exec(@cmd)
end

deallocate cmds

-- modify column type nvarchar( < 255) - change to nvarchar(= 255)

declare cmds cursor for 
Select'

	exec(''ALTER TABLE [' + Table_Name + '] ALTER COLUMN ' + column_name + ' nvarchar(255)'')
	
'
From    INFORMATION_SCHEMA.columns 
Where   Table_Name like @ddname + '%' and character_maximum_length < 255 and character_maximum_length > 0 and data_type = 'nvarchar' AND  Table_Name not like '%_qry%' AND  Table_Name not like '%_WKB'

open cmds
while 1=1
begin
    fetch cmds into @cmd
    if @@fetch_status != 0 break
    exec(@cmd)
end

deallocate cmds

--------------------------------------------------------------------------------------------- 
-- UPDATE SYNCHHISTORY FOR NON LINKED TABLES
---------------------------------------------------------------------------------------------

declare cmds cursor for 
Select'

	declare @synchcount int
	declare @tablereccount int
	set @tablereccount = (SELECT COUNT(*) FROM [' + Table_Name + '])
	set @synchcount = (SELECT COUNT(*) FROM [' + @ddname + 'SynchHistory] WHERE sTableName = ''' + Table_Name + ''')
	
	IF @tablereccount > 0 and @synchcount = 0
		BEGIN
			
			insert into [' + @ddname + 'SynchHistory] 
			([sID]
			,[sGUID]
			,[sTableName]
			,[swhen]
			,[sStatus]
			,[sequence]
			,[sBy]
			,[sdelete]
			,[updates]
			,[noconflict]) 
			
			(SELECT 
				[GUID1] AS sID, 
				NEWID() AS sGUID, 
				''' + Table_Name + ''' AS sTableName, 
				LEFT(CONCAT(CONVERT(nvarchar,CONVERT(date,getdate())),''T'',CONVERT(nvarchar,CONVERT(time,getdate(),108))),16)+''Z''  AS sWhen, 
				''SP Prep for V4'' AS sStatus, 
				1 AS sequence, 
				''OASIS Admin'' AS sBy, 
				''false'' AS sDelete, 
				''true'' AS updates, 
				''true'' AS noconclict
			FROM [' + Table_Name + '])

		END
	Else
	BEGIN 
		IF @tablereccount > 0 and @synchcount > 0
		BEGIN
			insert into [' + @ddname + 'SynchHistory] 
			([sID]
			,[sGUID]
			,[sTableName]
			,[swhen]
			,[sStatus]
			,[sequence]
			,[sBy]
			,[sdelete]
			,[updates]
			,[noconflict]) 
			
			(SELECT 
				[GUID1] AS sID, 
				NEWID() AS sGUID, 
				''' + Table_Name + ''' AS sTableName, 
				LEFT(CONCAT(CONVERT(nvarchar,CONVERT(date,getdate())),''T'',CONVERT(nvarchar,CONVERT(time,getdate(),108))),16)+''Z''  AS sWhen, 
				''SP Prep for V4'' AS sStatus, 
				1 AS sequence, 
				''OASIS Admin'' AS sBy, 
				''false'' AS sDelete, 
				''true'' AS updates, 
				''true'' AS noconclict
			FROM [' + Table_Name + ']
			WHERE GUID1 NOT IN (SELECT [sID] FROM [' + @ddname + 'SynchHistory]  HS WHERE HS.sTableName = ''' + Table_Name + '''))
		END 
	END		
'
From    INFORMATION_SCHEMA.TABLES 
Where   TABLE_TYPE = 'BASE TABLE'
		and Table_Name like @ddname + '%' 
		and not right(Table_Name, 12) = 'SynchHistory' 
		and not right(Table_Name, 20) = 'SynchHistoryOverview' 
		and not left(right(Table_Name, 1 + len(Table_Name) - (dbo.InString(Table_Name, '_', 2))),4) = 'link' 
		and not right(Table_Name, 4) = '_GEO'
		and not right(Table_Name, 4) = '_WKB'

open cmds
while 1=1
begin
    fetch cmds into @cmd
    if @@fetch_status != 0 break
    exec(@cmd)
end

deallocate cmds

--------------------------------------------------------------------------------------------- 
-- UPDATE SYNCHHISTORY FOR LINKED TABLES
--------------------------------------------------------------------------------------------- 

declare cmds cursor for 
Select'

	declare @synchcount int
	declare @tablereccount int
	set @tablereccount = (SELECT COUNT(*) FROM [' + Table_Name + '])
	set @synchcount = (SELECT COUNT(*) FROM [' + @ddname + 'SynchHistory] WHERE sTableName = ''' + Table_Name + ''')
	
	IF @tablereccount > 0 and @synchcount = 0
		BEGIN
			
			insert into [' + @ddname + 'SynchHistory] 
			([sID]
			,[sGUID]
			,[sTableName]
			,[swhen]
			,[sStatus]
			,[sequence]
			,[sBy]
			,[sdelete]
			,[updates]
			,[noconflict]) 
			
			(SELECT 
				[GUID2] AS sID, 
				NEWID() AS sGUID, ''' + Table_Name + ''' AS sTableName, 
				LEFT(CONCAT(CONVERT(nvarchar,CONVERT(date,getdate())),''T'',CONVERT(nvarchar,CONVERT(time,getdate(),108))),16)+''Z'' AS sWhen, 
				''SP Prep for V4'' AS sStatus, 
				1 AS sequence, 
				''OASIS Admin'' AS sBy, 
				''false'' AS sDelete, 
				''true'' AS updates, 
				''true'' AS noconclict
			FROM [' + Table_Name + '])

		END
	Else
	BEGIN 
		IF @tablereccount > 0 and @synchcount > 0
		BEGIN
			insert into [' + @ddname + 'SynchHistory] 
			([sID]
			,[sGUID]
			,[sTableName]
			,[swhen]
			,[sStatus]
			,[sequence]
			,[sBy]
			,[sdelete]
			,[updates]
			,[noconflict]) 
			
			(SELECT 
				[GUID1] AS sID, 
				NEWID() AS sGUID, 
				''' + Table_Name + ''' AS sTableName, 
				LEFT(CONCAT(CONVERT(nvarchar,CONVERT(date,getdate())),''T'',CONVERT(nvarchar,CONVERT(time,getdate(),108))),16)+''Z''  AS sWhen, 
				''SP Prep for V4'' AS sStatus, 
				1 AS sequence, 
				''OASIS Admin'' AS sBy, 
				''false'' AS sDelete, 
				''true'' AS updates, 
				''true'' AS noconclict
			FROM [' + Table_Name + ']
			WHERE GUID1 NOT IN (SELECT [sID] FROM [' + @ddname + 'SynchHistory]  HS WHERE HS.sTableName = ''' + Table_Name + '''))
		END 
	END	
		
'
From    INFORMATION_SCHEMA.TABLES 
Where   TABLE_TYPE = 'BASE TABLE'
		and Table_Name like @ddname + '%'
		and left(right(Table_Name, 1 + len(Table_Name) - (dbo.InString(Table_Name, '_', 2))),4) = 'link' 

open cmds
while 1=1
begin
    fetch cmds into @cmd
    if @@fetch_status != 0 break
    exec(@cmd)
end

deallocate cmds


go

