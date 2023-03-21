using Microsoft.EntityFrameworkCore;
using ArtBiathlon.DAL.Interfaces;
using ArtBiathlon.DataEntity;
using ArtBiathlon.Interfaces;
using ArtBiathlon.Models;
using ArtBiathlon.Response;
using ArtBiathlon.Services.Interfaces;

namespace ArtBiathlon.Services.Implementations
{
    public class PersonalAccountService : IPersonalAccountService
    {
        private readonly IBaseRepository<User> _userRepository;
       // private readonly IBaseRepository<Training> _trainingRepository;
        //private readonly IBaseRepository<Indicator> _indicatorRepository;
        //private readonly IBaseRepository<TrainingTemplate> _trainingTemplateRepository;
        private readonly IBaseRepository<MailingTopic> _mailingTopicRepository;
        private readonly IBaseRepository<MailingTopicSubscriber> _mailingTopicSubscriberRepository;
        private readonly ILogger<PersonalAccountService> _logger;

        public PersonalAccountService(
            IBaseRepository<MailingTopic> mailingTopicRepository, ILogger<PersonalAccountService> logger,
            IBaseRepository<User> userRepository, IBaseRepository<MailingTopicSubscriber> mailingTopicSubscriberRepository)
        {
            _mailingTopicRepository = mailingTopicRepository;
            _logger = logger;
            _userRepository = userRepository;
            _mailingTopicSubscriberRepository = mailingTopicSubscriberRepository;
        }

        public async Task<IBaseResponse<bool>> CreateSubscribe(string login, int[] titles)
        {
            try
            {
                var user = await _userRepository.GetAll().FirstOrDefaultAsync(x => x.Login == login);

                var ids = _mailingTopicSubscriberRepository
                    .GetAll()
                    .Where(x => x.UserId == user.Id)
                    .Select(x => x.MailingTopicId)
                    .ToList();

                var indexes = titles
                    .Where(x => !ids.Contains(x))
                    .ToList();

                for (int i = 0; i < indexes.Count; i++)
                {
                    _mailingTopicSubscriberRepository.Create(new MailingTopicSubscriber { MailingTopicId = indexes[i], UserId = (int)user.Id });
                }

                return new BaseResponse<bool>
                {
                    Data = true,
                    StatusCode = Enums.StatusCode.OK
                };
            }
            catch(Exception exception)
            {
                _logger.LogError(exception, $"[{nameof(PersonalAccountService)}]: {exception.Message}");
                return new BaseResponse<bool>
                {
                    Description = exception.Message,
                    StatusCode = Enums.StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<List<MailingTopic>>> GetMailingTopics()
        {
            try
            {
                var list = await _mailingTopicRepository.GetAll().ToListAsync();

                return new BaseResponse<List<MailingTopic>>
                {
                    Data = list,
                    StatusCode = Enums.StatusCode.OK
                };
            }
            catch(Exception exception)
            {
                _logger.LogError(exception, $"[{nameof(PersonalAccountService)}]: {exception.Message}");
                return new BaseResponse<List<MailingTopic>>
                {
                    Description = exception.Message,
                    StatusCode = Enums.StatusCode.InternalServerError
                };
            }
        }

       /* public async Task<IBaseResponse<List<TrainingInformation>>> GetTrainings(string login)
        {
            try
            {
                var user = await _userRepository.GetAll().FirstOrDefaultAsync(x => x.Login == login);

                var trainings = await _indicatorRepository
                    .GetAll()
                    .Where(ind => ind.UserId == user.Id)
                    .Join(_trainingRepository.GetAll(), ind => ind.Date, train => train.Date, (ind, train) => new { ind, train })
                    .Join(_trainingTemplateRepository.GetAll(), selector => selector.train.TrainTemplateId,
                        template => template.Id, (selector, template) => new { selector.ind, template.Title })
                    .Select(result => new TrainingInformation
                    {
                        Id = result.ind.Id,
                        AveragePulse = result.ind.AveragePulse,
                        MaximumPulse = result.ind.MaximumPulse,
                        MinimumPulse = result.ind.MinimumPulse,
                        Steps = result.ind.Steps,
                        AverageSpeed = result.ind.AverageSpeed,
                        Title = result.Title,
                        Calories = result.ind.Calories,
                        Date = result.ind.Date,
                        Duration = result.ind.Duration,
                        Pressure = result.ind.Pressure,
                        UserId = result.ind.UserId
                    }).ToListAsync();

                return new BaseResponse<List<TrainingInformation>>
                {
                    Data = trainings,
                    StatusCode = Enums.StatusCode.OK
                };
            }
            catch(Exception exception)
            {
                _logger.LogError(exception, $"[{nameof(PersonalAccountService)}]: {exception.Message}");
                return new BaseResponse<List<TrainingInformation>>
                {
                    Description = exception.Message,
                    StatusCode = Enums.StatusCode.InternalServerError
                };
            }
        }*/
    }
}
