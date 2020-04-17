using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Repository.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository.EF.Transaction
{
    public class DbTransaction : ITransaction, IDisposable
    {
        private bool disposed = false;
        private readonly DbContext context;
        private readonly IDbContextTransaction transaction;
        private object syncObj;

        public DbTransaction(DbContext context, object syncObj)
        {
            this.context = context ?? throw new NullReferenceException(nameof(context));
            this.syncObj = syncObj ?? throw new NullReferenceException(nameof(syncObj));
            this.transaction = context.Database.BeginTransaction();
        }


        #region Transaction

        public void Commit()
        {
            context.SaveChanges();

            transaction.Commit();
        }

        public void Rollback()
        {
            transaction.Rollback();
        }

        #endregion


        public T GetById<T>(object id) where T : class
        {
            return context.Find<T>(id);
        }

        public T AddOrUpdate<T>(T obj) where T : class, IEntity
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

        public void Remove<T>(T obj) where T : class
            => context.Remove(obj);

        public void Remove<T>(object id) where T : class
        {
            var entity = context.Set<T>().Find(id);

            if (entity != null)
            {
                context.Remove(entity);
            }
        }

        public ISet<T> Set<T>() where T : class, IEntity
            => new EFDbSet<T>(context, syncObj);


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
                this.transaction.Dispose();
                this.context.Dispose();

                disposed = true;
            }
        }

        ~DbTransaction()
        {
            this.Dispose(false);
        }

        #endregion
    }
}
