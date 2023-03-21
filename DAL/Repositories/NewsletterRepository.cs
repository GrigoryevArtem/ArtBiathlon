using ArtBiathlon.DAL.Interfaces;
using ArtBiathlon.DataEntity;

namespace ArtBiathlon.DAL.Repositories
{
    public class NewsletterRepository : IBaseRepository<Mailing>
    {
        private readonly ApplicationDbContext _db;

        public NewsletterRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public IQueryable<Mailing> GetAll()
        {
            return _db.Mailings;
        }

        public async Task Delete(Mailing entity)
        {
            _db.Mailings.Remove(entity);
            await _db.SaveChangesAsync();
        }

        public async Task Create(Mailing entity)
        {
            await _db.Mailings.AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<Mailing> Update(Mailing entity)
        {
            _db.Mailings.Update(entity);
            await _db.SaveChangesAsync();

            return entity;
        }
    }
}