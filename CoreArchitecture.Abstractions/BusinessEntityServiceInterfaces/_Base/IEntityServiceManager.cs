using CoreArchitecture.Common.DTOs._Base;
using System.Linq.Expressions;


namespace CoreArchitecture.Abstractions.BusinessEntityServiceInterfaces
{
    public interface IEntityServiceManager<TDto> : IEntityServiceManager<TDto, Guid> where TDto : BaseDto<Guid>
    { }

    public interface IEntityServiceManager<TDto, TKey>  where TDto : BaseDto<TKey>
    {
        #region Repository
        TDto GetById(TKey id);
        TDto GetByIdReadOnly(TKey id);
        Task<TDto> GetByIdAsync(TKey id);
        Task<TDto> GetByIdReadOnlyAsync(TKey id);
        IQueryable<TDto> GetList();
        Task<IQueryable<TDto>> GetListAsync();
        IQueryable<TDto> GetWhere(Expression<Func<TDto, bool>> predicate);
        Task<IQueryable<TDto>> GetWhereAsync(Expression<Func<TDto, bool>> predicate);

        //With SaveChanges
        TKey PersistAdd(TDto entity);
        Task<TKey> PersistAddAsync(TDto entity);
        Task<int> PersistAddRangeAsync(IList<TDto> entites);
        int PersistUpdate(TDto entity);
        Task<int> PersistUpdateAsync(TDto entity);
        int PersistUpdateRange(TDto[] entities);
        Task<int> PersistUpdateRangeAsync(TDto[] entities);
        int PersistDelete(TDto entity);
        Task<int> PersistDeleteAsync(TDto entity);
        int PersistDeletePermanently(TKey id);
        Task<int> PersistDeletePermanentlyAsync(TKey id);

        //Without SaveChanges
        TKey Add(TDto entity);
        Task<TKey> AddAsync(TDto entity);
        Task<int> AddRangeAsync(IList<TDto> entites);
        int Update(TDto entity);
        int UpdateRange(TDto[] entities);
        int Delete(TDto entity);
        int DeletePermanently(TKey id);
        IQueryable<TDto> IncludeMany(params Expression<Func<TDto, object>>[] includes);
        int ExecuteSQL(string sql);
        #endregion Repository
    }
}
