using System;
using IdentityServer3.Core.Models;
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
		public abstract DbClient Client { get; set; }
	}
}