using ArtBiathlon.DataEntity;
using ArtBiathlon.Interfaces;

namespace ArtBiathlon.Services.Interfaces
{
    public interface IMailingTopicService : IBaseService<MailingTopic>
    {
        Task<IBaseResponse<bool>> Update(MailingTopic model);
    }
}
