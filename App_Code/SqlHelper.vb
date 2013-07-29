Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Imports System.Text
Imports Microsoft.SqlServer.Management.Smo
Imports Microsoft.SqlServer.Management.Common
Imports Microsoft.SqlServer.Server
Namespace System.Data.SqlClient
    ''' <summary>
    ''' Helper class that makes it easier to work with the sql provider.
    ''' </summary>
    Public NotInheritable Class SQLHelper
        Private Shared stringOfBackslashChars As String = "\¥Š₩∖﹨＼"
        Private Shared stringOfQuoteChars As String = "'`´ʹʺʻʼˈˊˋ˙̀́‘’‚′‵❛❜＇"

        Private Shared sqlServer As String
        Private Shared sqlUserId As String
        Private Shared sqlPassword As String
        Private Shared sqlTrustedConnection As String

        ' this class provides only static methods
        Private Sub New()
        End Sub

#Region "ExecuteScript"

        ''' <summary>
        ''' Executes a script against a SQL database.  The <see cref="SQLConnection"/> is assumed to be
        ''' open when the method is called and remains open after the method completes.
        ''' </summary>
        ''' <param name="connection"><see cref="SQLConnection"/> object to use</param>
        ''' <param name="scriptText">SQL script to be executed</param>
        ''' <returns></returns>
        Public Shared Function ExecuteScript(connection As SqlConnection, scriptText As String) As Integer
            'create a command and prepare it for execution
            Dim svrConnection As New ServerConnection(connection)
            Dim server As New Server(svrConnection)
            Dim result As Integer = server.ConnectionContext.ExecuteNonQuery(scriptText)
            Return result
        End Function

        ''' <summary>
        ''' Executes a script against a SQL database.  The <see cref="SQLConnection"/> is assumed to be
        ''' using the <see cref="SQLConnection.ConnectionString"/> given.
        ''' </summary>
        ''' <param name="connectionString"><see cref="SQLConnection.ConnectionString"/> to use</param>
        ''' <param name="scriptText">SQL script to be executed</param>
        ''' <returns></returns>
        Public Shared Function ExecuteScript(connectionString As String, scriptText As String) As Integer
            'create & open a SqlConnection, and dispose of it after we are done.
            Using cn As New SqlConnection(connectionString)
                cn.Open()

                'call the overload that takes a connection in place of the connection string
                Return ExecuteScript(cn, scriptText)
            End Using
        End Function
#End Region
#Region "ExecuteNonQuery"
        ''' <summary>
        ''' Executes a single command against a SQL database.  The <see cref="SQLConnection"/> is assumed to be
        ''' open when the method is called and remains open after the method completes.
        ''' </summary>
        ''' <param name="connection"><see cref="SQLConnection"/> object to use</param>
        ''' <param name="commandText">SQL command to be executed</param>
        ''' <param name="commandParameters">Array of <see cref="SQLParameter"/> objects to use with the command.</param>
        ''' <returns></returns>
        Public Shared Function ExecuteNonQuery(connection As SqlConnection, commandText As String, ParamArray commandParameters As SqlParameter()) As Integer
            'create a command and prepare it for execution
            Dim cmd As New SqlCommand()
            cmd.Connection = connection
            cmd.CommandText = commandText
            cmd.CommandType = CommandType.Text

            If commandParameters IsNot Nothing Then
                For Each p As SqlParameter In commandParameters
                    cmd.Parameters.Add(p)
                Next
            End If

            Dim result As Integer = cmd.ExecuteNonQuery()
            cmd.Parameters.Clear()

            Return result
        End Function

        ''' <summary>
        ''' Executes a single command against a SQL database.  A new <see cref="SQLConnection"/> is created
        ''' using the <see cref="SQLConnection.ConnectionString"/> given.
        ''' </summary>
        ''' <param name="connectionString"><see cref="SQLConnection.ConnectionString"/> to use</param>
        ''' <param name="commandText">SQL command to be executed</param>
        ''' <param name="parms">Array of <see cref="SQLParameter"/> objects to use with the command.</param>
        ''' <returns></returns>
        Public Shared Function ExecuteNonQuery(connectionString As String, commandText As String, ParamArray parms As SqlParameter()) As Integer
            'create & open a SqlConnection, and dispose of it after we are done.
            Using cn As New SqlConnection(connectionString)
                cn.Open()

                'call the overload that takes a connection in place of the connection string
                Return ExecuteNonQuery(cn, commandText, parms)
            End Using
        End Function
#End Region

#Region "ExecuteDataSet"

        ''' <summary>
        ''' Executes a single SQL command and returns the first row of the resultset.  A new SQLConnection object
        ''' is created, opened, and closed during this method.
        ''' </summary>
        ''' <param name="connectionString">Settings to be used for the connection</param>
        ''' <param name="commandText">Command to execute</param>
        ''' <param name="parms">Parameters to use for the command</param>
        ''' <returns>DataRow containing the first row of the resultset</returns>
        Public Shared Function ExecuteDataRow(connectionString As String, commandText As String, ParamArray parms As SqlParameter()) As DataRow
            Dim ds As DataSet = ExecuteDataset(connectionString, commandText, parms)
            If ds Is Nothing Then
                Return Nothing
            End If
            If ds.Tables.Count = 0 Then
                Return Nothing
            End If
            If ds.Tables(0).Rows.Count = 0 Then
                Return Nothing
            End If
            Return ds.Tables(0).Rows(0)
        End Function


        ''' <summary>
        ''' Executes a single SQL command and returns the first row of the resultset.  A new SQLConnection object
        ''' is created, opened, and closed during this method.
        ''' </summary>
        ''' <param name="connectionString">Settings to be used for the connection</param>
        ''' <param name="commandText">Command to execute</param>
        ''' <param name="parms">Parameters to use for the command</param>
        ''' <returns>DataTable containing the first table of the resultset</returns>
        Public Shared Function ExecuteDataTable(connectionString As String, commandText As String, ParamArray parms As SqlParameter()) As DataTable
            Dim ds As DataSet = ExecuteDataset(connectionString, commandText, parms)
            If ds Is Nothing Then
                Return Nothing
            End If
            If ds.Tables.Count = 0 Then
                Return Nothing
            End If
            Return ds.Tables(0)
        End Function

        ''' <summary>
        ''' Executes a single SQL command and returns the resultset in a <see cref="DataSet"/>.  
        ''' A new SQLConnection object is created, opened, and closed during this method.
        ''' </summary>
        ''' <param name="connectionString">Settings to be used for the connection</param>
        ''' <param name="commandText">Command to execute</param>
        ''' <returns><see cref="DataSet"/> containing the resultset</returns>
        Public Shared Function ExecuteDataset(connectionString As String, commandText As String) As DataSet
            'pass through the call providing null for the set of SqlParameters
            Return ExecuteDataset(connectionString, commandText, DirectCast(Nothing, SqlParameter()))
        End Function

        ''' <summary>
        ''' Executes a single SQL command and returns the resultset in a <see cref="DataSet"/>.  
        ''' A new SQLConnection object is created, opened, and closed during this method.
        ''' </summary>
        ''' <param name="connectionString">Settings to be used for the connection</param>
        ''' <param name="commandText">Command to execute</param>
        ''' <param name="commandParameters">Parameters to use for the command</param>
        ''' <returns><see cref="DataSet"/> containing the resultset</returns>

        Public Shared Function ExecuteDataset(connectionString As String, commandText As String, ParamArray commandParameters As SqlParameter()) As DataSet
            'create & open a SqlConnection, and dispose of it after we are done.
            Using cn As New SqlConnection(connectionString)
                cn.Open()

                'call the overload that takes a connection in place of the connection string
                Return ExecuteDataset(cn, commandText, commandParameters)
            End Using
        End Function

        ''' <summary>
        ''' Executes a single SQL command and returns the resultset in a <see cref="DataSet"/>.  
        ''' The state of the <see cref="SQLConnection"/> object remains unchanged after execution
        ''' of this method.
        ''' </summary>
        ''' <param name="connection"><see cref="SQLConnection"/> object to use</param>
        ''' <param name="commandText">Command to execute</param>
        ''' <returns><see cref="DataSet"/> containing the resultset</returns>

        Public Shared Function ExecuteDataset(connection As SqlConnection, commandText As String) As DataSet
            'pass through the call providing null for the set of SqlParameters
            Return ExecuteDataset(connection, commandText, DirectCast(Nothing, SqlParameter()))
        End Function

        ''' <summary>
        ''' Executes a single SQL command and returns the resultset in a <see cref="DataSet"/>.  
        ''' The state of the <see cref="SQLConnection"/> object remains unchanged after execution
        ''' of this method.
        ''' </summary>
        ''' <param name="connection"><see cref="SQLConnection"/> object to use</param>
        ''' <param name="commandText">Command to execute</param>
        ''' <param name="commandParameters">Parameters to use for the command</param>
        ''' <returns><see cref="DataSet"/> containing the resultset</returns>

        Public Shared Function ExecuteDataset(connection As SqlConnection, commandText As String, ParamArray commandParameters As SqlParameter()) As DataSet
            'create a command and prepare it for execution
            Dim cmd As New SqlCommand()
            cmd.Connection = connection
            cmd.CommandText = commandText
            cmd.CommandType = CommandType.Text

            If commandParameters IsNot Nothing Then
                For Each p As SqlParameter In commandParameters
                    cmd.Parameters.Add(p)
                Next
            End If

            'create the DataAdapter & DataSet
            Dim da As New SqlDataAdapter(cmd)
            Dim ds As New DataSet()

            'fill the DataSet using default values for DataTable names, etc.
            da.Fill(ds)

            ' detach the SQLParameters from the command object, so they can be used again.			
            cmd.Parameters.Clear()

            'return the dataset
            Return ds
        End Function

        ''' <summary>
        ''' Updates the given table with data from the given <see cref="DataSet"/>
        ''' </summary>
        ''' <param name="connectionString">Settings to use for the update</param>
        ''' <param name="commandText">Command text to use for the update</param>
        ''' <param name="ds"><see cref="DataSet"/> containing the new data to use in the update</param>
        ''' <param name="tablename">Tablename in the dataset to update</param>
        Public Shared Sub UpdateDataSet(connectionString As String, commandText As String, ds As DataSet, tablename As String)
            Dim cn As New SqlConnection(connectionString)
            cn.Open()
            Dim da As New SqlDataAdapter(commandText, cn)
            Dim cb As New SqlCommandBuilder(da)
            cb.ToString()
            da.Update(ds, tablename)
            cn.Close()
        End Sub

