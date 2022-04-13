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
        Task<Sponsor> GetSponsor(string name);
        int AddSponsor(Sponsor sponsor);
        Task<Sponsor> UpdateSponsor(Sponsor sponsor);
        int DeleteSponsor(Guid id);




        //Concert Sponsors
        IEnumerable<ConcertSponsor> GetConcertSponsors { get; }
        IEnumerable<ConcertSponsor> GetSponsorByName(string name);
        int AddConcertSponsor(ConcertSponsor sponsor);
        Task<ConcertSponsor> UpdateConcertSponsor(ConcertSponsor sponsor);
        int DeleteConcertSponsor(Guid id);
    }
}
