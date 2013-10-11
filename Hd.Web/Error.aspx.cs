// 
// Copyright (c) 2005-2013 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 

using System;
using System.Collections;
using Hd.Web.Extensions;

public partial class Error : PersisterBasePage
{
	protected void Page_Load(object sender, EventArgs e)
	{
		var error = Application["LastError"] as Exception;

		if (error != null)
		{
			var stack = new Stack();
			stack.Push(error);

			while (!ReferenceEquals(error.InnerException, null))
			{
				stack.Push(error.InnerException);
				error = error.InnerException;
			}
			ltError.Text = string.Empty;

			while (stack.Count != 0)
			{
				var ex = (Exception)stack.Pop();
				ltError.Text += "Error Message: <pre>" + ex.Message + "</pre><br />"
					+ "Error Source:" + ex.Source + "<br />"
					+ "Error Help Link:" + ex.HelpLink + "<br />"
					+ "Error Stack Trace:" + "<br />"
					+ "<pre>" + ex.StackTrace + "</pre>";
				ltError.Text += "<br/>";
			}
		}
	}
}
