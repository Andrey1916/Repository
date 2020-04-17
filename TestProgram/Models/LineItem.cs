using Repository;
using System;
using System.ComponentModel.DataAnnotations;

namespace TestProgram.Models
{
    public class LineItem : IEntity
    {
        [Key]
        public Guid Id { get; set; }
        public int LineNum { get; set; }
        public int NumBooks { get; set; }
        public decimal BookPrice { get; set; }

        public Guid OrderId { get; set; }
        public Order Order { get; set; }

        public Guid BookId { get; set; }
        public Book Book { get; set; }
    }
}