#End Region

#Region "ExecuteDataReader"

        ''' <summary>
        ''' Executes a single command against a SQL database, possibly inside an existing transaction.
        ''' </summary>
        ''' <param name="connection"><see cref="SQLConnection"/> object to use for the command</param>
        ''' <param name="transaction"><see cref="SQLTransaction"/> object to use for the command</param>
        ''' <param name="commandText">Command text to use</param>
        ''' <param name="commandParameters">Array of <see cref="SQLParameter"/> objects to use with the command</param>
        ''' <param name="ExternalConn">True if the connection should be preserved, false if not</param>
        ''' <returns><see cref="SQLDataReader"/> object ready to read the results of the command</returns>
        Private Shared Function ExecuteReader(connection As SqlConnection, transaction As SqlTransaction, commandText As String, commandParameters As SqlParameter(), ExternalConn As Boolean) As SqlDataReader
            'create a command and prepare it for execution
            Dim cmd As New SqlCommand()
            cmd.Connection = connection
            cmd.Transaction = transaction
            cmd.CommandText = commandText
            cmd.CommandType = CommandType.Text

            If commandParameters IsNot Nothing Then
                For Each p As SqlParameter In commandParameters
                    cmd.Parameters.Add(p)
                Next
            End If

            'create a reader
            Dim dr As SqlDataReader

            ' call ExecuteReader with the appropriate CommandBehavior
            If ExternalConn Then
                dr = cmd.ExecuteReader()
            Else
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            End If

            ' detach the SqlParameters from the command object, so they can be used again.
            cmd.Parameters.Clear()

            Return dr
        End Function

        ''' <summary>
        ''' Executes a single command against a SQL database.
        ''' </summary>
        ''' <param name="connectionString">Settings to use for this command</param>
        ''' <param name="commandText">Command text to use</param>
        ''' <returns><see cref="SQLDataReader"/> object ready to read the results of the command</returns>
        Public Shared Function ExecuteReader(connectionString As String, commandText As String) As SqlDataReader
            'pass through the call providing null for the set of SqlParameters
            Return ExecuteReader(connectionString, commandText, DirectCast(Nothing, SqlParameter()))
        End Function

        ''' <summary>
        ''' Executes a single command against a SQL database.
        ''' </summary>
        ''' <param name="connectionString">Settings to use for this command</param>
        ''' <param name="commandText">Command text to use</param>
        ''' <param name="commandParameters">Array of <see cref="SQLParameter"/> objects to use with the command</param>
        ''' <returns><see cref="SQLDataReader"/> object ready to read the results of the command</returns>
        Public Shared Function ExecuteReader(connectionString As String, commandText As String, ParamArray commandParameters As SqlParameter()) As SqlDataReader
            'create & open a SqlConnection
            Dim cn As New SqlConnection(connectionString)
            cn.Open()

            Try
                'call the private overload that takes an internally owned connection in place of the connection string
                Return ExecuteReader(cn, Nothing, commandText, commandParameters, False)
            Catch
                'if we fail to return the SqlDatReader, we need to close the connection ourselves
                cn.Close()
                Throw
            End Try
        End Function
