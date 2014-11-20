// 
// Copyright (c) 2005-2013 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 

using System;
using System.Web.UI;
using Hd.Portal;
using Hd.Portal.Components;
using Hd.Web.Extensions;
using Tp.RequestServiceProxy;

namespace Hd.Web.Controls
{
    public partial class RequestList : UserControl
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            voteManager.VoteAdded += voteManager_VoteAdded;
            
            if (Settings.Scope == RequestScope.Private)
                Response.Redirect("~/MyRequests.aspx");
        }

        private void voteManager_VoteAdded(Object sender, VoteEventArgs args)
        {
            if (!Requester.IsLogged || Portal.Request.IsRequesterAttached(args.RequestID.Value, Requester.LoggedUserID.Value))
                return;
            var dto = new RequestRequesterDTO
                      {
                          RequestID = args.RequestID.Value,
                          RequesterID = Requester.Logged.ID
                      };
            Portal.Request.AddRequester(args.RequestID.Value, dto);
        }
    }
}