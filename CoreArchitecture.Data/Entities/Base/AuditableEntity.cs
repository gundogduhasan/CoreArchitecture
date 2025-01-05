namespace CoreArchitecture.Data.Entities.Base
{
    public class AuditableEntity : AuditableEntity<Guid>
    { }

    public class AuditableEntity<TKey> : BaseEntity<TKey>
    {
        public Guid CreatorUserId { get; set; }
        public Guid? ModifierUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModificationDate { get; set; }
    }
}
