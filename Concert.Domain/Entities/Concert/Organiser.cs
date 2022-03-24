using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Concert.Domain.Entities.Concert
{
    public class Organiser
    {
        [Key]
        public Guid OrganiserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
  
    }
}
