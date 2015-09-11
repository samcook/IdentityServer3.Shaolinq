using System;
using Platform.Validation;
using Shaolinq;

namespace IdentityServer3.Shaolinq.DataModel
{
	[DataAccessObject(Name = "Token")]
	public abstract class DbToken : DataAccessObject<Guid>
	{
		[PersistedMember]
		[Index(IndexName = "Key_TokenType_idx", Unique = true, CompositeOrder = 0)]
		public abstract string Key { get; set; }

		[PersistedMember]
		[Index(IndexName = "Key_TokenType_idx", Unique = true, CompositeOrder = 1)]
		public abstract DbTokenType TokenType { get; set; }

		[PersistedMember]
		[SizeConstraint(MaximumLength = 200)]
		public abstract string SubjectId { get; set; }

		[PersistedMember]
		[ValueRequired]
		[SizeConstraint(MaximumLength = 200)]
		public abstract string ClientId { get; set; }

		[PersistedMember]
		[ValueRequired]
		[SizeConstraint(SizeFlexibility = SizeFlexibility.LargeVariable)]
		public abstract string JsonCode { get; set; }

		public DateTimeOffset Expiry
		{
			get
			{
				return new DateTimeOffset(ExpiryDateTime, ExpiryOffset);
			}
			set
			{
				ExpiryDateTime = value.DateTime;
				ExpiryOffset = value.Offset;
			}
		}

		[PersistedMember]
		[ValueRequired]
		public abstract DateTime ExpiryDateTime { get; set; }

		[PersistedMember]
		[ValueRequired]
		public abstract TimeSpan ExpiryOffset { get; set; }
	}
}