using Microsoft.EntityFrameworkCore;
using Store.Core.Entities;
using Store.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Repository
{
    public static class SpecificationsEvaluator<TEntity,TKey> where TEntity : BaseEntity<TKey>
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery,ISpecifications<TEntity,TKey> spec, bool forCount = false)
        {
            var query = inputQuery;
            if (spec.Criteria != null)
            {
                query = query.Where(spec.Criteria);
            }
            if (!forCount)
            {
                if (spec.OrderBy != null)
                {
                    query = query.OrderBy(spec.OrderBy);
                }
                else if (spec.OrderByDesc != null)
                {
                    query = query.OrderByDescending(spec.OrderByDesc);
                }
                if (spec.IsPaginationEnabled)
                {
                    query = query.Skip(spec.Skip).Take(spec.Take);
                }
                query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));
            }
            return query;
        }
    }
}
