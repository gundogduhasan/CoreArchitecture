using CoreArchitecture.Abstractions.BusinessEntityServiceInterfaces;
using CoreArchitecture.Common.DTOs;

namespace CoreArchitecture.Api.Controllers.ODataControllers
{
    public class TestDepartmentController : BaseODataController<TestDepartmentDto, ITestDepartmentEntityService>
    {
        private readonly ITestDepartmentEntityService _testDepartmentEntityService;

        public TestDepartmentController(ITestDepartmentEntityService testDepartmentEntityService) : base(testDepartmentEntityService)
        {
            _testDepartmentEntityService = testDepartmentEntityService;
        }
    }
}