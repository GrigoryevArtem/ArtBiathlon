using Microsoft.EntityFrameworkCore;
using ArtBiathlon.DAL.Interfaces;
using ArtBiathlon.Enums;
using ArtBiathlon.Interfaces;
using ArtBiathlon.Response;
using ArtBiathlon.Services.Interfaces;
using ArtBiathlon.DataEntity;

namespace WebRunApplication.Services.Implementations
{
    public class ForumReactionService : IForumReactionService
    {
        private readonly ILogger<ForumReactionService> _logger;
        private readonly IBaseRepository<ForumReaction> _forumReactionRepository;

        public ForumReactionService(ILogger<ForumReactionService> logger, IBaseRepository<ForumReaction> forumReactionRepository)
        {
            _logger = logger;
            _forumReactionRepository = forumReactionRepository;
        }

        public async Task<IBaseResponse<ForumReaction>> Create(ForumReaction model)
        {
            try
            {
                await _forumReactionRepository.Create(model);

                return new BaseResponse<ForumReaction>
                {
                    Data = model,
                    StatusCode = StatusCode.OK,
                };
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"[{nameof(ForumReactionService)}.{nameof(Create)}] error: {exception.Message}");
                return new BaseResponse<ForumReaction>()
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
                var reaction = await _forumReactionRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);
                if (reaction is null)
                {
                    return new BaseResponse<bool>
                    {
                        StatusCode = StatusCode.NotFound,
                        Data = false
                    };
                }

                await _forumReactionRepository.Delete(reaction);
                _logger.LogInformation($"[{nameof(ForumReactionService)}.{nameof(Delete)}] реакция удалена");

                return new BaseResponse<bool>
                {
                    StatusCode = StatusCode.OK,
                    Data = true
                };
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"[{nameof(ForumReactionService)}.{nameof(Delete)}] error: {exception.Message}");
                return new BaseResponse<bool>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = $"Внутренняя ошибка: {exception.Message}"
                };
            }
        }

        public async Task<IBaseResponse<IEnumerable<ForumReaction>>> GetAll()
        {
            try
            {
                var reactions = await _forumReactionRepository.GetAll().ToListAsync();
                _logger.LogInformation($"[{nameof(ForumReactionService)}.{nameof(GetAll)}] получено реакций {reactions.Count}");

                return new BaseResponse<IEnumerable<ForumReaction>>
                {
                    Data = reactions,
                    StatusCode = StatusCode.OK,
                };
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"[{nameof(ForumReactionService)}.{nameof(GetAll)}] error: {exception.Message}");
                return new BaseResponse<IEnumerable<ForumReaction>>
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = $"Внутренняя ошибка: {exception.Message}"
                };
            }
        }
    }
}
