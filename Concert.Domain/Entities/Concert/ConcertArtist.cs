using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Concert.Domain.Entities.Concert
{
    public class ConcertArtist
    {
        [Key]
        public Guid ConcertArtistId { get; set; }
        [ForeignKey("Artist")]
        public Guid ArtistId { get; set; }
        [ForeignKey("ConcertModel")]
        public Guid ConcertId { get; set; }
        public Artist Artist { get; set; }
        public ConcertModel Concert { get; set; }
    }
}
