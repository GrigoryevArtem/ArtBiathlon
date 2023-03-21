using ArtBiathlon.DAL.Interfaces;
using ArtBiathlon.DataEntity;

namespace ArtBiathlon.DAL.Repositories
{
    public class NewsletterTopicRepository : IBaseRepository<MailingTopic>
    {
        private readonly ApplicationDbContext _db;

        public NewsletterTopicRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public IQueryable<MailingTopic> GetAll()
        {
            return _db.MailingTopics;
        }

        public async Task Delete(MailingTopic entity)
        {
            _db.MailingTopics.Remove(entity);
            await _db.SaveChangesAsync();
        }

        public async Task Create(MailingTopic entity)
        {
            await _db.MailingTopics.AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<MailingTopic> Update(MailingTopic entity)
        {
            _db.MailingTopics.Update(entity);
            await _db.SaveChangesAsync();

            return entity;
        }
    }
}

