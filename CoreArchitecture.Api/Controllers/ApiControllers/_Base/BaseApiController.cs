using CoreArchitecture.Abstractions.BusinessEntityServiceInterfaces;
using CoreArchitecture.Common.Constants;
using CoreArchitecture.Common.DTOs._Base;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Claims;

namespace CoreArchitecture.Api.Controllers.ApiControllers
{

    public abstract class BaseApiController<TDto, TService> : BaseApiController<Guid, TDto, TService>
       where TDto : BaseDto<Guid>
       where TService : IEntityServiceManager<TDto, Guid>
    {
        private readonly TService baseService;

        protected BaseApiController(TService baseService) : base(baseService)
        {
            this.baseService = baseService;
        }
    }

    [ApiController]
    [Route(ApplicationConstants.ApiRoutePrefix + "/[controller]")]
    public abstract class BaseApiController<TKey, TDto, TService> : ControllerBase
    where TDto : BaseDto<TKey>
    where TService : IEntityServiceManager<TDto, TKey>
    {
        private readonly TService baseService;
        protected BaseApiController(TService baseService)
        {
            this.baseService = baseService;
        }

        /// <summary>
        /// Gets entity list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
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


        [HttpGet("filter")]
        public IActionResult GetFilteredData([FromQuery] Dictionary<string, string> filters)
        {
            // Dinamik olarak bir predicate oluşturmak için kullanılır.
            var parameter = Expression.Parameter(typeof(TDto), "dto");
            Expression combinedExpression = Expression.Constant(true); // Başlangıç değeri true, tüm predicate'ler ile birleşecek.

            foreach (var filter in filters)
            {
                var property = typeof(TDto).GetProperty(filter.Key);

                if (property == null)
                {
                    return BadRequest($"Property '{filter.Key}' not found in '{typeof(TDto).Name}'.");
                }

                // Property'e erişmek için bir Expression oluşturulur.
                var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                var filterValue = Expression.Constant(filter.Value);

                // String tipi için "Contains" metodunu kullanarak bir ifade oluştur.
                if (property.PropertyType == typeof(string))
                {
                    MethodInfo? containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                    if (containsMethod != null)
                    {
                        var containsExpression = Expression.Call(propertyAccess, containsMethod, filterValue);
                        combinedExpression = Expression.AndAlso(combinedExpression, containsExpression);
                    }
                }
                else
                {
                    // Diğer türler için eşitlik kontrolü.
                    var equalsExpression = Expression.Equal(propertyAccess, filterValue);
                    combinedExpression = Expression.AndAlso(combinedExpression, equalsExpression);
                }
            }

            // Dinamik olarak oluşturulan Expression, bir lambda Expression<Func<T, bool>> haline dönüştürülür.
            var predicate = Expression.Lambda<Func<TDto, bool>>(combinedExpression, parameter);

            // Repository'deki GetWhere metodunu çağırarak veriyi filtreler.
            var filteredData = baseService.GetWhere(predicate).ToList();

            if (filteredData == null || !filteredData.Any())
            {
                return NotFound("No matching data found.");
            }

            return Ok(filteredData);
        }

        /// <summary>
        /// Gets single entity by given primary key (key)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public virtual async Task<IActionResult> Get(TKey id)
        {
            return Ok(await baseService.GetByIdAsync(id));
        }

        [HttpPost]
        public virtual async Task<IActionResult> Post(TDto Dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                SetCreationInfoOfDto(Dto);

                return Ok(await baseService.PersistAddAsync(Dto));
            }
            catch
            {
                //TODO: Log kaydı.
                throw;
            }

        }

        [HttpPatch("{id}")]
        public virtual async Task<IActionResult> Patch(TKey id, [FromBody] JsonPatchDocument<TDto> patchDoc)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //!!!Burası async olmayan GetById kullandığında
            //cannot be tracked because another instance with the same key value for {'Id'} is already being tracked.
            //hatası alınıyor bu hata sebebi ve bu çözümün neden işe yaradığı güzel bir şekilde araştırılıp anlaşılmalı.
            TDto dto = await baseService.GetByIdReadOnlyAsync(id);
            patchDoc.ApplyTo(dto);

            SetModificationInfoOfDto(dto);

            return Ok(await baseService.PersistUpdateAsync(dto));
        }

        [HttpPut("{id}")]
        public virtual async Task<IActionResult> Put(TKey id, TDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id == null)
                return BadRequest("Keys can not be null");

            if (id != null && !id.Equals(dto.Id))
                return BadRequest("Keys are not match");

            SetModificationInfoOfDto(dto);

            return Ok(await baseService.PersistUpdateAsync(dto));
        }

        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> Delete(TKey id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(await baseService.PersistDeletePermanentlyAsync(id));
        }

        protected Guid GetLoggedInUserId()
        {
            Claim? user = User?.FindFirst(ClaimTypes.NameIdentifier);

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
