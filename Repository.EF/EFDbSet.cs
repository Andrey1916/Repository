using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Repository.EF
{
    class EFDbSet<TEntity> : ISet<TEntity>, IDisposable where TEntity : class, IEntity
    {
        private bool disposed = false;
        private readonly DbContext context;
        private readonly object syncObj;

        public EFDbSet(DbContext context, object syncObj)
        {
            this.context = context ?? throw new NullReferenceException(nameof(context));
            this.syncObj = syncObj ?? throw new NullReferenceException(nameof(syncObj));
        }

        public Type ElementType => ((IQueryable)context.Set<TEntity>()).ElementType;

        public Expression Expression => ((IQueryable)context.Set<TEntity>()).Expression;

        public IQueryProvider Provider => ((IQueryable)context.Set<TEntity>()).Provider;


        public T GetById<T>(object id) where T : class
        {
            return context.Find<T>(id);
        }

        public TEntity AddOrUpdate(TEntity obj)
        {
            lock (syncObj)
            {
                bool exist = context.Set<TEntity>().Any(entity => entity.Id == obj.Id);

                if (!exist)
                {
                    context.Add<TEntity>(obj);
                }
                else
                {
                    context.Update<TEntity>(obj);
                }

                context.SaveChanges();
            }
            return obj;
        }

        public IEnumerator<TEntity> GetEnumerator()
        {
            return ((IEnumerable<TEntity>)context.Set<TEntity>()).GetEnumerator();
        }

        public void Remove(TEntity obj)
        {
            context.Remove(obj);
        }

        public void Remove(object id)
        {
            var entity = context.Set<TEntity>().Find(id);

            if (entity != null)
            {
                context.Remove(entity);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)context.Set<TEntity>()).GetEnumerator();
        }

        public Repository.Query.IIncludableQueryable<TEntity, TProperty> Include<TProperty>(Expression<Func<TEntity, TProperty>> navigationPropertyPath)
            => new Query.IncludableQueryable<TEntity, TProperty>(context.Set<TEntity>().Include(navigationPropertyPath));

        #region Disposing
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                { /* Освобождаем управляемые ресурсы */ }
                // освобождаем неуправляемые объекты
                this.context.Dispose();

                disposed = true;
            }
        }

        ~EFDbSet()
        {
            Dispose(false);
        }
        #endregion
    }
}
