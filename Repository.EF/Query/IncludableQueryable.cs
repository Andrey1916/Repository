using Repository.Query;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Repository.EF.Query
{
    public class IncludableQueryable<TEntity, TProperty> : Repository.Query.IIncludableQueryable<TEntity, TProperty> where TEntity : class
    {
        private Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<TEntity, TProperty> includableQueryable;

        public IncludableQueryable(Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<TEntity, TProperty> includableQueryable)
        {
            this.includableQueryable = includableQueryable;
        }

        public Expression Expression => this.includableQueryable.Expression;
        public Type ElementType => this.includableQueryable.ElementType;
        public IQueryProvider Provider => this.includableQueryable.Provider;

        public IEnumerator<TEntity> GetEnumerator() => this.includableQueryable.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        public IIncludableQueryable<TEntity, TCurrentProperty> ThenInclude<TCurrentProperty>(Expression<Func<TProperty, TCurrentProperty>> navigationPropertyPath)
        {
            return new IncludableQueryable<TEntity, TCurrentProperty>(includableQueryable.ThenInclude(navigationPropertyPath));
        }
    }
}
