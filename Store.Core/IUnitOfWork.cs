using Store.Core.Entities;
using Store.Core.Repositories.Contract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Core
{
    public interface IUnitOfWork
    {
        Task<int> SaveChanges();
        IGenericRepository<TEntity,TKey> Repository<TEntity,TKey>() where TEntity :BaseEntity<TKey>;
    }
}