#End Region

#Region "ExecuteScalar"

        ''' <summary>
        ''' Execute a single command against a SQL database.
        ''' </summary>
        ''' <param name="connectionString">Settings to use for the update</param>
        ''' <param name="commandText">Command text to use for the update</param>
        ''' <returns>The first column of the first row in the result set, or a null reference if the result set is empty.</returns>
        Public Shared Function ExecuteScalar(connectionString As String, commandText As String) As Object
            'pass through the call providing null for the set of SQLParameters
            Return ExecuteScalar(connectionString, commandText, DirectCast(Nothing, SqlParameter()))
        End Function

        ''' <summary>
        ''' Execute a single command against a SQL database.
        ''' </summary>
        ''' <param name="connectionString">Settings to use for the command</param>
        ''' <param name="commandText">Command text to use for the command</param>
        ''' <param name="commandParameters">Parameters to use for the command</param>
        ''' <returns>The first column of the first row in the result set, or a null reference if the result set is empty.</returns>
        Public Shared Function ExecuteScalar(connectionString As String, commandText As String, ParamArray commandParameters As SqlParameter()) As Object
            'create & open a SqlConnection, and dispose of it after we are done.
            Using cn As New SqlConnection(connectionString)
                cn.Open()

                'call the overload that takes a connection in place of the connection string
                Return ExecuteScalar(cn, commandText, commandParameters)
            End Using
        End Function

        ''' <summary>
        ''' Execute a single command against a SQL database.
        ''' </summary>
        ''' <param name="connection"><see cref="SQLConnection"/> object to use</param>
        ''' <param name="commandText">Command text to use for the command</param>
        ''' <returns>The first column of the first row in the result set, or a null reference if the result set is empty.</returns>
        Public Shared Function ExecuteScalar(connection As SqlConnection, commandText As String) As Object
            'pass through the call providing null for the set of SQLParameters
            Return ExecuteScalar(connection, commandText, DirectCast(Nothing, SqlParameter()))
        End Function

        ''' <summary>
        ''' Execute a single command against a SQL database.
        ''' </summary>
        ''' <param name="connection"><see cref="SQLConnection"/> object to use</param>
        ''' <param name="commandText">Command text to use for the command</param>
        ''' <param name="commandParameters">Parameters to use for the command</param>
        ''' <returns>The first column of the first row in the result set, or a null reference if the result set is empty.</returns>
        Public Shared Function ExecuteScalar(connection As SqlConnection, commandText As String, ParamArray commandParameters As SqlParameter()) As Object
            'create a command and prepare it for execution
            Dim cmd As New SqlCommand()
            cmd.Connection = connection
            cmd.CommandText = commandText
            cmd.CommandType = CommandType.Text

            If commandParameters IsNot Nothing Then
                For Each p As SqlParameter In commandParameters
                    cmd.Parameters.Add(p)
                Next
            End If

            'execute the command & return the results
            Dim retval As Object = cmd.ExecuteScalar()

            ' detach the SqlParameters from the command object, so they can be used again.
            cmd.Parameters.Clear()
            Return retval

        End Function

