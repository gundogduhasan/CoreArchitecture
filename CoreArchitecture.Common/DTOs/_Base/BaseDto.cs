namespace CoreArchitecture.Common.DTOs._Base
{
    public class BaseDto : BaseDto<Guid> { }
    public class BaseDto<TKey>
    {
        public TKey? Id { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
    }
}
