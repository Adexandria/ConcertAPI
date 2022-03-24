using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Concert.Domain.Entities.Concert
{
    public class ConcertModel
    {
        [Key]
        public Guid ConcertId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
    }
}
