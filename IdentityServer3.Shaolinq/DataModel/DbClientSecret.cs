using System;
using Platform.Validation;
using Shaolinq;

namespace IdentityServer3.Shaolinq.DataModel
{
	[DataAccessObject(Name = "ClientSecret")]
	public abstract class DbClientSecret : DataAccessObject<Guid>
	{
		[PersistedMember]
		[ValueRequired]
		[SizeConstraint(MaximumLength = 250)]
		public abstract string Value { get; set; }

		[PersistedMember]
		[SizeConstraint(MaximumLength = 250)]
		public abstract string Type { get; set; }

		[PersistedMember]
		[SizeConstraint(MaximumLength = 2000)]
		public abstract string Description { get; set; }

		[PersistedMember]
		public abstract DateTime? Expiration { get; set; }

		[BackReference]
		public abstract DbClient Client { get; set; }
	}
}