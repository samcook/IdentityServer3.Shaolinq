using System;
using IdentityServer3.Core.Models;
using Platform.Validation;
using Shaolinq;

namespace IdentityServer3.Shaolinq.DataModel
{
	[DataAccessObject(Name = "Scope")]
	public abstract class DbScope : DataAccessObject<Guid>
	{
		public DbScope SetDefaults()
		{
			this.Type = ScopeType.Resource;
			this.IncludeAllClaimsForUser = false;
			this.Enabled = true;
			this.ShowInDiscoveryDocument = true;

			return this;
		}

		[PersistedMember]
		public abstract bool Enabled { get; set; }

		[PersistedMember]
		[ValueRequired]
		[SizeConstraint(MaximumLength = 200)]
		[Index]
		public abstract string Name { get; set; }

		[PersistedMember]
		[SizeConstraint(MaximumLength = 200)]
		public abstract string DisplayName { get; set; }

		[PersistedMember]
		[SizeConstraint(MaximumLength = 200)]
		public abstract string Description { get; set; }

		[PersistedMember]
		public abstract bool Required { get; set; }
		[PersistedMember]
		public abstract bool Emphasize { get; set; }
		[PersistedMember]
		public abstract ScopeType Type { get; set; }
		[RelatedDataAccessObjects]
		public abstract RelatedDataAccessObjects<DbScopeClaim> ScopeClaims { get; }
		[PersistedMember]
		public abstract bool IncludeAllClaimsForUser { get; set; }

		[PersistedMember]
		[SizeConstraint(MaximumLength = 200)]
		public abstract string ClaimsRule { get; set; }

		[PersistedMember]
		public abstract bool ShowInDiscoveryDocument { get; set; }
	}
}