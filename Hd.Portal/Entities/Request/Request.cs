// 
// Copyright (c) 2005-2012 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Hd.Portal.Components;
using Hd.Portal.Components.LastActionProcessor;
using Hd.Portal.Entities.EntityState;
using Hd.Portal.Entities.Request;
using Hd.QueryExtensions;
using log4net;
using Tp.AttachmentServiceProxy;
using Tp.EntityStateServiceProxy;
using Tp.FileServiceProxy;
using Tp.RequestServiceProxy;

namespace Hd.Portal
{
	public class Request : RequestDTO, IEntity
	{
		private const string ELLIPSIS = "...";
		private const int SHORT_DESC_SIZE = 200;
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		public Request()
		{
			IsPrivate = false;
		}

		public GeneralUser Owner
		{
			get { return DataPortal.Instance.Retrieve(typeof (GeneralUser), OwnerID) as GeneralUser; }
		}

		public GeneralUser LastEditor
		{
			get { return DataPortal.Instance.Retrieve(typeof (GeneralUser), LastEditorID) as GeneralUser; }
		}

		public EntityState EntityState
		{
			get { return DataPortal.Instance.Retrieve(typeof (EntityState), EntityStateID) as EntityState; }
		}

		public RequestType RequestType
		{
			get { return DataPortal.Instance.Retrieve(typeof (RequestType), RequestTypeID) as RequestType; }
		}

		public long RequestersCount
		{
			get
			{
				if (IsNew)
				{
					return 0;
				}

				var cache = new RequestersCountCache();

				var childs = cache.GetCachedChilds(ID.Value);

				if (childs != null)
				{
					foreach (RequestersCount requestersCount in childs)
					{
						return requestersCount.Count;
					}
				}

				var service = ServiceManager.GetService<RequestService>();
				return service.GetRequestersCount(ID);
			}
		}

		public RequestRequesterDTO[] Requesters
		{
			get
			{
				var service = ServiceManager.GetService<RequestService>();
				return service.RetrieveRequestersForRequest(ID.Value);
			}
		}

		public Priority Priority
		{
			get { return DataPortal.Instance.Retrieve(typeof (Priority), PriorityID) as Priority; }
		}

		public List<RelatedEntity> RelatedEntities
		{
			get
			{
				if (IsNew)
				{
					return new List<RelatedEntity>();
				}

				var service = ServiceManager.GetService<RequestService>();
				return new List<RelatedEntity>(service.RetrieveRelatedEntities(ID));
			}
		}

		public virtual List<FileAttachment> FileAttachments
		{
			get { return new List<FileAttachment>(); }
			set
			{
				foreach (var fileAttachment in value)
				{
				}
			}
		}

		public List<Team> Teams
		{
			get
			{
				var cache = new TeamCache();
				var list = new List<Team>();

				if (!ID.HasValue)
				{
					return list;
				}

				var childs = cache.GetCachedChilds(ID.Value);

				if (childs != null)
				{
					list.AddRange(childs.Cast<Team>());

					return list;
				}

				LoadTeams(list);

				return list;
			}
		}

		public bool IsUrgent { get; set; }

		public bool IsNew
		{
			get { return !ID.HasValue || ID <= 0; }
		}

		public string ShortDescription
		{
			get
			{
				if (string.IsNullOrEmpty(Description))
				{
					return null;
				}
				var description = StringUtils.StripTags(Description);
				if (description.Length < SHORT_DESC_SIZE)
				{
					return description;
				}
				return description.Substring(0, SHORT_DESC_SIZE - ELLIPSIS.Length) + ELLIPSIS;
			}
		}

		private void LoadTeams(List<Team> list)
		{
			var service = ServiceManager.GetService<RequestService>();
			var teams = service.RetrieveTeamsForRequest(ID.Value);

			foreach (var dto in teams)
			{
				var team = new Team();
				var actor = DataPortal.Instance.Retrieve(typeof (Role), dto.RoleID) as Role;

				if (actor != null)
				{
					team.ActorName = actor.Name;
				}

				var user = DataPortal.Instance.Retrieve(typeof (User), dto.UserID) as User;

				if (user != null)
				{
					team.UserName = string.Format("{0} {1}", user.FirstName, user.LastName);
				}

				list.Add(team);
			}
		}

		public static Request RetrieveOrCreate(int? requestID)
		{
			var request = new Request();
			if (requestID > 0)
			{
				request = Retrieve(requestID) ?? request;
			}
			if (request.IsNew)
			{
				var list = RetrieveProducts();
				if (list.Count == 1)
				{
					request.ProjectID = list[0].ID;
				}
			}
			return request;
		}

		public static Request Retrieve(int? requestID, bool skipSecurity)
		{
			if (!requestID.HasValue)
			{
				return null;
			}

			SelectQuery query;
			if (skipSecurity)
			{
				query = RequestQueryFactory.CreateDefaultQuery().InitialQuery;
			}
			else
			{
				query = RequestQueryFactory.CreateRetreaveQuery().InitialQuery;
			}
			query.PageSettings = new PageSettings {PageSize = 1, PageIndex = 1};

			query.AddCompare("RequestID", new Parameter(requestID), CompareOperator.Equal);
			query.OrderByTerms.Clear();
			var list = DataPortal.Instance.Retrieve<Request>(query);
			if (list.Count == 0)
			{
				return null;
			}
			return list[0];
		}

		public static Request Retrieve(int? requestID)
		{
			return Retrieve(requestID, false);
		}

