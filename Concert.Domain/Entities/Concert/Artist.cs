using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Concert.Domain.Entities.Concert
{
    public class Artist
    {
        [Key]
        public Guid ArtistId { get; set; }
        public string Name { get; set; }
        public string Bio { get; set; }

    }
}
