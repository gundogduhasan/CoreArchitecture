namespace CoreArchitecture.Common.DTOs._Base
{
    public class AuditableDto : AuditableDto<Guid>
    { }

    public class AuditableDto<TKey> : BaseDto<TKey>
    {
        public Guid CreatorUserId { get; set; }
        public Guid? ModifierUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModificationDate { get; set; }
    }
}
