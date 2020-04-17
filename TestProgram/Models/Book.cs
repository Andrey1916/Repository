using Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TestProgram.Models
{
    public class Book : IEntity
    {
        [Key]
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime PublishedOn { get; set; }
        public string Publisher { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal ActualPrice { get; set; }
        public string PromotionalText { get; set; }
        public string ImageUrl { get; set; }
        public bool SoftDeleted { get; set; }

        public ICollection<BookAuthor> BookAuthors { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<LineItem> LineItems { get; set; }
    }
}