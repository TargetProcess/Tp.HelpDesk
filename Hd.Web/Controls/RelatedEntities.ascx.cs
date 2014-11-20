// 
// Copyright (c) 2005-2013 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 

using System;
using System.Web.UI;
using Hd.Portal;
using Hd.Web.Extensions;

public partial class Controls_RelatedEntities : UserControl, ITabControl
{
    public void LoadContent(object argument)
    {
        try
        {
            Request request = Hd.Portal.Request.Retrieve(argument as int?);

            if (request == null)
                throw new Exception("The request could not be found");

            grid.DataSource = request.RelatedEntities;
            grid.DataBind();
        }
        catch (Exception)
        {
            // ignore, FIX for a bug introduced by UI overhaul when
            // dynamic tab for Related Entities was removed.
        }
    }
}