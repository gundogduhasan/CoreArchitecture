using CoreArchitecture.Abstractions.BusinessEntityServiceInterfaces;
using CoreArchitecture.Abstractions.DataAccessInterfaces;
using CoreArchitecture.Common.DTOs._Base;
using AutoMapper;
using System.Linq.Expressions;


namespace CoreArchitecture.Business.EntityServices._Base
{
    public class BaseEntityService<TDto, TEntity> : BaseEntityService<TDto, TEntity, Guid> where TDto : BaseDto<Guid>
    {
        protected BaseEntityService(IRepository<TEntity, Guid> _repository, IUnitOfWork unitOfWork, IMapper mapper)
            : base(_repository, unitOfWork, mapper)
        {
        }
    }

    public class BaseEntityService<TDto, TEntity, TKey> : IEntityServiceManager<TDto, TKey> where TDto : BaseDto<TKey>
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IRepository<TEntity, TKey> _repository;
        protected readonly IMapper _mapper;
        protected BaseEntityService(IRepository<TEntity, TKey> repository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(_mapper));
        }

        // GetById
        public TDto GetById(TKey id)
        {
            var entity = _repository.GetById(id);
            return _mapper.Map<TDto>(entity); // Entity -> DTO dönüşümü
        }

        public TDto GetByIdReadOnly(TKey id)
        {
            var entity = _repository.GetByIdReadOnly(id);
            return _mapper.Map<TDto>(entity); // Entity -> DTO dönüşümü
        }
        public async Task<TDto> GetByIdAsync(TKey id)
        {
            try
            {
                var entity = await _repository.GetByIdAsync(id);
                return _mapper.Map<TDto>(entity); // Entity -> DTO dönüşümü
            }
            catch
            {
                //TODO: Log kaydı
                throw;
            }

        }
        public async Task<TDto> GetByIdReadOnlyAsync(TKey id)
        {
            try
            {
                var entity = await _repository.GetByIdReadOnlyAsync(id);
                return _mapper.Map<TDto>(entity); // Entity -> DTO dönüşümü
            }
            catch
            {
                //TODO: Log kaydı
                throw;
            }
        }
        // GetList
        public IQueryable<TDto> GetList()
        {
            var entities = _repository.GetList();
            return _mapper.ProjectTo<TDto>(entities); // IQueryable dönüşümü
        }

        public async Task<IQueryable<TDto>> GetListAsync()
        {
            var entities = await _repository.GetListAsync();
            return _mapper.ProjectTo<TDto>(entities); // IQueryable dönüşümü
        }

        // GetWhere
        public IQueryable<TDto> GetWhere(Expression<Func<TDto, bool>> predicate)
        {
            // Predicate dönüşümü gerekli
            var entityPredicate = _mapper.Map<Expression<Func<TEntity, bool>>>(predicate);
            var entities = _repository.GetWhere(entityPredicate);
            return _mapper.ProjectTo<TDto>(entities); // IQueryable dönüşümü
        }

        public async Task<IQueryable<TDto>> GetWhereAsync(Expression<Func<TDto, bool>> predicate)
        {
            var entityPredicate = _mapper.Map<Expression<Func<TEntity, bool>>>(predicate);
            var entities = await _repository.GetWhereAsync(entityPredicate);
            return _mapper.ProjectTo<TDto>(entities); // IQueryable dönüşümü
        }

        // PersistAdd
        public TKey PersistAdd(TDto dto)
        {
            var entity = _mapper.Map<TEntity>(dto); // DTO -> Entity dönüşümü
            return _repository.PersistAdd(entity);
        }

        public async Task<TKey> PersistAddAsync(TDto dto)
        {
            var entity = _mapper.Map<TEntity>(dto); // DTO -> Entity dönüşümü
            return await _repository.PersistAddAsync(entity);
        }

        public async Task<int> PersistAddRangeAsync(IList<TDto> dtos)
        {
            var entities = _mapper.Map<List<TEntity>>(dtos); // DTO -> Entity dönüşümü
            return await _repository.PersistAddRangeAsync(entities);
        }

        // PersistUpdate
        public int PersistUpdate(TDto dto)
        {
            var entity = _mapper.Map<TEntity>(dto); // DTO -> Entity dönüşümü
            return _repository.PersistUpdate(entity);
        }

        public async Task<int> PersistUpdateAsync(TDto dto)
        {
            var entity = _mapper.Map<TEntity>(dto); // DTO -> Entity dönüşümü
            return await _repository.PersistUpdateAsync(entity);
        }

        public int PersistUpdateRange(TDto[] dtos)
        {
            var entities = _mapper.Map<TEntity[]>(dtos); // DTO -> Entity dönüşümü
            return _repository.PersistUpdateRange(entities);
        }

        public async Task<int> PersistUpdateRangeAsync(TDto[] dtos)
        {
            var entities = _mapper.Map<TEntity[]>(dtos); // DTO -> Entity dönüşümü
            return await _repository.PersistUpdateRangeAsync(entities);
        }

        // PersistDelete
        public int PersistDelete(TDto dto)
        {
            var entity = _mapper.Map<TEntity>(dto); // DTO -> Entity dönüşümü
            return _repository.PersistDelete(entity);
        }

        public async Task<int> PersistDeleteAsync(TDto dto)
        {
            var entity = _mapper.Map<TEntity>(dto); // DTO -> Entity dönüşümü
            return await _repository.PersistDeleteAsync(entity);
        }

        public int PersistDeletePermanently(TKey id)
        {
            return _repository.PersistDeletePermanently(id);
        }

        public async Task<int> PersistDeletePermanentlyAsync(TKey id)
        {
            return await _repository.PersistDeletePermanentlyAsync(id);
        }

        // Add
        public TKey Add(TDto dto)
        {
            var entity = _mapper.Map<TEntity>(dto); // DTO -> Entity dönüşümü
            return _repository.Add(entity);
        }

        public async Task<TKey> AddAsync(TDto dto)
        {
            var entity = _mapper.Map<TEntity>(dto); // DTO -> Entity dönüşümü
            return await _repository.AddAsync(entity);
        }

        public async Task<int> AddRangeAsync(IList<TDto> dtos)
        {
            var entities = _mapper.Map<List<TEntity>>(dtos); // DTO -> Entity dönüşümü
            return await _repository.AddRangeAsync(entities);
        }

        // Update
        public int Update(TDto dto)
        {
            var entity = _mapper.Map<TEntity>(dto); // DTO -> Entity dönüşümü
            return _repository.Update(entity);
        }

        public int UpdateRange(TDto[] dtos)
        {
            var entities = _mapper.Map<TEntity[]>(dtos); // DTO -> Entity dönüşümü
            return _repository.UpdateRange(entities);
        }

        // Delete
        public int Delete(TDto dto)
        {
            var entity = _mapper.Map<TEntity>(dto); // DTO -> Entity dönüşümü
            return _repository.Delete(entity);
        }

        public int DeletePermanently(TKey id)
        {
            return _repository.DeletePermanently(id);
        }

        // IncludeMany (Opsiyonel Dönüşüm)
        public IQueryable<TDto> IncludeMany(params Expression<Func<TDto, object>>[] includes)
        {
            return _repository.IncludeMany().Select(entity => _mapper.Map<TDto>(entity));
        }

        // ExecuteSQL
        public int ExecuteSQL(string sql)
        {
            return _repository.ExecuteSQL(sql);
        }
    }
}
