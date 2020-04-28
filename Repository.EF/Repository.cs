using Repository.EF.Transaction;
using Repository.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository.EF
{
    public class DbRepository : IRepository
    {
        private readonly string connectionString;
        private readonly List<Type> types = new List<Type>();
        private static readonly object syncObj = new object();

        public DbRepository(string connectionString, IEnumerable<Type> types)
        {
            if ( string.IsNullOrEmpty(connectionString) )
            {
                throw new ArgumentException("Connection string is null or empty");
            }

            if ( types == null || !types.Any() )
            {
                throw new ArgumentException("List of types is null or empty");
            }

            this.connectionString = connectionString;

            this.types.AddRange(types);

            using (var context = new DataContext(connectionString, types))
            {
#if LOGGING
                context.GetService<ILoggerFactory>().AddProvider(new MyLoggerProvider());
#endif
                context.Database.EnsureCreated();
            }
        }

        public T GetById<T>(object id) where T : class
        {
            using (var context = new DataContext(connectionString, types))
            {
                return context.Find<T>(id);
            }
        }

        public T AddOrUpdate<T>(T obj) where T : class, IEntity
        {
            using (var context = new DataContext(connectionString, types))
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
        }

        public void Remove<T>(T obj) where T : class
        {
            using (var context = new DataContext(connectionString, types))
            {
                context.Remove(obj);
            }
        }

        public void Remove<T>(object id) where T : class
        {
            using (var context = new DataContext(connectionString, types))
            {
                var entity = context.Set<T>().Find(id);

                if (entity != null)
                {
                    context.Remove(entity);
                }
            }
        }

        public ISet<T> Set<T>() where T : class, IEntity
        {
            var context = new DataContext(connectionString, types);

            return new EFDbSet<T>(context, syncObj);
        }

        public ITransaction BeginTransaction()
        {
            var context = new DataContext(connectionString, types);

            return new DbTransaction(context, syncObj);
        }
    }
}
