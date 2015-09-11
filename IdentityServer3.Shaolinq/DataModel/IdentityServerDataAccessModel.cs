using IdentityServer3.Shaolinq.DataModel.Interfaces;
using Shaolinq;

namespace IdentityServer3.Shaolinq.DataModel
{
	[DataAccessModel]
	public abstract class IdentityServerDataAccessModel :
		DataAccessModel,
		IIdentityServerClientDataAccessModel,
		IIdentityServerScopeDataAccessModel,
		IIdentityServerOperationalDataAccessModel
	{
		[DataAccessObjects]
		public abstract DataAccessObjects<DbClient> Clients { get; }

		[DataAccessObjects]
		public abstract DataAccessObjects<DbClientClaim> ClientsClaims { get; }

		[DataAccessObjects]
		public abstract DataAccessObjects<DbClientCorsOrigin> ClientCorsOrigins { get; }

		[DataAccessObjects]
		public abstract DataAccessObjects<DbClientCustomGrantType> ClientCustomGrantTypes { get; }

		[DataAccessObjects]
		public abstract DataAccessObjects<DbClientIdpRestriction> ClientIdpRestrictions { get; }

		[DataAccessObjects]
		public abstract DataAccessObjects<DbClientPostLogoutRedirectUri> ClientPostLogoutRedirectUris { get; }

		[DataAccessObjects]
		public abstract DataAccessObjects<DbClientRedirectUri> ClientRedirectUris { get; }

		[DataAccessObjects]
		public abstract DataAccessObjects<DbClientScope> ClientScopes { get; }

		[DataAccessObjects]
		public abstract DataAccessObjects<DbClientSecret> ClientSecrets { get; }

		[DataAccessObjects]
		public abstract DataAccessObjects<DbScope> Scopes { get; }
 
		[DataAccessObjects]
		public abstract DataAccessObjects<DbScopeClaim> ScopeClaims { get; }

		[DataAccessObjects]
		public abstract DataAccessObjects<DbConsent> Consents { get; }

		[DataAccessObjects]
		public abstract DataAccessObjects<DbToken> Tokens { get; }
	}
}