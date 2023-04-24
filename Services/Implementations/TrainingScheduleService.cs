using ArtBiathlon.DAL.Interfaces;
using ArtBiathlon.DataEntity;
using ArtBiathlon.Enums;
using ArtBiathlon.Interfaces;
using ArtBiathlon.Response;
using ArtBiathlon.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ArtBiathlon.Services.Implementations;

public class TrainingScheduleService: ITrainingSchedule
{
    private readonly ILogger<TrainingSchedule> _logger;
    private readonly IBaseRepository<TrainingSchedule> _trainingScheduleRepository;

    public TrainingScheduleService(ILogger<TrainingSchedule> logger, IBaseRepository<TrainingSchedule> trainingScheduleRepository)
    {
        _logger = logger;
        _trainingScheduleRepository = trainingScheduleRepository;        
    }

    public async Task<IBaseResponse<TrainingSchedule>> Create(TrainingSchedule model)
    {
        try
        {
            await _trainingScheduleRepository.Create(model);

            return new BaseResponse<TrainingSchedule>()
            {
                Data = model,
                Description = "Новая тренировка добавлена",
                StatusCode = StatusCode.OK
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"[{nameof(TrainingScheduleService)}.{nameof(Create)}] error: {ex.Message}");
            return new BaseResponse<TrainingSchedule>()
            {
                StatusCode = StatusCode.InternalServerError,
                Description = $"Внутренняя ошибка: {ex.Message}"
            };
        }
    }

    public async Task<IBaseResponse<IEnumerable<TrainingSchedule>>> GetAll()
    {
        try
        {
            var trainings = await _trainingScheduleRepository.GetAll().ToListAsync();

            _logger.LogInformation($"[{nameof(TrainingScheduleService)}.{nameof(GetAll)}] получено элементов {trainings.Count}");
            return new BaseResponse<IEnumerable<TrainingSchedule>>()
            {
                Data = trainings,
                StatusCode = StatusCode.OK
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"[TrainingScheduleService.GetAll] error: {ex.Message}");
            return new BaseResponse<IEnumerable<TrainingSchedule>>()
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
            var training = await _trainingScheduleRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);
            if (training is null)
            {
                return new BaseResponse<bool>
                {
                    StatusCode = StatusCode.NotFound,
                    Data = false
                };
            }

            await _trainingScheduleRepository.Delete(training);
            _logger.LogInformation($"[TrainingScheduleService.DeleteUser] тренировка удалена");

            return new BaseResponse<bool>
            {
                StatusCode = StatusCode.OK,
                Data = true
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"[TrainingScheduleService.DeleteTraining] error: {ex.Message}");
            return new BaseResponse<bool>
            {
                StatusCode = StatusCode.InternalServerError,
                Description = $"Внутренняя ошибка: {ex.Message}"
            };
        }
    }  
}