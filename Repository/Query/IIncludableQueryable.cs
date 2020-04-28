using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Repository.Query
{
    public interface IIncludableQueryable<out TEntity, TProperty> : IQueryable<TEntity> where TEntity : class
    {
        IIncludableQueryable<TEntity, TCurrentProperty> ThenInclude<TCurrentProperty>(Expression<Func<TProperty, TCurrentProperty>> navigationPropertyPath);
    }
}