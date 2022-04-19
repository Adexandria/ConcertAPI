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
    public class SponsorRepository :ISponsor
    {
        readonly DbService _db;
        readonly ConcurrencyService concurrency;
        public SponsorRepository(DbService _db,ConcurrencyService concurrency)
        {
            this._db = _db;
            this.concurrency = concurrency;
        }


        //ConcertSponsor
        public IEnumerable<ConcertSponsor> GetConcertSponsors
        {
            get
            {
                return _db.ConcertSponsors.FromSqlRaw("SELECT * FROM dbo.ConcertSponsors").Include(s=>s.Concert).Include(s=>s.Sponsor).AsNoTracking()
                    .OrderBy(s=>s.ConcertSponsorId);
            }
        }

        public IEnumerable<ConcertSponsor> GetSponsorByName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new NullReferenceException(nameof(name));
            }
            Guid sponsorId = _db.Sponsors.FromSqlInterpolated($"Select * From dbo.Sponsors Where Name = {name}").Select(s => s.SponsorId).FirstOrDefault();
            return _db.ConcertSponsors.FromSqlInterpolated($"SELECT * FROM dbo.ConcertSponsors SponsorId = {sponsorId}").Include(s => s.Concert).Include(s => s.Sponsor).AsNoTracking()
                    .OrderBy(s => s.ConcertSponsorId);

        }

        public int AddConcertSponsor(ConcertSponsor sponsor)
        {
            if(sponsor == null)
            {
                throw new NullReferenceException(nameof(sponsor));
            }
            sponsor.ConcertSponsorId = Guid.NewGuid();
            //Raw sql command to insert concertSponsor into the table
            string commandText = "INSERT ConcertSponsors (ConcertSponsorId,ConcertId,SponsorId) VALUES (@ConcertSponsorId,@ConcertId,@SponsorId)";

            List<SqlParameter> sqlParameters = GetConcertSponsorSqlParameters(sponsor);
            IEnumerable<object> parameters = sqlParameters;

            int noOfRowInserted = _db.Database.ExecuteSqlRaw(commandText, parameters);
            Save();
            return noOfRowInserted;
        }
     
        public async Task<ConcertSponsor> UpdateConcertSponsor(ConcertSponsor sponsor)
        {
            if(sponsor == null)
            {
                throw new NullReferenceException(nameof(sponsor));
            }
            ConcertSponsor currentSponsor = await GetConcertSponsorById(sponsor.ConcertSponsorId);
            //copies updated properties from sponsor to currentSponsor
            _db.Entry(currentSponsor).CurrentValues.SetValues(sponsor);
            string commandText = "UPDATE ConcertSponsors SET SponsorId = @SponsorId,ConcertId = @ConcertId WHERE @ConcertSponsorId = ConcertSponsorId";

            List<SqlParameter> sqlParameters = GetConcertSponsorSqlParameters(currentSponsor);
            _db.Database.ExecuteSqlRaw(commandText, sqlParameters);
            concurrency.DbConcurreny<ConcertSponsor>();

            ConcertSponsor concertSponsor = await GetConcertSponsorById(currentSponsor.ConcertSponsorId);
            return concertSponsor;
        }

        public int DeleteConcertSponsor(Guid id)
        {
            if (id == null)
            {
                throw new NullReferenceException(nameof(id));
            }
            int noOfRowDeleted = _db.Database.ExecuteSqlInterpolated($"Delete from ConcertSponsors where ConcertSponsorId ={id}");
            Save();
            return noOfRowDeleted;
        }



        //Sponsors
        public async Task<Sponsor> GetSponsor(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new NullReferenceException(name);
            }
            return await _db.Sponsors.FromSqlRaw("Select * From dbo.Sponsors").Where(s => s.Name.Contains(name)).OrderBy(s => s.SponsorId).FirstOrDefaultAsync();
        }

        public int AddSponsor(Sponsor sponsor)
        {
            if (sponsor == null)
            {
                throw new NullReferenceException(nameof(sponsor));
            }
            sponsor.SponsorId = Guid.NewGuid();
            //Raw sql command to insert sponsor into the table
            string commandText = "INSERT Sponsors (Name,SponsorLevel,SponsorId) VALUES (@Name,@SponsorLevel,@SponsorId)";

            List<SqlParameter> sqlParameters = GetSponsorSqlParameters(sponsor);
            IEnumerable<object> parameters = sqlParameters;

            int noOfRowInserted = _db.Database.ExecuteSqlRaw(commandText, parameters);
            Save();
            return noOfRowInserted;
        }

        public async Task<Sponsor> UpdateSponsor(Sponsor sponsor)
        {
            if (sponsor == null)
            {
                throw new NullReferenceException(nameof(sponsor));
            }
            Sponsor currentSponsor = await GetSponsor(sponsor.SponsorId);
            //copies updated properties from sponsor to currentSponsor
            _db.Entry(currentSponsor).CurrentValues.SetValues(sponsor);
            string commandText = "UPDATE Sponsors SET Name = @Name , SponsorLevel = @SponsorLevel Where SponsorId = @SponsorId";

            List<SqlParameter> sqlParameters = GetSponsorSqlParameters(currentSponsor);
            _db.Database.ExecuteSqlRaw(commandText, sqlParameters);
            concurrency.DbConcurreny<Sponsor>();
            Sponsor concertSponsor = await GetSponsor(currentSponsor.SponsorId);
            return concertSponsor;
        }

        public int DeleteSponsor(Guid id)
        {
            if (id == null)
            {
                throw new NullReferenceException(nameof(id));
            }
            int noOfRowDeleted = _db.Database.ExecuteSqlInterpolated($"Delete from Sponsors where SponsorId ={id}");
            Save();
            return noOfRowDeleted;
        }






        private async Task<Sponsor> GetSponsor(Guid id)
        {
            if (id == null)
            {
                throw new NullReferenceException(nameof(id));
            }
            return await _db.Sponsors.FromSqlInterpolated($"Select * From Sponsors Where SponsorId = {id}").AsNoTracking().FirstOrDefaultAsync();
        }
        private void Save()
        {
            _db.SaveChangesAsync();
        }


        private async Task<ConcertSponsor> GetConcertSponsorById(Guid sponsorId)
        {
            if(sponsorId == null)
            {
                throw new NullReferenceException(nameof(sponsorId));
            }
            return await _db.ConcertSponsors.FromSqlInterpolated($"Select * From dbo.ConcertSponsors Where ConcertSponsorId = {sponsorId}").AsNoTracking().FirstOrDefaultAsync();
        }
        private List<SqlParameter> GetConcertSponsorSqlParameters(ConcertSponsor sponsor)
        {
            SqlParameter concertSponsorId = new SqlParameter("@ConcertSponsorId", sponsor.ConcertSponsorId);
            SqlParameter concertId= new SqlParameter("@ConcertId", sponsor.ConcertId);
            SqlParameter sponsorId = new SqlParameter("@SponsorId", sponsor.SponsorId);
            List<SqlParameter> sqlParameters = new List<SqlParameter>
            {
                concertSponsorId,
                concertId,
                sponsorId            
            };
            return sqlParameters;
        }
        private List<SqlParameter> GetSponsorSqlParameters(Sponsor sponsor)
        {
            SqlParameter level = new SqlParameter("@SponsorLevel", (int)sponsor.SponsorLevel);
            SqlParameter name = new SqlParameter("@Name", sponsor.Name);
            SqlParameter sponsorId = new SqlParameter("@SponsorId", sponsor.SponsorId);
            List<SqlParameter> sqlParameters = new List<SqlParameter>
            {
                level,
                name,
                sponsorId
            };
            return sqlParameters;
        }


    }
}
