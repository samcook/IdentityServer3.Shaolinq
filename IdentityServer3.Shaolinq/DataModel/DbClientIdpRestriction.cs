using System;
using Platform.Validation;
using Shaolinq;

namespace IdentityServer3.Shaolinq.DataModel
{
	[DataAccessObject(Name = "ClientIdpRestriction")]
	public abstract class DbClientIdpRestriction : DataAccessObject<Guid>
	{
		[PersistedMember]
		[ValueRequired]
		[SizeConstraint(MaximumLength = 200)]
		public abstract string Provider { get; set; }

		[BackReference]
		[Index]
		public abstract DbClient Client { get; set; }
	}
}