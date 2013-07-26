Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient

Public Class ImmapUtil

    Private Shared ReadOnly _Instance As New ImmapUtil()

    Public Enum SaveType As Integer
        INSERT = 0
        UPDATE = 1
    End Enum

    Public Const TrueDelete As Boolean = True

    Shared Sub New()
    End Sub

    Private Sub New()
    End Sub

    Public Shared ReadOnly Property Instance() As ImmapUtil
        Get
            Return _Instance
        End Get
    End Property

    Public Shared ReadOnly Property GetInstance() As ImmapUtil
        Get
            Return _Instance
        End Get
    End Property

    Public Shared Function getConnectionStringByDatabase(ByVal databasename As String) As String
        Return SQLHelper.SetConnectionString(databasename)
    End Function

    Public Shared Function NewGUid() As String
        Dim sGUID As String = String.Format("{0}{1}{2}", "{", System.Guid.NewGuid().ToString().ToUpper(), "}")
        Return sGUID
    End Function

    Public Function CalculateMD5(ByVal s As String) As String
        Return (CalculateMD5(s, New System.Text.UnicodeEncoding))
    End Function

    Public Function CalculateMD5( _
                ByVal s As String, _
                ByVal enc As System.Text.Encoding) As String
        Dim md5 As New System.Security.Cryptography.MD5CryptoServiceProvider
        Dim input(), output() As Byte
        Dim hash As New System.Text.StringBuilder(32)

        input = enc.GetBytes(s)
        output = md5.ComputeHash(input)

        For Each b As Byte In output
            hash.Append(String.Format("{0:X2}", b))
        Next
        Return (hash.ToString())
    End Function


    Public Sub SaveData(ByVal Database As String,
                           ByVal UserGroup As String,
                           ByVal SettingName As String,
                           ByVal SettingValueColumnName As String,
                           ByVal SettingValueData As String)
        'Dim sqlCommand As New SqlCommand()
        Dim commandText As String = Nothing
        Dim dr As SqlDataReader = Nothing
        commandText = "SELECT SettingName FROM " & UserGroup & "AppSettings WHERE SettingName=@SettingName"

        Try
            Dim connStr As String = ImmapUtil.getConnectionStringByDatabase(Database)
            dr = SQLHelper.ExecuteReader(connStr, commandText, New SqlParameter("@SettingName", SettingName))
            If dr.HasRows Then
                dr.Close()
                commandText = "UPDATE " & UserGroup & "AppSettings SET " & SettingValueColumnName & _
                            "=@SettingValueData WHERE SettingName=@SettingName"
                SQLHelper.ExecuteNonQuery(connStr, commandText,
                                              New SqlParameter("@SettingValueData", SettingValueData),
                                              New SqlParameter("@SettingName", SettingName))
            Else
                dr.Close()
                Dim id = SQLHelper.ExecuteScalar(connStr, "SELECT MAX([ID])+1 FROM " & "[dbo].[" & UserGroup & "AppSettings" & "]")
                If IsDBNull(id) = True Then
                    id = 1
                End If
                commandText = "INSERT INTO " & UserGroup & "AppSettings (id,SettingName," & SettingValueColumnName & ") VALUES(" & id.ToString() & ",@SettingName,@SettingValueData)"
                'commandText = "INSERT INTO " & UserGroup & "AppSettings (SettingName," & SettingValueColumnName & ") VALUES(@SettingName,@SettingValueData)"
                SQLHelper.ExecuteNonQuery(connStr, commandText, New SqlParameter("@SettingValueData", SettingValueData), New SqlParameter("@SettingName", SettingName))
            End If
        Catch ex As Exception

        End Try
        If IsNothing(dr) = False AndAlso dr.IsClosed = False Then
            dr.Close()
            dr = Nothing

        End If
        'sqlCommand.Dispose()
    End Sub

    Public Sub IncreseSetting(ByVal Database As String,
                              ByVal UserGroup As String,
                              ByVal SettingName As String,
                              ByVal SettingValueColumnName As String)
        Dim commandText As String = Nothing
        Dim dr As SqlDataReader = Nothing
        Dim conStr As String = ImmapUtil.getConnectionStringByDatabase(Database)
        commandText = "SELECT [SettingName]," & SettingValueColumnName & " FROM " & UserGroup & "AppSettings WHERE SettingName=@SettingName"
        Try
            dr = SQLHelper.ExecuteReader(conStr,
                                         commandText,
                                         New SqlParameter("@SettingName", SettingName))
            Dim SettingValueData As Integer
            If dr.HasRows Then
                dr.Read()
                If dr.IsDBNull(1) Then
                    SettingValueData = 1
                Else
                    SettingValueData = CInt(dr.GetString(1))
                    SettingValueData += 1
                End If
                dr.Close()
                commandText = "UPDATE " & UserGroup & "AppSettings SET " & SettingValueColumnName & "=@SettingValueData WHERE SettingName=@SettingName"

                SQLHelper.ExecuteNonQuery(conStr, commandText, New SqlParameter("@SettingValueData", SettingValueData.ToString()), New SqlParameter("@SettingName", SettingName))
            Else
                commandText = "INSERT INTO " & UserGroup & "AppSettings (SettingName," &
                              SettingValueColumnName & ") VALUES(@SettingName,'1')"
                SQLHelper.ExecuteNonQuery(conStr,
                                              commandText,
                                              New SqlParameter("@SettingName", SettingName))
            End If
        Catch ex As Exception

        End Try
        If IsNothing(dr) = False AndAlso dr.IsClosed = False Then
            dr.Close()
            dr = Nothing
        End If
        commandText = Nothing
    End Sub

    Public Sub UpdateSynchHistory(ByVal database As String, ByVal UserGroup As String,
                                  ByVal tableName As String, ByVal sId As String, Optional ByVal isDelete As Boolean = False)
        Dim commandText As String = Nothing
        Dim conStr As String = ImmapUtil.getConnectionStringByDatabase(database)
        Dim sBy As String = "Administrator Web"
        Dim sqlreader As SqlDataReader
        Dim sDate As Date = Date.Now()
        'sDate.Year() & "-" & sDate.Month() & "-" & sDate.Day() & "T" & sDate.ToString("HH:mm:ss") & "Z"
        '2012-05-08T04:10:35Z
        commandText = "SELECT [sID], [sGUID], [sTableName], [swhen], [sStatus], [sequence], [sBy], [sdelete]," &
                      " [updates], [noconflict] FROM [dbo].[" & UserGroup & "SynchHistory" & "]" &
                      " WHERE UPPER([sTableName]) LIKE UPPER(@tableName) AND UPPER([sId]) LIKE UPPER(@sId)"
        'Try
        sqlreader = SQLHelper.ExecuteReader(conStr, commandText,
                                                New SqlParameter("@tableName", tableName),
                                                New SqlParameter("@sId", sId))
        Dim sequence As Integer
        Dim sGUID As String
        If sqlreader.HasRows Then
            sqlreader.Read()
            If sqlreader.IsDBNull(5) Then
                sequence = 1
            Else
                sequence = sqlreader.GetInt32(5)
                sequence += 1
            End If
            sGUID = sqlreader.GetString(1)
            'ImmapUtil.NewGUid()
            sqlreader.Close()
            commandText = "UPDATE " & UserGroup & "SynchHistory"
            commandText &= " SET sID='" & sId & "'"
            commandText &= " ,sWhen='" & sDate.Year().ToString("0000") & "-" & sDate.Month().ToString("00") & "-" & sDate.Day().ToString("00") & "T" & sDate.ToString("HH:mm:ss") & "Z" & "'"
            commandText &= " ,sStatus='pending'"
            commandText &= " ,sequence=" & sequence
            commandText &= " ,sBy='" & sBy & "'"
            If isDelete = False Then
                commandText &= " ,sdelete='false'"
            Else
                commandText &= " ,sdelete='true'"
            End If
            commandText &= " ,updates='false'"
            commandText &= " ,noconflict='false'"
            commandText &= " WHERE sGUID=@sGUID AND sTableName=@tableName"
            SQLHelper.ExecuteNonQuery(conStr, commandText,
                                      New SqlParameter("@sGUID", sGUID),
                                      New SqlParameter("@tableName", tableName))
        Else
            commandText = "INSERT INTO " & UserGroup & "SynchHistory " &
                    "([sID], [sGUID], [sTableName], [swhen], [sStatus], [sequence], [sBy], [sdelete], [updates], [noconflict])" &
                    "VALUES(@sID, @sGUID, @sTableName, @swhen, 'pending', 1, @sBy, 'false', 'false', 'false')"
            SQLHelper.ExecuteNonQuery(conStr, commandText,
                                      New SqlParameter("@sGUID", ImmapUtil.NewGUid()),
                                      New SqlParameter("@sId", sId),
                                      New SqlParameter("@sTableName", tableName),
                                      New SqlParameter("@swhen", sDate.Year().ToString("0000") & "-" & sDate.Month().ToString("00") & "-" & sDate.Day().ToString("00") & "T" & sDate.ToString("HH:mm:ss") & "Z"),
                                      New SqlParameter("@sBy", sBy))
        End If
        If IsNothing(sqlreader) = False AndAlso sqlreader.IsClosed = False Then
            sqlreader.Close()
            sqlreader = Nothing
        End If
        'Catch ex As Exception

        'End Try
    End Sub

    Public Function CheckIfNull(sString As Object) As String
        Dim value As String
        If String.IsNullOrWhiteSpace(sString) Then
            value = ""
        ElseIf String.IsNullOrEmpty(sString) Then
            value = ""
        Else
            value = sString
        End If
        Return value
    End Function


