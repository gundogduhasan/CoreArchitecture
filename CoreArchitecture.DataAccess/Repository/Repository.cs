using CoreArchitecture.Abstractions.DataAccessInterfaces;
using CoreArchitecture.Data.DBContext;
using CoreArchitecture.Data.Entities.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CoreArchitecture.DataAccess.Repository
{
    public class Repository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        private readonly CoreArchitectureDBContext _context;

        public Repository(CoreArchitectureDBContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public TEntity GetById(TKey id)
        {
            var entity = GetDBSet().Find(id);

            if (entity == null)
                throw new KeyNotFoundException($"The entity with ID '{id}' was not found.");
            
            return entity;
        }
        public TEntity GetByIdReadOnly(TKey id)
        {
            var entity = GetDBSet().AsNoTracking().FirstOrDefault(x=> x.Id != null && x.Id.Equals(id));

            if (entity == null)
                throw new KeyNotFoundException($"The entity with ID '{id}' was not found.");

            return entity;
        }
        public async Task<TEntity> GetByIdAsync(TKey id)
        {
            var entity = await GetDBSet().FindAsync(id);

            if (entity == null)
                throw new KeyNotFoundException($"The entity with ID '{id}' was not found.");
            
            return entity;
        }
        public async Task<TEntity> GetByIdReadOnlyAsync(TKey id)
        {
            var entity = await GetDBSet().AsNoTracking().FirstOrDefaultAsync(x => x.Id != null && x.Id.Equals(id));

            if (entity == null)
                throw new KeyNotFoundException($"The entity with ID '{id}' was not found.");

            return entity;
        }
        public IQueryable<TEntity> GetList()
        {
            return GetListAsQueryable();
        }
        public async Task<IQueryable<TEntity>> GetListAsync()
        {
            return await Task.FromResult(GetListAsQueryable());
        }
        public IQueryable<TEntity> GetWhere(Expression<Func<TEntity, bool>> predicate)
        {
            return GetListAsQueryable().Where(predicate);
        }
        public async Task<IQueryable<TEntity>> GetWhereAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Task.FromResult(GetListAsQueryable().Where(predicate));
        }

        //With SaveChanges
        public TKey PersistAdd(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Added;

            _context.Add(entity);

            _context.SaveChanges();

            return entity.Id;
        }
        public async Task<TKey> PersistAddAsync(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Added;

            await _context.AddAsync(entity);

            await _context.SaveChangesAsync();

            return entity.Id;
        }
        public async Task<int> PersistAddRangeAsync(IList<TEntity> entities)
        {
            foreach (TEntity entity in entities)
                _context.Entry<TEntity>(entity).State = EntityState.Added;


            await _context.AddRangeAsync(entities);

            return await _context.SaveChangesAsync();
        }
        public int PersistUpdate(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            _context.Entry(entity).CurrentValues.SetValues(entity);

            _context.Update(entity);

            return _context.SaveChanges();
        }
        public async Task<int> PersistUpdateAsync(TEntity entity)
        {
            _context.Entry(entity).CurrentValues.SetValues(entity);
            _context.Entry(entity).State = EntityState.Modified;

            _context.Update(entity);

            return await _context.SaveChangesAsync();
        }
        public int PersistUpdateRange(TEntity[] entities)
        {
            foreach (TEntity entity in entities)
            {
                _context.Entry<TEntity>(entity).State = EntityState.Modified;
                _context.Entry(entity).CurrentValues.SetValues(entity);
            }

            _context.UpdateRange(entities);

            return _context.SaveChanges();
        }
        public async Task<int> PersistUpdateRangeAsync(TEntity[] entities)
        {
            foreach (TEntity entity in entities)
            {
                _context.Entry<TEntity>(entity).State = EntityState.Modified;
                _context.Entry(entity).CurrentValues.SetValues(entity);
            }

            _context.UpdateRange(entities);

            return await _context.SaveChangesAsync();
        }
        public int PersistDelete(TEntity entity)
        {
            entity.IsDeleted = true;

            _context.Entry<TEntity>(entity).State = EntityState.Modified;
            _context.Entry(entity).CurrentValues.SetValues(entity);

            _context.Update(entity);

            return _context.SaveChanges();
        }
        public async Task<int> PersistDeleteAsync(TEntity entity)
        {
            entity.IsDeleted = true;

            _context.Entry<TEntity>(entity).State = EntityState.Modified;
            _context.Entry(entity).CurrentValues.SetValues(entity);

            _context.Update(entity);

            return await _context.SaveChangesAsync();
        }
        public int PersistDeletePermanently(TKey id)
        {
            TEntity entity = GetById(id);

            _context.Entry<TEntity>(entity).State = EntityState.Deleted;

            _context.Remove(entity);

            return _context.SaveChanges();
        }
        public async Task<int> PersistDeletePermanentlyAsync(TKey id)
        {
            TEntity entity = GetById(id);

            _context.Entry<TEntity>(entity).State = EntityState.Deleted;

            _context.Remove(entity);

            return await _context.SaveChangesAsync();
        }

        //Without SaveChanges
        public TKey Add(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Added;

            _context.Add(entity);

            return entity.Id;
        }
        public async Task<TKey> AddAsync(TEntity entity)
        {
            //try
            //{
                _context.Entry(entity).State = EntityState.Added;

                await _context.AddAsync(entity);
                return entity.Id;
            //}
            //catch (Exception ex)
            //{

            //    throw ex;
            //}
        }
        public async Task<int> AddRangeAsync(IList<TEntity> entities)
        {
            foreach (TEntity entity in entities)
                _context.Entry<TEntity>(entity).State = EntityState.Added;

            await _context.AddRangeAsync(entities);

            return 0;
        }
        public int Update(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            _context.Entry(entity).CurrentValues.SetValues(entity);

            _context.Update(entity);

            return 0;
        }
        public int UpdateRange(TEntity[] entities)
        {
            foreach (TEntity entity in entities)
            {
                _context.Entry<TEntity>(entity).State = EntityState.Modified;
                _context.Entry(entity).CurrentValues.SetValues(entity);
            }

            _context.UpdateRange(entities);

            return 0;
        }
        public int Delete(TEntity entity)
        {
            entity.IsDeleted = true;

            _context.Entry<TEntity>(entity).State = EntityState.Modified;
            _context.Entry(entity).CurrentValues.SetValues(entity);

            _context.Update(entity);

            return 0;
        }
        public int DeletePermanently(TKey id)
        {
            TEntity entity = GetById(id);

            _context.Entry<TEntity>(entity).State = EntityState.Deleted;

            _context.Remove(entity);

            return 0;
        }     
        public IQueryable<TEntity> IncludeMany(params Expression<Func<TEntity, object>>[] includes)
        {
            //return GetDBSet().IncludeMultiple(includes);
            return GetDBSet();
        }
        public int ExecuteSQL(string sql)
        {
            return _context.Database.ExecuteSqlRaw(sql);
        }
        private IQueryable<TEntity> GetListAsQueryable()
        {
            return GetDBSet().AsQueryable();
        }
        private DbSet<TEntity> GetDBSet()
        {
            return _context.Set<TEntity>();
        }
    }
}
