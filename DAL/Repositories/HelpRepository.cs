using ArtBiathlon.DAL.Interfaces;
using ArtBiathlon.DataEntity;

namespace ArtBiathlon.DAL.Repositories
{
    public class HelpRepository : IBaseRepository<Help>
    {
        private readonly ApplicationDbContext _db;

        public HelpRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public IQueryable<Help> GetAll()
        {
            return _db.Helps;
        }

        public async Task Delete(Help entity)
        {
            _db.Helps.Remove(entity);
            await _db.SaveChangesAsync();
        }

        public async Task Create(Help entity)
        {
            await _db.Helps.AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<Help> Update(Help entity)
        {
            _db.Helps.Update(entity);
            await _db.SaveChangesAsync();

            return entity;
        }
    }
}