#Region "Colour"
    Public Sub Hex2RGB(strHexColor As String, ByRef r As Byte, ByRef g As Byte, ByRef b As Byte)
        Dim HexColor As String
        Dim i As Byte
        On Error Resume Next
        ' make sure the string is 6 characters long
        ' (it may have been given in &H###### format, we want ######)
        strHexColor = Microsoft.VisualBasic.Right((strHexColor), 6)
        ' however, it may also have been given as or #***** format, so add 0's in front
        HexColor = Nothing
        For i = 1 To (6 - Len(strHexColor))
            HexColor = HexColor & "0"
        Next
        HexColor = HexColor & strHexColor
        ' convert each set of 2 characters into  bytes, using vb's cbyte function
        r = CByte("&H" & Microsoft.VisualBasic.Right$(HexColor, 2))
        g = CByte("&H" & Microsoft.VisualBasic.Mid$(HexColor, 3, 2))
        b = CByte("&H" & Microsoft.VisualBasic.Left$(HexColor, 2))
    End Sub

    Public Sub Hex2BGR(strHexColor As String, ByRef r As Byte, ByRef g As Byte, ByRef b As Byte)
        Dim HexColor As String
        Dim i As Byte
        On Error Resume Next
        ' make sure the string is 6 characters long
        ' (it may have been given in &H###### format, we want ######)
        strHexColor = Microsoft.VisualBasic.Right((strHexColor), 6)
        ' however, it may also have been given as or #***** format, so add 0's in front
        HexColor = Nothing
        For i = 1 To (6 - Len(strHexColor))
            HexColor = HexColor & "0"
        Next
        HexColor = HexColor & strHexColor
        ' convert each set of 2 characters into bytes, using vb's cbyte function
        b = CByte("&H" & Microsoft.VisualBasic.Right$(HexColor, 2))
        g = CByte("&H" & Microsoft.VisualBasic.Mid$(HexColor, 3, 2))
        r = CByte("&H" & Microsoft.VisualBasic.Left$(HexColor, 2))
    End Sub

    Public Function RGB2Hex(r As Byte, g As Byte, b As Byte) As String
        On Error Resume Next
        ' convert to long using vb's rgb function, then use the long2rgb function
        RGB2Hex = Long2Hex(RGB(r, g, b))
    End Function


    Public Sub Long2RGB(LongColor As Long, r As Byte, g As Byte, b As Byte)
        On Error Resume Next
        ' convert to hex using vb's hex function,then use the hex2rgb function
        Hex2RGB(Hex(LongColor), r, g, b)
    End Sub


    Public Function RGB2Long(r As Byte, g As Byte, b As Byte) As Long
        On Error Resume Next
        ' use vb's rgb function
        RGB2Long = RGB(r, g, b)
    End Function


    Public Function Long2Hex(LongColor As Long) As String
        On Error Resume Next
        ' use vb's hex function
        Long2Hex = Hex(LongColor)
    End Function


    Public Function Hex2Long(strHexColor As String) As Long
        Dim r As Byte
        Dim g As Byte
        Dim b As Byte
        On Error Resume Next
        ' use the hex2rgb function to get the red green and blue bytes
        Hex2RGB(strHexColor, r, g, b)
        ' convert to long using vb's rgb function
        Hex2Long = RGB(r, g, b)

    End Function

    Public Function Hex2LongVB6(strHexColor As String) As Long
        Dim r As Byte
        Dim g As Byte
        Dim b As Byte
        On Error Resume Next
        ' use the hex2rgb function to get the red green and blue bytes
        Hex2RGB(strHexColor, r, g, b)
        ' convert to long using vb's rgb function
        Return RGB(b, g, r)
    End Function

    Public Function Long2HexVB6(longColor As String) As String
        Dim r As Byte
        Dim g As Byte
        Dim b As Byte
        On Error Resume Next
        ' use the Hex2BGR function to get the red green and blue bytes
        Hex2BGR(Hex(longColor).ToString(), r, g, b)
        ' convert to HEX using vb's RGB2Hex function
        Dim tmpHexColor As String = RGB2Hex(r, g, b)
        ' make sure the string is 6 characters long
        While tmpHexColor.Length < 6
            If tmpHexColor.Length < 6 Then
                tmpHexColor = "0" & tmpHexColor
            End If
        End While
        Return tmpHexColor
    End Function
#End Region
End Class
