﻿using Concert.Domain.Entities.Concert;
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
        Task<ConcertModel> GetConcertById(Guid id);
        void AddConcert(ConcertModel concert);
        Task<ConcertModel> UpdateConcert(ConcertModel concert);
        void DeleteConcert(ConcertModel concert);



        
    }
}
