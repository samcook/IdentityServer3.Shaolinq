using Shaolinq;

namespace IdentityServer3.Shaolinq.DataModel.Interfaces
{
	public interface IIdentityServerOperationalDataAccessModel
	{
		DataAccessObjects<DbConsent> Consents { get; }
		DataAccessObjects<DbToken> Tokens { get; }
	}
}