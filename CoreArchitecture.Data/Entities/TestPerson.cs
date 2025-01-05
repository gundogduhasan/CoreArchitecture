using CoreArchitecture.Data.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreArchitecture.Data.Entities
{
    public class TestPerson : AuditableEntity
    {
        public TestDepartment? Department { get; set; }
        public Guid DepartmentId { get; set; }
        public required string Name { get; set; }
        public required string Lastname { get; set; }
        public int Salary { get; set; }
    }
}
