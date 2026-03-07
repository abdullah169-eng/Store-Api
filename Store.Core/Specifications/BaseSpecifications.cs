using Store.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Store.Core.Specifications
{
    public class BaseSpecifications<TEntity,TKey>:ISpecifications<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        public Expression<Func<TEntity, bool>> Criteria { get; set; }=null!;
        public List<Expression<Func<TEntity, object>>> Includes { get; set; }=new List<Expression<Func<TEntity, object>>>();
        public Expression<Func<TEntity, object>> OrderBy { get; set; } = null!;
        public Expression<Func<TEntity, object>> OrderByDesc { get; set; } = null!;
        public int Skip { get; set; }
        public int Take { get; set; }
        public bool IsPaginationEnabled { get; set; }
        public bool forCount { get; set; }=false;
        public BaseSpecifications()
        {
        }
        public BaseSpecifications(Expression<Func<TEntity, bool>> criteria)
        {
            Criteria = criteria;
        }
        public void AddOrderBy(Expression<Func<TEntity, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }
        public void AddOrderByDescending(Expression<Func<TEntity, object>> orderByDescExpression)
        {
            OrderByDesc = orderByDescExpression;
        }
        public void ApplyPagination(int take, int skip)
        {
            IsPaginationEnabled= true;
            Skip = skip;
            Take= take;
        }
    }
}
