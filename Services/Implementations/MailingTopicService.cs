using ArtBiathlon.DAL.Interfaces;
using ArtBiathlon.DataEntity;
using ArtBiathlon.Enums;
using ArtBiathlon.Interfaces;
using ArtBiathlon.Response;
using ArtBiathlon.Services.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace ArtBiathlon.Services.Implementations
{
    public class MailingTopicService : IMailingTopicService
    {
        private readonly ILogger<MailingTopicService> _logger;
        private readonly IBaseRepository<MailingTopic> _mailingTopicRepository;

        public MailingTopicService(ILogger<MailingTopicService> logger, IBaseRepository<MailingTopic> mailingTopicRepository)
        {
            _logger = logger;
            _mailingTopicRepository = mailingTopicRepository;
        }

        public async Task<IBaseResponse<MailingTopic>> Create(MailingTopic model)
        {
            try
            {
                var mailingTopic = await _mailingTopicRepository.GetAll().FirstOrDefaultAsync(x => x.Title == model.Title);
                if (mailingTopic is not null)
                {
                    return new BaseResponse<MailingTopic>
                    {
                        StatusCode = StatusCode.AlreadyExists,
                        Description = "Рассылка с такой темой уже есть"
                    };
                }

                await _mailingTopicRepository.Create(model);

                return new BaseResponse<MailingTopic>
                {
                    StatusCode = StatusCode.OK,
                    Data = model
                };
            }
            catch(Exception exception)
            {
                _logger.LogError(exception, $"[{nameof(MailingTopicService)}.{nameof(Create)}] error: {exception.Message}");
                return new BaseResponse<MailingTopic>()
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
                var mailingTopic = await _mailingTopicRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);
                if (mailingTopic is null)
                {
                    return new BaseResponse<bool>
                    {
                        StatusCode = StatusCode.NotFound,
                        Data = false
                    };
                }

                await _mailingTopicRepository.Delete(mailingTopic);
                _logger.LogInformation($"[{nameof(MailingTopicService)}.{nameof(Delete)}] тема рассылки удалена");

                return new BaseResponse<bool>
                {
                    StatusCode = StatusCode.OK,
                    Data = true
                };
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"[{nameof(MailingTopicService)}.{nameof(Delete)}] error: {exception.Message}");
                return new BaseResponse<bool>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = $"Внутренняя ошибка: {exception.Message}"
                };
            }
        }

        public async Task<IBaseResponse<IEnumerable<MailingTopic>>> GetAll()
        {
            try
            {
                var mailingTopics = await _mailingTopicRepository.GetAll().ToListAsync();
                _logger.LogInformation($"[{nameof(MailingTopicService)}.{nameof(GetAll)}] получено тем рассылок {mailingTopics.Count}");

                return new BaseResponse<IEnumerable<MailingTopic>>
                {
                    Data = mailingTopics,
                    StatusCode = StatusCode.OK,
                };
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"[{nameof(MailingTopicService)}.{nameof(GetAll)}] error: {exception.Message}");
                return new BaseResponse<IEnumerable<MailingTopic>>
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = $"Внутренняя ошибка: {exception.Message}"
                };
            }
        }

        public async Task<IBaseResponse<bool>> Update(MailingTopic model)
        {
            try
            {
                var mailingTopic = await _mailingTopicRepository.GetAll().FirstOrDefaultAsync(x => x.Title == model.Title);
                if (mailingTopic is null)
                {
                    return new BaseResponse<bool>
                    {
                        StatusCode = StatusCode.NotFound,
                        Data = false
                    };
                }

                await _mailingTopicRepository.Update(model);

                return new BaseResponse<bool>
                {
                    Data = true,
                    StatusCode = StatusCode.OK
                };
            }
            catch(Exception exception)
            {
                _logger.LogError(exception, $"[{nameof(MailingTopicService)}.{nameof(Update)}] error: {exception.Message}");
                return new BaseResponse<bool>
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = $"Внутренняя ошибка: {exception.Message}"
                };
            }
        }
    }
}
