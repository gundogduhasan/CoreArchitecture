using CoreArchitecture.Common.DTOs._Base;

namespace CoreArchitecture.Common.DTOs
{
    public class TestPersonDto : AuditableDto
    {
        public string? Name { get; set; }
        public string? Lastname { get; set; }
        public int Salary { get; set; }
        public Guid DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        public TestDepartmentDto? PersonDepartment { get; set; }
    }
}
