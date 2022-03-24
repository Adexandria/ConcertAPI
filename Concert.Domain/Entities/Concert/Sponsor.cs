using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Concert.Domain.Entities.Concert
{
    public class Sponsor
    {
        [Key]
        public Guid SponsorId { get; set; }
        public string Name { get; set; }
        public Level SponsorLevel { get; set; }

    }
}
