using System;
using Platform.Validation;
using Shaolinq;

namespace IdentityServer3.Shaolinq.DataModel
{
	[DataAccessObject(Name = "Consent")]
	public abstract class DbConsent : DataAccessObject<Guid>
	{
		[PersistedMember]
		[Index(IndexName = "Subject_ClientId_idx", Unique = true, CompositeOrder = 0)]
		[SizeConstraint(MaximumLength = 200)]
		public abstract string Subject { get; set; }

		[PersistedMember]
		[Index(IndexName = "Subject_ClientId_idx", Unique = true, CompositeOrder = 1)]
		[SizeConstraint(MaximumLength = 200)]
		public abstract Guid ClientId { get; set; }

		[PersistedMember]
		[ValueRequired]
		[SizeConstraint(MaximumLength = 200)]
		public abstract string Scopes { get; set; }
	}
}