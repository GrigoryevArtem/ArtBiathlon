using ArtBiathlon.DAL.Interfaces;
using ArtBiathlon.DataEntity;
using ArtBiathlon.Enums;
using ArtBiathlon.Interfaces;
using ArtBiathlon.Response;
using ArtBiathlon.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ArtBiathlon.Services.Implementations;

public class HrvIndicatorService : IHrvIndicatorService
{
        private readonly ILogger<HrvIndicatorService> _logger;
        private readonly IBaseRepository<HrvIndicator> _hrvIndicatorRepository;

        public HrvIndicatorService(ILogger<HrvIndicatorService> logger, IBaseRepository<HrvIndicator> hrvIndicatorsRepository)
        {
            _logger = logger;
            _hrvIndicatorRepository = hrvIndicatorsRepository;        
        }

        public async Task<IBaseResponse<HrvIndicator>> Create(HrvIndicator model)
        {
            try
            {
                var hrvIndicator = await _hrvIndicatorRepository.GetAll().FirstOrDefaultAsync(x => x.Date == model.Date && x.IdBiathlete == model.IdBiathlete);
                if (hrvIndicator is not null)
                {
                    return new BaseResponse<HrvIndicator>()
                    {
                        Description = "Замер HRV-показателей в эту дату уже существует",
                        StatusCode = StatusCode.AlreadyExists
                    };
                }

                await _hrvIndicatorRepository.Create(model);

                return new BaseResponse<HrvIndicator>()
                {
                    Data = model,
                    Description = "HRV-показатель добавлен",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[{nameof(HrvIndicatorService)}.{nameof(Create)}] error: {ex.Message}");
                return new BaseResponse<HrvIndicator>
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = $"Внутренняя ошибка: {ex.Message}"
                };
            }
        }

        public async Task<IBaseResponse<IEnumerable<HrvIndicator>>> GetAll()
        {
            try
            {
                var hrvIndicators = await _hrvIndicatorRepository.GetAll().ToListAsync();

                _logger.LogInformation($"[{nameof(HrvIndicatorService)}.{nameof(GetAll)}] получено элементов {hrvIndicators.Count}");
                return new BaseResponse<IEnumerable<HrvIndicator>>()
                {
                    Data = hrvIndicators,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[HrvIndicatorService.GetAll] error: {ex.Message}");
                return new BaseResponse<IEnumerable<HrvIndicator>>()
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
                ///??????
                var hrvIndicators = await _hrvIndicatorRepository.GetAll().FirstOrDefaultAsync(x => x.IdBiathlete == id);
                if (hrvIndicators is null)
                {
                    return new BaseResponse<bool>
                    {
                        StatusCode = StatusCode.NotFound,
                        Data = false
                    };
                }

                await _hrvIndicatorRepository.Delete(hrvIndicators);
                _logger.LogInformation($"[HrvIndicatorService.DeleteHrvIndicator] HRV-показатель удален");

                return new BaseResponse<bool>
                {
                    StatusCode = StatusCode.OK,
                    Data = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[HrvIndicatorService.DeleteHrvIndicator] error: {ex.Message}");
                return new BaseResponse<bool>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = $"Внутренняя ошибка: {ex.Message}"
                };
            }
        }

        public async Task<IBaseResponse<bool>> Delete(DateTime date, long id)
        {
            try
            {
                var hrvIndicators = await _hrvIndicatorRepository.GetAll().FirstOrDefaultAsync(x => x.Date == date && x.IdBiathlete == id);
                if (hrvIndicators is null)
                {
                    return new BaseResponse<bool>
                    {
                        StatusCode = StatusCode.NotFound,
                        Data = false
                    };
                }

                await _hrvIndicatorRepository.Delete(hrvIndicators);
                _logger.LogInformation($"[HrvIndicatorService.DeleteHrvIndicator] HRV-показатель удален");

                return new BaseResponse<bool>
                {
                    StatusCode = StatusCode.OK,
                    Data = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[HrvIndicatorService.DeleteHrvIndicator] error: {ex.Message}");
                return new BaseResponse<bool>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = $"Внутренняя ошибка: {ex.Message}"
                };
            }
        }
}