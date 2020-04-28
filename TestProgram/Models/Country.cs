using Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TestProgram.Models
{
    public class Country : IEntity
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }

        public ICollection<Author> Authors { get; set; }
    }
}
