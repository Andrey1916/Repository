using Repository;
using System;
using System.ComponentModel.DataAnnotations;

namespace TestProgram.Models
{
    public class Review : IEntity
    {
        [Key]
        public Guid Id { get; set; }
        public string VoterName { get; set; }
        public int NumStars { get; set; }
        public string Comment { get; set; }

        public Guid BookId { get; set; }
        public Book Book { get; set; }
    }
}