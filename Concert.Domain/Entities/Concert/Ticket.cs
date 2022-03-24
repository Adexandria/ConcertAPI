using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Concert.Domain.Entities.Concert
{
    public class Ticket
    {
        [Key]
        public Guid TicketId { get; set; }
        public TicketPackage Package { get; set; }
        public int Price { get; set; }
        [ForeignKey("ConcertModel")]
        public Guid ConcertId { get; set; }
        public ConcertModel Concert { get; set; }
    }
}
