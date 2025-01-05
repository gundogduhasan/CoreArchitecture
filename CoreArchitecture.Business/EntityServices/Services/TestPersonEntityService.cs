using CoreArchitecture.Abstractions.BusinessEntityServiceInterfaces;
using CoreArchitecture.Abstractions.DataAccessInterfaces;
using CoreArchitecture.Business.EntityServices._Base;
using CoreArchitecture.Common.DTOs;
using CoreArchitecture.Data.Entities;
using AutoMapper;

namespace CoreArchitecture.Business.EntityServices
{
    public class TestPersonEntityService : BaseEntityService<TestPersonDto, TestPerson>, ITestPersonEntityService
    {
        private readonly IRepository<TestPerson, Guid> _testPersonRepository;
        public TestPersonEntityService(IRepository<TestPerson, Guid> testPersonRepository, IUnitOfWork unitOfWork, IMapper mapper) : base(testPersonRepository, unitOfWork, mapper)
        {
            _testPersonRepository = testPersonRepository ?? throw new ArgumentNullException(nameof(testPersonRepository));
        }
    }
}