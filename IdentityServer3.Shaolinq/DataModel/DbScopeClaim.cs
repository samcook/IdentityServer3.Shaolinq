using System;
using Platform.Validation;
using Shaolinq;

namespace IdentityServer3.Shaolinq.DataModel
{
	[DataAccessObject(Name = "ScopeClaim")]
	public abstract class DbScopeClaim : DataAccessObject<Guid>
	{
		[PersistedMember]
		[ValueRequired]
		[SizeConstraint(MaximumLength = 200)]
		public abstract string Name { get; set; }

		[PersistedMember]
		[SizeConstraint(MaximumLength = 1000)]
		public abstract string Description { get; set; }

		[PersistedMember]
		public abstract bool AlwaysIncludeInIdToken { get; set; }

		[BackReference]
		[Index]
		public abstract DbScope Scope { get; set; }
	}
}