using ArtBiathlon.Interfaces;

namespace ArtBiathlon.Services.Interfaces
{
    public interface IMailSenderService
    {
        Task<IBaseResponse<bool>> SendMessage(uint userId, string emailTo, string message, string topic);
    }
}
