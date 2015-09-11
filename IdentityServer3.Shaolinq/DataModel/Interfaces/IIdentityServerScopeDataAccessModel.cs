using Shaolinq;

namespace IdentityServer3.Shaolinq.DataModel.Interfaces
{
	public interface IIdentityServerScopeDataAccessModel
	{
		DataAccessObjects<DbScope> Scopes { get; }
		DataAccessObjects<DbScopeClaim> ScopeClaims { get; }
	}
}