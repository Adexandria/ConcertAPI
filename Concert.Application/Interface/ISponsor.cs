using Concert.Domain.Entities.Concert;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Concert.Application.Interface
{
    public interface ISponsor
    {
        //Sponsors
        Task AddSponsor(Sponsor sponsor);
        Task<Sponsor> UpdateSponsor(Sponsor sponsor);
        Task DeleteSponsor(Sponsor sponsor);




        //Concert Sponsors
        IEnumerable<ConcertSponsor> GetConcertSponsors { get; }
        IEnumerable<ConcertSponsor> GetSponsorByName(string name);
        Task AddConcertSponsor(ConcertSponsor sponsor);
        Task<ConcertSponsor> UpdateConcertSponsor(ConcertSponsor sponsor);
        Task DeleteConcertSponsor(ConcertSponsor sponsor);
    }
}
