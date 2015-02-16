// 
// Copyright (c) 2005-2013 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 

using System;
using System.Web.UI.WebControls;
using Hd.Web.Extensions;

namespace Hd.Web
{
    public partial class Ideas : PersisterBasePage
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            try
            {
                if (Request.QueryString[0].ToLower() == "true" && controller.Grid.SortExpression != "size(request.Requesters)")
                    controller.Grid.Sort("size(request.Requesters)", SortDirection.Descending);

            }
            catch (Exception)
            {

            }

            controller.RefreshGrid();
        }
    }
}