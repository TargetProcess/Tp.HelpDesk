// 
// Copyright (c) 2005-2008 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Security;

namespace Hd.Portal.Components
{
	public class HttpUserRetriever : IUserRetriever
	{
		private static readonly string LOGGED_USER = "LOGGED_USER";

		public Requester LoggedUser
		{
			get
			{
				HttpContext current = HttpContext.Current;

				if (ReferenceEquals(current, null))
				{
					throw new ApplicationException("HttpContextRetriever could not be used for non web applications");
				}

				Requester generalUser = HttpContext.Current.Items[LOGGED_USER] as Requester;

				if (!ReferenceEquals(generalUser, null))
				{
					return generalUser;
				}

				int userID;

				if (current.User.Identity.IsAuthenticated)
				{
					if (Int32.TryParse(current.User.Identity.Name, out userID))
					{
						Requester loggedUser = Requester.Retrieve(userID);
						HttpContext.Current.Items[LOGGED_USER] = loggedUser;
						return loggedUser;
					}
					else
					{
						//Fix only for Web Forms...
						FormsAuthentication.SignOut();
						HttpContext.Current.Response.Redirect(FormsAuthentication.LoginUrl);
					}
				}

				return null;
			}
		}

		public void SetUser(Requester user)
		{
			HttpContext.Current.Items[LOGGED_USER] = user;
		}
	}
}