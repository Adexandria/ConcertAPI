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
        Task AddArtist(Artist artist);
        Task<Artist> UpdateArtist(Artist artist);
        Task DeleteArtist(Artist artist);
    



        //Concert Artist
        IEnumerable<ConcertArtist> GetConcertArtists { get; }
        IEnumerable<ConcertArtist> GetArtistByName(string name);
        Task AddConcertArtist(ConcertArtist artist);
        Task<ConcertArtist> UpdateConcertArtist(ConcertArtist artists);
        Task DeleteConcertArtist(ConcertArtist artists);
    }
}
