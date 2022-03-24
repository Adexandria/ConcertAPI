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
        Task AddOrganiser(Organiser organiser);
        Task<Organiser> UpdateOrganiser(Organiser organiser);
        Task DeleteOrganiser(Organiser organiser);


        //Concert Organiser
        IEnumerable<ConcertOrganiser> GetConcertOrganisers { get; }
        IEnumerable<ConcertOrganiser> GetOrganiserByName(string name);
        IEnumerable<ConcertOrganiser> GetOrganiserByConcert(string name);
        Task AddConcertOrganiser(ConcertOrganiser organiser);
        Task<ConcertOrganiser> UpdateConcertOrganiser(ConcertOrganiser organiser);
        Task DeleteConcertOrganiser(ConcertOrganiser organiser);
    }
}
