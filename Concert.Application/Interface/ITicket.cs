using Concert.Domain.Entities.Concert;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Concert.Application.Interface
{
    public interface ITicket
    {
        IEnumerable<Ticket> SearchTicketByPrice(string concertName,int price);
        IEnumerable<Ticket> SearchTicketByName(string ConcertName);
        int AddTicket(Ticket ticket);
        Task<Ticket> UpdateTicket(Ticket ticket);
        int DeleteTicket(Guid ticketId);
    }
}
