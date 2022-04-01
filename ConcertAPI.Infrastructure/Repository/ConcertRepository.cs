using Concert.Application.Interface;
using Concert.Domain.Entities.Concert;
using Concert.Infrastructure.Service;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Concert.Infrastructure.Repository
{
    public class ConcertRepository : IConcert
    {
        readonly DbService _db;
        public ConcertRepository(DbService _db)
        {
            this._db = _db;
        }
        public IEnumerable<ConcertModel> GetConcerts
        {
            get
            {
                return _db.Concerts.FromSqlRaw("SELECT * FROM dbo.Concerts")
                    .AsNoTracking().OrderBy(s => s.ConcertId);
            }
        }

        public void AddConcert(ConcertModel concert)
        {
           if(concert == null)
            {
                throw new NullReferenceException(nameof(concert));
            }
            concert.ConcertId = Guid.NewGuid();

            var commandText = "INSERT Concerts (Concert) VALUES (@Concert)";
            var name = new SqlParameter("@Concert", concert);
            int noOfRowInserted =  _db.Database.ExecuteSqlRaw(commandText,name);
            if(noOfRowInserted == 0)
            {
                throw new Exception();

            }
            Save();
        }

        public void DeleteConcert(ConcertModel concert)
        {
            if (concert == null)
            {
                throw new NullReferenceException(nameof(concert));
            }

            var commandText = "DELETE Concerts (Concert) VALUES (@Concert)";
            var name = new SqlParameter("@Concert", concert);
            int noOfRowInserted = _db.Database.ExecuteSqlRaw(commandText, name);
            if (noOfRowInserted == 0)
            {
                throw new Exception();

            }
             Save();
        }

        public IEnumerable<ConcertModel> GetConcertsbyName(string name)
        {
            return _db.Concerts.FromSqlRaw("SELECT * FROM dbo.Concerts").Where(s=>s.Name.StartsWith(name))
                  .AsNoTracking().OrderBy(s => s.ConcertId);
        }

        public async Task<ConcertModel> UpdateConcert(ConcertModel concert)
        {
            ConcertModel currentConcert = await GetConcertById(concert.ConcertId);
            if (currentConcert == null)
            {
                throw new NullReferenceException(nameof(currentConcert));
            }
            _db.Entry(currentConcert).CurrentValues.SetValues(concert);
            Save();
            return await GetConcertById(currentConcert.ConcertId);

        }
        public async Task<ConcertModel> GetConcertById(Guid id)
        {
            return await _db.Concerts.FromSqlInterpolated($"SELECT * FROM dbo.Concerts").Where(s=>s.ConcertId == id)
                 .AsNoTracking().FirstOrDefaultAsync();
        }
        private void Save()
        {
              _db.SaveChangesAsync();
        }
    }
}
