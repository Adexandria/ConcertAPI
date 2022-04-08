using Concert.Domain.Entities.Concert;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Concert.Application.Interface
{
    public interface IOrganiser
    {
        //Organiser
        int AddOrganiser(Organiser organiser);
        Task<Organiser> UpdateOrganiser(Organiser organiser);
        int DeleteOrganiser(Guid organiserId);


        //Concert Organiser
        IEnumerable<ConcertOrganiser> GetConcertOrganisers { get; }
        IEnumerable<ConcertOrganiser> GetOrganiserByName(string name);
        IEnumerable<ConcertOrganiser> GetOrganiserByConcert(string name);
        int AddConcertOrganiser(ConcertOrganiser organiser);
        Task<ConcertOrganiser> UpdateConcertOrganiser(ConcertOrganiser organiser);
        int DeleteConcertOrganiser(Guid concertOrganiserId);
    }
}
