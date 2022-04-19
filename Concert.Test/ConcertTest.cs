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
    public class ConcertTest
    {
        readonly IConcert _concert;
        readonly DbService _db;
        public ConcertTest()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DbService>();
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Database = Concert.Test;Integrated Security=True;Connect Timeout=30;");
            this._db = new DbService(optionsBuilder.Options);
            this._concert = new ConcertRepository(_db);
        }
        [Fact]
        public void AddConcert_Test()
        {
            ConcertModel concert = new ConcertModel
            {
                Address = "Place",
                Description = "another place",
                Name = "Adefest"
            };
            ConcertModel concert1 = new ConcertModel
            {
                Address = "Place",
                Description = "another place",
                Name = "Adefest"
            };
            _concert.AddConcert(concert1);
            int result = _concert.AddConcert(concert);
            Assert.Equal(1, result);
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

        [Fact]
        public void DeleteConcert_Test()
        {
            Guid concertId = Guid.Parse("814d3792-b9b8-49ba-bef4-1e9754813d7d");
            int result = _concert.DeleteConcert(concertId);
            Assert.Equal(0, result);

        }
       
        [Fact]
        public void UpdateConcert_Test()
        {
            Guid concertId = Guid.Parse("9c0deee5-4674-4560-ac4f-2121d32c9ad7");
            ConcertModel concert = new ConcertModel
            {
                Address = "Places",
                ConcertId = concertId,
                Description = "another place",
                Name = "Adefest"
            };
            ConcertModel currentModel = _concert.UpdateConcert(concert).Result;
            Assert.Equal(currentModel.Address, concert.Address);
            Assert.Equal(currentModel.ConcertId, concert.ConcertId);
            Assert.Equal(currentModel.Description, concert.Description);
            Assert.Equal(currentModel.Name, concert.Name);

        }
    }
}
