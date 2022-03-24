using Concert.Domain.Entities.Concert;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Concert.Infrastructure.Service
{
    public class DbService:DbContext
    {
        public DbService(DbContextOptions<DbService> options):base(options)
        {

        }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<ConcertArtist> ConcertArtists { get; set; }
        public DbSet<Organiser> Organiser { get; set; }
        public DbSet<ConcertOrganiser> ConcertOrganiser { get; set; }
        public DbSet<Sponsor> Sponsors { get; set; }
        public DbSet<ConcertSponsor> ConcertSponsors { get; set; }
        public DbSet<ConcertDate> ConcertDate { get; set; }
        public DbSet<ConcertModel> Concerts { get; set; }
    }
}
