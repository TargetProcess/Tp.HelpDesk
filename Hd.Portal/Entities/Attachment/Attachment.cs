// 
// Copyright (c) 2005-2008 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 

using Tp.AttachmentServiceProxy;

namespace Hd.Portal
{
	public class Attachment : AttachmentDTO, IEntity
	{
		public GeneralUser Owner
		{
			get { return DataPortal.Instance.Retrieve(typeof (GeneralUser), OwnerID) as GeneralUser; }
		}

		public static AttachmentInfo RetrieveAttachmentInfo(int attachmentID)
		{
			if (attachmentID <= 0)
			{
				throw new EntityNotFoundException();
			}

			var serviceWse = ServiceManager.GetService<AttachmentService>();
			var attachmentInfo = serviceWse.EnsureAttachment(attachmentID);

			if (string.IsNullOrEmpty(attachmentInfo.UniqueFileName))
			{
				throw new EntityNotFoundException();
			}

			if (attachmentInfo.General == null)
			{
				throw new AccessDeniedException();
			}

			var request = Request.Retrieve(attachmentInfo.General.GeneralID);
			
			if (request == null)
			{
				throw new AccessDeniedException();
			}
			
			if (!PermissionManager.HaveRightToViewRequest(request))
			{
				throw new AccessDeniedException();
			}

			return attachmentInfo;
		}
	}
}