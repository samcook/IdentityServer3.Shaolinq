using System;
using IdentityServer3.Core.Models;
using Platform.Validation;
using Shaolinq;

namespace IdentityServer3.Shaolinq.DataModel
{
	[DataAccessObject(Name = "Client")]
	public abstract class DbClient : DataAccessObject<Guid>
	{
		[PersistedMember]
		public abstract bool Enabled { get; set; }

		//[PersistedMember]
		//[ValueRequired]
		//[SizeConstraint(MaximumLength = 200)]
		//[Index(Unique = true)]
		//public abstract string ClientId { get; set; }

		[RelatedDataAccessObjects]
		public abstract RelatedDataAccessObjects<DbClientSecret> ClientSecrets { get; }

		[PersistedMember]
		[ValueRequired]
		[SizeConstraint(MaximumLength = 200)]
		public abstract string ClientName { get; set; }
		[PersistedMember]
		[SizeConstraint(MaximumLength = 200)]
		public abstract string ClientUri { get; set; }
		[PersistedMember]
		public abstract string LogoUri { get; set; }

		[PersistedMember]
		public abstract bool RequireConsent { get; set; }
		[PersistedMember]
		public abstract bool AllowRememberConsent { get; set; }

		[PersistedMember]
		public abstract Flows Flow { get; set; }
		[PersistedMember]
		public abstract bool AllowClientCredentialsOnly { get; set; }

		[RelatedDataAccessObjects]
		public abstract RelatedDataAccessObjects<DbClientRedirectUri> RedirectUris { get; }
		[RelatedDataAccessObjects]
		public abstract RelatedDataAccessObjects<DbClientPostLogoutRedirectUri> PostLogoutRedirectUris { get; }

		[PersistedMember]
		public abstract bool AllowAccessToAllScopes { get; set; }
		[RelatedDataAccessObjects]
		public abstract RelatedDataAccessObjects<DbClientScope> AllowedScopes { get; }

		// in seconds
		[PersistedMember]
		[ValueRangeConstraint(MinimumValue = 0)]
		public abstract int IdentityTokenLifetime { get; set; }
		[PersistedMember]
		[ValueRangeConstraint(MinimumValue = 0)]
		public abstract int AccessTokenLifetime { get; set; }
		[PersistedMember]
		[ValueRangeConstraint(MinimumValue = 0)]
		public abstract int AuthorizationCodeLifetime { get; set; }

		[PersistedMember]
		[ValueRangeConstraint(MinimumValue = 0)]
		public abstract int AbsoluteRefreshTokenLifetime { get; set; }
		[PersistedMember]
		[ValueRangeConstraint(MinimumValue = 0)]
		public abstract int SlidingRefreshTokenLifetime { get; set; }

		[PersistedMember]
		public abstract TokenUsage RefreshTokenUsage { get; set; }
		[PersistedMember]
		public abstract bool UpdateAccessTokenOnRefresh { get; set; }

		[PersistedMember]
		public abstract TokenExpiration RefreshTokenExpiration { get; set; }

		[PersistedMember]
		public abstract AccessTokenType AccessTokenType { get; set; }

		[PersistedMember]
		public abstract bool EnableLocalLogin { get; set; }
		[RelatedDataAccessObjects]
		public abstract RelatedDataAccessObjects<DbClientIdpRestriction> IdentityProviderRestrictions { get; }

		[PersistedMember]
		public abstract bool IncludeJwtId { get; set; }

		[RelatedDataAccessObjects]
		public abstract RelatedDataAccessObjects<DbClientClaim> Claims { get; }
		[PersistedMember]
		public abstract bool AlwaysSendClientClaims { get; set; }
		[PersistedMember]
		public abstract bool PrefixClientClaims { get; set; }

		[PersistedMember]
		public abstract bool AllowAccessToAllGrantTypes { get; set; }

		[RelatedDataAccessObjects]
		public abstract RelatedDataAccessObjects<DbClientCustomGrantType> AllowedCustomGrantTypes { get; }
		[RelatedDataAccessObjects]
		public abstract RelatedDataAccessObjects<DbClientCorsOrigin> AllowedCorsOrigins { get; }
	}
}