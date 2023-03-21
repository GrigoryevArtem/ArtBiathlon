using ArtBiathlon.DAL.Interfaces;
using ArtBiathlon.DataEntity;

namespace ArtBiathlon.DAL.Repositories
{
    public class ForumMessageRepository : IBaseRepository<ForumMessage>
    {
        private readonly ApplicationDbContext _db;

        public ForumMessageRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public IQueryable<ForumMessage> GetAll()
        {
            return _db.ForumMessages;
        }

        public async Task Delete(ForumMessage entity)
        {
            _db.ForumMessages.Remove(entity);
            await _db.SaveChangesAsync();
        }

        public async Task Create(ForumMessage entity)
        {
            await _db.ForumMessages.AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<ForumMessage> Update(ForumMessage entity)
        {
            _db.ForumMessages.Update(entity);
            await _db.SaveChangesAsync();

            return entity;
        }
    }
}

