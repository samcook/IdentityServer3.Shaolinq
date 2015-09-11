using Shaolinq;

namespace IdentityServer3.Shaolinq.DataModel.Interfaces
{
	public interface IIdentityServerClientDataAccessModel
	{
		DataAccessObjects<DbClient> Clients { get; }
		DataAccessObjects<DbClientClaim> ClientsClaims { get; }
		DataAccessObjects<DbClientCorsOrigin> ClientCorsOrigins { get; }
		DataAccessObjects<DbClientCustomGrantType> ClientCustomGrantTypes { get; }
		DataAccessObjects<DbClientIdpRestriction> ClientIdpRestrictions { get; }
		DataAccessObjects<DbClientPostLogoutRedirectUri> ClientPostLogoutRedirectUris { get; }
		DataAccessObjects<DbClientRedirectUri> ClientRedirectUris { get; }
		DataAccessObjects<DbClientScope> ClientScopes { get; }
		DataAccessObjects<DbClientSecret> ClientSecrets { get; }
	}
}