using CoreArchitecture.Common.DTOs._Base;

namespace CoreArchitecture.Common.DTOs
{
    public class TestDepartmentDto : AuditableDto
    {
        public required string Name { get; set; }
        public int Order { get; set; }
    }
}
