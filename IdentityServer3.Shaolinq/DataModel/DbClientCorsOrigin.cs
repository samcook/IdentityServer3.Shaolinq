using System;
using Platform.Validation;
using Shaolinq;

namespace IdentityServer3.Shaolinq.DataModel
{
	[DataAccessObject(Name = "ClientCorsOrigin")]
	public abstract class DbClientCorsOrigin : DataAccessObject<Guid>
	{
		[PersistedMember]
		[ValueRequired]
		[SizeConstraint(MaximumLength = 150)]
		public abstract string Origin { get; set; }

		[BackReference]
		[Index]
		public abstract DbClient Client { get; set; }
	}
}