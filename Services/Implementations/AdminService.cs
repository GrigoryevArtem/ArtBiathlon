using Microsoft.EntityFrameworkCore;
using ArtBiathlon.DAL.Interfaces;
using ArtBiathlon.DataEntity;
using ArtBiathlon.Interfaces;
using ArtBiathlon.Models;
using ArtBiathlon.Response;
using ArtBiathlon.Services.Interfaces;

namespace ArtBiathlon.Services.Implementations
{
    public class AdminService : IAdminService
    {
        private readonly IBaseRepository<User> _userRepository;
        private readonly IBaseRepository<MailingTopic> _mailingTopicRepository;
        private readonly IBaseRepository<Mailing> _mailingRepository;
        private readonly IBaseRepository<MailingTopicSubscriber> _mailingTopicSubscriberRepository;
        private readonly IBaseRepository<Help> _helpRepository;
        private readonly ILogger<AdminService> _logger;

        public AdminService(
            IBaseRepository<Help> helpRepository, IBaseRepository<User> userRepository,
            ILogger<AdminService> logger, IBaseRepository<MailingTopic> mailingTopicRepository,
            IBaseRepository<Mailing> mailingRepository, IBaseRepository<MailingTopicSubscriber> mailingTopicSubscriberRepository)
        {
            _helpRepository = helpRepository;
            _logger = logger;
            _userRepository = userRepository;
            _mailingTopicRepository = mailingTopicRepository;
            _mailingRepository = mailingRepository;
            _mailingTopicSubscriberRepository = mailingTopicSubscriberRepository;
        }

        public async Task<IBaseResponse<bool>> CreateAnswer(uint id, string answer)
        {
            try
            {
                var model = await _helpRepository.GetAll().FirstOrDefaultAsync(help => help.Id == id);
                model.Answer = answer;

                if (model is null)
                {
                    return new BaseResponse<bool>
                    {
                        Description = "Данной записи не существует",
                        StatusCode = Enums.StatusCode.NotFound
                    };
                }

                var user = await _userRepository.GetAll().FirstOrDefaultAsync(user => user.Id == model.UserId);

                if (user is null)
                {
                    return new BaseResponse<bool>
                    {
                        Description = "Данный пользователь не найден",
                        StatusCode = Enums.StatusCode.NotFound
                    };
                }

                if (string.IsNullOrEmpty(user.Email))
                {
                    return new BaseResponse<bool>
                    {
                        Description = "У пользователя не задан Email",
                        StatusCode = Enums.StatusCode.NotFound
                    };
                }

                var userEmail = user.Email;

                var sender = new MailSender("runapp90@mail.ru", userEmail, "RunApp");

                await sender.Send("Ответ на вопрос", $"Администратор дал ответ на ваш вопрос.\nВопрос: {model.Question}\nОтвет: {model.Answer}");

                await _helpRepository.Update(model);

                return new BaseResponse<bool>
                {
                    Data = true,
                    StatusCode = Enums.StatusCode.OK
                };
            }
            catch(Exception exception)
            {
                _logger.LogError(exception, $"[{nameof(AdminService)}]: {exception.Message}");
                return new BaseResponse<bool>
                {
                    Description = exception.Message,
                    StatusCode = Enums.StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<List<HelpViewModel>>> GetQuestions()
        {
            try
            {
                var list = _helpRepository
                    .GetAll()
                    .Select(help => new HelpViewModel
                    {
                        UserId = help.UserId,
                        Answer = help.Answer,
                        Question = help.Question,
                        Date = help.Date,
                        Id = help.Id,
                        UserFio = _userRepository
                            .GetAll()
                            .FirstOrDefault(user => user.Id == help.UserId)!
                        .FIO
                    })
                    .OrderBy(x => x.Answer == null)
                    .ToList();

                return new BaseResponse<List<HelpViewModel>>
                {
                    Data = list,
                    StatusCode = Enums.StatusCode.OK
                };
            }
            catch(Exception exception)
            {
                _logger.LogError(exception, $"[{nameof(AdminService)}]: {exception.Message}");
                return new BaseResponse<List<HelpViewModel>>
                {
                    Description = exception.Message,
                    StatusCode = Enums.StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<Dictionary<string, (int, int, int)>>> GetTopicsInformation()
        {
            try
            {
                var dict = _mailingTopicRepository
                    .GetAll()
                    .Join(_mailingRepository.GetAll(), t => t.Id, m => m.MailingTopicId, (t, m) => new
                    {
                        Title = t.Title,
                        SubscribeCount = _mailingRepository.GetAll().Count(x => x.MailingTopicId == t.Id),
                        UserCount = _mailingTopicSubscriberRepository.GetAll().Count(x => x.MailingTopicId == t.Id),
                        Subscribers = _mailingTopicSubscriberRepository.GetAll().Where(x => x.MailingTopicId == t.Id).GroupBy(x => x.UserId)
                    })
                    .GroupBy(x => x.Title, x => new { x.SubscribeCount, x.UserCount, x.Subscribers })
                    .ToDictionary(x => x.Key, x => (x.Select(y => y.SubscribeCount).Count(), x.Sum(y => y.UserCount), x.Select(y => y.Subscribers).Count()));

                return new BaseResponse<Dictionary<string, (int, int, int)>>
                {
                    Data = dict,
                    StatusCode = Enums.StatusCode.OK
                };
            }
            catch(Exception exception)
            {
                _logger.LogError(exception, $"[{nameof(AdminService)}]: {exception.Message}");
                return new BaseResponse<Dictionary<string, (int, int, int)>>
                {
                    Description = exception.Message,
                    StatusCode = Enums.StatusCode.InternalServerError
                };
            }
        }
    }
}
