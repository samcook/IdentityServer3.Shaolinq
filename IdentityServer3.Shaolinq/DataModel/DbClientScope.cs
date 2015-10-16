using System;
using IdentityServer3.Core.Models;
using Platform.Validation;
using Shaolinq;

namespace IdentityServer3.Shaolinq.DataModel
{
	[DataAccessObject(Name = "ClientScope")]
	public abstract class DbClientScope : DataAccessObject<Guid>
	{
		[PersistedMember]
		[ValueRequired]
		[SizeConstraint(MaximumLength = 200)]
		public abstract string Scope { get; set; }

		[BackReference]
		[Index]
		public abstract DbClient Client { get; set; }
	}
}