Imports System.Data.SqlClient
Imports System.Data
Imports Ext.Net
Imports Immap.Service
Partial Class UserGroups
    Inherits System.Web.UI.Page

    Public Const ASCENDING As String = " ASC"
    Public Const DESCENDING As String = " DESC"

    Private dtUser As DataTable
    Private dtUserGroup As DataTable

    ' Private Property SQLHelper As Object

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Dim database As String = Convert.ToString(Session("database"))
        ImmapService.GetInstance().CheckDatabaseIsExitIfNotRedirectTologin()
        If Not (Page.IsPostBack) Then
            Session("TITLE") = "User Groups"
            RefreshUserGroupStore()
        End If
    End Sub

    Protected Sub UserStore_Refresh(sender As Object, e As StoreRefreshDataEventArgs)
        RefreshUserStore()
    End Sub

    Protected Sub UserGroupStore_Refresh(sender As Object, e As StoreRefreshDataEventArgs)
        RefreshUserGroupStore()
    End Sub

    Protected Sub UserGroup_RowSelect(sender As Object, e As DirectEventArgs)
        Dim GPID As String = e.ExtraParams("GPID")
        Session("GPID") = GPID
        If String.IsNullOrEmpty(GPID) = True Then
            Exit Sub
        End If
        dtUser = LoadUsersAsDataTable(CInt(GPID))
        If dtUser Is Nothing OrElse dtUser.Rows.Count <= 0 Then
            Me.Store2.DataSource = New DataTable()
        Else
            Me.Store2.DataSource = dtUser
        End If
        Me.Store2.DataBind()
    End Sub

    Protected Sub UserGroup_RowDelete(sender As Object, e As DirectEventArgs)
        Dim GPID As String = e.ExtraParams("GPID")
        Dim GPName As String = e.ExtraParams("GPName")
        Dim SettingTablePrefix As String = e.ExtraParams("SettingTablePrefix")
        If String.IsNullOrEmpty(GPID) = True Then
            e.Success = False
            e.ErrorMessage = "The list of user group is empty"
            Exit Sub
        End If

        If String.IsNullOrEmpty(GPName) = True Then
            e.Success = False
            e.ErrorMessage = "The list of user group is empty"
            Exit Sub
        End If

        If CheckUserGroupContainUser(Session("database"), GPID) Then
            e.Success = False
            e.ErrorMessage = "The list of user group is empty"
            Exit Sub
        End If
        Try
            SQLHelper.ExecuteNonQuery(ImmapUtil.getConnectionStringByDatabase(Session("database")),
                        Me.DropTableCommandText(SettingTablePrefix))

            SQLHelper.ExecuteNonQuery(ImmapUtil.getConnectionStringByDatabase(Session("database")),
                                    "DELETE FROM UserGroups WHERE [ID] = @ID",
                                    New SqlParameter("@ID", CInt(GPID)))
            e.Success = True
            RefreshUserGroupStore()
        Catch ex As Exception
            e.Success = False
        End Try

    End Sub

    Private Sub RefreshUserStore()

        Dim GPID As String = CStr(Session("GPID"))
        If String.IsNullOrEmpty(GPID) = True Then
            Exit Sub
        End If
        dtUser = LoadUsersAsDataTable(CInt(GPID))
        If dtUser Is Nothing OrElse dtUser.Rows.Count <= 0 Then
            Me.Store2.DataSource = New DataTable()
        Else
            Me.Store2.DataSource = dtUser
        End If
        Me.Store2.DataBind()
    End Sub

    Private Sub ClearUserStore()
        Me.Store2.DataSource = New DataTable()
        Me.Store2.DataBind()
    End Sub

    Private Sub RefreshUserGroupStore()
        dtUserGroup = LoadUserGroupsAsDataTable()
        If dtUserGroup Is Nothing OrElse dtUserGroup.Rows.Count <= 0 Then
            Me.Store1.DataSource = New DataTable()
        Else
            Me.Store1.DataSource = dtUserGroup
        End If
        Me.Store1.DataBind()

    End Sub

    Protected Sub btnInsert_Click(sender As Object, e As System.EventArgs)
        Me.InsertOrUpdate(ImmapUtil.SaveType.INSERT)
    End Sub

    Protected Sub btnUpdate_Click(sender As Object, e As System.EventArgs)
        If (String.IsNullOrEmpty(txtGPID.Text)) Then
            Dim msg As New MessageBox()
            msg.Show(New MessageBoxConfig() With {
                 .Title = "Warning",
                 .Message = "Please select user group",
                 .Buttons = MessageBox.Button.OK,
                 .Icon = MessageBox.Icon.WARNING,
                .AnimEl = Me.FormPanel1.ClientID
          })
            Exit Sub
        End If
        Me.InsertOrUpdate(ImmapUtil.SaveType.UPDATE, txtGPID.Text)
    End Sub

    Protected Sub InsertOrUpdate(ByVal saveType As ImmapUtil.SaveType, Optional ByVal Id As Integer = -1)
        Dim commandText As String = Nothing

        If (saveType = ImmapUtil.SaveType.INSERT) Then
            commandText = "INSERT INTO [UserGroups] ([Name],[Description],[SettingTablePrefix],[sGUID]) VALUES (@Name,@Description,@SettingTablePrefix,@sGUID);"
        ElseIf (saveType = ImmapUtil.SaveType.UPDATE) Then '
            commandText = "UPDATE [UserGroups] SET [Name]=@Name,[Description]=@Description WHERE ID=@ID"
        End If
        Dim isDuplicate As Boolean = CheckDuplicateName(Session("database"), txtGPName.Text, Id)
        If isDuplicate Then
            Dim msg As New MessageBox()
            msg.Show(New MessageBoxConfig() With {
                   .Title = "Warning",
                   .Message = "This user group name is duplicated",
                   .Buttons = MessageBox.Button.OK,
                   .Icon = MessageBox.Icon.WARNING,
                  .AnimEl = Me.FormPanel1.ClientID
            })
            Exit Sub
        End If
        Try
            Dim connStr As String = ImmapUtil.getConnectionStringByDatabase(Session("database"))
            SQLHelper.ExecuteNonQuery(connStr,
                                      commandText,
                                      New SqlParameter("@Name", txtGPName.Text),
                                      New SqlParameter("@Description", txtGPDesc.Text),
                                      New SqlParameter("@SettingTablePrefix", txtGPName.Text.Replace(" ", "")),
                                      New SqlParameter("@sGUID", ImmapUtil.NewGUid()),
                                      New SqlParameter("@ID", txtGPID.Text))
            If (saveType = ImmapUtil.SaveType.INSERT) Then
                SQLHelper.ExecuteNonQuery(connStr, Me.CreateTableCommandText(txtGPName.Text))
            End If
        Catch ex As Exception
        End Try
        RefreshUserGroupStore()
        ClearUserStore()
        Dim sm As RowSelectionModel = TryCast(Me.GridPanel1.SelectionModel.Primary, RowSelectionModel)
        sm.ClearSelections()
        GridPanel1.Call("clearMemory")
        Me.FormPanel1.Reset()
    End Sub

    Protected Function LoadUsersAsDataTable(ByVal id As Integer) As DataTable
        Dim commandText As String = Nothing
        Dim dt As New DataTable()
        commandText = "SELECT  u.[id]," &
                      " [user] AS 'Username'," &
                      " [pwd]," &
                      " [Fname] AS 'First Name'," &
                      " [Lname] AS 'Last Name'," &
                      " [SettingUrl]," &
                      " g.[Name] AS 'Group'" &
                      " FROM [users] u " &
                      " INNER JOIN [userGroups] as g ON g.id = u.UserGroupID WHERE u.UserGroupID = @Id; "
        Try
            dt = SQLHelper.ExecuteDataTable(ImmapUtil.getConnectionStringByDatabase(Session("database")),
                                                commandText, New SqlParameter("@Id", id))
        Catch ex As Exception
        End Try
        Return dt
    End Function
    Public Function CheckDuplicateName(database As String, Name As String, Optional Id As Integer = -1) As Boolean
        Dim isNotValid As Boolean = False
        Dim commantText As String = "SELECT [ID],[Name],[Description] FROM Usergroups WHERE UPPER(Name) LIKE UPPER(@Name)"
        If (Id <> -1) Then
            commantText &= " AND id<>@Id"
        End If
        Dim sqlreader = SQLHelper.ExecuteReader(ImmapUtil.getConnectionStringByDatabase(Session("database")),
                                                commantText,
                                                New SqlParameter("@Name", Name.Replace(" ", "")),
                                                New SqlParameter("@id", Id))
        If sqlreader.HasRows Then
            isNotValid = True
        End If

        If IsNothing(sqlreader) = False AndAlso sqlreader.IsClosed = False Then
            sqlreader.Close()
            sqlreader = Nothing
        End If
        Return isNotValid
    End Function
    Protected Function CheckUserGroupNameDuplicate(sender As Object, e As RemoteValidationEventArgs) As Boolean
        If CheckDuplicateName(Session("database"), txtGPName.Text) Then
            e.Success = False
            e.ErrorMessage = "This user group name is duplicated"
            Return True
        Else
            e.Success = True
            Return False
        End If
    End Function
    Public Function CheckUserGroupContainUser(database As String, UserGroupId As Integer) As Boolean
        Dim isNotValid As Boolean = False
        Dim sqlreader As SqlDataReader = Nothing
        sqlreader = SQLHelper.ExecuteReader(ImmapUtil.getConnectionStringByDatabase(Session("database")),
                                                "SELECT u.ID,u.[user],u.[Fname],u.[Lname] FROM [Users] u WHERE u.[UserGroupID] = @UserGroupID;",
                                                New SqlParameter("@UserGroupID", UserGroupId))
        If sqlreader.HasRows Then
            isNotValid = True
        End If
        If IsNothing(sqlreader) = False AndAlso sqlreader.IsClosed = False Then
            sqlreader.Close()
            sqlreader = Nothing
        End If
        Return isNotValid

    End Function

    Protected Function LoadUserGroupsAsDataTable() As DataTable
        Dim commandText As String = "SELECT [ID] AS GPID, [Name] AS GPName,[Description] AS GPDesc,[SettingTablePrefix],[sGUID] FROM [UserGroups]"
        Dim dt As New DataTable()
        Try
            dt = SQLHelper.ExecuteDataTable(ImmapUtil.getConnectionStringByDatabase(Session("database")), commandText)
        Catch ex As Exception
        End Try
        Return dt
    End Function

    Public Function CreateTableCommandText(TableName As String) As String
        Dim AppSettingsStr As String = "CREATE TABLE [dbo].[" & TableName & "AppSettings](" &
            "[ID] [int] NOT NULL IDENTITY (1, 1)," &
            "[SettingDesc] [ntext] NULL," &
            "[SettingName] [nvarchar](100) NOT NULL," &
            "[SettingValue1] [ntext] NULL," &
            "[SettingValue2] [ntext] NULL," &
            "[SettingValue3] [ntext] NULL," &
            "[SettingValue4] [ntext] NULL," &
            "[SettingValue5] [ntext] NULL," &
            "[SettingValue6] [ntext] NULL," &
            "[SettingValue7] [ntext] NULL," &
            "[SettingValue8] [ntext] NULL," &
            "[SettingValue9] [ntext] NULL," &
            "[SettingValue10] [ntext] NULL" &
            ") ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]"

        AppSettingsStr = String.Format("SELECT * INTO [{0}Appsettings] FROM [iMMAPAppSettings]", TableName)

        Dim Charting As String = "CREATE TABLE [dbo].[" & TableName & "Charting](" &
            "[sGUID] [nvarchar](150) NULL," &
            "[sName] [nvarchar](255) NULL," &
            "[sDescription] [ntext] NULL," &
            "[oOCTFile] [image] NULL," &
            "[oQRYFile] [image] NULL," &
            "[oConstrantFile] [image] NULL" &
            ") ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]"

        Dim ClientDBUpdates As String = "CREATE TABLE [dbo].[" & TableName & "ClientDBUpdates](" &
            "[ID] [int] NOT NULL IDENTITY (1, 1)," &
            "[sTitle] [nvarchar](250) NULL," &
            "[sGUID] [nvarchar](150) NULL," &
            "[sSQL] [ntext] NULL," &
            "[sVersion] [nvarchar](50) NULL," &
            "[sDescription] [ntext] NULL," &
            "[sTimeStamp] [datetime] NULL" &
            ") ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]"

        Dim ClientFileLocations As String = "CREATE TABLE [dbo].[" & TableName & "ClientFileLocations](" &
            "[ID] [int] NOT NULL IDENTITY (1, 1)," &
            "[Name] [nvarchar](250) NULL," &
            "[ClientLocationPath] [ntext] NULL," &
            "[Description] [ntext] NULL" &
            ") ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]"

        Dim DataPacks As String = "CREATE TABLE [dbo].[" & TableName & "DataPacks](" &
            "[ID] [int] NOT NULL IDENTITY (1, 1)," &
            "[GUID] [nvarchar](255) NULL," &
            "[Update] [bit] NOT NULL," & "[Name] [ntext] NULL," &
            "[Description] [ntext] NULL," &
            "[UserGroupID] [int] NULL," &
            "[Path] [ntext] NULL," &
            "[FolderConst] [ntext] NULL" &
            ") ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]"
        Dim DynamicDataDefs As String = "CREATE TABLE [dbo].[" & TableName & "DynamicDataDefs](" &
            "[DDDefName] [nvarchar](255) NULL," &
            "[Description] [nvarchar](255) NULL," &
            "[AccessRights] [ntext] NULL," &
            "[ConnectionString] [ntext] NULL," &
            "[Synch] [bit] NOT NULL," &
            "[EnableDataEntry] [bit] NOT NULL," &
            "[EnableReporting] [bit] NOT NULL," &
            "[LockedFields] [ntext] NULL," &
            "[ExcludedFields] [ntext] NULL" &
            ") ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]"

        Dim FeedGroups As String = "CREATE TABLE [dbo].[" & TableName & "FeedGroups](" &
            "[GroupId] [int] NOT NULL," &
            "[GroupText] [nvarchar](50) NULL," &
            "[CustomGroup] [bit] NOT NULL" & ") ON [PRIMARY]"
        Dim Feeds As String = "CREATE TABLE [dbo].[" & TableName & "Feeds](" &
            "[FeedID] [int] NOT NULL," &
            "[GroupID] [int] NOT NULL," &
            "[CustomId] [int] NULL," &
            "[FeedName] [nvarchar](255) NULL," &
            "[FeedDescription] [ntext] NULL," &
            "[FeedURL] [nvarchar](255) NULL," &
            "[FeedImageURL] [nvarchar](255) NULL," &
            "[CheckInterval] [int] NULL," &
            "[Subscribed] [nvarchar](50) NULL," &
            "[LastCheck] [datetime] NULL" &
            ") ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]"

        Dim GeoBookMarks As String = "CREATE TABLE [dbo].[" & TableName & "GeoBookMarks](" &
            "[ID] [int] NOT NULL," &
            "[Name] [nvarchar](50) NULL," &
            "[X] [float] NULL," &
            "[Y] [float] NULL," &
            "[Z] [float] NULL," &
            "[Description] [nvarchar](255) NULL," &
            "[UseSymbol] [bit] NOT NULL," &
            "[SymbolChar] [nvarchar](50) NULL," &
            "[SymbolFont] [nvarchar](150) NULL," &
            "[SymbolSize] [nvarchar](50) NULL," &
            "[MapName] [nvarchar](250) NULL," &
            "[BmkrID] [int] NULL," &
            "[GUID1] [nvarchar](120) NULL," &
            "[dTimeStamp] [datetime] NULL," &
            "[OwnerGUID] [nvarchar](120) NULL," &
            "[Deleted] [bit] NOT NULL," &
            "[isURLMark] [bit] NOT NULL," &
            "[sURL] [ntext] NULL" &
            ") ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]"

        Dim GeoBookMarksCategories As String = "CREATE TABLE [dbo].[" & TableName & "GeoBookMarksCategories](" &
            "[ID] [int] NOT NULL," &
            "[Name] [nvarchar](50) NULL," &
            "[Description] [nvarchar](50) NULL," &
            "[GUID1] [nvarchar](255) NULL" & ") ON [PRIMARY]"

        Dim GISGridTableSettings As String = "CREATE TABLE [dbo].[" & TableName & "GISGridTableSettings](" &
            "[ID] [int] NOT NULL," &
            "[name] [nvarchar](255) NULL," &
            "[alias] [nvarchar](255) NULL," & "[visible] [bit] NOT NULL," &
            "[datasetwarning] [bit] NOT NULL," &
            "[warninglevel] [int] NULL," &
            "[MaxRec] [int] NULL," &
            "[excludedFlds] [ntext] NULL," &
            "[isURLLayer] [bit] NOT NULL," &
            "[autoRunUrls] [bit] NOT NULL," &
            "[URLLayerField] [ntext] NULL" &
            ") ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]"

        Dim Incidents_ChartSettings As String = "CREATE TABLE [dbo].[" & TableName & "Incidents_ChartSettings](" &
            "[GUID1] [nvarchar](255) NOT NULL," &
            "[QueryName] [nvarchar](255) NULL," &
            "[OCTSettings] [image] NULL," &
            "[SQLCommand] [ntext] NULL," &
            "[UseChart] [bit] NULL," &
            "[bAutoLoadReport] [bit] NULL," &
            "[FilterSQL] [ntext] NULL," &
            "[Group] [nvarchar](255) NULL" &
            ") ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]"

        Dim Lang As String = "CREATE TABLE [dbo].[" & TableName & "Lang](" &
            "[sGUID] [uniqueidentifier] NULL," &
            "[Name] [nvarchar](250) NULL," &
            "[iCtrlnx] [int] NULL," &
            "[sType] [nvarchar](50) NULL," &
            "[Container] [nvarchar](150) NULL," &
            "[sDescription] [ntext] NULL," &
            "[sDefault] [ntext] NULL," &
            "[Spanish] [ntext] NULL" &
            ") ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]"

        Dim Maps As String = "CREATE TABLE [dbo].[" & TableName & "Maps](" &
            "[ID] [int] NOT NULL," &
            "[Name] [nvarchar](250) NULL," &
            "[Alias] [nvarchar](150) NULL," &
            "[MapPath] [nvarchar](50) NULL," &
            "[FileName] [nvarchar](250) NULL," &
            "[Image] [nvarchar](150) NULL," &
            "[ThumbNail] [nvarchar](50) NULL," &
            "[CreatedBy] [nvarchar](50) NULL," &
            "[CreatedDate] [nvarchar](50) NULL," &
            "[Description] [ntext] NULL," &
            "[Contact] [nvarchar](255) NULL," &
            "[Restrictions] [nvarchar](250) NULL," &
            "[Copyright] [nvarchar](150) NULL," &
            "[url] [nvarchar](255) NULL," &
            "[StandardLyrs] [nvarchar](250) NULL," &
            "[Source] [nvarchar](255) NULL," &
            "[AdminLyr1Name] [nvarchar](250) NULL," &
            "[AdminLyr2Name] [nvarchar](250) NULL," &
            "[AdminLyr3Name] [nvarchar](250) NULL," &
            "[AdminLyr4Name] [nvarchar](250) NULL," &
            "[AdminLyr5Name] [nvarchar](250) NULL" &
            ") ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]"

        Dim PrintTemplates As String = "CREATE TABLE [dbo].[" & TableName & "PrintTemplates](" &
            "[IDGUID] [uniqueidentifier] NULL," &
            "[Name] [nvarchar](250) NULL," &
            "[Format] [nvarchar](50) NULL," &
            "[Width] [int] NULL," &
            "[Height] [int] NULL," &
            "[FileName] [nvarchar](250) NULL," &
            "[description] [ntext] NULL," &
            "[islandscape] [bit] NOT NULL," &
            "[isportrait] [bit] NOT NULL," &
            "[note] [ntext] NULL," &
            "[copyright] [ntext] NULL," &
            "[MapTitle] [nvarchar](250) NULL," &
            "[MapSubTitle] [nvarchar](250) NULL," &
            "[MapIDPrefix] [nvarchar](50) NULL," &
            "[blob_tpl] [image] NULL" &
            ") ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]"

        Dim Resources As String = "CREATE TABLE [dbo].[" & TableName & "Resources](" &
            "[LayerName] [nvarchar](255) NULL," &
            "[PlaceName] [nvarchar](255) NULL," &
            "[IndexOfFirstItem] [int] NULL" &
            ") ON [PRIMARY]"
        Dim SynchFeed As String = "CREATE TABLE [dbo].[" & TableName & "SynchFeed](" &
            "[sGUID] [uniqueidentifier] NULL," &
            "[sLocalID] [nvarchar](150) NULL," &
            "[sBy] [nvarchar](250) NULL," &
            "[sDescription] [ntext] NULL," &
            "[Title] [nvarchar](250) NULL," &
            "[time] [nvarchar](80) NULL," &
            "[deleted] [nvarchar](10) NULL," &
            "[TableName] [nvarchar](250) NULL," &
            "[isGeoTable] [bit] NOT NULL" &
            ") ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]"

        Dim SynchFeedsHistory As String = "CREATE TABLE [dbo].[" & TableName & "SynchFeedsHistory](" &
            "[sID] [uniqueidentifier] NULL," &
            "[sGUID] [nvarchar](150) NULL," &
            "[sBy] [nvarchar](250) NULL," &
            "[sequence] [int] NULL," &
            "[swhen] [nvarchar](80) NULL," &
            "[deleted] [nvarchar](10) NULL," &
            "[updates] [nvarchar](50) NULL," &
            "[noconflicts] [nvarchar](5) NULL" &
            ") ON [PRIMARY]"

        Dim SynchFiles As String = "CREATE TABLE [dbo].[" & TableName & "SynchFiles](" &
            "[ID] [int] NOT NULL," & "[GUID] [nvarchar](150) NULL," &
            "[title] [nvarchar](250) NULL," &
            "[FileName] [ntext] NULL," &
            "[Description] [ntext] NULL," &
            "[ClientLocation] [nvarchar](50) NULL," &
            "[FileDate] [nvarchar](50) NULL," &
            "[Version] [nvarchar](50) NULL," &
            "[ServeLlocation] [ntext] NULL," & "[UserGroupID] [int] NULL," &
            "[UpdateDate] [nvarchar](50) NULL," &
            "[Action] [int] NULL," & "[updates] [int] NULL," &
            "[deleted] [bit] NOT NULL," &
            "[noconflicts] [bit] NOT NULL," &
            "[link] [ntext] NULL" &
            ") ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]"

        Dim SynchFolders As String = "CREATE TABLE [dbo].[" & TableName & "SynchFolders](" &
            "[ID] [int] NOT NULL," &
            "[GUID] [nvarchar](120) NULL," &
            "[Name] [nvarchar](250) NULL," &
            "[Description] [ntext] NULL," &
            "[Path] [nvarchar](250) NULL," &
            "[ClientPath] [nvarchar](250) NULL," &
            "[bUse] [bit] NOT NULL" &
            ") ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]"
        Dim SynchHistory As String = "CREATE TABLE [dbo].[" & TableName & "SynchHistory](" &
            "[sID] [nvarchar](50) NULL," &
            "[sGUID] [nvarchar](128) NULL," &
            "[sTableName] [nvarchar](50) NULL," &
            "[swhen] [nvarchar](50) NULL," &
            "[sStatus] [nvarchar](50) NULL," &
            "[sequence] [int] NULL," &
            "[sBy] [nvarchar](255) NULL," &
            "[sdelete] [nvarchar](50) NULL," &
            "[updates] [nvarchar](50) NULL," &
            "[noconflict] [nvarchar](50) NULL" &
            ") ON [PRIMARY]"

        Dim SynchTables As String = "CREATE TABLE [dbo].[" & TableName & "SynchTables](" &
            "[sGUID] [nvarchar](150) NULL," &
            "[sName] [nvarchar](250) NULL," &
            "[sTableName] [nvarchar](250) NULL," &
            "[sDescription] [ntext] NULL," &
            "[isGeoTable] [bit] NOT NULL," &
            "[OwnerID] [nvarchar](50) NULL," &
            "[AllowWrite] [bit] NOT NULL," &
            "[SynchFrequency] [int] NULL," &
            "[AutoUpdate] [bit] NOT NULL," &
            "[InLegend] [bit] NOT NULL," &
            "[sCaption] [nvarchar](250) NULL," &
            "[IsActive] [bit] NOT NULL" & ") ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]"

        Dim ThemeGroups As String = "CREATE TABLE [dbo].[" & TableName & "ThemeGroups](" &
            "[ID] [int] NULL," &
            "[Name] [nvarchar](250) NULL," &
            "[Description] [ntext] NULL" &
            ") ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]"

        Dim Themes As String = "CREATE TABLE [dbo].[" & TableName & "Themes](" &
            "[ID] [int] NULL," & "[Name] [nvarchar](255) NULL," &
            "[ThemeGroup] [int] NULL," &
            "[Description] [ntext] NULL," &
            "[AnalysisField] [nvarchar](50) NULL," &
            "[Maps] [ntext] NULL," &
            "[AnalysisLayer] [nvarchar](250) NULL," &
            "[ThemeConfigName] [nvarchar](250) NULL" &
            ") ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]"
        Dim ttkGISLayerSQLInProject As String = "CREATE TABLE [dbo].[" & TableName & "ttkGISLayerSQLInProject](" &
            "[LayerName] [nvarchar](255) NULL," &
            "[LayerCaption] [nvarchar](255) NULL," &
            "[Dialect] [nvarchar](255) NULL," &
            "[ADO] [ntext] NULL," &
            "[Sequence] [int] NULL," &
            "[XMIN] [float] NULL," &
            "[XMAX] [float] NULL," &
            "[YMIN] [float] NULL," &
            "[YMAX] [float] NULL," & "[SHAPETYPE] [int] NULL," &
            "[Transparency] [int] NULL," & "[IsVisible] [bit] NULL," &
            "[IsExpanded] [bit] NULL," & "[INISettings] [ntext] NULL" &
            ") ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]"

        Dim ttkGISProjectDef As String = "CREATE TABLE [dbo].[" & TableName & "ttkGISProjectDef](" &
            "[InUse] [bit] NOT NULL," &
            "[sName] [nvarchar] (255) NULL," &
            "[MapData] [ntext] NULL," &
            "[sGUID] [nvarchar](255) NULL," &
            "[XMIN] [float] NULL," &
            "[XMAX] [float] NULL," &
            "[YMIN] [float] NULL," &
            "[YMAX] [float] NULL," &
            "[sInfo] [nvarchar](255) NULL," &
            "[oImagePreview] [image] NULL," &
            "[bSavedToDB] [bit] NOT NULL," &
            "[sFilePath] [nvarchar](255) NULL," &
            "[bUGMap] [bit] NOT NULL," &
            "[centerX] [nvarchar](255) NULL," &
            "[centerY] [nvarchar](255) NULL," &
            "[EPSG] [nvarchar](255) NULL," &
            "[scale] [nvarchar](255) NULL," &
            "[CreatedBy] [nvarchar](50) NULL," &
            "[CreatedDate] [nvarchar](50) NULL," &
            "[Description] [ntext] NULL," &
            "[Contact] [nvarchar](255) NULL," &
            "[Restrictions] [nvarchar](250) NULL," &
            "[Copyright] [nvarchar](150) NULL," &
            "[url] [nvarchar](255) NULL," &
            "[StandardLyrs] [nvarchar](250) NULL," &
            "[Source] [nvarchar](255) NULL," &
            "[AdminLyr1Name] [nvarchar](250) NULL," &
            "[AdminLyr2Name] [nvarchar](250) NULL," &
            "[AdminLyr3Name] [nvarchar](250) NULL," &
            "[AdminLyr4Name] [nvarchar](250) NULL," &
            "[AdminLyr5Name] [nvarchar](250) NULL" &
            ") ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]"


        Dim Webtiles As String = "CREATE TABLE [dbo].[" & TableName & "WebTiles](" &
            "[Caption] [nvarchar](255) NULL," &
            "[URL1] [nvarchar](255) NULL," &
            "[URL2] [nvarchar](255) NULL," &
            "[URL3] [nvarchar](255) NULL," &
            "[ESPGNumber] [int] NULL," &
            "[ImageFormat] [nvarchar](255) NULL," &
            "[ForceWGS] [bit] NOT NULL," &
            ") ON [PRIMARY]"

        Dim allTable As String = AppSettingsStr & ";" &
            Charting & ";" &
            ClientDBUpdates & ";" &
            ClientFileLocations & ";" &
            DataPacks & ";" &
            DynamicDataDefs & ";" &
            FeedGroups & ";" & Feeds & ";" &
            GeoBookMarks & ";" &
            GeoBookMarksCategories & ";" &
            GISGridTableSettings & ";" &
            Incidents_ChartSettings & ";" & Lang & ";" &
            Maps & ";" &
            PrintTemplates & ";" &
            Resources & ";" &
            SynchFeed & ";" &
            SynchFeedsHistory & ";" &
            SynchFiles & ";" &
            SynchHistory & ";" &
            SynchFolders & ";" &
            SynchTables & ";" &
            ThemeGroups & ";" &
            Themes & ";" &
            ttkGISLayerSQLInProject & ";" &
            ttkGISProjectDef & ";" &
            Webtiles & ";"
        Return allTable
    End Function
    Public Function DropTableCommandText(TableName As String) As String
        Dim AppSettings As String = "DROP TABLE [dbo].[" & TableName & "AppSettings];"
        Dim Charting As String = "DROP TABLE [dbo].[" & TableName & "Charting];"
        Dim ClientDBUpdates As String = "DROP TABLE [dbo].[" & TableName & "ClientDBUpdates];"
        Dim ClientFileLocations As String = "DROP TABLE [dbo].[" & TableName & "ClientFileLocations];"
        Dim DataPacks As String = "DROP TABLE [dbo].[" & TableName & "DataPacks];"
        Dim DynamicDataDefs As String = "DROP TABLE [dbo].[" & TableName & "DynamicDataDefs];"
        Dim FeedGroups As String = "DROP TABLE [dbo].[" & TableName & "FeedGroups];"
        Dim Feeds As String = "DROP TABLE [dbo].[" & TableName & "Feeds];"
        Dim GeoBookMarks As String = "DROP TABLE [dbo].[" & TableName & "GeoBookMarks];"
        Dim GeoBookMarksCategories As String = "DROP TABLE [dbo].[" & TableName & "GeoBookMarksCategories];"
        Dim GISGridTableSettings As String = "DROP TABLE [dbo].[" & TableName & "GISGridTableSettings];"
        Dim Incidents_ChartSettings As String = "DROP TABLE [dbo].[" & TableName & "Incidents_ChartSettings];"
        Dim Lang As String = "DROP TABLE [dbo].[" & TableName & "Lang];"
        Dim Maps As String = "DROP TABLE [dbo].[" & TableName & "Maps];"
        Dim PrintTemplates As String = "DROP TABLE [dbo].[" & TableName & "PrintTemplates];"
        Dim Resources As String = "DROP TABLE [dbo].[" & TableName & "Resources];"
        Dim SynchFeed As String = "DROP TABLE [dbo].[" & TableName & "SynchFeed];"
        Dim SynchFiles As String = "DROP TABLE [dbo].[" & TableName & "SynchFiles];"
        Dim SynchFeedsHistory As String = "DROP TABLE [dbo].[" & TableName & "SynchFeedsHistory];"
        Dim SynchHistory As String = "DROP TABLE [dbo].[" & TableName & "SynchHistory];"
        Dim SynchFolders As String = "DROP TABLE [dbo].[" & TableName & "SynchFolders];"
        Dim SynchTables As String = "DROP TABLE [dbo].[" & TableName & "SynchTables];"
        Dim ThemeGroups As String = "DROP TABLE [dbo].[" & TableName & "ThemeGroups];"
        Dim Themes As String = "DROP TABLE [dbo].[" & TableName & "Themes];"
        Dim ttkGISLayerSQLInProject As String = "DROP TABLE [dbo].[" & TableName & "ttkGISLayerSQLInProject];"
        Dim ttkGISProjectDef As String = "DROP TABLE [dbo].[" & TableName & "ttkGISProjectDef];"
        Dim Webtiles As String = "DROP TABLE [dbo].[" & TableName & "WebTiles];"
        Dim allTable As String = AppSettings & " " & Charting & " " & ClientDBUpdates & " " & ClientFileLocations & " " & DataPacks & " " & DynamicDataDefs & " " _
                                 & FeedGroups & " " & Feeds & " " & GeoBookMarks & " " & GeoBookMarksCategories & " " _
                                 & GISGridTableSettings & " " & Incidents_ChartSettings & " " & _
                                 Lang & " " & Maps & " " & PrintTemplates & " " & Resources & " " _
                                 & SynchFeed & " " & SynchFiles & " " & SynchFeedsHistory & " " & SynchFolders & " " & SynchHistory & " " _
                                 & SynchTables & " " & ThemeGroups & " " & Themes & " " & ttkGISLayerSQLInProject & " " _
                                 & ttkGISProjectDef & " " & Webtiles

        Return allTable
    End Function
End Class
