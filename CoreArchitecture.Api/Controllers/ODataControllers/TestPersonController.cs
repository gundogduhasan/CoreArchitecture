using CoreArchitecture.Abstractions.BusinessEntityServiceInterfaces;
using CoreArchitecture.Common.DTOs;

namespace CoreArchitecture.Api.Controllers.ODataControllers
{
    public class TestPersonController : BaseODataController<TestPersonDto, ITestPersonEntityService>
    {
        private readonly ITestPersonEntityService _testPersonEntityService;

        public TestPersonController(ITestPersonEntityService testPersonEntityService) : base(testPersonEntityService)
        {
            _testPersonEntityService = testPersonEntityService;
        }
    }
}