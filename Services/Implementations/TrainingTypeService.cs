using ArtBiathlon.DAL.Interfaces;
using ArtBiathlon.DataEntity;
using ArtBiathlon.Enums;
using ArtBiathlon.Interfaces;
using ArtBiathlon.Response;
using ArtBiathlon.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ArtBiathlon.Services.Implementations;

public class TrainingTypeService: ITrainingTypeService
{
    private readonly ILogger<TrainingTypeService> _logger;
    private readonly IBaseRepository<TrainingType> _trainingTypeRepository;

    public TrainingTypeService(ILogger<TrainingTypeService> logger, IBaseRepository<TrainingType> trainingTypeRepository)
    {
        _logger = logger;
        _trainingTypeRepository = trainingTypeRepository;        
    }

    public async Task<IBaseResponse<TrainingType>> Create(TrainingType model)
    {
        try
        {
          /*  var user = await _userRepository.GetAll().FirstOrDefaultAsync(x => x.Login == model.Login);
            if (user is not null)
            {
                return new BaseResponse<User>()
                {
                    Description = "Пользователь с таким логином уже есть",
                    StatusCode = StatusCode.AlreadyExists
                };
            }
        */
            await _trainingTypeRepository.Create(model);

            return new BaseResponse<TrainingType>()
            {
                Data = model,
                Description = "Новый вид тренировки добавлен",
                StatusCode = StatusCode.OK
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"[{nameof(TrainingTypeService)}.{nameof(Create)}] error: {ex.Message}");
            return new BaseResponse<TrainingType>()
            {
                StatusCode = StatusCode.InternalServerError,
                Description = $"Внутренняя ошибка: {ex.Message}"
            };
        }
    }

    public async Task<IBaseResponse<IEnumerable<TrainingType>>> GetAll()
    {
        try
        {
            var trainingTypes = await _trainingTypeRepository.GetAll().ToListAsync();

            _logger.LogInformation($"[{nameof(TrainingTypeService)}.{nameof(GetAll)}] получено элементов {trainingTypes.Count}");
            return new BaseResponse<IEnumerable<TrainingType>>()
            {
                Data = trainingTypes,
                StatusCode = StatusCode.OK
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"[TrainingTypeService.GetAll] error: {ex.Message}");
            return new BaseResponse<IEnumerable<TrainingType>>()
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
            var trainingType = await _trainingTypeRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);
            if (trainingType is null)
            {
                return new BaseResponse<bool>
                {
                    StatusCode = StatusCode.NotFound,
                    Data = false
                };
            }

            await _trainingTypeRepository.Delete(trainingType);
            _logger.LogInformation($"[TrainingTypeService.DeleteTrainingType] вид тренировки удален");

            return new BaseResponse<bool>
            {
                StatusCode = StatusCode.OK,
                Data = true
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"[TrainingTypeService.DeleteTrainingType] error: {ex.Message}");
            return new BaseResponse<bool>
            {
                StatusCode = StatusCode.InternalServerError,
                Description = $"Внутренняя ошибка: {ex.Message}"
            };
        }
    }
}