<%@ Import namespace="log4net.Config"%>
<%@ Import namespace="log4net"%>
<%@ Import namespace="System.Xml"%>
<%@ Application Language="C#" %>

<script runat="server">
    private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    
    void Application_Start(object sender, EventArgs e) 
    {
        XmlElement config = (XmlElement)ConfigurationManager.GetSection("log4net");
        XmlConfigurator.Configure(config);
        log.Info("Application was started");
        BeginRequest += global_asax_BeginRequest;
        EndRequest += global_asax_EndRequest;
    }

    void global_asax_EndRequest(object sender, EventArgs e)
    {
        log.DebugFormat("End Request '{0}'", HttpContext.Current.Request.Url.AbsoluteUri);
    }

    void global_asax_BeginRequest(object sender, EventArgs e)
    {
        log.DebugFormat("Begin Request '{0}'", HttpContext.Current.Request.Url.AbsoluteUri);
    }
    
    void Application_End(object sender, EventArgs e) 
    {
        log.Info("Application was stopped");
    }
        
    void Application_Error(object sender, EventArgs e) 
    { 
        log.Error("Application Error", Server.GetLastError());
    }

    void Session_Start(object sender, EventArgs e) 
    {}

    void Session_End(object sender, EventArgs e) 
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }
       
</script>
