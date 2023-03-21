using ArtBiathlon.DataEntity;
using ArtBiathlon.Interfaces;
using ArtBiathlon.Models;

namespace ArtBiathlon.Services.Interfaces
{
    public interface IPersonalAccountService
    {
        Task<IBaseResponse<List<MailingTopic>>> GetMailingTopics();

        Task<IBaseResponse<bool>> CreateSubscribe(string login, int[] titles);

        //Task<IBaseResponse<List<TrainingInformation>>> GetTrainings(string login);
    }
}
