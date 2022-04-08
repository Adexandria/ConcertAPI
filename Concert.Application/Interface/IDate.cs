using Concert.Domain.Entities.Concert;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Concert.Application.Interface
{
   public  interface IDate
   {
        //Concert Date
        IEnumerable<ConcertDate> GetConcertByDate(DateTime date);
        int AddConcertDate(ConcertDate date);
        Task<ConcertDate> UpdateConcertDate(ConcertDate date);
        int DeleteConcertDate(Guid id);
    }
}
