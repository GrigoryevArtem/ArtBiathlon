using ArtBiathlon.DAL.Interfaces;
using ArtBiathlon.DataEntity;
using ArtBiathlon.Enums;
using ArtBiathlon.Interfaces;
using ArtBiathlon.Response;
using ArtBiathlon.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ArtBiathlon.Services.Implementations;

public class TrainingService: ITrainingService
{
    private readonly ILogger<TrainingService> _logger;
    private readonly IBaseRepository<Training> _trainingRepository;

    public TrainingService(ILogger<TrainingService> logger, IBaseRepository<Training> trainingRepository)
    {
        _logger = logger;
        _trainingRepository = trainingRepository;        
    }

    public async Task<IBaseResponse<Training>> Create(Training model)
    {
        try
        {
            await _trainingRepository.Create(model);

            return new BaseResponse<Training>()
            {
                Data = model,
                Description = "Новый тип тренировки добавлен",
                StatusCode = StatusCode.OK
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"[{nameof(TrainingService)}.{nameof(Create)}] error: {ex.Message}");
            return new BaseResponse<Training>()
            {
                StatusCode = StatusCode.InternalServerError,
                Description = $"Внутренняя ошибка: {ex.Message}"
            };
        }
    }

    public async Task<IBaseResponse<IEnumerable<Training>>> GetAll()
    {
        try
        {
            var trainings = await _trainingRepository.GetAll().ToListAsync();

            _logger.LogInformation($"[{nameof(TrainingService)}.{nameof(GetAll)}] получено элементов {trainings.Count}");
            return new BaseResponse<IEnumerable<Training>>()
            {
                Data = trainings,
                StatusCode = StatusCode.OK
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"[TrainingService.GetAll] error: {ex.Message}");
            return new BaseResponse<IEnumerable<Training>>()
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
            var training = await _trainingRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);
            if (training is null)
            {
                return new BaseResponse<bool>
                {
                    StatusCode = StatusCode.NotFound,
                    Data = false
                };
            }

            await _trainingRepository.Delete(training);
            _logger.LogInformation($"[UserService.DeleteUser] пользователь удален");

            return new BaseResponse<bool>
            {
                StatusCode = StatusCode.OK,
                Data = true
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"[TrainingService.DeleteTraining] error: {ex.Message}");
            return new BaseResponse<bool>
            {
                StatusCode = StatusCode.InternalServerError,
                Description = $"Внутренняя ошибка: {ex.Message}"
            };
        }
    }
}