using System;
using Platform.Validation;
using Shaolinq;

namespace IdentityServer3.Shaolinq.DataModel
{
	[DataAccessObject(Name = "ClientCustomGrantType")]
	public abstract class DbClientCustomGrantType : DataAccessObject<Guid>
	{
		[PersistedMember]
		[ValueRequired]
		[SizeConstraint(MaximumLength = 250)]
		public abstract string GrantType { get; set; }

		[BackReference]
		[Index]
		public abstract DbClient Client { get; set; }
	}
}