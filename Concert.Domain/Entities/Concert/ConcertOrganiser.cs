using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Concert.Domain.Entities.Concert
{
    public class ConcertOrganiser
    {
        [Key]
        public Guid ConcertOrganiserId { get; set; }
        [ForeignKey("Organiser")]
        public Guid OrganiserId { get; set; }
        [ForeignKey("ConcertModel")]
        public Guid ConcertId { get; set; }
        public Organiser Organiser { get; set; }
        public ConcertModel Concert { get; set; }
    }
}