		public static Request RetrieveToEdit(int? requestID)
		{
			if (!requestID.HasValue)
				return null;
			var query = RequestQueryFactory.CreateQueryToEdit().InitialQuery;
			query.PageSettings = new PageSettings {PageSize = 1, PageIndex = 1};
			query.AddCompare("RequestID", new Parameter(requestID), CompareOperator.Equal);
			query.OrderByTerms.Clear();
			var list = DataPortal.Instance.Retrieve<Request>(query);
			if (list.Count == 0)
				return null;
			return list[0];
		}

		public static Request RetrieveToEditOrCreate(int? requestID)
		{
			var request = new Request();
			if (requestID > 0)
			{
				request = RetrieveToEdit(requestID) ?? request;
			}
			if (request.IsNew)
			{
				var list = RetrieveProducts();
				if (list.Count == 1)
				{
					request.ProjectID = list[0].ID;
				}
			}
			return request;
		}

		public static void Save(Request request)
		{
			var service = ServiceManager.GetService<RequestService>();
			var dto = new DataConverter<Request>().Convert<RequestDTO>(request);
			dto.SourceType = RequestSourceEnum.External;
			var actionTypeEnum = request.IsNew ? ActionTypeEnum.Add : ActionTypeEnum.Update;
			try
			{
				if(request.IsNew)
				{
					request.ID = service.Save(dto, Requester.Logged.ID, request.IsUrgent);
					request.RequestID = request.ID;
				}
				else
				{
					request.LastEditorID = Requester.Logged.ID;
					service.Update(dto);
				}

				var actionProcessor = new ActionProcessor();

				actionProcessor.ProcessAction(actionTypeEnum, request);
			}
			catch (Exception ex)
			{
				log.Debug("Error catched", ex);

				var error = ex.Message;

				if (ex.Message.Contains("AccessDeniedException"))
				{
					error = "The request was not posted. Access is denied.";
				}

				if (ex.Message.Contains("password is invalid"))
				{
					error = "The request was not posted. The provided password is invalid.";
				}

				ActionProcessor.SetLastAction(error, null, ActionTypeEnum.None);
				ActionProcessor.IsError = true;
			}
		}

		public static void RemoveRequester(int? requestID, int? requesterID)
		{
			var service = ServiceManager.GetService<RequestService>();
			service.RemoveRequester(requestID, requesterID);
			ActionProcessor.LastAction = "The request was detached";
		}

		public static IList<Product> RetrieveProducts()
		{
			var service = ServiceManager.GetService<RequestService>();
			return RetrieveProducts(service.RetrieveProducts(), Requester.Logged);
		}

		public static IList<Product> RetrieveProducts(Product[] allProducts, Requester requester)
		{
			var products = new List<Product>();
			foreach (var product in allProducts)
			{
				if (!product.CompanyID.HasValue)
				{
					products.Add(product);
					continue;
				}

				if (requester != null && requester.CompanyID.HasValue && product.CompanyID == requester.CompanyID)
				{
					products.Add(product);
				}
			}
			return products;
		}

		public static void AddRequester(int requestID, RequestRequesterDTO requesterDTO)
		{
			var service = ServiceManager.GetService<RequestService>();
			service.AddRequestRequesterToRequest(requestID, requesterDTO);
			ActionProcessor.LastAction = "Your vote added";
		}

		public static bool IsRequesterAttached(int requestID, int requesterID)
		{
			var service = ServiceManager.GetService<RequestService>();
			return service.IsRequesterAttached(requestID, requesterID);
		}

		public void AddAttachments(List<FileAttachment> fileAttachments)
		{
			if (fileAttachments.Count == 0)
			{
				return;
			}

			var requestId = ID.Value;
			var service = ServiceManager.GetService<FileService>();
			var requestService = ServiceManager.GetService<RequestService>();
			const int chunkSize = 100*1024;
		
			foreach (var attachment in fileAttachments)
			{
				var fileName = string.Format("{0}_{1}", Guid.NewGuid(), attachment.OriginalName);

				using (var stream = attachment.FileStream)
				{
					var buffer = new byte[chunkSize];
					var bytesRead = stream.Read(buffer, 0, chunkSize);
					if (bytesRead > 0 && bytesRead < chunkSize)
					{
						var lastBuffer = new byte[bytesRead];
						Buffer.BlockCopy(buffer, 0, lastBuffer, 0, bytesRead);
						buffer = lastBuffer;
					}
					long uploadedLength = 0;

					while (bytesRead > 0)
					{
						service.AppendChunk(fileName, buffer, uploadedLength, bytesRead);
						uploadedLength += bytesRead;
						buffer = new byte[chunkSize];
						bytesRead = stream.Read(buffer, 0, chunkSize);
						if (bytesRead > 0 && bytesRead < chunkSize)
						{
							var lastBuffer = new byte[bytesRead];
							Buffer.BlockCopy(buffer, 0, lastBuffer, 0, bytesRead);
							buffer = lastBuffer;
						}
					}
				}

				var attachmentId = requestService.AddAttachmentToRequest(requestId, fileName, attachment.Description);
				var attachmentServiceWse = ServiceManager.GetService<AttachmentService>();
				var attachmentDto = attachmentServiceWse.GetByID(attachmentId);
				attachmentDto.OriginalFileName = attachment.OriginalName;
				attachmentDto.OwnerID = Requester.Logged.ID.Value;
				attachmentServiceWse.Update(attachmentDto);
			}
		}
	}
}