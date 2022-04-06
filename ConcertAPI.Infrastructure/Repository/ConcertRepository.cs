using Concert.Application.Interface;
using Concert.Domain.Entities.Concert;
using Concert.Infrastructure.Service;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

        //Get All Concerts
        public IEnumerable<ConcertModel> GetConcerts
        {
            get
            {
                return _db.Concerts.FromSqlRaw("SELECT * FROM dbo.Concerts")
                    .AsNoTracking().OrderBy(s => s.ConcertId);
            }
        }

        //Get Concert By Name
        public IEnumerable<ConcertModel> GetConcertsbyName(string name)
        {
            return _db.Concerts.FromSqlRaw("SELECT * FROM dbo.Concerts").Where(s => s.Name.StartsWith(name))
                  .AsNoTracking().OrderBy(s => s.ConcertId);
        }

        //Get Concert by Id
        public async Task<ConcertModel> GetConcertById(Guid id)
        {
            return await _db.Concerts.FromSqlRaw("SELECT * FROM dbo.Concerts").Where(s => s.ConcertId == id)
                 .AsNoTracking().FirstOrDefaultAsync();
        }

        //Add Concert
        public int AddConcert(ConcertModel concert)
        {
           if(concert == null)
           {
                throw new NullReferenceException(nameof(concert));
           }
            concert.ConcertId = Guid.NewGuid();
            //Raw sql command to insert concert into the table
            string commandText = "INSERT Concerts (Name,ConcertId,Address,Description) VALUES (@ConcertName,@ConcertId,@ConcertAddress,@ConcertDescription)";
            
            List<SqlParameter> sqlParameters = GetSqlParameters(concert);
            IEnumerable<object> parameters = sqlParameters;
            
            int noOfRowInserted =  _db.Database.ExecuteSqlRaw(commandText,parameters);
            Save();
            return noOfRowInserted;
        }

        //Delete concert by Id
        public int DeleteConcert(Guid id)
        {
            if (id == null)
            {
                throw new NullReferenceException(nameof(id));
            }

            //Delete concert by id and return 1(true) or 0(false)
            int noOfRowDeleted = _db.Database.ExecuteSqlInterpolated($"Delete from Concerts where ConcertId ={id}");
            Save();
            return noOfRowDeleted;
        }

     
        //Update concert
        public async Task<ConcertModel> UpdateConcert(ConcertModel concert)
        {
            ConcertModel currentConcert = await GetConcertById(concert.ConcertId);
            if (currentConcert == null)
            {
                throw new NullReferenceException(nameof(currentConcert));
            }
            //copies updated properties from concert to currentConcert
            _db.Entry(currentConcert).CurrentValues.SetValues(concert);
            string commandText = "UPDATE Concerts SET Name = @ConcertName, Address = @ConcertAddress ,Description= @ConcertDescription WHERE ConcertId = @ConcertId";
           
            List<SqlParameter> sqlParameters = GetSqlParameters(currentConcert);
            _db.Database.ExecuteSqlRaw(commandText, sqlParameters);
           
            Save();
            //Gets updated concert by Id
            ConcertModel updatedConcert = await GetConcertById(currentConcert.ConcertId);
            return updatedConcert;
        }


       
        private void Save()
        {
              _db.SaveChangesAsync();
        }

        private List<SqlParameter> GetSqlParameters(ConcertModel concert)
        {
            SqlParameter name = new SqlParameter("@ConcertName", concert.Name);
            SqlParameter id = new SqlParameter("@ConcertId", concert.ConcertId);
            SqlParameter address = new SqlParameter("@ConcertAddress", concert.Address);
            SqlParameter description = new SqlParameter("@ConcertDescription",concert.Description);
            List<SqlParameter> sqlParameters = new List<SqlParameter>
            {
                name,
                id,
                address,
                description
            };
            return sqlParameters;
        }
    }
}
