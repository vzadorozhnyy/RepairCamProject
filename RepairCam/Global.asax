<%@ Application Language="C#" %>
<%@ Import Namespace="System.Security.Principal" %>
<%@ Import Namespace="DataModel" %>

<script runat="server">

    private void Application_Start(object sender, EventArgs e) {
        // Code that runs on application startup
        string lConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        DataManager.Inst.Init(lConnectionString);
    }

    private void Application_End(object sender, EventArgs e) {
        //  Code that runs on application shutdown
    }

    private void Application_Error(object sender, EventArgs e) {
        // Code that runs when an unhandled error occurs
    }

    private void Session_Start(object sender, EventArgs e) {
        // Code that runs when a new session is started
        IPrincipal lPrincipal = HttpContext.Current.User;
        if (lPrincipal != null)
            DataManagerLocal.Inst.LoggedUser = DataManager.Inst.GetUserByLogin(lPrincipal.Identity.Name);
    }

    private void Session_End(object sender, EventArgs e) {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.
    }</script>