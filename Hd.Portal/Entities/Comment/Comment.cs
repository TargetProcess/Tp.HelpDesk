// 
// Copyright (c) 2005-2012 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 

using System;
using Hd.Portal.Components.LastActionProcessor;

using log4net;

using Tp.CommentServiceProxy;
using Tp.EntityStateServiceProxy;

namespace Hd.Portal
{
	public class Comment : CommentDTO, IEntity
	{
		private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		public GeneralUser Owner
		{
			get { return DataPortal.Instance.Retrieve(typeof (GeneralUser), OwnerID) as GeneralUser; }
		}

		public static void Delete(int id)
		{
			var serviceWse = ServiceManager.GetService<CommentService>();
			serviceWse.Delete(id);
		}

		public static Comment RetrieveOrCreate(int? id)
		{
			var comment = new Comment {OwnerID = Requester.Logged.ID};

			comment = (DataPortal.Instance.Retrieve(typeof (Comment), id) as Comment) ?? comment;

			return comment;
		}

		public static bool TryRetrieve(int? id, out Comment comment)
		{
			try
			{
				comment = (DataPortal.Instance.Retrieve(typeof(Comment), id) as Comment);
				return comment != null;
			}
			catch (Exception)
			{
				comment = null;
				return false;
			}
		}

		public void Save()
		{
			var serviceWse = ServiceManager.GetService<CommentService>();

			var commentDto = new DataConverter<Comment>().Convert<CommentDTO>(this);
			try
			{
				bool isNew = !commentDto.ID.HasValue || commentDto.ID.Value <= 0;
				if (isNew)
				{
					serviceWse.Create(commentDto);
				}
				else
				{
					serviceWse.Update(commentDto);
				}

				var actionProcessor = new ActionProcessor();
				actionProcessor.ProcessAction(isNew ? ActionTypeEnum.Add : ActionTypeEnum.Update, this);
			}
			catch (Exception e)
			{
				log.Debug("Error catched", e);

				if (e.Message.Contains("should not be empty"))
				{
					ActionProcessor.LastAction = ("The description of comment should not be empty");
					ActionProcessor.IsError = true;
				}
				else
				{
					throw;
				}
			}
		}
	}
}