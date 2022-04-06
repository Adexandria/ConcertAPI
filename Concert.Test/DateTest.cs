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
    public class DateTest
    {
        readonly IDate _date;
        readonly DbService _db;
        public DateTest()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DbService>();
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Database = Concert.Test;Integrated Security=True;Connect Timeout=30;");
            this._db = new DbService(optionsBuilder.Options);
            this._date = new DateRepository(_db);
        }

        [Fact]
        public void AddDate_Test()
        {
            ConcertDate newDate = new ConcertDate
            {
                ConcertId = Guid.Parse("9c0deee5-4674-4560-ac4f-2121d32c9ad7"),
                DateTime = new DateTime(2022, 04, 26)
            };
            ConcertDate newDate1 = new ConcertDate
            {
                ConcertId = Guid.Parse("9c0deee5-4674-4560-ac4f-2121d32c9ad7"),
                DateTime = new DateTime(2022, 04, 26)
            };
            _date.AddConcertDate(newDate1);
            int rowInserted = _date.AddConcertDate(newDate);
            Assert.Equal(1, rowInserted);
        }
       [Fact]
        public void GetAllDates_Test()
        {
            DateTime date = new DateTime(2022, 04, 26);
            IEnumerable<ConcertDate> dates = _date.GetConcertByDate(date);
            Assert.NotNull(dates);
        }
        [Fact]
        public void DeleteDateById_Test()
        {
            Guid dateId = Guid.Parse("b9a98655-5df2-436f-9b8e-2e2d9fa87584");
            int rowdeleted =  _date.DeleteConcertDate(dateId);
            Assert.Equal(1, rowdeleted);
        }
        [Fact]
        public void UpdateDate_Test()
        {
            ConcertDate updatedDate = new ConcertDate
            {
                DateId = Guid.Parse("8028a21c-99ae-4f96-84eb-8907603e06ba"),
                ConcertId = Guid.Parse("9c0deee5-4674-4560-ac4f-2121d32c9ad7"),
                DateTime = new DateTime(2022, 05, 26)
            };
            ConcertDate currentDate = _date.UpdateConcertDate(updatedDate).Result;
            Assert.Equal(currentDate.DateTime, updatedDate.DateTime);
            Assert.Equal(currentDate.ConcertId, updatedDate.ConcertId);
            Assert.Equal(currentDate.DateId, updatedDate.DateId);
        }
    }
}
