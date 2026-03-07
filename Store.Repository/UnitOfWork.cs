using Store.Core;
using Store.Core.Entities;
using Store.Core.Repositories.Contract;
using Store.Repository.Data.Contexts;
using Store.Repository.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Store.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreDbContext _storeDbContext;
        private Hashtable _repositories;
        public UnitOfWork(StoreDbContext storeDbContext)
        {
            _storeDbContext = storeDbContext;
            _repositories = new Hashtable();
        }
        public IGenericRepository<TEntity, TKey> Repository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
        {
            if (!_repositories.ContainsKey(typeof(TEntity).Name))
            {
                var repository = new GenericRepository<TEntity, TKey>(_storeDbContext);
                _repositories.Add(typeof(TEntity).Name, repository);
            }
            return _repositories[typeof(TEntity).Name] as IGenericRepository<TEntity, TKey>;
        }
        public async Task<int> SaveChanges() =>await _storeDbContext.SaveChangesAsync();   
    }
}
