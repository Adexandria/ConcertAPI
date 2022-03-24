using Concert.Domain.Entities.Concert;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Concert.Application.Interface
{
    public interface ITicket
    {
        IEnumerable<Ticket> SearchTicketByPrice(int price);
        Task AddTicket(Ticket ticket);
        Task<Ticket> UpdateTicket(Ticket ticket);
        Task DeleteTicket(Ticket ticket);
    }
}
