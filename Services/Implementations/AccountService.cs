using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ArtBiathlon.DAL.Interfaces;
using ArtBiathlon.DataEntity;
using ArtBiathlon.Interfaces;
using ArtBiathlon.Models;
using ArtBiathlon.Response;
using ArtBiathlon.Services.Interfaces;

namespace ArtBiathlon.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly IBaseRepository<User> _userRepository;
        private readonly ILogger<AccountService> _logger;

        public AccountService(ILogger<AccountService> logger, IBaseRepository<User> userRepository)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<BaseResponse<ClaimsIdentity>> Login(AuthorizationModel model)
        {
            try
            {
                var user = await _userRepository.GetAll().FirstOrDefaultAsync(x => x.Login == model.Login);
                if (user is null)
                {
                    return new BaseResponse<ClaimsIdentity>
                    {
                        Description = "Пользователь не найден"
                    };
                }

                if (user.Password != model.Password)
                {
                    return new BaseResponse<ClaimsIdentity>
                    {
                        Description = "Введен неверный пароль"
                    };
                }

                var result = Authenticate(user);

                return new BaseResponse<ClaimsIdentity>
                {
                    Data = result,
                    StatusCode = Enums.StatusCode.OK
                };
            }
            catch(Exception exception)
            {
                _logger.LogError(exception, $"[AccountService]: {exception.Message}");

                return new BaseResponse<ClaimsIdentity>
                {
                    Description = exception.Message,
                    StatusCode = Enums.StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<ClaimsIdentity>> Register(RegisterModel model)
        {
            try
            {
                var user = await _userRepository.GetAll().FirstOrDefaultAsync(x => x.Login == model.Login);

                if (user is not null)
                {
                    return new BaseResponse<ClaimsIdentity>
                    {
                        Description = "Данный логин занят",
                        StatusCode = Enums.StatusCode.AlreadyExists
                    };
                }

                user = new User
                {
                    FIO = model.FIO,
                    Login = model.Login,
                    Password = model.Password,
                    Rank = model.Rank,
                    Gender = model.Gender,
                    Email = model.Email,
                    Status = model.Role,
                    Date = model.Date,
                    EmailAccessPassword = null
                    
                };

                await _userRepository.Create(user);

                var result = Authenticate(user);

                return new BaseResponse<ClaimsIdentity>
                {
                    Data = result,
                    Description = "Пользователь зарегистрирован",
                    StatusCode = Enums.StatusCode.OK
                };
            }
            catch(Exception exception)
            {
                _logger.LogError(exception, $"[AccountService]: {exception.Message}");

                return new BaseResponse<ClaimsIdentity>
                {
                    Description = exception.Message,
                    StatusCode = Enums.StatusCode.InternalServerError
                };
            }
        }

        private ClaimsIdentity Authenticate(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Status.ToString())
            };

            return new ClaimsIdentity(claims, "ApplicationCookie",
                ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
        }
    }
}
