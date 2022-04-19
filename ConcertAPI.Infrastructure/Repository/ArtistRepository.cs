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
    public class ArtistRepository : IArtist
    {
        readonly DbService _db;
        public ArtistRepository(DbService _db)
        {
            this._db = _db;
        }
        //Concert Artist
        public IEnumerable<ConcertArtist> GetConcertArtists
        {
            get
            {
                return _db.ConcertArtists.FromSqlRaw("Select * From dbo.ConcertArtists").OrderBy(s => s.ConcertId).Include(s=>s.Concert).Include(s=>s.Artist);
            }
        }
        public IEnumerable<ConcertArtist> GetArtistByName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new NullReferenceException(nameof(name));
            }
            Guid artistId = _db.Artists.FromSqlInterpolated($"Select * From dbo.Artists Where Name = {name}").Select(s => s.ArtistId).FirstOrDefault();

            return _db.ConcertArtists.FromSqlInterpolated($"Select * From dbo.ConcertArtists Where ArtistId = {artistId}").Include(s=>s.Artist).Include(s=>s.Concert);
        }
        public int AddConcertArtist(ConcertArtist artist)
        {
            if(artist == null)
            {
                throw new NullReferenceException(nameof(artist));
            }
            artist.ConcertArtistId = Guid.NewGuid();
            //Raw sql command to insert ConcertArtist into the table
            string commandText = "INSERT ConcertArtists (ConcertArtistId,ConcertId,ArtistId) VALUES (@ConcertArtistId,@ConcertId,@ArtistId)";

            List<SqlParameter> sqlParameters = GetConcertArtistSqlParameters(artist);
            IEnumerable<object> parameters = sqlParameters;

            int noOfRowInserted = _db.Database.ExecuteSqlRaw(commandText, parameters);
            Save();
            return noOfRowInserted;
        }
       
        public async Task<ConcertArtist> UpdateConcertArtist(ConcertArtist artist)
        {

            if (artist == null)
            {
                throw new NullReferenceException(nameof(artist));
            }
            ConcertArtist currentConcert = await GetConcertArtistById(artist.ConcertArtistId);
            if (currentConcert == null)
            {
                throw new NullReferenceException(nameof(currentConcert));
            }
            //copies updated properties from artist to currentConcert
            _db.Entry(currentConcert).CurrentValues.SetValues(artist);
            string commandText = "UPDATE ConcertArtists SET ArtistId = @ArtistId,ConcertId = @ConcertId WHERE @ConcertArtistId = ConcertArtistId";

            List<SqlParameter> sqlParameters = GetConcertArtistSqlParameters(currentConcert);
            _db.Database.ExecuteSqlRaw(commandText, sqlParameters);
            Save();

            ConcertArtist concertArtist = await GetConcertArtistById(currentConcert.ConcertArtistId);
            return concertArtist;
        }


        public int DeleteConcertArtist( Guid concertArtistId)
        {
           if(concertArtistId == null)
           {
                throw new NullReferenceException(nameof(concertArtistId));
           }
            //Delete concertArtist by id and return 1(true) or 0(false)
            int noOfRowDeleted = _db.Database.ExecuteSqlInterpolated($"Delete from ConcertArtists where ConcertArtistId ={concertArtistId}");
            Save();
            return noOfRowDeleted;
        }




        public int AddArtist(Artist artist)
        {
            if (artist == null)
            {
                throw new NullReferenceException(nameof(artist));
            }
            artist.ArtistId = Guid.NewGuid();
            //Raw sql command to insert Artist into the table
            string commandText = "INSERT Artists (Bio,Name,ArtistId) VALUES (@Bio,@Name,@ArtistId)";

            List<SqlParameter> sqlParameters = GetArtistSqlParameters(artist);
            IEnumerable<object> parameters = sqlParameters;

            int noOfRowInserted = _db.Database.ExecuteSqlRaw(commandText, parameters);
            Save();
            return noOfRowInserted;
        }

        public async Task<Artist> UpdateArtist(Artist artist)
        {
            if(artist == null)
            {
                throw new NullReferenceException(nameof(artist));
            }
            Artist currentArtist = await GetArtistById(artist.ArtistId);
            if(currentArtist == null)
            {
                throw new NullReferenceException(nameof(currentArtist));
            }
            //copies updated properties from artist to currentArtist
            _db.Entry(currentArtist).CurrentValues.SetValues(artist);
            string commandText = "UPDATE Artists SET Name = @Name,Bio= @Bio WHERE ArtistId = @ArtistId";

            List<SqlParameter> sqlParameters = GetArtistSqlParameters(currentArtist);
            _db.Database.ExecuteSqlRaw(commandText, sqlParameters);
            Save();

            Artist updatedArtist = await GetArtistById(currentArtist.ArtistId);
            return updatedArtist;
        }

        public int DeleteArtist(Guid artistId)
        {
            if(artistId == null)
            {
                throw new NullReferenceException(nameof(artistId));
            }
            //Delete Artist by artistid and return 1(true) or 0(false)
            int noOfRowDeleted = _db.Database.ExecuteSqlInterpolated($"Delete from Artists where ArtistId ={artistId}");
            Save();
            return noOfRowDeleted;
        }
        //search by artist name
        public IEnumerable<Artist> GetArtist(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new NullReferenceException(name);
            }
            return  _db.Artists.FromSqlRaw("Select * From dbo.Artists").Where(s => s.Name.Contains(name)).OrderBy(s => s.ArtistId).AsNoTracking();
        }

        private async Task<Artist> GetArtistById(Guid artistId)
        {
            return await _db.Artists.FromSqlInterpolated($"Select * From dbo.Artists Where ArtistId = {artistId}").AsNoTracking().FirstOrDefaultAsync();

        }


        private async Task<ConcertArtist> GetConcertArtistById(Guid concertArtistId)
        {
            return await _db.ConcertArtists.FromSqlInterpolated($"Select * From dbo.ConcertArtists Where ConcertArtistId = {concertArtistId}").FirstOrDefaultAsync();
        }
        private void Save()
        {
            _db.SaveChangesAsync();
        }
        private List<SqlParameter> GetConcertArtistSqlParameters(ConcertArtist artist)
         {
            SqlParameter artistId = new SqlParameter("@ArtistId", artist.ArtistId);
            SqlParameter concertId = new SqlParameter("@ConcertId", artist.ConcertId);
            SqlParameter concertArtistId = new SqlParameter("@ConcertArtistId", artist.ConcertArtistId);
            List<SqlParameter> sqlParameters = new List<SqlParameter>
            {
                artistId,
                concertId,
                concertArtistId,
            };
            return sqlParameters;
         }
        private List<SqlParameter> GetArtistSqlParameters(Artist artist)
        {
            SqlParameter artistId = new SqlParameter("@ArtistId", artist.ArtistId);
            SqlParameter bio = new SqlParameter("@Bio", artist.Bio);
            SqlParameter name= new SqlParameter("@Name", artist.Name);
            List<SqlParameter> sqlParameters = new List<SqlParameter>
            {
                artistId,
                bio,
                name,
            };
            return sqlParameters;
        }

        
    }
}
