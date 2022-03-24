using Concert.Domain.Entities.Concert;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Concert.Application.Interface
{
    public interface IConcert
    {
        //Concert Model
        IEnumerable<ConcertModel> GetConcerts { get; }
        IEnumerable<ConcertModel> GetConcertsbyName(string name);
        Task AddConcert(ConcertModel concert);
        Task<ConcertModel> UpdateConcert(ConcertModel concert);
        Task DeleteConcert(ConcertModel concert);



        
    }
}
