using System;

namespace Repository.Middlewares
{
    public static class Middleware
    {
        public static IRepositoryBase UseMiddleware<T>(this IRepositoryBase repository) where T : IRepositoryBase
        {
            return (IRepositoryBase)Activator.CreateInstance(typeof(T), new object[] { repository });
        }

        public static IRepository UseMiddleware<T>(this IRepository repository) where T : IRepository
        {
            return (IRepository)Activator.CreateInstance(typeof(T), new object[] { repository });
        }
    }
}