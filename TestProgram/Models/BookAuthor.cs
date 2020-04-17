using Repository;
using System;
using System.ComponentModel.DataAnnotations;

namespace TestProgram.Models
{
    public class BookAuthor : IEntity
    {
        [Key]
        public Guid Id { get; set; }

        public Guid BookId { get; set; }
        public Book Book { get; set; }

        public Guid AuthorId { get; set; }
        public Author Author { get; set; }
    }
}