#End Region



#Region "Utility methods"

        ''' <summary>
        ''' Set connection string by database name
        ''' </summary>
        ''' <param name="database">database name</param>
        ''' <returns>The connectiong string.</returns>
        Public Shared Function SetConnectionString(ByVal Database As String) As String
            sqlServer = System.Configuration.ConfigurationManager.AppSettings("Server,Port").ToString()
            sqlUserId = System.Configuration.ConfigurationManager.AppSettings("UserId").ToString()
            sqlPassword = System.Configuration.ConfigurationManager.AppSettings("Password").ToString()
            sqlTrustedConnection = System.Configuration.ConfigurationManager.AppSettings("TrustedConnection").ToString()
            Return "Server=" & sqlServer & ";Database=" & Database & _
                                                ";User Id=" & sqlUserId & ";Password=" & sqlPassword & _
                                                ";Trusted_Connection=" & sqlTrustedConnection & ";"
        End Function


        ''' <summary>
        ''' Escapes the string.
        ''' </summary>
        ''' <param name="value">The string to escape</param>
        ''' <returns>The string with all quotes escaped.</returns>
        Public Shared Function EscapeString(value As String) As String
            Dim sb As New StringBuilder()
            For Each c As Char In value
                'sb.Append(c);
                'else if (
                If stringOfQuoteChars.IndexOf(c) >= 0 OrElse stringOfBackslashChars.IndexOf(c) >= 0 Then
                    sb.Append("\")
                End If
                sb.Append(c)
            Next
            Return sb.ToString()
        End Function

        Public Shared Function DoubleQuoteString(value As String) As String
            Dim sb As New StringBuilder()
            For Each c As Char In value
                If stringOfQuoteChars.IndexOf(c) >= 0 Then
                    sb.Append(c)
                ElseIf stringOfBackslashChars.IndexOf(c) >= 0 Then
                    sb.Append("\")
                End If
                sb.Append(c)
            Next
            Return sb.ToString()
        End Function

#End Region
    End Class
End Namespace
