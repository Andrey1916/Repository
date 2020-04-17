using Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TestProgram.Models
{
    public class Order : IEntity
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime DateOrderedUtc { get; set; }
        public DateTime ExpectedDeliveryDate { get; set; }
        public string CustomerName { get; set; }

        public ICollection<LineItem> LineItems { get; set; }
    }
}