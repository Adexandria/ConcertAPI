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
    public class DateRepository : IDate
    {
        readonly DbService _db;
        public DateRepository(DbService _db)
        {
            this._db = _db;
        }
        public IEnumerable<ConcertDate> GetConcertByDate(DateTime date)
        {
            return _db.ConcertDate.FromSqlRaw("SELECT * FROM dbo.ConcertDate").Where(s => s.DateTime.Date== date.Date)
                 .AsNoTracking().OrderBy(s => s.ConcertId);
        }
        public int AddConcertDate(ConcertDate date)
        {
            if (date== null)
            {
                throw new NullReferenceException(nameof(date));
            }
            date.DateId = Guid.NewGuid();

            //Raw sql command to insert Date into the table
            string commandText = "INSERT ConcertDate (DateId,ConcertId,DateTime) VALUES (@DateId,@ConcertId,@DateTime)";

            List<SqlParameter> sqlParameters = GetSqlParameters(date);
            IEnumerable<object> parameters = sqlParameters;

            int noOfRowInserted = _db.Database.ExecuteSqlRaw(commandText, parameters);
            Save();
            return noOfRowInserted;
        }

        public int DeleteConcertDate(Guid id)
        {
            if (id == null)
            {
                throw new NullReferenceException(nameof(id));
            }

            //Delete Date by id and return 1(true) or 0(false)
            int noOfRowDeleted = _db.Database.ExecuteSqlInterpolated($"Delete from ConcertDate where DateId ={id}");
            Save();
            return noOfRowDeleted;
        }

        public async Task<ConcertDate> UpdateConcertDate(ConcertDate date)
        {
            ConcertDate currentDate = await GetDateById(date.DateId);
            if (currentDate == null)
            {
                throw new NullReferenceException(nameof(currentDate));
            }
            //copies updated properties from date to currentDate
            _db.Entry(currentDate).CurrentValues.SetValues(date);
            string commandText = "UPDATE ConcertDate SET DateId = @DateId,DateTime = @DateTime WHERE ConcertId = @ConcertId";

            List<SqlParameter> sqlParameters = GetSqlParameters(currentDate);
            _db.Database.ExecuteSqlRaw(commandText, sqlParameters);

            Save();
            //Gets updated Date by Id
            ConcertDate updatedDate = await GetDateById(currentDate.DateId);
            return updatedDate;
        }

        private async Task<ConcertDate> GetDateById(Guid dateId)
        {
            return await _db.ConcertDate.FromSqlInterpolated($"Select * From dbo.ConcertDate Where DateId = {dateId}").AsNoTracking().FirstOrDefaultAsync();
        }

        private void Save()
        {
            _db.SaveChangesAsync();
        }
        private List<SqlParameter> GetSqlParameters(ConcertDate date)
        {
            SqlParameter dateId = new SqlParameter("@DateId", date.DateId);
            SqlParameter concertId = new SqlParameter("@ConcertId", date.ConcertId);
            SqlParameter dateTime = new SqlParameter("@DateTime", date.DateTime);
            List<SqlParameter> sqlParameters = new List<SqlParameter>
            {
                dateId,
                concertId,
                dateTime,
            };
            return sqlParameters;
        }
    }
}
