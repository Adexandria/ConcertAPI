using Concert.Application.Interface;
using Concert.Domain.Entities;
using Concert.Domain.Entities.Concert;
using Concert.Infrastructure.Repository;
using Concert.Infrastructure.Service;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Concert.Test
{
    public class SponsorTest
    {
        readonly ISponsor _sponsor;

        public SponsorTest()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DbService>();
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Database = Concert.Test;Integrated Security=True;Connect Timeout=30;");
            DbService _db = new DbService(optionsBuilder.Options);
            ConcurrencyService concurrency = new ConcurrencyService(_db);
            this._sponsor = new SponsorRepository(_db,concurrency);
        }

        //Sponsor
        [Fact]
        public void GetSponsorByName_Test()
        {
            Sponsor sponsor = _sponsor.GetSponsor("Adeola").Result;
            Assert.NotNull(sponsor);
        }
        [Fact]
        public void AddSponsor_Test()
        {
            Sponsor sponsor = new Sponsor
            {
                SponsorId = Guid.NewGuid(),
                Name = "Adeola",
                SponsorLevel = Level.Gold
            };
            Sponsor sponsor1 = new Sponsor
            {
                SponsorId = Guid.NewGuid(),
                Name = "Adeola",
                SponsorLevel = Level.Gold
            };
            _sponsor.AddSponsor(sponsor);
           int noOfRowInserted = _sponsor.AddSponsor(sponsor1);
           Assert.Equal(1, noOfRowInserted);
        }
        [Fact]
        public void UpdateSponsor_Test()
        {
            Sponsor sponsor = new Sponsor
            {
                SponsorId = Guid.Parse("f4b18623-d3ad-4fcb-a3de-ce13e42819be"),
                Name = "Adeola",
                SponsorLevel = Level.Platinum
            };
            Sponsor updatedSponsor = _sponsor.UpdateSponsor(sponsor).Result;

            Assert.Equal(sponsor.Name, updatedSponsor.Name);
            Assert.Equal(sponsor.SponsorId, updatedSponsor.SponsorId);
            Assert.Equal(sponsor.SponsorLevel, updatedSponsor.SponsorLevel);
             
        }
        [Fact]
        public void DeleteSponsor_Test()
        {
            Guid sponsorId = Guid.Parse("f3417081-bba0-4dc9-8baa-53d84a24e1f1");
            int noOfRowDeleted = _sponsor.DeleteSponsor(sponsorId);
            Assert.Equal(0, noOfRowDeleted);
        }

        //concert sponsor
        [Fact]
        public void GetConcertSponsor_Test()
        {
            IEnumerable<ConcertSponsor> concertSponsors = _sponsor.GetConcertSponsors;
            Assert.NotNull(concertSponsors);
        }
        [Fact]
        public void GetConcertSponsorByName_Test()
        {
            IEnumerable<ConcertSponsor> concertSponsors = _sponsor.GetSponsorByName("Adeola");
            Assert.NotNull(concertSponsors);
        }

        [Fact]
        public void AddConcertSponsor_Test()
        {
            ConcertSponsor concertSponsor = new ConcertSponsor
            {
                SponsorId = Guid.Parse("32b87147-deef-4c96-aad1-582100d12f5f"),
                ConcertId = Guid.Parse("879745f8-8cf3-4553-8205-113f759d551f"),
                ConcertSponsorId = Guid.NewGuid()
            
            };
            ConcertSponsor concertSponsor1 = new ConcertSponsor
            {
                SponsorId = Guid.Parse("32b87147-deef-4c96-aad1-582100d12f5f"),
                ConcertId = Guid.Parse("879745f8-8cf3-4553-8205-113f759d551f"),
                ConcertSponsorId = Guid.NewGuid()

            };

            _sponsor.AddConcertSponsor(concertSponsor);
            int noOfRowInserted =_sponsor.AddConcertSponsor(concertSponsor1);
            Assert.Equal(1, noOfRowInserted);
        }

        [Fact]
        public void UpdateConcertSponsor_Test()
        {
            ConcertSponsor concertSponsor = new ConcertSponsor
            {
                SponsorId = Guid.Parse("aed5683f-9e8c-4c25-bee0-916662cc45f3"),
                ConcertId = Guid.Parse("879745f8-8cf3-4553-8205-113f759d551f"),
                ConcertSponsorId = Guid.Parse("3d233d40-18f4-4d23-8f72-647754929ead")

            };
            ConcertSponsor updatedConcertSponsor = _sponsor.UpdateConcertSponsor(concertSponsor).Result;
            Assert.Equal(concertSponsor.SponsorId, updatedConcertSponsor.SponsorId);
            Assert.Equal(concertSponsor.ConcertId, updatedConcertSponsor.ConcertId);
            Assert.Equal(concertSponsor.ConcertSponsorId, updatedConcertSponsor.ConcertSponsorId);
        }
        [Fact]
        public void DeleteConcerSponsor_Test()
        {
            Guid ConcertSponsorId = Guid.Parse("ed79cb58-bc3b-4565-801a-f83b669ada13");
            int noOfRowDeleted = _sponsor.DeleteConcertSponsor(ConcertSponsorId);
            Assert.Equal(1, noOfRowDeleted);
        }
    }
}
