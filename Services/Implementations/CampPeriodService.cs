using ArtBiathlon.DAL.Interfaces;
using ArtBiathlon.DataEntity;
using ArtBiathlon.Enums;
using ArtBiathlon.Interfaces;
using ArtBiathlon.Response;
using ArtBiathlon.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ArtBiathlon.Services.Implementations;

public class CampPeriodService : ICampPeriodService
{
    private readonly ILogger<CampPeriodService> _logger;
    private readonly IBaseRepository<CampPeriod> _campPeriodRepository;

    public CampPeriodService(ILogger<CampPeriodService> logger, IBaseRepository<CampPeriod> campPeriodRepository)
    {
        _logger = logger;
        _campPeriodRepository = campPeriodRepository;
    }

    public async Task<IBaseResponse<CampPeriod>> Create(CampPeriod model)
    {
        try
        {
           /* var user = await _userRepository.GetAll().FirstOrDefaultAsync(x => x.Login == model.Login);
            if (user is not null)
            {
                return new BaseResponse<User>()
                {
                    Description = "Пользователь с таким логином уже есть",
                    StatusCode = StatusCode.AlreadyExists
                };
            }*/

            await _campPeriodRepository.Create(model);

            return new BaseResponse<CampPeriod>()
            {
                Data = model,
                Description = "Тренировочный сбор добавлен",
                StatusCode = StatusCode.OK
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"[{nameof(CampPeriodService)}.{nameof(Create)}] error: {ex.Message}");
            return new BaseResponse<CampPeriod>()
            {
                StatusCode = StatusCode.InternalServerError,
                Description = $"Внутренняя ошибка: {ex.Message}"
            };
        }
    }

    public async Task<IBaseResponse<IEnumerable<CampPeriod>>> GetAll()
    {
        try
        {
            var campPeriods = await _campPeriodRepository.GetAll().ToListAsync();

            _logger.LogInformation($"[{nameof(CampPeriodService)}.{nameof(GetAll)}] получено элементов {campPeriods.Count}");
            return new BaseResponse<IEnumerable<CampPeriod>>()
            {
                Data = campPeriods,
                StatusCode = StatusCode.OK
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"[CampPeriodService.GetAll] error: {ex.Message}");
            return new BaseResponse<IEnumerable<CampPeriod>>()
            {
                StatusCode = StatusCode.InternalServerError,
                Description = $"Внутренняя ошибка: {ex.Message}"
            };
        }
    }

    public async Task<IBaseResponse<bool>> Delete(long id)
    {
        try
        {
            var campPeriod = await _campPeriodRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);
            if (campPeriod is null)
            {
                return new BaseResponse<bool>
                {
                    StatusCode = StatusCode.NotFound,
                    Data = false
                };
            }

            await _campPeriodRepository.Delete(campPeriod);
            _logger.LogInformation($"[CampPeriodService.DeleteCampPeriod] тренировочный сбор удален");

            return new BaseResponse<bool>
            {
                StatusCode = StatusCode.OK,
                Data = true
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"[CampPeriodService.DeleteCampPeriod] error: {ex.Message}");
            return new BaseResponse<bool>
            {
                StatusCode = StatusCode.InternalServerError,
                Description = $"Внутренняя ошибка: {ex.Message}"
            };
        }
    }
}