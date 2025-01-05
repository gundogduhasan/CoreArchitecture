using System.Linq.Expressions;

namespace CoreArchitecture.Abstractions.DataAccessInterfaces
{
    public interface IRepository<TEntity, TKey> 
    {
        TEntity GetById(TKey id);
        TEntity GetByIdReadOnly(TKey id);
        Task<TEntity> GetByIdAsync(TKey id);
        Task<TEntity> GetByIdReadOnlyAsync(TKey id);
        IQueryable<TEntity> GetList();
        Task<IQueryable<TEntity>> GetListAsync();
        IQueryable<TEntity> GetWhere(Expression<Func<TEntity, bool>> predicate);
        Task<IQueryable<TEntity>> GetWhereAsync(Expression<Func<TEntity, bool>> predicate);

        //With SaveChanges
        TKey PersistAdd(TEntity entity);
        Task<TKey> PersistAddAsync(TEntity entity);
        Task<int> PersistAddRangeAsync(IList<TEntity> entites);
        int PersistUpdate(TEntity entity);
        Task<int> PersistUpdateAsync(TEntity entity);
        int PersistUpdateRange(TEntity[] entities);
        Task<int> PersistUpdateRangeAsync(TEntity[] entities);
        int PersistDelete(TEntity entity);
        Task<int> PersistDeleteAsync(TEntity entity);
        int PersistDeletePermanently(TKey id);
        Task<int> PersistDeletePermanentlyAsync(TKey id);

        //Without SaveChanges
        TKey Add(TEntity entity);
        Task<TKey> AddAsync(TEntity entity);
        Task<int> AddRangeAsync(IList<TEntity> entites);
        int Update(TEntity entity);
        int UpdateRange(TEntity[] entities);
        int Delete(TEntity entity);
        int DeletePermanently(TKey id);
        IQueryable<TEntity> IncludeMany(params Expression<Func<TEntity, object>>[] includes);
        int ExecuteSQL(string sql);
    }
}
