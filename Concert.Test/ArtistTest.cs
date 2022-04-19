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
    public class ArtistTest
    {
        readonly IArtist _artist;
        readonly DbService _db;
        public ArtistTest()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DbService>();
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Database = Concert.Test;Integrated Security=True;Connect Timeout=30;");
            this._db = new DbService(optionsBuilder.Options);
            this._artist = new ArtistRepository(_db);
        }


        [Fact]
        public void GetArtistsByName_Test()
        {
            string name = "Adeola";
            IEnumerable<Artist> currentArtists = _artist.GetArtist(name);
            Assert.NotNull(currentArtists);
        }
        [Fact]
        public void AddArtist_Test()
        {
            Artist newArtist = new Artist
            {
                Name = "Adeola",
                Bio = "Adeola is from a small town in nigeria. She started singing after watching lemondade mouth.She was in  love with the 70's pop and was inspired by it. ",
                ArtistId = Guid.NewGuid()
            };

            Artist newArtist1 = new Artist
            {
                Name = "Adeola",
                Bio = "Adeola is from a small town in nigeria. She started singing after watching lemondade mouth.She was in  love with the 70's pop and was inspired by it. ",
                ArtistId = Guid.NewGuid()
            };
            _artist.AddArtist(newArtist1);
            int noOfRowInserted = _artist.AddArtist(newArtist);
            Assert.Equal(1, noOfRowInserted);
        }
        [Fact]
        public void UpdateArtist_Test()
        {
            Artist updateArtist = new Artist
            {
                Name = "Adeole",
                Bio = "Adeola is from a small town in nigeria. She started singing after watching lemondade mouth.She was in  love with the 70's pop and was inspired by it. ",
                ArtistId = Guid.Parse("51bafae2-8d8c-403c-8a41-5276ce845474")
            };
            Artist updatedArtist = _artist.UpdateArtist(updateArtist).Result;
            Assert.NotNull(updatedArtist);
            /*   Assert.Equal(updateArtist.ArtistId, updatedArtist.ArtistId);
               Assert.Equal(updateArtist.Bio, updatedArtist.Bio);
               Assert.Equal(updateArtist.Name, updatedArtist.Name);*/
        }
        [Fact]
        public void DeleteArtist_Test()
        {
            Guid artistId = Guid.Parse("59484b55-afa4-46f9-a339-ac63503b006f");
            int noOfRowDeleted = _artist.DeleteArtist(artistId);
            Assert.Equal(1, noOfRowDeleted);
        }



        //Concert Artist Test

        [Fact]
        public void GetConcertArtists()
        {
            IEnumerable<ConcertArtist> concertArtists = _artist.GetConcertArtists;
            Assert.NotNull(concertArtists);
        }

        [Fact]
        public void GetConcertArtistByName_Test()
        {
            string artistName = "Adeola";
            IEnumerable<ConcertArtist> concertArtists = _artist.GetArtistByName(artistName);
            Assert.NotNull(concertArtists);
        }
        [Fact]
        public void AddConcertArtist_Test()
        {
            ConcertArtist concertArtist = new ConcertArtist
            {
                ConcertArtistId = Guid.NewGuid(),
                ArtistId = Guid.Parse("51bafae2-8d8c-403c-8a41-5276ce845474"),
                ConcertId = Guid.Parse("535b6531-245c-4303-b1e3-f65a9a48bd0e")
            };
            ConcertArtist concertArtist1 = new ConcertArtist
            {
                ConcertArtistId = Guid.NewGuid(),
                ArtistId = Guid.Parse("51bafae2-8d8c-403c-8a41-5276ce845474"),
                ConcertId = Guid.Parse("9c0deee5-4674-4560-ac4f-2121d32c9ad7")
            };
            _artist.AddConcertArtist(concertArtist1);
            int noOfRowInserted = _artist.AddConcertArtist(concertArtist);
            Assert.Equal(1, noOfRowInserted);
        }
        [Fact]
        public void UpdateConcertArtist_Test()
        {
            ConcertArtist currentConcertArtist = new ConcertArtist
            {
                ConcertArtistId = Guid.Parse("34bf36cd-5b48-42f2-addf-643c2ec13b12"),
                ArtistId = Guid.Parse("535b6531-245c-4303-b1e3-f65a9a48bd0e"),
                ConcertId = Guid.Parse("9c0deee5-4674-4560-ac4f-2121d32c9ad7")
            };
            ConcertArtist updatedConcertArtist = _artist.UpdateConcertArtist(currentConcertArtist).Result;
            Assert.NotNull(updatedConcertArtist);
            /*Assert.Equal(currentConcertArtist.ArtistId, updatedConcertArtist.ArtistId);
            Assert.Equal(currentConcertArtist.ConcertArtistId, updatedConcertArtist.ConcertArtistId);
            Assert.Equal(currentConcertArtist.ConcertId, updatedConcertArtist.ConcertId);*/
        }
        [Fact]
        public void DeleteConcertArtist_Test()
        {
            Guid concertArtistId = Guid.Parse("19921551-27c2-4f8c-bac5-b760649ea431");
            int noOfRowDeleted = _artist.DeleteConcertArtist(concertArtistId);
            Assert.Equal(1, noOfRowDeleted);
        }
    }
}
