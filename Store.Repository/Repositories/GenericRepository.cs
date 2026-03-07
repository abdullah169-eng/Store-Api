using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Store.Core.Entities;
using Store.Core.Repositories.Contract;
using Store.Core.Specifications;
using Store.Repository.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Repository.Repositories
{
    public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        private readonly StoreDbContext _storeDbContext;
        public GenericRepository(StoreDbContext storeDbContext)
        {
            _storeDbContext = storeDbContext;
        }
        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _storeDbContext.Set<TEntity>().ToListAsync();
        }
        public async Task<TEntity> GetAsync(TKey Id)
        {
            return await _storeDbContext.Set<TEntity>().FindAsync(Id);
        }
        public async Task AddAsync(TEntity entity)
        {
             await _storeDbContext.AddAsync(entity);
        }
        public void Update(TEntity entity)
        {
            _storeDbContext.Update(entity);
        }
        public void Delete(TEntity entity)
        {
            _storeDbContext.Remove(entity);
        }
        public async Task<IEnumerable<TEntity>> GetAllWithSpecAsync(ISpecifications<TEntity, TKey> spec)
        {
            return await ApplySpecifications(spec).ToListAsync();
        }
        public async Task<TEntity> GetWithSpecAsync(ISpecifications<TEntity, TKey> spec)
        {
            return await ApplySpecifications(spec).FirstOrDefaultAsync();
        }
        private IQueryable<TEntity> ApplySpecifications(ISpecifications<TEntity, TKey> spec)
        {
            return SpecificationsEvaluator<TEntity, TKey>.GetQuery(_storeDbContext.Set<TEntity>().AsQueryable(), spec);
        }
        public Task<int> GetCountAsync(ISpecifications<TEntity, TKey> spec)
        {
            return SpecificationsEvaluator<TEntity, TKey>
                .GetQuery(_storeDbContext.Set<TEntity>(), spec, true)
                .CountAsync();
        }
    }
}
