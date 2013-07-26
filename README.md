OASISAdmin
==========

OASISAdmin

install support:
	Microsoft? SQL Server? 2012 Native Client 
	x86 Package (http://go.microsoft.com/fwlink/?LinkID=239647&clcid=0x409)
	x64 Package (http://go.microsoft.com/fwlink/?LinkID=239648&clcid=0x409)
	.NET Framework 4.0 (http://www.microsoft.com/en-us/download/details.aspx?id=17718)
	
Please connection string on web.config:
	<appSettings>
		<add key="Server,Port" value="sqlserver url,port " />
		<add key="UserId" value="sql server user id" />
		<add key="Password" value="sql server user password" />
		<add key="DbUrl" value="the web site that return list of daatabse as xml schema form Classic ADO" />
	</appSettings>