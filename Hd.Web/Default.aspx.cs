// 
// Copyright (c) 2005-2013 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 

using System;
using System.IO;
using Hd.Portal;
using Hd.Web.Extensions;

public partial class Default : PersisterBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        phMessage.Visible = !Requester.IsLogged;

        String homeContent = "<p>You may:</p><ul><li>Post requests/ideas/issues</li><li>Vote for requests</li><li>View your requests with statuses</li><li>Discuss requests via comments threads</li><li>Attach files to requests</li><li>View related bugs and user stories</li></ul>";
        
        try { homeContent = File.ReadAllText(Server.MapPath("~/App_Data/home.txt")); }
        catch (Exception) { }

        litHome.Text = homeContent;
    }
}