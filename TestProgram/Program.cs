#define CHECK_TRANSACTION

#define All_Threads_Write_In_User
#define Read_And_Write_In_One_Table
#define Read_From_One_Table
#define Write_In_Two_Tables
#define Read_From_Two_Tables
#define Read_And_Write_From_Two_Tables_In_Different_Threads



using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Repository;
using Repository.EF;
using Repository.EF.Helpers;
using Repository.Middlewares;
using TestProgram.Models;

namespace TestProgram
{
    // https://www.thereformedprogrammer.net/wp-content/uploads/2018/03/GenericBizRunner-database-diagram-768x698.png
    class Program
    {
        private const string connectionStringSQLite = "Filename = file_db.db";
        public const int THREAD_COUNT = 10;

        static readonly List<Thread> threads = new List<Thread>();

        static void Main(string[] args)
        {
            var types = new List<Type>
            {
                typeof(Author),
                typeof(Book),
                typeof(BookAuthor),
                typeof(LineItem),
                typeof(Order),
                typeof(Review),
                typeof(Country)
            };

            IRepository repository = new DbRepository(connectionStringSQLite, types);

            var author1 = repository.AddOrUpdate(
                new Author
                {
                    Id = Guid.NewGuid(),
                    Name = "Говард Филлипс Лавкрафт"
                }
                );

            var author2 = repository.Set<Author>().AddOrUpdate(
                new Author
                {
                    Id = Guid.NewGuid(),
                    Name = "Рикардо Милос"
                }
                );

#if All_Threads_Write_In_User
            #region All threads write in user
            Console.WriteLine("1) All threads write in user");
            Console.WriteLine();

            for (int i = 0; i < THREAD_COUNT; i++)
            {
                threads.Add(
                    new Thread(() => {
                        Console.WriteLine($"Thread number { Thread.CurrentThread.ManagedThreadId } (write) start.");

                        for (int j = 0; j < 50; j++)
                        {
                            repository.AddOrUpdate(new Book
                            {
                                Id          = Guid.NewGuid(),
                                ActualPrice = 10M,
                                Description = "Some description",
                                Title       = "Magic book " + j
                            });
                        }

                        Console.WriteLine($"Thread number { Thread.CurrentThread.ManagedThreadId } (write) complite.");
                    })
                    );
            }

                
            foreach (var t in threads)
            {
                t.Start();
            }

            foreach (var t in threads)
            {
                t.Join();
            }

            #endregion
#endif

#if Read_And_Write_In_One_Table
            #region Read and write in one table
            Console.WriteLine();
            Console.WriteLine("2) Read and write in one table");
            Console.WriteLine();

            threads.Clear();

            for (int i = 0; i < THREAD_COUNT; i++)
            {
                threads.Add(
                    new Thread(() =>
                    {
                        Console.WriteLine($"Thread number { Thread.CurrentThread.ManagedThreadId } (read and write) start.");

                        for (int j = 0; j < 100; j++)
                        {
                            var books = repository.Set<Book>().Skip(10).Take(30);

                            repository.AddOrUpdate(new Book
                            {
                                Id          = Guid.NewGuid(),
                                ActualPrice = 10M,
                                Description = "Some description",
                                Title       = "Magic book " + j
                            });
                        }

                        Console.WriteLine($"Thread number { Thread.CurrentThread.ManagedThreadId } (read and write) complite.");
                    })
                    );
            }

            foreach (var t in threads)
            {
                t.Start();
            }

            foreach (var t in threads)
            {
                t.Join();
            }
            #endregion
#endif

#if Read_From_One_Table
            #region Read from one table
            Console.WriteLine();
            Console.WriteLine("3) Read from one table");
            Console.WriteLine();

            threads.Clear();

            for (int i = 0; i < THREAD_COUNT; i++)
            {
                threads.Add(
                    new Thread(() =>
                    {
                        Console.WriteLine($"Thread number { Thread.CurrentThread.ManagedThreadId } (read) start.");

                        for (int j = 0; j < 100; j++)
                        {
                            var books = repository.Set<Book>().Skip(10).Take(40);
                        }

                        Console.WriteLine($"Thread number { Thread.CurrentThread.ManagedThreadId } (read) complite.");
                    })
                    );
            }

            foreach (var t in threads)
            {
                t.Start();
            }

            foreach (var t in threads)
            {
                t.Join();
            }
            #endregion
#endif

#if Write_In_Two_Tables
            #region Write in two tables
            Console.WriteLine();
            Console.WriteLine("4) Write in two tables");
            Console.WriteLine();

            threads.Clear();

            for (int i = 0; i < THREAD_COUNT; i++)
            {
                threads.Add(
                    new Thread(() =>
                    {
                        Console.WriteLine($"Thread number { Thread.CurrentThread.ManagedThreadId } (write) start.");

                        for (int j = 0; j < 100; j++)
                        {
                            var bookId    = Guid.NewGuid();
                            var authorId  = Guid.NewGuid();
                            var countryId = Guid.NewGuid();

                            repository.AddOrUpdate(new Country
                            {
                                Id   = countryId,
                                Name = "GB"
                            });

                            repository.AddOrUpdate(new Book
                            {
                                Id          = bookId,
                                ActualPrice = 10M,
                                Description = "Some description",
                                Title       = "Magic book " + j
                            });

                            repository.AddOrUpdate(new Author
                            {
                                Id        = authorId,
                                Name      = "Говард Филлипс Лавкрафт",
                                CountryId = countryId
                            });

                            repository.Set<BookAuthor>().AddOrUpdate(new BookAuthor
                            {
                                Id       = Guid.NewGuid(),
                                BookId   = bookId,
                                AuthorId = authorId
                            });
                        }

                        Console.WriteLine($"Thread number { Thread.CurrentThread.ManagedThreadId } (write) complite.");
                    })
                    );
            }

            foreach (var t in threads)
            {
                t.Start();
            }

            foreach (var t in threads)
            {
                t.Join();
            }
            #endregion
#endif

#if Read_From_Two_Tables
            #region Read from two tables
            Console.WriteLine();
            Console.WriteLine("5) Read from two table");
            Console.WriteLine();

            threads.Clear();

            for (int i = 0; i < THREAD_COUNT; i++)
            {
                threads.Add(
                    new Thread(() =>
                    {
                        Console.WriteLine($"Thread number { Thread.CurrentThread.ManagedThreadId } (read) start.");

                        for (int j = 0; j < 100; j++)
                        {
                            var books = repository.Set<Book>().Skip(10).Take(40);

                            var authors = repository.Set<Author>().Skip(10).Take(40);
                        }

                        Console.WriteLine($"Thread number { Thread.CurrentThread.ManagedThreadId } (read) complite.");
                    })
                    );
            }

            foreach (var t in threads)
            {
                t.Start();
            }

            foreach (var t in threads)
            {
                t.Join();
            }
            #endregion
#endif

#if Read_And_Write_From_Two_Tables_In_Different_Threads
            #region Read and write from two tables in different threads
            Console.WriteLine();
            Console.WriteLine("6) Read and write from two tables in different threads");
            Console.WriteLine();

            threads.Clear();

            {
                int i = 0;

                for (; i < THREAD_COUNT / 2; i++)
                {
                    threads.Add(
                        new Thread(() =>
                        {
                            Console.WriteLine($"Thread number { Thread.CurrentThread.ManagedThreadId } (read) start.");
                            for (int j = 0; j < 100; j++)
                            {
                                var books = repository.Set<Book>().Skip(10).Take(40);

                                var authors = repository.Set<Author>().Skip(10).Take(40);
                            }

                            Console.WriteLine($"Thread number { Thread.CurrentThread.ManagedThreadId } (read) complite.");
                        })
                        );
                }

                for (; i < THREAD_COUNT; i++)
                {
                    threads.Add(
                        new Thread(() =>
                        {
                            Console.WriteLine($"Thread number { Thread.CurrentThread.ManagedThreadId } (write) start.");

                            for (int j = 0; j < 100; j++)
                            {
                                repository.AddOrUpdate(new Book
                                {
                                    Id          = Guid.NewGuid(),
                                    ActualPrice = 10M,
                                    Description = "Some description",
                                    Title       = "Magic book " + j
                                });
                            }

                            Console.WriteLine($"Thread number { Thread.CurrentThread.ManagedThreadId } (write) complite.");
                        })
                        );
                }
            }

            foreach (var t in threads)
            {
                t.Start();
            }

            foreach (var t in threads)
            {
                t.Join();
            }
            #endregion
#endif


            #region Linq
            Console.WriteLine();
            Console.WriteLine("7) Linq");
            Console.WriteLine();

            dynamic[] results = new dynamic[9];

            /* IEnumerable<Book> */
            results[0] = repository.Set<Book>();
            /* IEnumerable<BookAuthor> */
            results[1] = repository.Set<BookAuthor>().Include(ba => ba.Author)
                                                     .ThenInclude(a => a.Country)
                                                     .Where(ba => ba.Author.Country.Name.StartsWith("GB"))
                                                     .ToList();
            /* Book */
            results[2] = repository.Set<Book>().FirstOrDefault();
            /* IEnumerable<Book> */
            results[3] = repository.Set<Book>().Where(b => b.Title.StartsWith("Magic"));
            /* IEnumerable<Book> */
            results[4] = repository.Set<Book>().Where(b => b.Title.StartsWith("Magic"))
                                               .Take(3);
            /* bool */
            results[5] = repository.Set<Book>().Where(b => b.Title.StartsWith("Magic"))
                                               .Any();
            /* int */
            results[6] = repository.Set<Book>().Where(b => b.Title.StartsWith("Magic"))
                                               .Count();
            /* IEnumerable<anonimType> */
            results[7] = repository.Set<Book>().Where(b => b.Title.StartsWith("Magic"))
                                               .Take(5)
                                               .Select(b => new { b.Id, b.Title});
            /* string */
            results[8] = repository.Set<Book>().Where(b => b.Title.StartsWith("Magic"))
                                               .Take(5)
                                               .Select(b => new { b.Id, b.Title })
                                               .ToSql();

            Console.WriteLine("Results: ");
            for (int i = 0; i < results.Length; i++)
            {
                Console.WriteLine(results[i].ToString());
            }

            #endregion


#if CHECK_TRANSACTION
            #region Transaction
            Console.WriteLine();
            Console.WriteLine("8) Transaction");
            Console.WriteLine();

            using (var transaction = repository.BeginTransaction())
            {
                transaction.AddOrUpdate(new Book
                {
                    Id          = Guid.NewGuid(),
                    ActualPrice = 10M,
                    Description = "Some description",
                    Title       = "Magic book "
                });

                int count = transaction.Set<Book>().Count();

                Console.WriteLine(count);

                transaction.Commit();
            }
            #endregion
#endif
        }
    }
}