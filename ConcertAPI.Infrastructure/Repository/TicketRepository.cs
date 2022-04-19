using Concert.Application.Interface;
using Concert.Domain.Entities.Concert;
using Concert.Infrastructure.Service;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concert.Infrastructure.Repository
{
   public class TicketRepository :ITicket
   {
        readonly DbService _db;
        public TicketRepository(DbService _db)
        {
            this._db = _db;
        }



        public IEnumerable<Ticket> SearchTicketByName(string concertName)
        {
            Guid concertId = GetTicketByConcertName(concertName);
            return _db.Tickets.FromSqlInterpolated($"Select * From dbo.Tickets Where ConcertId = {concertId}").OrderBy(s => s.TicketId).Include(s => s.Concert).AsNoTracking(); ;
        }

        public IEnumerable<Ticket> SearchTicketByPrice(string concertName,int price)
        {
            if(price == 0)
            {
                throw new NullReferenceException(nameof(price));
            }

            Guid concertId = GetTicketByConcertName(concertName);
            return _db.Tickets.FromSqlInterpolated($"Select * From dbo.Tickets Where Price = {price}, ConcertId = {concertId}").Include(s=>s.Concert).AsNoTracking().OrderBy(s => s.TicketId);
        }
        
        public int AddTicket(Ticket ticket)
        {
            if(ticket == null)
            {
                throw new NullReferenceException(nameof(ticket));
            }
            ticket.TicketId = Guid.NewGuid();
            //Raw sql command to insert ticket into the table
            string commandText = "INSERT Tickets (Package,TicketId,ConcertId,Price) VALUES (@Package,@TicketId,@ConcertId,@Price)";

            List<SqlParameter> sqlParameters = GetSqlParameters(ticket);
            IEnumerable<object> parameters = sqlParameters;

            int noOfRowInserted = _db.Database.ExecuteSqlRaw(commandText, parameters);
            Save();
            return noOfRowInserted;
        }

        public int DeleteTicket(Guid ticketId)
        {
            if (ticketId == null)
            {
                throw new NullReferenceException(nameof(ticketId));
            }
            //Delete organiser by ticketId and return 1(true) or 0(false)
            int noOfRowDeleted = _db.Database.ExecuteSqlInterpolated($"Delete from Tickets where TicketId ={ticketId}");
            Save();
            return noOfRowDeleted;
        }

        public async Task<Ticket> UpdateTicket(Ticket ticket)
        {
            if (ticket == null)
            {
                throw new NullReferenceException(nameof(ticket));
            }
            Ticket currentTicket = await GetTicketId(ticket.TicketId);
            _db.Entry(currentTicket).CurrentValues.SetValues(ticket);
            string commandText = "UPDATE Tickets SET Package = @Package,ConcertId = @ConcertId,Price= @Price WHERE TicketId = @TicketId";

            List<SqlParameter> sqlParameters = GetSqlParameters(currentTicket);
            _db.Database.ExecuteSqlRaw(commandText, sqlParameters);
            Save();

            Ticket updatedTicket = await GetTicketId(currentTicket.TicketId);
            return updatedTicket;

        }







        private async Task<Ticket> GetTicketId(Guid ticketId)
        {
            if (ticketId == null)
            {
                throw new NullReferenceException(nameof(ticketId));
            }
            return await _db.Tickets.FromSqlInterpolated($"Select * From dbo.Tickets Where TicketId ={ticketId} ").AsNoTracking().Include(s=>s.Concert).FirstOrDefaultAsync();
        } 
        private Guid GetTicketByConcertName(string concertName)
        {
            if (string.IsNullOrEmpty(concertName))
            {
                throw new NullReferenceException(nameof(concertName));
            }
            return _db.Concerts.FromSqlInterpolated($"Select * From dbo.Concerts Where Name = {concertName}").Select(s => s.ConcertId).FirstOrDefault();

        }

        private void Save()
        {
            _db.SaveChanges();
        }
        private List<SqlParameter> GetSqlParameters(Ticket ticket)
        {
            SqlParameter ticketId= new SqlParameter("@TicketId", ticket.TicketId);
            SqlParameter price = new SqlParameter("@Price", ticket.Price);
            SqlParameter package = new SqlParameter("@Package",ticket.Package);
            SqlParameter concertId = new SqlParameter("@ConcertId", ticket.ConcertId);
            List<SqlParameter> sqlParameters = new List<SqlParameter>
            {
                price,
                package,
                concertId,
                ticketId

            };
            return sqlParameters;
        }

    }
}
