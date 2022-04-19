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
    public class OrganiserRepository : IOrganiser
    {
        readonly DbService _db;
        public OrganiserRepository(DbService _db)
        {
            this._db = _db;
        }


        //Concert Organiser
        public IEnumerable<ConcertOrganiser> GetConcertOrganisers
        {
            get
            {
                return _db.ConcertOrganiser.FromSqlRaw("SELECT * FROM dbo.ConcertOrganiser").Include(s=>s.Organiser).Include(s=>s.Concert)
                   .AsNoTracking().OrderBy(s => s.ConcertId);
            }
        }

        public IEnumerable<ConcertOrganiser> GetOrganiserByName(string name)
        {
            Guid organiserId =  _db.Organiser.FromSqlRaw("SELECT * FROM dbo.Organiser")
                .Where(s=>s.Name==name).Select(s=>s.OrganiserId).FirstOrDefault();

            return _db.ConcertOrganiser.FromSqlRaw("SELECT * FROM dbo.ConcertOrganiser").Where(s=>s.OrganiserId == organiserId)
                  .Include(s => s.Organiser).Include(s => s.Concert).AsNoTracking().OrderBy(s => s.ConcertId);
           
        }

        public IEnumerable<ConcertOrganiser> GetOrganiserByConcert(string name)
        {

            Guid concertId = _db.Concerts.FromSqlRaw("SELECT * FROM dbo.Concerts").Where(s => s.Name == name)
               .Select(s => s.ConcertId).FirstOrDefault();
            return _db.ConcertOrganiser.FromSqlRaw("SELECT * FROM dbo.ConcertOrganiser").Where(s => s.ConcertId==concertId)
                  .Include(s => s.Organiser).Include(s => s.Concert).AsNoTracking().OrderBy(s => s.ConcertId);

        }

        public int AddConcertOrganiser(ConcertOrganiser organiser)
        {
            if(organiser == null)
            {
                throw new NullReferenceException(nameof(organiser));
            }
            organiser.ConcertOrganiserId = Guid.NewGuid();
            //Raw sql command to insert concertOrganiser into the table
            string commandText = "INSERT ConcertOrganiser (ConcertOrganiserId,ConcertId,OrganiserId) VALUES (@ConcertOrganiserId,@ConcertId,@OrganiserId)";

            List<SqlParameter> sqlParameters = GetConcertOrganiserSqlParameters(organiser);
            IEnumerable<object> parameters = sqlParameters;

            int noOfRowInserted = _db.Database.ExecuteSqlRaw(commandText, parameters);
            Save();
            return noOfRowInserted;
        }
        
        public  async Task<ConcertOrganiser> UpdateConcertOrganiser(ConcertOrganiser organiser)
        {
            if (organiser == null)
            {
                throw new NullReferenceException(nameof(organiser));
            }
            ConcertOrganiser concertOrganiser = await GetConcertOrganiserById(organiser.ConcertOrganiserId);
            _db.Entry(concertOrganiser).CurrentValues.SetValues(organiser);
            string commandText = "UPDATE ConcertOrganiser SET OrganiserId = @OrganiserId,ConcertId = @ConcertId WHERE @ConcertOrganiserId = ConcertOrganiserId";

            List<SqlParameter> sqlParameters = GetConcertOrganiserSqlParameters(concertOrganiser);
            _db.Database.ExecuteSqlRaw(commandText, sqlParameters);
            Save();

            ConcertOrganiser updatedConcertOrganiser = await GetConcertOrganiserById(concertOrganiser.ConcertOrganiserId);
            return updatedConcertOrganiser;
        }

        public int DeleteConcertOrganiser(Guid concertOrganiserId)
        {
           if(concertOrganiserId == null)
            {
                throw new NullReferenceException(nameof(concertOrganiserId));
            }
            //Delete concertOrganiser by id and return 1(true) or 0(false)
            int noOfRowDeleted = _db.Database.ExecuteSqlInterpolated($"Delete from ConcertOrganiser where ConcertOrganiserId ={concertOrganiserId}");
            Save();
            return noOfRowDeleted;
        }



        //Organiser

        public IEnumerable<Organiser> GetOrganiser(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new NullReferenceException(name);
            }
            return _db.Organiser.FromSqlRaw("Select * From dbo.Organiser").Where(s => s.Name.Contains(name)).OrderBy(s => s.OrganiserId).AsNoTracking();
        }

        public int AddOrganiser(Organiser organiser)
        {

            if (organiser == null)
            {
                throw new NullReferenceException(nameof(organiser));
            }
            organiser.OrganiserId = Guid.NewGuid();
            //Raw sql command to insert concertOrganiser into the table
            string commandText = "INSERT Organiser (Name,Email,OrganiserId) VALUES (@Name,@Email,@OrganiserId)";

            List<SqlParameter> sqlParameters = GetOrganiserSqlParameters(organiser);
            IEnumerable<object> parameters = sqlParameters;

            int noOfRowInserted = _db.Database.ExecuteSqlRaw(commandText, parameters);
            Save();
            return noOfRowInserted;
        }

        public async Task<Organiser> UpdateOrganiser(Organiser organiser)
        {
            if (organiser == null)
            {
                throw new NullReferenceException(nameof(organiser));
            }
            Organiser currentOrganiser = await GetOrganiserById(organiser.OrganiserId);
            _db.Entry(currentOrganiser).CurrentValues.SetValues(organiser);
            string commandText = "UPDATE Organiser SET Name = @Name, Email= @Email WHERE OrganiserId = @OrganiserId";

            List<SqlParameter> sqlParameters = GetOrganiserSqlParameters(currentOrganiser);
            _db.Database.ExecuteSqlRaw(commandText, sqlParameters);
            Save();

            Organiser updatedOrganiser = await GetOrganiserById(currentOrganiser.OrganiserId);
            return updatedOrganiser;
        }

        public int DeleteOrganiser(Guid organiserId)
        {
            if (organiserId == null)
            {
                throw new NullReferenceException(nameof(organiserId));
            }
            //Delete organiser by id and return 1(true) or 0(false)
            int noOfRowDeleted = _db.Database.ExecuteSqlInterpolated($"Delete from Organiser where OrganiserId ={organiserId}");
            Save();
            return noOfRowDeleted;
        }


       






        private async Task<Organiser> GetOrganiserById(Guid organiserId)
        {
            if (organiserId == null)
            {
                throw new NullReferenceException(nameof(organiserId));
            }
            return await _db.Organiser.FromSqlInterpolated($"Select * from Organiser where OrganiserId ={organiserId}").AsNoTracking().FirstOrDefaultAsync();
            
        }

        private void Save()
        {
            _db.SaveChangesAsync();
        }
        private async Task<ConcertOrganiser> GetConcertOrganiserById(Guid concertOrganiserId)
        {
            return await _db.ConcertOrganiser.FromSqlInterpolated($"Select * From dbo.ConcertOrganiser Where ConcertOrganiserId = {concertOrganiserId}").AsNoTracking().FirstOrDefaultAsync();
        }
        private List<SqlParameter> GetConcertOrganiserSqlParameters(ConcertOrganiser organiser)
        {
            SqlParameter concertOrganiserId = new SqlParameter("@ConcertorganiserId", organiser.ConcertOrganiserId);
            SqlParameter concertId = new SqlParameter("@ConcertId", organiser.ConcertId);
            SqlParameter organiserId = new SqlParameter("@OrganiserId", organiser.OrganiserId);
            List<SqlParameter> sqlParameters = new List<SqlParameter>
            {
                concertOrganiserId,
                concertId,
                organiserId,
                
            };
            return sqlParameters;
        }
        private List<SqlParameter> GetOrganiserSqlParameters(Organiser organiser)
        {
            SqlParameter email = new SqlParameter("@Email", organiser.Email);
            SqlParameter name = new SqlParameter("@Name", organiser.Name);
            SqlParameter organiserId = new SqlParameter("@OrganiserId", organiser.OrganiserId);
            List<SqlParameter> sqlParameters = new List<SqlParameter>
            {
                email,
                name,
                organiserId

            };
            return sqlParameters;
        }

       
    }
}
