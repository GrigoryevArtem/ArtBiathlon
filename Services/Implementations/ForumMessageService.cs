using Microsoft.EntityFrameworkCore;
using ArtBiathlon.DAL.Interfaces;

using ArtBiathlon.Enums;
using ArtBiathlon.Interfaces;
using ArtBiathlon.Response;
using ArtBiathlon.Services.Interfaces;
using ArtBiathlon.DataEntity;

namespace WebRunApplication.Services.Implementations
{
    public class ForumMessageService : IForumMessageService
    {
        private readonly ILogger<ForumMessageService> _logger;
        private readonly IBaseRepository<ForumMessage> _forumMessageRepository;

        public ForumMessageService(ILogger<ForumMessageService> logger, IBaseRepository<ForumMessage> forumMessageRepository)
        {
            _logger = logger;
            _forumMessageRepository = forumMessageRepository;
        }

        public async Task<IBaseResponse<ForumMessage>> Create(ForumMessage model)
        {
            try
            {
                await _forumMessageRepository.Create(model);

                return new BaseResponse<ForumMessage>
                {
                    Data = model,
                    StatusCode = StatusCode.OK,
                };
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"[{nameof(ForumMessageService)}.{nameof(Create)}] error: {exception.Message}");
                return new BaseResponse<ForumMessage>()
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
                var message = await _forumMessageRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);
                if (message is null)
                {
                    return new BaseResponse<bool>
                    {
                        StatusCode = StatusCode.NotFound,
                        Data = false
                    };
                }

                await _forumMessageRepository.Delete(message);
                _logger.LogInformation($"[{nameof(ForumMessageService)}.{nameof(Delete)}] сообщение удалено");

                return new BaseResponse<bool>
                {
                    StatusCode = StatusCode.OK,
                    Data = true
                };
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"[{nameof(ForumMessageService)}.{nameof(Delete)}] error: {exception.Message}");
                return new BaseResponse<bool>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = $"Внутренняя ошибка: {exception.Message}"
                };
            }
        }

        public async Task<IBaseResponse<IEnumerable<ForumMessage>>> GetAll()
        {
            try
            {
                var messages = await _forumMessageRepository.GetAll().ToListAsync();
                _logger.LogInformation($"[{nameof(ForumMessageService)}.{nameof(GetAll)}] получено сообщений {messages.Count}");

                return new BaseResponse<IEnumerable<ForumMessage>>
                {
                    Data = messages,
                    StatusCode = StatusCode.OK,
                };
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"[{nameof(ForumMessageService)}.{nameof(GetAll)}] error: {exception.Message}");
                return new BaseResponse<IEnumerable<ForumMessage>>
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = $"Внутренняя ошибка: {exception.Message}"
                };
            }
        }
    }
}
