using Concert.Application.Interface;
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
    public class OrganiserTest
    {
        readonly IOrganiser _organiser;
        
        public OrganiserTest()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DbService>();
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Database = Concert.Test;Integrated Security=True;Connect Timeout=30;");
            DbService _db = new DbService(optionsBuilder.Options);
            this._organiser = new OrganiserRepository(_db);
        }
        [Fact]
        public void GetOrganisers_Test()
        {
            IEnumerable<Organiser> organisers = _organiser.GetOrganiser("Name");
            Assert.NotNull(organisers);
        }

        [Fact]
        public void AddOrganiser_Test()
        {
            Organiser newOrganiser = new Organiser
            {
                Email = "adeola@gmail.com",
                Name = "Adeola",
                OrganiserId = Guid.NewGuid()
            };
            Organiser newOrganiser1 = new Organiser
            {
                Email = "adeola@gmail.com",
                Name = "Adeola12",
                OrganiserId = Guid.NewGuid()
            };
            _organiser.AddOrganiser(newOrganiser);
            int noOfRowInserted = _organiser.AddOrganiser(newOrganiser1);
            Assert.Equal(1, noOfRowInserted);
        }
        [Fact]
        public void UpdateOrganiser_Test()
        {
            Organiser updateOrganiser = new Organiser
            {
                Email = "adeola@gmail.com",
                Name = "Adeola12",
                OrganiserId = Guid.Parse("f05ea271-9d89-46a0-b7c7-89288753e3dd")
            };
            Organiser updatedOrganiser = _organiser.UpdateOrganiser(updateOrganiser).Result;
            Assert.Equal(updateOrganiser.Email, updatedOrganiser.Email);
            Assert.Equal(updateOrganiser.Name, updatedOrganiser.Name);
            Assert.Equal(updateOrganiser.OrganiserId, updatedOrganiser.OrganiserId);
        }

        [Fact]
        public void DeleteOrganiser_Test()
        {
            Guid organiserId = Guid.Parse("8fb484b2-1f80-46f3-b376-9326dd0af578");
            int noOfRowInserted = _organiser.DeleteOrganiser(organiserId);
            Assert.Equal(0, noOfRowInserted);
        }

        //Concert Organiser
        [Fact]
        public void GetConcertOrganisers_Test()
        {
            IEnumerable<ConcertOrganiser> concertOrganisers = _organiser.GetConcertOrganisers;
            Assert.NotNull(concertOrganisers);
        }

        [Fact]
        public void GetConcertOrganiserByName_Test()
        {
            IEnumerable<ConcertOrganiser> concertOrganisers = _organiser.GetOrganiserByName("Adeola");
            Assert.NotNull(concertOrganisers);
        }

        [Fact]
        public void GetConcertOrganiserByConcert_Test()
        {
            IEnumerable<ConcertOrganiser> concertOrganisers = _organiser.GetOrganiserByConcert("Adefest");
            Assert.NotNull(concertOrganisers);
        }

        [Fact]
        public void AddConcertOrganiser_Test()
        {
            ConcertOrganiser concertOrganiser = new ConcertOrganiser
            {
                OrganiserId = Guid.Parse("f05ea271-9d89-46a0-b7c7-89288753e3dd"),
                ConcertId = Guid.Parse("879745f8-8cf3-4553-8205-113f759d551f"),
                ConcertOrganiserId = Guid.NewGuid()
                
            };
            ConcertOrganiser concertOrganiser1 = new ConcertOrganiser
            {
                OrganiserId = Guid.Parse("f05ea271-9d89-46a0-b7c7-89288753e3dd"),
                ConcertId = Guid.Parse("879745f8-8cf3-4553-8205-113f759d551f"),
                ConcertOrganiserId = Guid.NewGuid()
            };
            _organiser.AddConcertOrganiser(concertOrganiser);
            int noOfRowInserted = _organiser.AddConcertOrganiser(concertOrganiser1);
            Assert.Equal(1, noOfRowInserted);
        }
        [Fact]
        public void UpdateConcertOrganiser_Test()
        {
            ConcertOrganiser concertOrganiser = new ConcertOrganiser
            {
                OrganiserId = Guid.Parse("f05ea271-9d89-46a0-b7c7-89288753e3dd"),
                ConcertId = Guid.Parse("49de8445-80e5-40b1-8f2b-d40d3de3248d"),
                ConcertOrganiserId = Guid.Parse("6785acd4-9b21-4008-9831-0ff2471d6060")
            };
            ConcertOrganiser updatedConcertOrganiser = _organiser.UpdateConcertOrganiser(concertOrganiser).Result;
            Assert.Equal(concertOrganiser.OrganiserId, updatedConcertOrganiser.OrganiserId);
            Assert.Equal(concertOrganiser.ConcertId, updatedConcertOrganiser.ConcertId);
            Assert.Equal(concertOrganiser.ConcertOrganiserId, updatedConcertOrganiser.ConcertOrganiserId);
        }
        [Fact]
        public void DeleteConcertOrganiser_Test()
        {
            Guid ConcertOrganiserId = Guid.Parse("f3417081-bba0-4dc9-8baa-53d84a24e1f1");
            int noOfRowDeleted = _organiser.DeleteConcertOrganiser(ConcertOrganiserId);
            Assert.Equal(1, noOfRowDeleted);
        }
    }
}
