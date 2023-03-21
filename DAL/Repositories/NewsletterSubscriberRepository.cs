using ArtBiathlon.DAL.Interfaces;
using ArtBiathlon.DataEntity;

namespace ArtBiathlon.DAL.Repositories
{
    public class NewsletterSubscriberRepository : IBaseRepository<MailingTopicSubscriber>
    {
        private readonly ApplicationDbContext _db;

        public NewsletterSubscriberRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public IQueryable<MailingTopicSubscriber> GetAll()
        {
            return _db.MailingTopicSubscribers;
        }

        public async Task Delete(MailingTopicSubscriber entity)
        {
            _db.MailingTopicSubscribers.Remove(entity);
            await _db.SaveChangesAsync();
        }

        public async Task Create(MailingTopicSubscriber entity)
        {
            await _db.MailingTopicSubscribers.AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<MailingTopicSubscriber> Update(MailingTopicSubscriber entity)
        {
            _db.MailingTopicSubscribers.Update(entity);
            await _db.SaveChangesAsync();

            return entity;
        }
    }
}