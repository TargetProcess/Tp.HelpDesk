// 
// Copyright (c) 2005-2013 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 

using Hd.Portal.Components;
using Hd.Portal.Components.LastActionProcessor;
using log4net;
using Tp.AuthenticationServiceProxy;
using Tp.EntityStateServiceProxy;
using Tp.RequesterServiceProxy;
using System.Globalization;
using System.Web;

namespace Hd.Portal
{
	public class Requester : RequesterDTO, IEntity
	{
		private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		public const int ANONYMOUS_USER_ID = 0;

		public static bool IsLogged
		{
			get { return Logged != null; }
		}

		public static Requester Logged
		{
			get
			{
				log.Debug("Getting logged requester");

				IUserRetriever retriever = HttpContext.Current != null
										   ? new HttpUserRetriever()
										   : new UserRetriever() as IUserRetriever;

				return retriever.LoggedUser;
			}
		}

		public bool IsNew
		{
			get { return !ID.HasValue || ID <= 0; }
		}

		public static bool IsLoggedAsAnonymous
		{
			get
			{
				return Settings.IsPublicMode && (Logged == null && HttpContext.Current.User.Identity.Name == ANONYMOUS_USER_ID.ToString(CultureInfo.InvariantCulture));
			}
		}

		/// <summary>
		/// Returns Logged User ID as for real user or anonymous user
		/// </summary>
		public static int? LoggedUserID
		{
			get
			{
				return IsLogged ? Logged.UserID : (IsLoggedAsAnonymous ? (int?)ANONYMOUS_USER_ID : null);
			}
		}

		public static bool Validate(string email, string password)
		{
			log.DebugFormat("Validate user with email '{0}'", email);
			var serviceWse = ServiceManager.GetService<AuthenticationService>();
			return serviceWse.ValidateRequester(email, password);
		}

		public static Requester FindByEmail(string email)
		{
			log.DebugFormat("Find user by email '{0}'", email);
			var service = ServiceManager.GetService<RequesterService>();
			RequesterDTO[] requesterDtos = service.Retrieve("from Requester as r where r.Email = ? and r.DeleteDate is null", new object[] { email });
			if (requesterDtos.Length == 0)
				return null;
			return DataConverter<Requester>.Convert(requesterDtos[0], new Requester()) as Requester;
		}

		public static bool ForgotPassword(string email)
		{
			var service = ServiceManager.GetService<RequesterService>();
			return service.ForgetPassword(email);
		}

		public bool CanChangeEmailTo(string email)
		{
			RequesterDTO dto = FindByEmail(email);
			if (dto == null)
				return true;
			return dto.ID == Logged.ID;
		}

		public static Requester Retrieve(int? identity)
		{
			log.DebugFormat("Retrieve user with id '{0}'", identity);
			return DataPortal.Instance.Retrieve(typeof(Requester), identity) as Requester;
		}

		public static Requester RetrieveOrCreate(int? requesterId)
		{
			Requester requester = null;
			if (requesterId > 0)
			{
				requester = Retrieve(requesterId);
			}
			return requester ?? new Requester();
		}

		public static Requester RetrieveLogged(object nothing)
		{
			return Logged;
		}

		public static void Save(Requester requester)
		{
			log.DebugFormat("Save requester with id '{0}'", requester.ID);
			var service = ServiceManager.GetService<RequesterService>();
			var dto = new DataConverter<Requester>().Convert<RequesterDTO>(requester);
			int? requesterId;

			if (requester.IsNew)
			{
				requesterId = service.Create(dto);
			}
			else
			{
				service.Update(dto);
				requesterId = dto.ID;
				DataPortal.Instance.ResetCachedValue(typeof(Requester), requesterId);
			}
			
			var actionProcessor = new ActionProcessor();
			requester.ID = requesterId;
			requester.UserID = requesterId;
			ActionTypeEnum actionTypeEnum = requester.IsNew ? ActionTypeEnum.Add : ActionTypeEnum.Update;
			actionProcessor.ProcessAction(actionTypeEnum, requester);
		}
	}
}
