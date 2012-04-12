// 
// Copyright (c) 2005-2008 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.Collections.Generic;
using System.Text;

namespace Hd.QueryExtensions
{
	/// <summary>
	/// InvalidQueryException exception can be thrown when the renderer decides that a query is invalid or incompatible with the target database.
	/// </summary>
	public class InvalidQueryException : ApplicationException
	{
		/// <summary>
		/// Creates a new InvalidQueryException
		/// </summary>
		/// <param name="text">Text of the exception</param>
		public InvalidQueryException(string text) : base(text) {}
	}
}