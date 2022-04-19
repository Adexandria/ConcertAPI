using Concert.Application.Interface;
using Concert.Domain.Entities.Concert;
using Concert.Infrastructure.Repository;
using Concert.Infrastructure.Service;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Concert.Test
{
    public class TicketTest
    {
        readonly ITicket _ticket;

        public TicketTest()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DbService>();
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Database = Concert.Test;Integrated Security=True;Connect Timeout=30;");
            DbService _db = new DbService(optionsBuilder.Options);
            this._ticket = new TicketRepository(_db);
            
        }
        [Fact]
        public void GetTicketsByPrice_Test()
        {
            IEnumerable<Ticket> tickets = _ticket.SearchTicketByPrice("Adefest", 1000);
            Assert.NotNull(tickets);

        }
        [Fact]
        public void GetTicketsByName_Test()
        {
            IEnumerable<Ticket> tickets = _ticket.SearchTicketByName("Adefest");
            Assert.NotNull(tickets);
        }
        [Fact]
        public void AddTicket_Test()
        {
            Ticket ticket = new Ticket
            {
                ConcertId = Guid.Parse("c61adb77-0206-46d4-9d1a-1c952c186fd1"),
                Price = 1000,
                Package = TicketPackage.Regular,
                TicketId = Guid.NewGuid()
            };
            Ticket ticket1 = new Ticket
            {
                ConcertId = Guid.Parse("c61adb77-0206-46d4-9d1a-1c952c186fd1"),
                Price = 1000,
                Package = TicketPackage.Regular,
                TicketId = Guid.NewGuid()
            };
            _ticket.AddTicket(ticket);
            int noOfRowInserted = _ticket.AddTicket(ticket1);
            Assert.Equal(1, noOfRowInserted);
        }
        [Fact]
        public void UpdateTicket_Test()
        {
            Ticket ticket = new Ticket
            {
                ConcertId = Guid.Parse("c61adb77-0206-46d4-9d1a-1c952c186fd1"),
                Price = 1000,
                Package = TicketPackage.VIP,
                TicketId = Guid.Parse("4a611e6c-1983-4fd0-b4a3-2cde352a1e21")
            };
            Ticket updatedTicket = _ticket.UpdateTicket(ticket).Result;
            Assert.Equal(ticket.TicketId, updatedTicket.TicketId);
            Assert.Equal(ticket.Price, updatedTicket.Price);
            Assert.Equal(ticket.Package, updatedTicket.Package);
            Assert.Equal(ticket.ConcertId, updatedTicket.ConcertId);
        }
        [Fact]
        public void DeleteTicket_Test()
        {
            Guid ticketId = Guid.Parse("857b833a-e670-4c98-a0fb-f1039f62d126");
            int noOfRowDeleted = _ticket.DeleteTicket(ticketId);
            Assert.Equal(0, noOfRowDeleted);
        }
    }
}
