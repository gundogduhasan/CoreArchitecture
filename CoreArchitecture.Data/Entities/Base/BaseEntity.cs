namespace CoreArchitecture.Data.Entities.Base
{
    public class BaseEntity : BaseEntity<Guid> { }

    public class BaseEntity<TKey>
    {
        public required TKey Id { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
    }
}
