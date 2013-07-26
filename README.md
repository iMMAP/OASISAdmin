OASISAdmin
==========

OASISAdmin

install support:

	1 Microsoft? SQL Server? 2012 Native Client 
	2 .NET Framework 4.0
	
Please connection string on web.config:

	<appSettings>
		<add key="Server,Port" value="sqlserver url,port " />
		<add key="UserId" value="sql server user id" />
		<add key="Password" value="sql server user password" />
		<add key="DbUrl" value="the web site that return list of daatabse as xml schema from Classic ADO" />
	</appSettings>

