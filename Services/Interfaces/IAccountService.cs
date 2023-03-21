using System.Security.Claims;
using ArtBiathlon.Models;
using ArtBiathlon.Response;

namespace ArtBiathlon.Services.Interfaces
{
    public interface IAccountService
    {
        Task<BaseResponse<ClaimsIdentity>> Login(AuthorizationModel model);

        Task<BaseResponse<ClaimsIdentity>> Register(RegisterModel model);
    }
}
