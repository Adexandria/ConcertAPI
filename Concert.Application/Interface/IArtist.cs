using Concert.Domain.Entities.Concert;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Concert.Application.Interface
{
    public interface IArtist
    {
        //Artist
        IEnumerable<Artist> GetArtist(string name);
        int AddArtist(Artist artist);
        Task<Artist> UpdateArtist(Artist artist);
        int DeleteArtist(Guid artistId);


        //Concert Artist
        IEnumerable<ConcertArtist> GetConcertArtists { get; }
        IEnumerable<ConcertArtist> GetArtistByName(string name);
        int AddConcertArtist(ConcertArtist artist);
        Task<ConcertArtist> UpdateConcertArtist(ConcertArtist artist);
        int DeleteConcertArtist(Guid concertArtistId);
    }
}
