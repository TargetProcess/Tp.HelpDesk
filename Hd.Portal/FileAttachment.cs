// 
// Copyright (c) 2005-2008 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Hd.Portal
{
	[Serializable]
	public class FileAttachment
	{
		private string originalName = string.Empty;
		private Stream fileStream = null;
		private string description = string.Empty;

		public string Description
		{
			get { return description; }
			set { description = value; }
		}

		public string OriginalName
		{
			get { return originalName; }
			set { originalName = value; }
		}

		public Stream FileStream
		{
			get { return fileStream; }
			set { fileStream = value; }
		}
	}
}