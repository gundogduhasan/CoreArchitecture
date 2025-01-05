using CoreArchitecture.Abstractions.BusinessEntityServiceInterfaces;
using CoreArchitecture.Common.DTOs;

namespace CoreArchitecture.Api.Controllers.ApiControllers
{
    public class TestPersonApiController : BaseApiController<TestPersonDto, ITestPersonEntityService>
    {
        private readonly ITestPersonEntityService _testPersonEntityService;

        public TestPersonApiController(ITestPersonEntityService testPersonEntityService) : base(testPersonEntityService)
        {
            _testPersonEntityService = testPersonEntityService;
        }
    }
}