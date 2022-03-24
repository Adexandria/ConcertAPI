using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Concert.Domain.Entities.Concert
{
    public class ConcertSponsor
    {
        [Key]
        public Guid ConcertSponsorId { get; set; }
        [ForeignKey("Sponsor")]
        public Guid SponsorId { get; set; }
        [ForeignKey("ConcertModel")]
        public Guid ConcertId { get; set; }
        public Sponsor Sponsor { get; set; }
        public ConcertModel Concert { get; set; }
    }
}
