using Microsoft.EntityFrameworkCore;
using ArtBiathlon.Services.Interfaces;

using ArtBiathlon.Enums;
using ArtBiathlon.DAL.Interfaces;
using ArtBiathlon.DataEntity;
using ArtBiathlon.Interfaces;
using ArtBiathlon.Response;

namespace ArtBiathlon.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly IBaseRepository<User> _userRepository;

        public UserService(ILogger<UserService> logger, IBaseRepository<User> userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;        
        }

        public async Task<IBaseResponse<User>> Create(User model)
        {
            try
            {
                var user = await _userRepository.GetAll().FirstOrDefaultAsync(x => x.Login == model.Login);
                if (user is not null)
                {
                    return new BaseResponse<User>()
                    {
                        Description = "Пользователь с таким логином уже есть",
                        StatusCode = StatusCode.AlreadyExists
                    };
                }

                await _userRepository.Create(model);

                return new BaseResponse<User>()
                {
                    Data = model,
                    Description = "Пользователь добавлен",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[{nameof(UserService)}.{nameof(Create)}] error: {ex.Message}");
                return new BaseResponse<User>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = $"Внутренняя ошибка: {ex.Message}"
                };
            }
        }

        public async Task<IBaseResponse<IEnumerable<User>>> GetAll()
        {
            try
            {
                var users = await _userRepository.GetAll().ToListAsync();

                _logger.LogInformation($"[{nameof(UserService)}.{nameof(GetAll)}] получено элементов {users.Count}");
                return new BaseResponse<IEnumerable<User>>()
                {
                    Data = users,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[UserSerivce.GetAll] error: {ex.Message}");
                return new BaseResponse<IEnumerable<User>>()
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
                var user = await _userRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);
                if (user is null)
                {
                    return new BaseResponse<bool>
                    {
                        StatusCode = StatusCode.NotFound,
                        Data = false
                    };
                }

                await _userRepository.Delete(user);
                _logger.LogInformation($"[UserService.DeleteUser] пользователь удален");

                return new BaseResponse<bool>
                {
                    StatusCode = StatusCode.OK,
                    Data = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[UserSerivce.DeleteUser] error: {ex.Message}");
                return new BaseResponse<bool>
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = $"Внутренняя ошибка: {ex.Message}"
                };
            }
        }
    }
}
