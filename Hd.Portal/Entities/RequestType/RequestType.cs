// 
// Copyright (c) 2005-2012 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 

using System.Collections;
using Tp.RequestServiceProxy;

namespace Hd.Portal
{
	public class RequestType : RequestTypeDTO, IEntity
	{
		public static IList RetrieveAll()
		{
			return DataPortal.Instance.RetrieveAll<RequestType>();
		}
	}
}