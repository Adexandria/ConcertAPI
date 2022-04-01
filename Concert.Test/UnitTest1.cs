using Concert.Application.Interface;
using Concert.Domain.Entities.Concert;
using Concert.Infrastructure.Repository;
using Concert.Infrastructure.Service;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using Xunit;

namespace Concert.Test
{
    public class UnitTest1
    {
        readonly IConcert _concert;
        readonly DbService _db;
        public UnitTest1()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DbService>();
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Database = Concert.Test;Integrated Security=True;Connect Timeout=30;");
            this._db = new DbService(optionsBuilder.Options);
            this._concert = new ConcertRepository(_db);
        }
        [Fact]
        public void AddConcert_Test()
        {
            Guid concertId = Guid.Parse("9acd1b77-9d79-4e88-8ed4-380aeec79be4");
            Guid concertId1 = Guid.Parse("2723403d-e078-44f5-87a4-1815c4f4d600");
            ConcertModel concert = new ConcertModel
            {
                Address= "Place",
                ConcertId = concertId,
                Description = "another place",
                Name = "Adefest"
            };

            ConcertModel concert1 = new ConcertModel
            {
                Address = "Place",
                ConcertId = concertId1,
                Description = "another place",
                Name = "Adefest"
            };
            _concert.AddConcert(concert);
            _concert.AddConcert(concert1);
            ConcertModel currentConcert = _concert.GetConcertById(concertId).Result;
            Assert.Equal(currentConcert, concert);
        }

        [Fact]
        public void GetConcerts_Test()
        {
            IEnumerable<ConcertModel> concerts = _concert.GetConcerts;
            Assert.NotNull(concerts);
        }
        [Fact]
        public void GetConcertByName_Test()
        {
            IEnumerable<ConcertModel> concerts = _concert.GetConcertsbyName("Adefest");
            Assert.NotNull(concerts);
        }

        /*[Fact]
        public void DeleteConcert_Test()
        {
            Guid concertId = Guid.Parse("2723403d-e078-44f5-87a4-1815c4f4d600");
            ConcertModel currentConcert = _concert.GetConcertById(concertId).Result;
            _concert.DeleteConcert(currentConcert);
            ConcertModel concert = _concert.GetConcertById(concertId).Result;
            Assert.Null(concert);

        }*/
       
        /*[Fact]
        public void UpdateConcert_Test()
        {
            Guid concertId = Guid.Parse("9acd1b77-9d79-4e88-8ed4-380aeec79be4");
            ConcertModel concert = new ConcertModel
            {
                Address = "Places",
                ConcertId = concertId,
                Description = "another place",
                Name = "Adefest"
            };
            ConcertModel currentModel = _concert.UpdateConcert(concert).Result;
            Assert.Equal(currentModel, concert);

        }*/
    }
}
