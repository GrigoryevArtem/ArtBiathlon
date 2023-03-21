using ArtBiathlon.DAL.Interfaces;
using ArtBiathlon.DataEntity;
using ArtBiathlon.Enums;
using ArtBiathlon.Interfaces;
using ArtBiathlon.Response;
using ArtBiathlon.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ArtBiathlon.Services.Implementations
{
    public class MailingTopicSubscriberService : IMailingTopicSubscriberService
    {
        private readonly ILogger<MailingTopicSubscriberService> _logger;
        private readonly IBaseRepository<MailingTopicSubscriber> _mailingTopicSubscriberRepository;

        public MailingTopicSubscriberService(ILogger<MailingTopicSubscriberService> logger, IBaseRepository<MailingTopicSubscriber> mailingTopicRepository)
        {
            _logger = logger;
            _mailingTopicSubscriberRepository = mailingTopicRepository;
        }

        public async Task<IBaseResponse<MailingTopicSubscriber>> Create(MailingTopicSubscriber model)
        {
            try
            {
                var mailingTopicSubscriber = await _mailingTopicSubscriberRepository
                    .GetAll()
                    .FirstOrDefaultAsync(x => x.MailingTopicId == model.MailingTopicId && x.UserId == model.UserId);

                if (mailingTopicSubscriber is not null)
                {
                    return new BaseResponse<MailingTopicSubscriber>
                    {
                        StatusCode = StatusCode.AlreadyExists,
                        Description = "Данный пользователь уже подписан на данную рассылку"
                    };
                }

                await _mailingTopicSubscriberRepository.Create(model);

                return new BaseResponse<MailingTopicSubscriber>
                {
                    Data = model,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"[{nameof(MailingTopicSubscriberService)}.{nameof(Create)}] error: {exception.Message}");
                return new BaseResponse<MailingTopicSubscriber>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = $"Внутренняя ошибка: {exception.Message}"
                };
            }
        }

        public async Task<IBaseResponse<bool>> Delete(long id)
        {
            try
            {
                var mailingTopicSubscriber = await _mailingTopicSubscriberRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);
                if (mailingTopicSubscriber is null)
                {
                    return new BaseResponse<bool>
                    {
                        StatusCode = StatusCode.NotFound,
                        Data = false
                    };
                }

                await _mailingTopicSubscriberRepository.Delete(mailingTopicSubscriber);
                _logger.LogInformation($"[{nameof(MailingTopicSubscriberService)}.{nameof(Delete)}] подписчик добавлен");

                return new BaseResponse<bool>
                {
                    StatusCode = StatusCode.OK,
                    Data = true
                };
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"[{nameof(MailingTopicSubscriberService)}.{nameof(Delete)}] error: {exception.Message}");
                return new BaseResponse<bool>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = $"Внутренняя ошибка: {exception.Message}"
                };
            }
        }

        public async Task<IBaseResponse<IEnumerable<MailingTopicSubscriber>>> GetAll()
        {
            try
            {
                var subscribers = await _mailingTopicSubscriberRepository.GetAll().ToListAsync();
                _logger.LogInformation($"[{nameof(MailingTopicSubscriber)}.{nameof(GetAll)}] получено подписчиков {subscribers.Count}");

                return new BaseResponse<IEnumerable<MailingTopicSubscriber>>
                {
                    Data = subscribers,
                    StatusCode = StatusCode.OK,
                };
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"[{nameof(MailingTopicSubscriberService)}.{nameof(GetAll)}] error: {exception.Message}");
                return new BaseResponse<IEnumerable<MailingTopicSubscriber>>
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = $"Внутренняя ошибка: {exception.Message}"
                };
            }
        }
    }
}
