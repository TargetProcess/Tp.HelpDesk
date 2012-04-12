using Tp.PasswordRecoveryServiceProxy;

namespace Hd.Portal.Entities.PasswordRecovery
{
	public class PasswordRecovery : PasswordRecoveryDTO, IEntity
	{
		public static Requester RetrieveByActivationKey(string activationKey)
		{
			var service = ServiceManager.GetService<PasswordRecoveryService>();
			PasswordRecoveryDTO recoveryDTO = service.RetrieveValidByActivationKey(activationKey);
			if (recoveryDTO == null)
				return null;
			return Requester.Retrieve(recoveryDTO.UserID);
		}
	}
}