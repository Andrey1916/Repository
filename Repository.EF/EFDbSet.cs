using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Repository.EF
{
    class EFDbSet<T> : ISet<T>, IDisposable where T : class, IEntity
    {
        private bool disposed = false;
        private readonly DbContext context;
        private readonly object syncObj;

        public EFDbSet(DbContext context, object syncObj)
        {
            this.context = context ?? throw new NullReferenceException(nameof(context));
            this.syncObj = syncObj ?? throw new NullReferenceException(nameof(syncObj));
        }

        public Type ElementType => ((IQueryable)context.Set<T>()).ElementType;

        public Expression Expression => ((IQueryable)context.Set<T>()).Expression;

        public IQueryProvider Provider => ((IQueryable)context.Set<T>()).Provider;


        public T GetById<T>(object id) where T : class
        {
            return context.Find<T>(id);
        }

        public T AddOrUpdate(T obj)
        {
            lock (syncObj)
            {
                bool exist = context.Set<T>().Any(entity => entity.Id == obj.Id);

                if (!exist)
                {
                    context.Add<T>(obj);
                }
                else
                {
                    context.Update<T>(obj);
                }

                context.SaveChanges();
            }
            return obj;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)context.Set<T>()).GetEnumerator();
        }

        public void Remove(T obj)
        {
            context.Remove(obj);
        }

        public void Remove(object id)
        {
            var entity = context.Set<T>().Find(id);

            if (entity != null)
            {
                context.Remove(entity);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)context.Set<T>()).GetEnumerator();
        }


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
