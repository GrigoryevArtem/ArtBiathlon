using ArtBiathlon.DAL.Interfaces;
using ArtBiathlon.DataEntity;
using ArtBiathlon.Enums;
using ArtBiathlon.Interfaces;
using ArtBiathlon.Response;
using ArtBiathlon.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;


namespace ArtBiathlon.Services.Implementations
{
    public class MailingService : IMailingService
    {
        private readonly ILogger<MailingService> _logger;
        private readonly IBaseRepository<Mailing> _mailingRepository;

        public MailingService(ILogger<MailingService> logger, IBaseRepository<Mailing> mailingRepository)
        {
            _logger = logger;
            _mailingRepository = mailingRepository;
        }

        public async Task<IBaseResponse<Mailing>> Create(Mailing model)
        {
            try
            {
                await _mailingRepository.Create(model);

                return new BaseResponse<Mailing>
                {
                    Data = model,
                    StatusCode = StatusCode.OK,
                };
            }
            catch(Exception exception)
            {
                _logger.LogError(exception, $"[{nameof(MailingService)}.{nameof(Create)}] error: {exception.Message}");
                return new BaseResponse<Mailing>()
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
                var mailing = await _mailingRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);
                if (mailing == null)
                {
                    return new BaseResponse<bool>
                    {
                        StatusCode = StatusCode.NotFound,
                        Data = false
                    };
                }

                await _mailingRepository.Delete(mailing);
                _logger.LogInformation($"[{nameof(MailingService)}.{nameof(Delete)}] рассылка удалена");

                return new BaseResponse<bool>
                {
                    StatusCode = StatusCode.OK,
                    Data = true
                };
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"[{nameof(MailingService)}.{nameof(Delete)}] error: {exception.Message}");
                return new BaseResponse<bool>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = $"Внутренняя ошибка: {exception.Message}"
                };
            }
        }

        public async Task<IBaseResponse<IEnumerable<Mailing>>> GetAll()
        {
            try
            {
                var mailings = await _mailingRepository.GetAll().ToListAsync();
                _logger.LogInformation($"[{nameof(MailingService)}.{nameof(GetAll)}] получено рассылок {mailings.Count}");

                return new BaseResponse<IEnumerable<Mailing>>
                {
                    Data = mailings,
                    StatusCode = StatusCode.OK,
                };
            }
            catch(Exception exception)
            {
                _logger.LogError(exception, $"[{nameof(MailingService)}.{nameof(GetAll)}] error: {exception.Message}");
                return new BaseResponse<IEnumerable<Mailing>>
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = $"Внутренняя ошибка: {exception.Message}"
                };
            }
        }
    }
}
