---------------------------------------------------------------------------------------------
-- THIS SAMPLE SETUP THAT WILL SET AUTOMATICALLY ON WEB ADMINTOOL
-- USE [OasisDB-Atlantis]
-- declare @tableprefix nvarchar(255)
-- set @tableprefix = 'iMMAP'
---------------------------------------------------------------------------------------------

--------------------------------------------------------------------------------------------- 
-- DO NOT CHANGE ANY CODE AFTER THIS POINT  
--------------------------------------------------------------------------------------------- 
declare @cmd varchar(4000)

IF NOT EXISTS (select * from Information_SCHEMA.columns where Table_name= + @tableprefix + 'WebTiles')
BEGIN
	exec('CREATE TABLE [dbo].[' + @tableprefix + 'WebTiles](
        [Caption] [nvarchar](255) NULL,
        [URL1] [nvarchar](255) NULL,
        [URL2] [nvarchar](255) NULL,
        [URL3] [nvarchar](255) NULL,
        [ESPGNumber] [int] NULL,
        [ImageFormat] [nvarchar](255) NULL,
        [ForceWGS] [bit] NOT NULL,
   ) ON [PRIMARY]')
END

GO