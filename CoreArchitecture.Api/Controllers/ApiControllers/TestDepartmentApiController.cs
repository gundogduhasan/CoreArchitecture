using CoreArchitecture.Abstractions.BusinessEntityServiceInterfaces;
using CoreArchitecture.Common.DTOs;

namespace CoreArchitecture.Api.Controllers.ApiControllers
{
    public class TestDepartmentApiController : BaseApiController<TestDepartmentDto, ITestDepartmentEntityService>
    {
        private readonly ITestDepartmentEntityService _testDepartmentEntityService;

        public TestDepartmentApiController(ITestDepartmentEntityService testDepartmentEntityService) : base(testDepartmentEntityService)
        {
            _testDepartmentEntityService = testDepartmentEntityService;
        }

        [HttpGet]
        public async override Task<IActionResult> Get()
        {
            var res = await _testDepartmentEntityService.GetWhereAsync(x => x.Name.Contains("test"));
            return Ok(res);
        }
    }
}