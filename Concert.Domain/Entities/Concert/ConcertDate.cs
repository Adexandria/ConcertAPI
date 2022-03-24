using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Concert.Domain.Entities.Concert
{
    public class ConcertDate
    {
        [Key]
        public Guid DateId { get; set; }
        public DateTime DateTime { get; set; }
        [ForeignKey("ConcertModel")]
        public Guid ConcertId { get; set; }
        public ConcertModel Concert { get; set; }
    }
}
