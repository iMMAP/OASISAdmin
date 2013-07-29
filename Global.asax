<%@ Application Language="VB" %>
<%@ Import Namespace="System.Web.Routing" %>
<script RunAt="server">

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs on application startup
        RegisterRoutes(RouteTable.Routes)
    End Sub
    
    
    Shared Sub RegisterRoutes(routes As RouteCollection)
        routes.MapPageRoute("Login",
              "Login",
              "~/Login.aspx", True)
        
        routes.MapPageRoute("User",
            "User",
            "~/User.aspx", True)
        
        routes.MapPageRoute("UserGroup",
            "UserGroup",
            "~/UserGroups.aspx", True)
        
        routes.MapPageRoute("Security",
            "Security",
            "~/SecurityModuleSettings.aspx", True)
        
        routes.MapPageRoute("Map",
            "Map",
            "~/MapViewSettings.aspx", True)
        
        routes.MapPageRoute("General",
            "General",
            "~/GeneralSettings.aspx", True)
        
        routes.MapPageRoute("Home",
            "Home",
            "~/Home.aspx", True)
        
        routes.MapPageRoute("Logout",
            "Logout",
            "~/LogOut.aspx", True)
        
        routes.MapPageRoute("DynamicModules",
            "DynamicModules",
            "~/DynamicDataDefsNew.aspx", True)
        
        routes.MapPageRoute("DataViews",
            "DataViews",
            "~/GISGridTableSettings.aspx", True)
        
        routes.MapPageRoute("RSS",
            "RSS",
            "~/Feeds.aspx", True)
        
        routes.MapPageRoute("RSSGroups",
            "RSSGroups",
            "~/FeedGroups.aspx", True)
        
        routes.MapPageRoute("MapLibrary",
            "MapLibrary",
            "~/TtkGISProjectDef.aspx", True)
        
        routes.MapPageRoute("ThemeGroups",
            "ThemeGroups",
            "~/ThemeGroups.aspx", True)
       
        routes.MapPageRoute("Themes",
            "Themes",
            "~/Themes.aspx", True)
        
        routes.MapPageRoute("GeoBookMarksCategories",
           "GeoBookMarksCategories",
           "~/GeoBookMarksCategories.aspx", True)
        
        routes.MapPageRoute("GeoBookMarks",
           "GeoBookMarks",
           "~/GeoBookMarks.aspx", True)
        
        routes.MapPageRoute("WebTiles",
            "WebTiles",
            "~/WebTiles.aspx", True)
    
        routes.MapPageRoute("Formatting",
            "Formatting",
            "~/DynamicDataManager.aspx", True)
        
        routes.MapPageRoute("Validation",
            "Validation",
            "~/Validation.aspx", True)
        
        routes.MapPageRoute("NearbyFeatures",
            "NearbyFeatures",
            "~/NearbyFeatures.aspx", True)
        
        routes.MapPageRoute("ChartSettings",
            "Charts",
            "~/ChartSettings.aspx", True)

        routes.MapPageRoute("Queries",
            "Queries",
            "~/Queries.aspx", True)
        
        routes.MapPageRoute("ComboRelations",
            "ComboRelations",
            "~/ComboRelations.aspx", True)
    End Sub
    
    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs on application shutdown
    End Sub
        
    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when an unhandled error occurs
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when a new session is started
        Session("UserAuthentication") = ""
        Session("database") = ""
        Session("Username") = ""
        Session("ID") = ""
        Session("IsPass") = ""
        Session("UserGroup") = ""
    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when a session ends. 
        ' Note: The Session_End event is raised only when the sessionstate mode
        ' is set to InProc in the Web.config file. If session mode is set to StateServer 
        ' or SQLServer, the event is not raised.
        Session("UserAuthentication") = ""
        Session("database") = ""
        Session("Username") = ""
        Session("ID") = ""
        Session("IsPass") = ""
        Session("UserGroup") = ""
        '  Session.Abandon()
    End Sub
       
</script>
