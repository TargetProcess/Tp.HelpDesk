// 
// Copyright (c) 2005-2009 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using Hd.Portal;
using Tp.AttachmentServiceProxy;
using Tp.FileServiceProxy;

namespace Hd.Web
{
	/// <summary>
	/// Undocumented.
	/// </summary>
	public class Attachment : IHttpHandler
	{
		public void ProcessRequest(HttpContext context)
		{
			int attachmentID;

			Int32.TryParse(context.Request.QueryString["AttachmentID"], out attachmentID);

			if (attachmentID <= 0)
			{
				AttachmentNotFound(context);
			}
			else
			{
				var serviceWse = ServiceManager.GetService<AttachmentService>();
				var attachmentInfo = serviceWse.EnsureAttachment(attachmentID);

				if (string.IsNullOrEmpty(attachmentInfo.UniqueFileName))
				{
					AttachmentNotFound(context);
				}
				else
				{
					DownloadAttachment(context, attachmentInfo);
				}
			}
		}

		private static void AttachmentNotFound(HttpContext context)
		{
			context.Response.ContentType = "text/plain";
			context.Response.Write("The attachment could not be found");
		}

		private static void DownloadAttachment(HttpContext context, AttachmentInfo attachmentInfo)
		{
			context.Response.Clear();

			context.Response.ContentType = "application/octet-stream";
			context.Response.AppendHeader("Content-Disposition", "attachment; filename=\"" + attachmentInfo.OriginalFileName + "\"");

			var fileService = ServiceManager.GetService<FileService>();
			long fileSize = fileService.GetFileSize(attachmentInfo.UniqueFileName);
			long receivedBytes = 0;
			const int chunkSize = 100*1024;

			while (receivedBytes < fileSize)
			{
				byte[] buffer = fileService.DownloadChunk(attachmentInfo.UniqueFileName, receivedBytes, chunkSize);
				context.Response.BinaryWrite(buffer);
				receivedBytes += buffer.Length;
			}

			context.Response.End();
		}

		public bool IsReusable
		{
			get { return true; }
		}
	}
}