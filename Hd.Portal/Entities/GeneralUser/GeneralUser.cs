// 
// Copyright (c) 2005-2008 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Tp.GeneralUserServiceProxy;

namespace Hd.Portal
{
	public class GeneralUser : GeneralUserDTO, IEntity
	{
		public string FullName
		{
			get
			{
				var firstName = ProtectEmailIfExists(FirstName);
				var lastName = ProtectEmailIfExists(LastName);
				return string.Format("{0} {1}", firstName, lastName).Trim();
			}
		}

		public string ProtectEmailIfExists(string word)
		{
			var regEx = new Regex("@.*$");
			return regEx.Replace(word ?? string.Empty, "");
		}
	}
}