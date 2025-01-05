using CoreArchitecture.Data.Entities.Base;

namespace CoreArchitecture.Data.Entities
{
    public class TestDepartment : AuditableEntity
    {
        public required string Name { get; set; }
        public int Order { get; set; }

    }
}
