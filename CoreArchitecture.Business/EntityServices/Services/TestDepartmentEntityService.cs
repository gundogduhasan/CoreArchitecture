using CoreArchitecture.Abstractions.BusinessEntityServiceInterfaces;
using CoreArchitecture.Abstractions.DataAccessInterfaces;
using CoreArchitecture.Business.EntityServices._Base;
using CoreArchitecture.Common.DTOs;
using CoreArchitecture.Data.Entities;
using AutoMapper;

namespace CoreArchitecture.Business.EntityServices
{
    public class TestDepartmentEntityService : BaseEntityService<TestDepartmentDto,TestDepartment>, ITestDepartmentEntityService
    {
        private readonly IRepository<TestDepartment, Guid> _testDepartmentRepository;
        public TestDepartmentEntityService(IRepository<TestDepartment, Guid> testDepartmentRepository, IUnitOfWork unitOfWork, IMapper mapper) 
            : base(testDepartmentRepository, unitOfWork, mapper)
        {
            _testDepartmentRepository = testDepartmentRepository ?? throw new ArgumentNullException(nameof(testDepartmentRepository));
        }
    }
}