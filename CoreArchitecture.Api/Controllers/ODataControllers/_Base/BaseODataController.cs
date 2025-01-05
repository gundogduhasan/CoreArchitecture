global using Microsoft.AspNetCore.Mvc;
using CoreArchitecture.Abstractions.BusinessEntityServiceInterfaces;
using CoreArchitecture.Common.DTOs._Base;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using System.Security.Claims;

namespace CoreArchitecture.Api.Controllers.ODataControllers
{
    public abstract class BaseODataController<TDto, TService> : BaseODataController<Guid, TDto, TService>
       where TDto : BaseDto<Guid>
       where TService : IEntityServiceManager<TDto, Guid>
    {
        private readonly TService baseService;

        protected BaseODataController(TService baseService) : base(baseService)
        {
            this.baseService = baseService;
        }
    }

    //[ApiController]
    //[Route(ApplicationConstants.ODataApiRoutePrefix + "[controller]")]"
    public abstract class BaseODataController<TKey, TDto, TService> : ODataController
    where TDto : BaseDto<TKey>
    where TService : IEntityServiceManager<TDto, TKey>
    {
        private readonly TService baseService;
        protected BaseODataController(TService baseService)
        {
            this.baseService = baseService;
        }

        /// <summary>
        /// Gets entity list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [EnableQuery]
        public virtual async Task<IActionResult> Get()
        {
            try
            {
                var result = await baseService.GetListAsync();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Gets single entity by given primary key (key)
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpGet]
        [EnableQuery]
        public virtual async Task<IActionResult> Get([FromODataUri] TKey key)
        {
            //OData fonksiyonlarını kullanabilmek için bu şekilde getiriyoruz single kayıtı.
            var result = await baseService.GetWhereAsync(x => (x.Id != null) && x.Id.Equals(key));

            if (result == null)
                return NotFound("Not Found");

            return Ok(SingleResult.Create(result));
        }

        [HttpPost]
        public virtual async Task<IActionResult> Post([FromBody] TDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                SetCreationInfoOfDto(dto);

                return Ok(await baseService.PersistAddAsync(dto));
            }
            catch
            {
                //TODO: Log kaydı.
                throw;
            }

        }

        [HttpPatch]
        [EnableQuery]
        public virtual async Task<IActionResult> Patch([FromODataUri] TKey key, Delta<TDto> dtoDelta)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            TDto dto = await baseService.GetByIdReadOnlyAsync(key);
            dtoDelta.Patch(dto);

            SetModificationInfoOfDto(dto);

            return Ok(await baseService.PersistUpdateAsync(dto));
        }

        [HttpPut]
        [EnableQuery]
        public virtual async Task<IActionResult> Put([FromODataUri] TKey key, [FromBody] TDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (key == null)
                return BadRequest("Keys can not be null");

            if (key != null && !key.Equals(dto.Id))
                return BadRequest("Keys are not match");

            SetModificationInfoOfDto(dto);

            return Ok(await baseService.PersistUpdateAsync(dto));
        }

        [HttpDelete]
        [EnableQuery]
        public virtual async Task<IActionResult> Delete([FromODataUri] TKey key)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(await baseService.PersistDeletePermanentlyAsync(key));
        }

        protected Guid GetLoggedInUserId()
        {
            Claim? user = User.FindFirst(ClaimTypes.NameIdentifier);

            if (user != null)
                return new Guid(user.Value);
            else
                return new Guid();
        }

        protected void SetModificationInfoOfDto(TDto dto)
        {
            if (dto is AuditableDto auditableDto)
            {
                auditableDto.ModificationDate = DateTime.Now;
                auditableDto.ModifierUserId = GetLoggedInUserId();
            }
        }

        protected void SetCreationInfoOfDto(TDto dto)
        {
            if (dto is AuditableDto auditableDto)
            {
                auditableDto.CreatedDate = DateTime.Now;
                auditableDto.CreatorUserId = GetLoggedInUserId();
            }
        }

        //protected CommonResponseModel<T> GenerateSuccessResponseModel<T>(T data, string message, string errorDetails = "")
        //{
        //    return new CommonResponseModel<T>(data, true, message, errorDetails);
        //}

        //protected CommonResponseModel<T> GenerateErrorResponseModel<T>(T data, string message, string errorDetails = "")
        //{
        //    return new CommonResponseModel<T>(data, false, message, errorDetails);
        //}

    }
}
