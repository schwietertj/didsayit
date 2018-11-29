using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DidSayItModels;
using Microsoft.EntityFrameworkCore;

namespace DidSayIt.Repository
{
    public abstract class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        internal readonly ApplicationDbContext _dbContext;
        
        protected GenericRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual IQueryable<TEntity> GetAll(bool includeInactive = false)
        {
            //return includeInactive ? _dbContext.Set<TEntity>().AsNoTracking() : _dbContext.Set<TEntity>().AsNoTracking().Where(x => x.Active);
            return includeInactive ? _dbContext.Set<TEntity>() : _dbContext.Set<TEntity>().Where(x => x.Active);
        }

        public async Task<TEntity> GetById(long id)
        {
            return await _dbContext.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<TEntity> Save(TEntity entity)
        {
            entity = SetCreatedModified(entity);
            _dbContext.Entry(entity).State = entity.Id > 0 ? EntityState.Modified : EntityState.Added;
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task Delete(long id)
        {
            var entity = await _dbContext.Set<TEntity>().FindAsync(id);
            entity.Active = false;
            await Save(entity);
        }

        private TEntity SetCreatedModified(TEntity entity)
        {
            if (entity.Id < 1)
            {
                entity.Created = DateTime.UtcNow;
            }

            entity.Modified = DateTime.UtcNow;

            return entity;
        }
    }
}
