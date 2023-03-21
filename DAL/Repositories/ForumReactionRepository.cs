using ArtBiathlon.DAL.Interfaces;
using ArtBiathlon.DataEntity;

namespace ArtBiathlon.DAL.Repositories
{
    public class ForumReactionRepository: IBaseRepository<ForumReaction>
    {
        private readonly ApplicationDbContext _db;

        public ForumReactionRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public IQueryable<ForumReaction> GetAll()
        {
            return _db.ForumReactions;
        }

        public async Task Delete(ForumReaction entity)
        {
            _db.ForumReactions.Remove(entity);
            await _db.SaveChangesAsync();
        }

        public async Task Create(ForumReaction entity)
        {
            await _db.ForumReactions.AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<ForumReaction> Update(ForumReaction entity)
        {
            _db.ForumReactions.Update(entity);
            await _db.SaveChangesAsync();

            return entity;
        }
    }
}
