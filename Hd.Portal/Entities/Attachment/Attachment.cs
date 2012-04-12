// 
// Copyright (c) 2005-2008 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.Collections.Generic;
using System.Text;

using Tp.AttachmentServiceProxy;

namespace Hd.Portal
{
	public class Attachment : AttachmentDTO, IEntity
	{
		public GeneralUser Owner
		{
			get { return DataPortal.Instance.Retrieve(typeof (GeneralUser), OwnerID) as GeneralUser; }
		}
	}
}