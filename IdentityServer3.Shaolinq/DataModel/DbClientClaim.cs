using System;
using IdentityServer3.Core.Models;
using Platform.Validation;
using Shaolinq;

namespace IdentityServer3.Shaolinq.DataModel
{
	[DataAccessObject(Name = "ClientClaim")]
	public abstract class DbClientClaim : DataAccessObject<Guid>
	{
		[PersistedMember]
		[ValueRequired]
		[SizeConstraint(MaximumLength = 250)]
		public abstract string Type { get; set; }

		[PersistedMember]
		[ValueRequired]
		[SizeConstraint(MaximumLength = 250)]
		public abstract string Value { get; set; }

		[BackReference]
		public abstract DbClient Client { get; set; }
	}
}