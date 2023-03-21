using Microsoft.EntityFrameworkCore;
using ArtBiathlon.DAL.Interfaces;
using ArtBiathlon.DAL.Repositories;
using ArtBiathlon.DataEntity;
using ArtBiathlon.Enums;
using ArtBiathlon.Interfaces;
using ArtBiathlon.Response;
using ArtBiathlon.Services.Interfaces;
using ArtBiathlon.Services.Implementations;

namespace ArtBiathlon.Services.Implementations
{
    public class HelpService : IHelpService
    {
        private readonly ILogger<HelpService> _logger;
        private readonly IBaseRepository<Help> _helpRepository;

        public HelpService(ILogger<HelpService> logger, IBaseRepository<Help> helpRepository)
        {
            _logger = logger;
            _helpRepository = helpRepository;
        }

        public async Task<IBaseResponse<Help>> Create(Help model)
        {
            try
            {
                await _helpRepository.Create(model);

                return new BaseResponse<Help>
                {
                    Data = model,
                    StatusCode = StatusCode.OK,
                };
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"[{nameof(HelpService)}.{nameof(Create)}] error: {exception.Message}");
                return new BaseResponse<Help>()
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
                var help = await _helpRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);
                if (help is null)
                {
                    return new BaseResponse<bool>
                    {
                        StatusCode = StatusCode.NotFound,
                        Data = false
                    };
                }

                await _helpRepository.Delete(help);
                _logger.LogInformation($"[{nameof(HelpService)}.{nameof(Delete)}] вопрос удален");

                return new BaseResponse<bool>
                {
                    StatusCode = StatusCode.OK,
                    Data = true
                };
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"[{nameof(HelpService)}.{nameof(Delete)}] error: {exception.Message}");
                return new BaseResponse<bool>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = $"Внутренняя ошибка: {exception.Message}"
                };
            }
        }

        public async Task<IBaseResponse<IEnumerable<Help>>> GetAll()
        {
            try
            {
                var helps = await _helpRepository.GetAll().ToListAsync();
                _logger.LogInformation($"[{nameof(HelpService)}.{nameof(GetAll)}] получено вопросов {helps.Count}");

                return new BaseResponse<IEnumerable<Help>>
                {
                    Data = helps,
                    StatusCode = StatusCode.OK,
                };
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"[{nameof(HelpService)}.{nameof(GetAll)}] error: {exception.Message}");
                return new BaseResponse<IEnumerable<Help>>
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = $"Внутренняя ошибка: {exception.Message}"
                };
            }
        }
    }
}
