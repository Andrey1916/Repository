using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace Repository.EF
{
    public class DataContext : DbContext
    {
        private readonly string connectionString;
        private readonly IEnumerable<Type> types;

        public DataContext(string connectionString, IEnumerable<Type> types) : base()
        {
            this.types = types;
            this.connectionString = connectionString;

            this.Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var type in types)
            {
                modelBuilder.Entity(type);
            }
        }
    }

    public class MyLoggerProvider : ILoggerProvider
    {
        public ILogger CreateLogger(string categoryName) => new MyLogger();

        public void Dispose() { }

        private class MyLogger : ILogger
        {
            public IDisposable BeginScope<TState>(TState state) => null;

            public bool IsEnabled(LogLevel logLevel) => true;

            public void Log<TState>(LogLevel logLevel,
                                    EventId eventId,
                                    TState state,
                                    Exception exception,
                                    Func<TState, Exception, string> formatter)
            {
                Console.WriteLine(formatter(state, exception));
            }
        }
    }
}