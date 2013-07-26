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

IF NOT EXISTS (select * from Information_SCHEMA.columns where Table_name= + @ddname + 'ComboRelations')
BEGIN
	exec('CREATE TABLE [dbo].[' + @ddname + 'ComboRelations](
		[GUID1] [nvarchar](255) NULL,
		[sTableName] [nvarchar](255) NULL,
		[sFieldName] [nvarchar](255) NULL,
		[sParentFieldName] [nvarchar](255) NULL
	) ON [PRIMARY]')
END

go

